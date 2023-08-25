using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Klarf.ViewModel
{
    class DefectImgViewerVM : INotifyPropertyChanged
    {
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
