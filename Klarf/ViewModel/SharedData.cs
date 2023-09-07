using Klarf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Klarf.ViewModel
{
    public class SharedData : INotifyPropertyChanged
    {
    
        #region 필드
        private static SharedData instance = null;
        private Wafer wafer;
        private string folderPath;
        private bool defectShowData;
        private int defectID;
        private List<Defect> defects;
        private BitmapSource curDefectImg;
        public TiffBitmapDecoder tiffDecoder;

        #endregion

        #region 속성

        public BitmapSource CurDefectImg
        {
            get { return curDefectImg; }
            set
            {
                curDefectImg = value;
                OnPropertyChanged(nameof(CurDefectImg));
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

        public List<Defect> Defects
        {
            get { return defects; }
            set
            {
                defects = value;
                OnPropertyChanged("Defects");
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

        public int DefectID
        {
            get { return defectID; }
            set
            {
                defectID = value;
                OnPropertyChanged("DefectID");
            }
        }
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
        #endregion

        #region 메서드

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}