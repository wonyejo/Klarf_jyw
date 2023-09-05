using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Klarf.ViewModel
{
    class WaferMapViewerVM : ViewModelBase
    {
        public Wafer wafer;
        
        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged(nameof(Wafer));

                if (wafer != null)
                {
                    DrawWaferMap(wafer);
                }
            }

        }


        public ObservableCollection<Shape> WaferMapShapes { get; set; } // Shapes 컬렉션을 추가

        public WaferMapViewerVM()
        {
            WaferMapShapes = new ObservableCollection<Shape>();

            if (Wafer != null)
            {
                LoadWaferData(SharedData.Instance.Wafer);
                DrawWaferMap(Wafer);
            }

            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;

            //defects = new ObservableCollection<Defect>();
        }

        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;

        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                Wafer = SharedData.Instance.Wafer;
            }

        }

        private void DrawWaferMap(Wafer wafer)
        {
            WaferMapShapes.Clear(); // 기존의 Shapes 제거

            double minX = wafer.Dies.Min(die => die.X);
            double maxY = wafer.Dies.Max(die => die.Y);

            foreach (var die in wafer.Dies)
            {
                var rect = new Rectangle();
                double cellWidth = 380.0 / wafer.GridWidth; // 화면의 크기를 웨이퍼 그리드 값으로 나눔
                double cellHeight = 380.0 / wafer.GridHeight;
                rect.Width = cellWidth;
                rect.Height = cellHeight;
                rect.Stroke = Brushes.Gray;
                rect.StrokeThickness = 1;

                if (die.IsDefectInDie)
                {
                    rect.Fill = Brushes.Red;
                    rect.Stroke = Brushes.White;
                }
                else
                {
                    rect.Fill = Brushes.Gray;
                    rect.Stroke = Brushes.DarkGray;
                }

                // die의 좌표를 상대좌표로 변환하여 적용
                double x = (die.X - minX) * cellWidth;
                double y = (maxY - die.Y) * cellHeight;

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                WaferMapShapes.Add(rect); // Shapes 컬렉션에 Rectangle 추가
            }
        }
    }
}
/*
 * 지원하신 분야와 관련하여, 자신이 보유한 직무역량 두 가지를 제시해 주세요. 그리고 각 역량에 대한 수준, 활용 사례, 성공/실패 경험 등 자신의 이야기를 자유롭게 들려주세요. [게임 플레이 경험, 기술스택, 업무툴, 전문지식, 외국어, 자격증 등]


제가 보유한 직무 역량 중 첫 번째는 C++을 포함한 다양한 프로그래밍 기술 스택을 보유하고 있다는 점입니다. 이러한 스택을 활용하여 인천인디게임아카데미에 참여하여 게임 개발에 참여한 경험이 있습니다.
또한, 리그오브레전드를 비롯한 다양한 게임을 플레이하며 게임 개발에 필요한 이해와 미적 감각을 키웠습니다.

 * 
 */