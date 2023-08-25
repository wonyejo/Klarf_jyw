using System;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Klarf.Model
{
    class tifDataReader
    {
        public ObservableCollection<BitmapImage> TifImgs { get; private set; }

        public tifDataReader(string tifFilePath)
        {
            // TIF 이미지들을 저장하기 위한 ObservableCollection
            TifImgs = new ObservableCollection<BitmapImage>();
            // 지정된 파일에서 TIF 이미지들을 불러옴
            LoadTifImages(tifFilePath);
        }

        private void LoadTifImages(string tifFilePath)
        {
            try
            {
                // FileStream을 사용하여 TIF 파일 열기
                using (FileStream stream = new FileStream(tifFilePath, FileMode.Open, FileAccess.Read))
                {
                    // TiffBitmapDecoder를 생성하여 TIF 파일 디코딩
                    TiffBitmapDecoder decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                    // TIF 파일의 각 프레임(페이지)에 대해 반복
                    foreach (BitmapFrame frame in decoder.Frames)
                    {
                        // 변환된 이미지를 저장할 새로운 BitmapImage 생성
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                        // MemoryStream을 사용하여 BitmapFrame을 PNG 형식으로 변환
                        MemoryStream memoryStream = new MemoryStream();
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(frame);
                        encoder.Save(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        // MemoryStream을 BitmapImage의 소스로 할당
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();

                        // 불러온 BitmapImage를 ObservableCollection에 추가
                        TifImgs.Add(bitmapImage);
                    }
                }
            }
            catch (Exception ex)
            {
                // 예외 처리
                MessageBox.Show($"TIF 이미지 불러오기 오류: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
