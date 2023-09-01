using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Klarf.ViewModel
{
    class WaferMapViewerVM : ViewModelBase
    {
        public Wafer wafer;

        public Wafer Wafer
        {
            get { return wafer; }
            set
            {
                wafer = value;
                OnPropertyChanged(nameof(Wafer));

                if (wafer != null)
                {
                    DrawWaferMap(wafer);
                }
            }

        }


        public ObservableCollection<Shape> WaferMapShapes { get; set; } // Shapes 컬렉션을 추가

        public WaferMapViewerVM()
        {
            WaferMapShapes = new ObservableCollection<Shape>();

            if (Wafer != null)
            {
                LoadWaferData(SharedData.Instance.Wafer);
                DrawWaferMap(Wafer);
            }

            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;

            //defects = new ObservableCollection<Defect>();
        }

        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;

        }
        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                Wafer = SharedData.Instance.Wafer;
            }

        }

        private void DrawWaferMap(Wafer wafer)
        {
            WaferMapShapes.Clear(); // 기존의 Shapes 제거

            double minX = wafer.Dies.Min(die => die.X);
            double maxY = wafer.Dies.Max(die => die.Y);

            foreach (var die in wafer.Dies)
            {
                var rect = new Rectangle();
                double cellWidth = 380.0 / wafer.GridWidth; // 화면의 크기를 웨이퍼 그리드 값으로 나눔
                double cellHeight = 380.0 / wafer.GridHeight;
                rect.Width = cellWidth;
                rect.Height = cellHeight;
                rect.Stroke = Brushes.Gray;
                rect.StrokeThickness = 1;

                if (die.IsDefectInDie)
                {
                    rect.Fill = Brushes.Red;
                }
                else
                {
                    rect.Fill = Brushes.Gray;
                }

                // die의 좌표를 상대좌표로 변환하여 적용
                double x = (die.X - minX) * cellWidth;
                double y = (maxY - die.Y) * cellHeight;

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                WaferMapShapes.Add(rect); // Shapes 컬렉션에 Rectangle 추가
            }
        }
    }
}
