using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM : ViewModelBase
    {

        public Wafer wafer;
       public string timeStamp;
        public string waferID;
        public string lotID;
        private int currentDieID;
        private int curDefectID;
        private int currentDieDefectID;
        private Defect selectedDefect;
        private Die selectedDie;
        private ObservableCollection<Defect> defects;
        private BitmapSource curDefectImg;
        public TiffBitmapDecoder tiffDecoder;
        public KlarfDataViewerVM()
        {
            defects = new ObservableCollection<Defect>();
            LoadWaferData(SharedData.Instance.Wafer);
            if(Wafer!=null) LoadDefectData(SharedData.Instance.Defects);
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
            GoPrevDefectCommand = new RelayCommand(GoPrevDefect);
            GoNextDefectCommand = new RelayCommand(GoNextDefect);
            

        }

        public BitmapSource CurDefectImg
        {
            get { return curDefectImg; }
            set
            {
                curDefectImg = value;
                OnPropertyChanged(nameof(CurDefectImg));
            }
        }

        public void LoadTiffData(TiffBitmapDecoder LoadedTiff)
        {
            tiffDecoder = LoadedTiff;

        }
        public void GoPrevDefect()
        {
            if (CurDefectID > 0)
            {
                CurDefectID--;
                selectedDefect = defects[CurDefectID];
                SharedData.Instance.DefectID = CurDefectID;
                CurDefectImg = SharedData.Instance.tiffDecoder.Frames[CurDefectID];
            }
           
        }
        public void GoNextDefect()
        {
            if (CurDefectID < TotalDefects - 1)
            {
                CurDefectID++;
                SelectedDefect = Defects[CurDefectID];
                SharedData.Instance.DefectID = CurDefectID;
                CurDefectImg = SharedData.Instance.tiffDecoder.Frames[CurDefectID];
            }
          

        }
        public ICommand GoPrevDefectCommand { get; private set; }
        public ICommand GoNextDefectCommand { get; private set; }
        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
               
                OnPropertyChanged(nameof(Wafer));
                OnPropertyChanged(nameof(DefectList));
                OnPropertyChanged(nameof(DieList));
                OnPropertyChanged(nameof(TotalDefects));
                OnPropertyChanged(nameof(TotalDies));
                
            }       

        }
        public ObservableCollection<Defect> Defects
        {
            get => defects;
            set
            {
                defects = value;
                OnPropertyChanged(nameof(Defects));
            }
        }
        public Die SelectedDie
        {
            get => selectedDie;
            set
            {
                if (selectedDie != value)
                {
                    selectedDie = value;
                    OnPropertyChanged(nameof(SelectedDie));
                  
                }
            }
        }
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set
            {
                if (selectedDefect != value)
                {
                    selectedDefect = value;
                    OnPropertyChanged(nameof(SelectedDefect));

                    

                    for (int i = 0; i < Wafer.Dies.Count; i++)
                    {
                        Die currentDie = Wafer.Dies[i];
                        if (currentDie.Defects.Contains(selectedDefect))
                        {
                            SelectedDie = currentDie;
                            CurrentDieID = i;
                            CurrentDieDefectID = currentDie.Defects.IndexOf(selectedDefect);
                            break;
                        }
                    }
                }
            }
        }
        public int CurDefectID
        {
            get { return curDefectID; }
            set
            {
                if (curDefectID != value)
                {
                    curDefectID = value;
                    OnPropertyChanged(nameof(CurDefectID));
                    OnPropertyChanged(nameof(DefectList));

                }
            }
        }

        public int CurrentDieDefectID
        {
            get { return currentDieDefectID; }
            set
            {
                if (currentDieDefectID != value)
                {
                    currentDieDefectID = value;
                    OnPropertyChanged(nameof(CurrentDieDefectID));
                    OnPropertyChanged(nameof(DieDefectList));
                }
            }
        }
       

        public int CurrentDieID
        {
            get { return currentDieID; }
            set
            {
                if (currentDieID != value)
                {
                    currentDieID = value;
                    OnPropertyChanged(nameof(currentDieID));
                    OnPropertyChanged(nameof(DieList));
                    SelectedDie = wafer.Dies[currentDieID];

                    if (SelectedDie.Defects.Any())
                    {
                        selectedDefect = SelectedDie.Defects[0];
                    }
                    else
                    {
                        selectedDefect = null;
                    }

                    OnPropertyChanged(nameof(TotalDieDefects));
                    OnPropertyChanged(nameof(DieDefectList));
                }
            }
        }

        public int TotalDefects
        {
            get { return Defects.Count; }
        }
        public int TotalDies
        {
            get { return wafer?.Dies?.Count ?? 0; }
        }
        public int TotalDieDefects
        {
            get { return SelectedDie?.Defects?.Count ?? 0; }
        }


        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
            if (wafer != null)
            {
                timeStamp = wafer.FileTimestamp;
                waferID = wafer.WaferID;
                lotID = wafer.LotID;

            }
      
        }
         private void LoadDefectData(List<Defect> Defects)
        {
            this.Defects = new ObservableCollection<Defect>(Defects);
           
        }
      
          
        public string DieList
        {
            get
            {
                if (TotalDies == 0)
                {
                    return $"0/0";
                }
                return $"{CurrentDieID + 1}/{TotalDies}";
            }
        }
        public string DefectList
        {
            get
            {
                if (TotalDefects == 0)
                {
                    return $"0/0";
                }
                return $"{CurDefectID + 1}/{TotalDefects}";
            }
        }
        public string DieDefectList
        {
            get
            {
                if (TotalDieDefects == 0)
                {
                    return $"0/0";
                }
                else
                {
                    return $"{CurrentDieDefectID + 1}/{TotalDieDefects}";
                }
               
            }
        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                LoadWaferData(SharedData.Instance.Wafer);
                
            }
            if(e.PropertyName =="Defects")
            {
                LoadDefectData(SharedData.Instance.Defects);
            }
            if(e.PropertyName == "tiffDecoder")
            {
                LoadTiffData(SharedData.Instance.tiffDecoder);
            }
           if(e.PropertyName== "curDefectImg")
            {
                SharedData.Instance.CurDefectImg = curDefectImg;
            }
        }

    }
}
/*
 * 1. 희망 직무를 지원한 이유와 해당 직무를 수행하기 위해 어떠한 노력이나 경험을 하였는지 작성해주세요.

[AR, 그리고 게임]
제가 학부 생활 동안 겪은 다양한 프로젝트는 저를 자연스럽게 게임과 이어주었고, 게임 개발자의 꿈을 갖게 해주었습니다.
경복궁 현장학습 효과를 높이기 위해 경복궁에 방 탈출 게임의 오락성을 가미한 교육용 안드로이드 앱을 만든 적이 있습니다. 여기에 AR 기술을 접목하고 싶었으나 AR 분야에 대한 지식 및 기술력 부재와 시간적 한계로 이를 미루게 되었습니다. 앞선 프로젝트를 마치고 방학 동안 AR 기술을 사용하기 위해 간단한 게임을 만드는 실습을 하며 유니티 에디터의 기본적인 사용법을 익혔고, 이듬해 자유 프로젝트 과목에서 AR을 활용해 두 번의 프로젝트를 진행하게 되었습니다.
AR을 활용한 첫 프로젝트는 캠퍼스 구조물 정보 제공과 길 찾기 서비스로 Vuforia 엔진을 사용해서 AR 환경을 구축했습니다. 캠퍼스 건물마다 터치하여 건물의 정보를 제공하는 UI가 필요해 씬에 지도를 넣고 해당 위치에 UI를 위치시킨 뒤, 씬 내의 카메라의 위치를 스마트폰에 내장된 GPS로부터 얻은 위도 경도 정보를 바탕으로 계산하여 이동시켰습니다. 또한, UI 작업을 주로 하며 UI의 이벤트 처리 및 업데이트, 리소스를 동적으로 할당하는 방법 등을 알게 되었습니다.
두 번째 프로젝트는 스마트폰 터치 등의 기존의 인터랙티브가 아닌 손으로 직접 AR 환경을 제어하는 사용자 경험을 제공하는 기술을 개발하는 것으로 OpenCVSharp을 사용해 손 추적 알고리즘을 구현하였습니다. 이렇게 구현한 추적을 더욱 정교하게 AR 환경에 적용하기 위해 손가락 끝점에 대응된 오브젝트가 손바닥을 향하도록 회전시키면서 두 물체 사이의 위치를 통해 각도를 계산했습니다. 이 과정에서 벡터와 삼각함수에 대한 개념을 다시 정리하고 원하는 값을 도출하기 위해 이를 활용하는 능력을 길렀습니다. 또한, 더욱 정확한 인식을 위해 딥러닝을 사용할 때, 입력 영상을 서버에 전송하기 위해 소켓 통신을 구현하였습니다. 이때 동기식으로 구현하여 서버에서 데이터를 처리하고 클라이언트에게 다시 데이터를 주기 전까지 모든 실행 흐름이 block 되어 있었기 때문에 통신에 대해 스레드를 생성했습니다. 그리고 큐를 만들어 서버로부터 데이터를 수신했을 때 큐에 데이터를 넣고, 메인 스레드에서 매 프레임 큐를 검사해 데이터가 있으면 꺼내 처리하였고 큐에 데이터를 넣고 빼는 작업은 임계 영역으로 만들어 경쟁 상태 발생을 막았습니다. 이렇게 멀티 스레드 프로그래밍도 경험할 수 있었습니다.
AR 프로젝트를 진행하며 유니티를 사용하면서 게임을 만들어보고 싶은 욕심이 생겼고, 바로 실행으로 옮겼습니다. 개인전 멀티 축구 게임을 개발했는데, 공이 더욱 실제 공처럼 움직이도록 하기 위해 Physics Material을 만들었고, 축구 게임에서의 골 판별을 하며 충돌에 대한 처리 방법을 알게 되었습니다. 미니맵을 만들며 렌더 텍스처를 사용해 씬 내에서 동적으로 만들어지는 화면을 텍스처로 만드는 방법, 플레이어 위치 보간을 위한 코루틴을 사용한 병렬 처리, 기타 UI 작업 및 오디오 소스 사용 등 유니티 엔진이 제공하는 많은 기능에 대해 알게 되었고, 실시간 동기화나 래그돌 사용으로 발생하는 버그를 해결하며 문제해결능력을 갖췄습니다.
이러한 노력과 경험이 컴투스 클라이언트 개발자의 역량으로 발휘될 것이라 생각하여 해당 직무에 지원하게 되었습니다.


2. 타인과의 협업 과정에서 겪은 갈등과 이를 해결하기 위해 어떤 노력을 하였는지 구체적으로 작성해주세요.

[스타일을 통일하다]
저학년 때 처음으로 다른 사람과 협업 프로젝트를 하면서 어려웠던 것은 내가 짠 코드와 다른 사람이 짠 코드를 합치는 과정에서 수정할 것이 많아지고 전체적으로 코드가 지저분해지는 것이었습니다. 띄어쓰기나 괄호 등 스타일이 다르고 변수명이나 함수명이 모두 각자의 스타일대로 지어져 통일성이 없었으며, 합칠 때는 다른 사람의 코드로 인해 내가 작성한 코드를 조금씩 변경해야 하는 일도 생겼습니다.
2학년 때 객체지향프로그래밍 수업에서 JAVA를 배웠는데 클래스, 변수, 함수 등의 명명규칙이 있다는 것을 알게 되었습니다. 이 수업에서 JAVA API를 참고할 때 모든 변수나 함수가 수업시간 때 배웠던 규칙대로 명명된 것을 확인할 수 있었습니다. 호기심에 이에 대해 찾아보았고 JAVA 뿐만 아니라 다른 언어도 프로그래머 사이에 약속한 규칙이 있다는 것을 알게 되었습니다. 뿐만 아니라 객체지향이라는 개념을 학습하면서 이런 객체지향적인 설계를 통해 코드를 모듈화할 수 있다는 것도 배웠습니다. 이러한 부분을 협업할 때 도입하면 앞서 언급한 문제는 일어나지 않을 것 같았습니다.
그래서 이후에 유니티 엔진을 사용한 프로젝트를 진행하게 되었을 때 협업을 위한 깃 레포지토리를 만들기 전에 C#의 코딩 스타일에 대해 먼저 문서로 정리했고, 팀원들에게 배포하여 규칙을 지키도록 했습니다. 또한, 다른 사람의 코드를 사용하기 쉽도록, 그리고 그 때 내가 작성한 코드가 영향을 받지 않도록 객체지향 언어인 C#의 장점을 이용해 코드를 클래스로 묶어 모듈화할 것을 강조했습니다. 그리고 객체의 캡슐화를 통해 다른 사람의 코드를 잘못 건드리는 일이 없도록 했습니다.
처음에는 같이 프로젝트를 진행하는 동기들이 제가 너무 유난을 떤다는 듯이 얘기하였습니다. 그러나 프로젝트를 진행하면서 이러한 노력이 이전보다 훨씬 효율적인 협업을 만들도록 도와준다는 것을 느꼈습니다. 팀원이 작성한 코드를 가져다가 사용하기 쉬웠을 뿐만 아니라 전체적으로 코드의 가독성을 높여주어 다른 사람이 짠 코드를 읽을 때도 더욱 쉬웠습니다. 그리고 높은 가독성은 코드의 유지보수에도 좋은 영향을 끼쳤습니다. 이후로는 팀원들이 먼저 저에게 코딩스타일 가이드 제정을 맡기게 되었고, 변수나 함수의 이름을 지을 때 저에게 물어보기도 하면서 규칙을 따르려는 모습을 보여주었습니다. 이는 곧 협업의 효율성과 직결되어 좋은 프로젝트 결과물을 만들어 낼 수 있었습니다.


3. 주어진 일이나 과제 수행 시 새로운 것을 접목하거나 남다른 아이디어를 통해 문제를 개선했던 경험에 대해 작성해주세요.

[새로운 멀티 게임 동기화 방식]
신개념 개인전 멀티 축구 게임을 개발하면서 네트워크 지연으로 인해 발생한 동기화 문제를 새로운 방법을 고안해 해결한 경험이 있습니다.
게임 개발 중 플레이했을 때 같이 게임을 하는 두 플레이어가 보는 화면이 달라지는 것을 보게 되었습니다. 이는 처음에 동기화를 고려하지 않고 간단하게 멀티 플레이 기능을 구현했기 때문에 네트워크 지연과 데이터 손실 등의 이유로 발생한 문제였습니다. 동기화 작업을 위해 락스텝, 서버 시뮬레이션 방식 등 여러 동기화 이론을 공부했습니다. 서버 시뮬레이션 방식은 서버에서 물리 연산이 가능해야 했지만 사용하고 있는 서버의 사양이 낮아 이 게임의 동기화 방식으로 채택할 수 없었습니다. 또한, 두 방식 모두 사용자의 입력을 전송해야 했습니다. 그러나 이미 캐릭터의 절대 좌표를 서버에 전송하고 서버에서는 다른 플레이어에게 이를 브로드캐스팅 하는 방식으로 구현하여 일반적인 락스텝 방식도 불가능했습니다. 많은 고민과 시행착오 끝에 분신 객체라는 개념을 고안해냈습니다. 사용자의 입력을 통해 로컬에서는 렌더링하지 않아 보이지 않는 분신 객체를 먼저 움직이고 서버에서는 이 객체의 좌표를 처리하도록 구현했습니다. 서버와 클라이언트의 부하를 줄이기 위해 위치 정보를 모아뒀다가 일정 시간 간격으로 전송하고 그에 따라 클라이언트 측에서 부드러운 움직임을 위한 보간을 하는 등의 작업을 추가하여 동기화의 퀄리티를 높였습니다. 또한, 물리적 충돌이나 래그돌로 Physic이 전환되는 등의 이벤트에 의해 분신 객체가 캐릭터 객체로 돌아오도록 구현해 분신 방식으로 인해 발생할 수 있는 버그를 해결하였습니다.
최종적으로 개발 중인 게임의 구현 상태에 맞추어 새로운 동기화 방식을 생각해내고 구현하여 함께 게임하는 플레이어가 일관된 화면을 볼 수 있도록 하였습니다. 버그 등 장애물을 제거하여 원활하게 게임을 즐길 수 있도록 완성도 높은 결과물을 만들어냈습니다. 교내 7+1학기제 자유 프로젝트 공모전에서 최우수상을 수상하며 그 노력을 인정받았습니다.


4. 프로그래밍을 하면서 가장 성취감이 있었던 경험에 대해서 작성해주세요.

[내가 만든 프로그램이 누군가를 웃게 할 때]
프로그래밍을 하면서 가장 성취감을 느꼈을 때는 게임을 개발할 때였습니다. 이전까지 과제나 공모전 등의 팀 프로젝트는 늘 제출만을 위한 프로그래밍이었습니다. 결과물을 제출한 이후에는 프로젝트를 진행하면서 사용한 언어, 툴, 기술에 대한 경험만이 남아있을 뿐, 더 이상 누군가가 사용하거나 보지 않는 프로그램이 되었습니다. 그러나 게임을 개발했을 때는 달랐습니다. 게임 개발이 거의 완료될 때쯤 개발하면서 미처 발견하지 못한 버그를 찾기 위해 동기들과 테스트를 한 적이 있었습니다. 목적은 버그를 찾는 것이었지만 테스트하는 프로그램이 게임이다 보니 재미있어서 계속하게 되었습니다. 직접 개발한 게임을 플레이하면서 즐거워하는 동기들을 보면서 이전에 프로그래밍을 하면서 느껴보지 못했던 정도의 성취감과 뿌듯함을 느꼈습니다. 특히 정말 재미있어서 자신의 친구들과 같이 그 게임을 플레이했다는 말과, 팀플레이도 가능하도록 만들어달라고 요구하는 동기들은 저에게 매우 큰 기쁨을 주었고 게임 개발에 더욱 열정적으로 임할 수 있도록 만들어주었습니다.

 * 
 */