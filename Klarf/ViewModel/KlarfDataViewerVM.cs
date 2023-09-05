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
      
        private int currentDieIndex;
        private int curDefectID;
        private int currentDieDefectIndex;
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
                SharedData.Instance.DefectIndex = CurDefectID;
                CurDefectImg = SharedData.Instance.tiffDecoder.Frames[CurDefectID];
            }
           
        }
        public void GoNextDefect()
        {
            if (CurDefectID < TotalDefects - 1)
            {
                CurDefectID++;
                SelectedDefect = Defects[CurDefectID];
                SharedData.Instance.DefectIndex = CurDefectID;
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
                            CurrentDieIndex = i;
                            CurrentDieDefectIndex = currentDie.Defects.IndexOf(selectedDefect);
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

        public int CurrentDieDefectIndex
        {
            get { return currentDieDefectIndex; }
            set
            {
                if (currentDieDefectIndex != value)
                {
                    currentDieDefectIndex = value;
                    OnPropertyChanged(nameof(CurrentDieDefectIndex));
                    OnPropertyChanged(nameof(DieDefectList));
                }
            }
        }
       

        public int CurrentDieIndex
        {
            get { return currentDieIndex; }
            set
            {
                if (currentDieIndex != value)
                {
                    currentDieIndex = value;
                    OnPropertyChanged(nameof(currentDieIndex));
                    OnPropertyChanged(nameof(DieList));
                    SelectedDie = wafer.Dies[currentDieIndex];

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
                return $"{CurrentDieIndex + 1}/{TotalDies}";
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
                    return $"{CurrentDieDefectIndex + 1}/{TotalDieDefects}";
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
