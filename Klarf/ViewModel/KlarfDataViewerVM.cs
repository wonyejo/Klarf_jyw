using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Klarf.Model;
using System.Linq;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM : ViewModelBase
    {
        #region 필드
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
        #endregion

        #region 생성자
        public KlarfDataViewerVM()
        {
            defects = new ObservableCollection<Defect>();
            LoadWaferData(SharedData.Instance.Wafer);
            if (Wafer != null) LoadDefectData(SharedData.Instance.Defects);
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
            GoPrevDefectCommand = new RelayCommand(GoPrevDefect);
            GoNextDefectCommand = new RelayCommand(GoNextDefect);
        }
        #endregion

        #region 속성
        public string TimeStamp
        {
            get { return timeStamp; }
            set
            {
                if (timeStamp != value)
                {
                    timeStamp = value;
                    OnPropertyChanged(nameof(TimeStamp));
                }
            }
        }

        public string WaferID
        {
            get { return waferID; }
            set
            {
                if (waferID != value)
                {
                    waferID = value;
                    OnPropertyChanged(nameof(WaferID));
                }
            }
        }

        public string LotID
        {
            get { return lotID; }
            set
            {
                if (lotID != value)
                {
                    lotID = value;
                    OnPropertyChanged(nameof(LotID));
                }
            }
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
        #endregion

        #region 메서드
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

                    SharedData.Instance.DefectID = SelectedDefect.DefectId;
                    CurDefectID = SelectedDefect.DefectId;
                    CurDefectImg = SharedData.Instance.tiffDecoder.Frames[CurDefectID];
                    OnPropertyChanged(nameof(DieList));
                    OnPropertyChanged(nameof(DieDefectList));
                    OnPropertyChanged(nameof(DefectList));
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
                    OnPropertyChanged(nameof(DieList));
                    OnPropertyChanged(nameof(DieDefectList));
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
                    OnPropertyChanged(nameof(DefectList));
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
      
                TimeStamp = wafer.FileTimestamp;
                WaferID = wafer.WaferID;
                LotID = wafer.LotID;
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
                return $"{CurDefectID}/{TotalDefects}";
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
            if (e.PropertyName == "Defects")
            {
                LoadDefectData(SharedData.Instance.Defects);
            }
            if (e.PropertyName == "tiffDecoder")
            {
                LoadTiffData(SharedData.Instance.tiffDecoder);
            }
            if (e.PropertyName == "curDefectImg")
            {
                SharedData.Instance.CurDefectImg = curDefectImg;
            }
            if (e.PropertyName == "defectID")
            {
                SharedData.Instance.DefectID = CurDefectID;
            }
        }
        #endregion
    }
}
