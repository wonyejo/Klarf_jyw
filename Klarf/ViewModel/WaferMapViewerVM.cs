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
    class WaferMapViewerVM: ViewModelBase
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

       private void DrawWaferMap(Wafer wafer) {
            WaferMapShapes.Clear(); // 기존의 Shapes 제거

            foreach (var die in wafer.Dies)
            {
                var rect = new Rectangle();
                rect.Width = 10;
                rect.Height = 10;
                rect.Stroke = Brushes.Black; 
                rect.StrokeThickness = 1;

                if (die.IsDefectInDie)
                {
                    rect.Fill = Brushes.Red;
                }
                else
                {
                    rect.Fill = Brushes.Gray;
                }

                double x = die.X + 9;
                double y = Math.Abs(die.Y - 24);
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                WaferMapShapes.Add(rect); // Shapes 컬렉션에 Rectangle 추가
            }
        }
    }
}
