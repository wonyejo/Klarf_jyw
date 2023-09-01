using Klarf.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using GalaSoft.MvvmLight.Messaging;
using System.IO;

namespace Klarf.ViewModel
{
    class DefectImgViewerVM : ViewModelBase
    {
        private int curDefectID;
        private BitmapSource curDefectImg;
        private TiffBitmapDecoder tiffDecoder;
        private string receivedFolderPath;
        private bool receivedDefectShow = false;

        public bool ReceivedDefectShow
        {
            get { return receivedDefectShow; }
            set
            {
                if (receivedDefectShow != value)
                {
                    receivedDefectShow = value;
                    OnPropertyChanged(nameof(receivedDefectShow));
                    LoadImage();
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
                    OnPropertyChanged(nameof(curDefectID));
                    LoadImage();
                }
            }
        }
        public string ReceivedFolderPath
        {
            get { return receivedFolderPath; }
            set
            {
                if (receivedFolderPath != value)
                {
                    receivedFolderPath = value;
                    OnPropertyChanged(nameof(ReceivedFolderPath));
                    LoadImage();
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


        public DefectImgViewerVM() {
            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        

    

        private string selectedFilePath;
        public string SelectedFilePath
        {
            get { return selectedFilePath; }
            set
            {
                selectedFilePath = value;
                OnPropertyChanged(nameof(SelectedFilePath));

                LoadImage(); // 이미지 로드 메서드 호출
            }
        }

 
        private void LoadImage()
        {
            if (!Directory.Exists(receivedFolderPath))
                return;

            var tifFiles = Directory.GetFiles(receivedFolderPath, "*.tif");
            if (tifFiles.Length == 0)
                return;

            var tifFile = tifFiles[0];
            tiffDecoder = new TiffBitmapDecoder(new Uri(tifFile, UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            if (tiffDecoder.Frames.Count > 0)
            {
                curDefectImg = tiffDecoder.Frames[CurDefectID];
            }
        }

        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefectIndexData")
            {
                CurDefectID = SharedData.Instance.DefectIndexData;
            }
            else if (e.PropertyName == "DefectShowData")
            {
                ReceivedDefectShow = SharedData.Instance.DefectShowData;
            }
            else if (e.PropertyName == "FolderPath")
            {
                ReceivedFolderPath = SharedData.Instance.FolderPath;
            }
        }
    }
}
