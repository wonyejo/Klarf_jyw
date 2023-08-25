using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klarf.Model;
using System.Collections.ObjectModel;
using Prism.Events;

namespace Klarf.ViewModel
{
    class KlarfDataViewerVM: ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public ObservableCollection<Defect> Defects { get; } = new ObservableCollection<Defect>();

        public KlarfDataViewerVM()
        {
            
        }
        public KlarfDataViewerVM(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<FileSelectedEvent>().Subscribe(OnFileSelected);
        }

        public KlarfDataViewerVM(ObservableCollection<Defect> defects)
        {
            Defects = defects;
        }

        private void OnFileSelected(string selectedFilePath)
        {
            KlarfDataReader klarfDataReader = new KlarfDataReader(selectedFilePath);

            // 이곳에서 선택한 파일의 경로를 사용하여 데이터를 처리
            // 예: KlarfDataReader 생성 등
        }
    }
}
