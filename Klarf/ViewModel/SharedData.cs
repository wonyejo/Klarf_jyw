using Klarf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarf.ViewModel
{
    public class SharedData : INotifyPropertyChanged
    {
        private static SharedData instance = null;
        private Wafer wafer;
        private string folderPath;
        private bool defectShowData;
        private int defectIndexData;
        public event PropertyChangedEventHandler PropertyChanged;

        public static SharedData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedData();
                }

                return instance;
            }
        }

        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }
        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged("Wafer");
            }
        }
        public bool DefectShowData
        {
            get { return defectShowData; }
            set
            {
                defectShowData = value;
                OnPropertyChanged("DefectShowData");
            }
        }
        public int DefectIndexData
        {
            get { return defectIndexData; }
            set
            {
                defectIndexData = value;
                OnPropertyChanged("DefectIndexData");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}