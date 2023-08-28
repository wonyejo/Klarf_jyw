using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM: ViewModelBase
    {
        public List<Defect> Defects { get; } = new List<Defect>();

        public KlarfDataViewerVM()
        {
            
        }
      
        public KlarfDataViewerVM(List<Defect> defects)
        {
            Defects = defects;
        }

      
    }
}
