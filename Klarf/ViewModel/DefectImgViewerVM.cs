using Prism.Events;
using Klarf.Events;
using Klarf.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using System.Collections.Generic;

namespace Klarf.ViewModel
{
    class DefectImgViewerVM : ViewModelBase
    {
        private int CurDefectID;
        private BitmapImage curDefectImg;
        private IEventAggregator _eventAggregator;

        

        public BitmapImage CurDefectImg
        {
            get { return curDefectImg; }
            set
            {
                curDefectImg = value;
                OnPropertyChanged(nameof(curDefectImg));
            }
        }


        public DefectImgViewerVM() { 
        }
        public DefectImgViewerVM(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // FileSelectedEvent 구독
            _eventAggregator.GetEvent<FileSelectedEvent>().Subscribe(OnFileSelected);
        }

        private void OnFileSelected(string selectedFilePath)
        {
            // 선택된 파일 경로를 이용하여 tifDataReader 생성
           
            tifDataReader tifDataReader = new tifDataReader(selectedFilePath);

            // tifDataReader에서 데이터 처리 로직 추가
            CurDefectImg = tifDataReader.TifImgs[CurDefectID];
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

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private void LoadImage()
        {
            if (!string.IsNullOrEmpty(SelectedFilePath))
            {
                ImageSource = new BitmapImage(new Uri(SelectedFilePath));
            }
            else
            {
                ImageSource = null;
            }
        }
     

    }
}
