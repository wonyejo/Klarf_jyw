using Klarf.View;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;

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
                    LoadTiff();
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
                    LoadTiff();
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
                    LoadTiff();
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


        public DefectImgViewerVM()
        {
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

                LoadTiff(); // 이미지 로드 메서드 호출
            }
        }
       

        private void LoadTiff()
        {
            if (!Directory.Exists(receivedFolderPath))
                return;

            var tifFiles = Directory.GetFiles(receivedFolderPath, "*.tif");
            if (tifFiles.Length == 0)
                return;

            var tifFile = tifFiles[0];
            tiffDecoder = new TiffBitmapDecoder(new Uri(tifFile, UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            SharedData.Instance.tiffDecoder = tiffDecoder;
            if (tiffDecoder.Frames.Count > 0)
            {
                LoadImg();
  
            }
        }

        private void LoadImg()
        {
            CurDefectImg = tiffDecoder.Frames[CurDefectID];
        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefectID")
            {
                CurDefectID = SharedData.Instance.DefectID;
            }
          
            else if (e.PropertyName == "FolderPath")
            {
                ReceivedFolderPath = SharedData.Instance.FolderPath;
            }
            else if(e.PropertyName== "CurDefectImg")
            {
                CurDefectImg = SharedData.Instance.CurDefectImg;
            }
           
        }
    }
}