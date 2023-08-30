using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM : ViewModelBase
    {

        public Wafer wafer;
        public List<Defect> defects;
    
       
        private int currentDieIndex;
        private int currentDefectIndex;
        private int currentDieDefectIndex;
        private Defect selectedDefect;
        private Die selectedDie;


        public KlarfDataViewerVM()
        {
            LoadWaferData(SharedData.Instance.Wafer);
            LoadDefectData(SharedData.Instance.Defects);
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
            
            defects = new List<Defect>();
          
        }
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
        public List<Defect> Defects
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
        public int CurrentDefectIndex
        {
            get { return currentDefectIndex; }
            set
            {
                if (currentDefectIndex != value)
                {
                    currentDefectIndex = value;
                    OnPropertyChanged(nameof(CurrentDefectIndex));
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
            get { return defects.Count; }
        }
        public int TotalDies
        {
            get { return wafer?.Dies?.Count ?? 0; }
        }
        public int TotalDieDefects
        {
            get { return SelectedDie?.Defects?.Count ?? 0; }
        }

        private void PreviousDefect()
        {
            if (CurrentDefectIndex > 0)
            {
                CurrentDefectIndex--;
                selectedDefect = defects[CurrentDefectIndex];
                SharedData.Instance.DefectIndexData = currentDefectIndex;
            }
        }


        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;

        }
         private void LoadDefectData(List<Defect> Defects)
        {
                defects = Defects;
            
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
                return $"{CurrentDefectIndex + 1}/{TotalDefects}";
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

    }
}
