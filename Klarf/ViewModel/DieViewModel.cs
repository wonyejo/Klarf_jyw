using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;

namespace Klarf.ViewModel
{
    public class DieViewModel : ViewModelBase
    {
        #region[필드]
        private Die die;
        private bool isSelected;
        #endregion
        #region[속성]


        public Die Die
        {
            get => die;
            set
            {
                die = value;
                OnPropertyChanged(nameof(Die));
            }
        }
        public Tuple<int, int> GridCoordinate
        {
            get
            {
                if (Die != null)
                {
                    return Die.GridCoordinate;
                }
                return null;
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        #endregion





    }
}
