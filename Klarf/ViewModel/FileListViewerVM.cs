using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Linq;
using Klarf.Model;
using Prism.Events;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace Klarf.ViewModel
{
    class FileListViewerVM : ViewModelBase
    {
        #region 필드
        private ObservableCollection<KlarfFile> klarfPaths;
        private KlarfFile selectedFile;
        private string selectedFolderPath;
        private RelayCommand _openFolderCommand;


        #endregion

        #region 속성
        public KlarfFile SelectedFile
        {
            get { return selectedFile; }
            set
            {
                if (selectedFile != value)
                {
                    selectedFile = value;
                    OnPropertyChanged("SelectedFile");
                    if (value != null)
                        LoadFromFile(value.Path);
                 
                }
            }
        }
        public string SelectedFolderPath
        {
            get
            {
                return selectedFolderPath;
            }
            set
            {
                selectedFolderPath = value;
                OnPropertyChanged(nameof(selectedFolderPath));
            }
        }

        public ObservableCollection<KlarfFile> KlarfPaths
        {
            get
            {
                return klarfPaths;
            }
            set
            {
                klarfPaths = value;
                OnPropertyChanged(nameof(KlarfPaths));
            }
        }
        public ICommand OpenFolderCommand
        {
            get
            {
                return _openFolderCommand ?? (_openFolderCommand = new RelayCommand(OpenFolderDialog));
            }
            set
            {

            }
        }
        #endregion

        #region 생성자
        public FileListViewerVM(IEventAggregator eventAggregator)
        {
            klarfPaths = new ObservableCollection<KlarfFile>();
            OpenFolderCommand = new RelayCommand(OpenFolderDialog);
        }

        public FileListViewerVM()
        {
            klarfPaths = new ObservableCollection<KlarfFile>();
            OpenFolderCommand = new RelayCommand(OpenFolderDialog);
        }
        #endregion

        #region 메서드
        private void OpenFolderDialog()
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SelectedFolderPath = folderDialog.SelectedPath;
                    LoadFileList(SelectedFolderPath);
                }
            }
        }

        private void LoadFileList(string folderPath)
        {
            string[] klarfExtensions = { ".001", ".tif" };

            if (Directory.Exists(folderPath))
            {
                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (klarfExtensions.Contains(extension))
                    {
                        DateTime updateTime = File.GetLastWriteTime(filePath);
                        klarfPaths.Add(new KlarfFile { Name = Path.GetFileName(filePath), Path = filePath, UpdateTime = updateTime });
                    }
                }
            }
        }

        public void LoadFromFile(string filePath)
        {
            List<Defect> Defects;
            List<Die> Dies;
            Wafer WaferInfo = new Wafer();

            Defects = new List<Defect>();
            Dies = new List<Die>();
            bool inDefectListSection = false;
            bool inSampleTestPlanSection = false;
            string[] lines = File.ReadAllLines(filePath); // Use 'filePath' here instead of 'klarfFilePath'

            foreach (string line in lines)
            {
                if (line.StartsWith("FileTimestamp"))
                {
                    // 파일 타임스탬프 정보 추출
                    string[] parts = line.Split(' ');
                    string date = parts[1];
                    string time = parts[2];
                    string fileTimestamp = $"{date} {time}";
                    // 파일 타임스탬프 정보로 Wafer 객체 초기화
                    WaferInfo.FileTimestamp = parts[0] + ": " + fileTimestamp;
                }
                else if (line.StartsWith("LotID"))
                {
                    // LotID 정보 추출
                    string[] parts = line.Split(' ');
                    string lotID = parts[1].Trim('"'); // 따옴표 제거
                    lotID = parts[1].Trim(';'); 
                    // LotID 정보로 Wafer 객체 초기화
                    WaferInfo.LotID = parts[0] + ": " + lotID;
                }
                else if (line.StartsWith("WaferID"))
                {
                    // WaferID 정보 추출
                    string[] parts = line.Split(' ');
                    string waferID = parts[1].Trim('"'); // 따옴표 제거
                    waferID = parts[1].Trim(';');
                    // WaferID 정보로 Wafer 객체 초기화
                    WaferInfo.WaferID = parts[0] + ": " + waferID;
                }
                
                if (line.StartsWith("SampleTestPlan"))
                {

                    inSampleTestPlanSection = true;
                }
                else if (inSampleTestPlanSection && line.Contains(";"))
                {
                    inSampleTestPlanSection = false;
                }
                else if (inSampleTestPlanSection)
                {
                    string[] parts = line.Split(' ');
                    int xValue = int.Parse(parts[0]);
                    int yValue = int.Parse(parts[1]);

                    Die die = new Die(xValue, yValue);
                    Dies.Add(die);  // Die 객체를 Dies ObservableCollection에 추가
                }

                if (line.StartsWith("DefectList"))
                {
                    inDefectListSection = true;
                    continue;
                }
                else if (inDefectListSection && line.Contains(";"))
                {
                    inDefectListSection = false;
                    break;
                }
                else if (inDefectListSection)
                {
                    string[] parts = line.Split(' ');

                    Defect defect = new Defect(
                        int.Parse(parts[0]),
                        double.Parse(parts[1]),
                        double.Parse(parts[2]),
                        int.Parse(parts[3]),
                        int.Parse(parts[4]),
                        int.Parse(parts[5]),
                        int.Parse(parts[6]),
                        int.Parse(parts[7]),
                        int.Parse(parts[8]),
                        int.Parse(parts[9]),
                        int.Parse(parts[10]),
                        int.Parse(parts[11]),
                        int.Parse(parts[12]),
                        int.Parse(parts[13]),
                        int.Parse(parts[14]),
                        int.Parse(parts[15]),
                        int.Parse(parts[16])
                    );
                    Defects.Add(defect);
                }
            }
            foreach (Defect defect in Defects)
            {
                int xIndex = defect.XIndex; // Defect의 xindex 가져오기
                int yIndex = defect.YIndex; // Defect의 yindex 가져오기

                // Dies에서 xindex와 yindex가 일치하는 Die를 찾기
                Die matchingDie = Dies.FirstOrDefault(die => die.X == xIndex && die.Y == yIndex);

                if (matchingDie != null)
                {
                    matchingDie.AddDefect(defect); // 해당 Die의 Defect 리스트에 Defect 추가
                }
            }

            WaferInfo.setDies(Dies);
            SharedData.Instance.Wafer = WaferInfo;
            
        }

        #endregion
    }
}
