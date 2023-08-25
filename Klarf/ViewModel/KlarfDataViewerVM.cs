using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM
    {
        public ObservableCollection<Defect> Defects { get; } = new ObservableCollection<Defect>();

        public KlarfDataViewerVM()
        {
            
        }

        public KlarfDataViewerVM(ObservableCollection<Defect> defects)
        {
            Defects = defects;
        }
      
    }
}
