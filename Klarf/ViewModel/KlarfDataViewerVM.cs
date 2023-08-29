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
        private ObservableCollection<Defect> defects;
        private Defect selectedDefect;
        private Die SelectedDie;
        private int currentDieIndex;
        private int currentDefectIndex;
        private int currentDieDefectIndex;

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

        public KlarfDataViewerVM()
        {
            defects = new ObservableCollection<Defect>();
        }


        private void LoadWaferData(Wafer loadedWafer)
        {
            wafer = loadedWafer;
        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                LoadWaferData(SharedData.Instance.Wafer);
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
