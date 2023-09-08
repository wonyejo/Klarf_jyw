using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Klarf.ViewModel
{
    class WaferMapViewerVM : ViewModelBase
    {
        #region 필드
        public Wafer wafer;
        public DieViewModel selectedDie;
        private ObservableCollection<Defect> defects;
        private Rectangle selectedDefectOutline;
        private int curDefectID;
        #endregion

        #region 속성
        public Rectangle SelectedDefectOutline
        {
            get { return selectedDefectOutline; }
            set
            {
                selectedDefectOutline = value;
                OnPropertyChanged(nameof(SelectedDefectOutline));
            }
        }

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

        public int CurDefectID
        {
            get { return curDefectID; }
            set
            {
                if (curDefectID != value)
                {
                    curDefectID = value;
                    OnPropertyChanged(nameof(CurDefectID));
                    DrawSelectedDefectOutline(CurDefectID);
                }
            }
        }

        public DieViewModel SelectedDie
        {
            get => selectedDie;
            set
            {
                if (selectedDie != value)
                {
                    if (selectedDie != null)
                        selectedDie.IsSelected = false;

                    selectedDie = value;

                    if (selectedDie != null)
                        selectedDie.IsSelected = true;

                    OnPropertyChanged(nameof(SelectedDie));
                }
            }
        }

        public ObservableCollection<Shape> WaferMapShapes { get; set; } // Shapes 컬렉션을 추가
        public ObservableCollection<Defect> Defects
        {
            get => defects;
            set
            {
                defects = value;
                OnPropertyChanged(nameof(Defects));
            }
        }
        #endregion

        #region 생성자
        public WaferMapViewerVM()
        {
            WaferMapShapes = new ObservableCollection<Shape>();

            if (Wafer != null)
            {
                LoadWaferData(SharedData.Instance.Wafer);
                DrawWaferMap(Wafer);
                LoadDefectData(SharedData.Instance.Defects);
                DrawSelectedDefectOutline(CurDefectID);

            }

            SharedData.Instance.PropertyChanged += SharedData_PropertyChanged;
        }
        #endregion

        #region 메서드
        private void LoadWaferData(Wafer loadedWafer)
        {
            Wafer = loadedWafer;
        }

        private void LoadDefectData(List<Defect> Defects)
        {
            this.Defects = new ObservableCollection<Defect>(Defects);
        }

        private void DrawSelectedDefectOutline(int selectedDefectID)
        {
            double minX = wafer.Dies.Min(die => die.X);
            double maxY = wafer.Dies.Max(die => die.Y);

            Defect selectedDefect = Defects.FirstOrDefault(defect => defect.DefectId-1 == selectedDefectID);

            if (selectedDefect != null)
            {
                double cellWidth = 380.0 / wafer.GridWidth;
                double cellHeight = 380.0 / wafer.GridHeight;
                double x = (selectedDefect.XIndex - minX) * cellWidth;
                double y = (maxY - selectedDefect.YIndex) * cellHeight;

                var rect = new Rectangle();
                rect.Width = cellWidth;
                rect.Height = cellHeight;
                rect.Stroke = Brushes.Blue;
                rect.StrokeThickness = 2;

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                RemoveSelectedDefectOutline();

                WaferMapShapes.Add(rect);
                selectedDefectOutline = rect;
            }
        }

        private void RemoveSelectedDefectOutline()
        {
            if (selectedDefectOutline != null)
            {
                WaferMapShapes.Remove(selectedDefectOutline);
                selectedDefectOutline = null;
            }
        }

        private void DrawWaferMap(Wafer wafer)
        {
            WaferMapShapes.Clear();

            double minX = wafer.Dies.Min(die => die.X);
            double maxY = wafer.Dies.Max(die => die.Y);

            foreach (var die in wafer.Dies)
            {
                var rect = new Rectangle();
                double cellWidth = 380.0 / wafer.GridWidth;
                double cellHeight = 380.0 / wafer.GridHeight;
                rect.Width = cellWidth;
                rect.Height = cellHeight;
                rect.Stroke = Brushes.Gray;
                rect.StrokeThickness = 1;

                if (die.IsDefectInDie)
                {
                    rect.Fill = Brushes.Red;
                    rect.Stroke = Brushes.White;
                }
                else
                {
                    rect.Fill = Brushes.Gray;
                    rect.Stroke = Brushes.DarkGray;
                }

                double x = (die.X - minX) * cellWidth;
                double y = (maxY - die.Y) * cellHeight;

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);

                WaferMapShapes.Add(rect);
            }
        }



        private void SharedData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                LoadWaferData(SharedData.Instance.Wafer);
            }
            if (e.PropertyName == "Defects")
            {
                LoadDefectData(SharedData.Instance.Defects);
            }
            if (e.PropertyName == "DefectID")
            {
                CurDefectID = SharedData.Instance.DefectID;
            }
        }
        #endregion
    }
}
