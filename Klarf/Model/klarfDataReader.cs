using System;
using System.IO;
using System.Collections.ObjectModel;
namespace Klarf.Model
{
    class KlarfDataReader
    { 
       
        public ObservableCollection<Defect> Defects { get; private set; }
        public ObservableCollection<Die> Dies { get; private set; }

        public KlarfDataReader(string klarfFilePath)
        {
            Defects = new ObservableCollection<Defect>();
            Dies = new ObservableCollection<Die>();
            bool inDefectListSection = false;
            bool inSampleTestPlanSection = false;
            string[] lines = File.ReadAllLines(klarfFilePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("SampleTestPlan"))
                {
                   
                    inSampleTestPlanSection = true;
                }
                else if (inSampleTestPlanSection && line.Contains(";"))
                {
                    inSampleTestPlanSection = false;
                }
                else if (inSampleTestPlanSection)
                {
                    string[] parts = line.Split(' ');
                    int xValue = int.Parse(parts[0]);
                    int yValue = int.Parse(parts[1]);

                    Die die = new Die(xValue, yValue);
                    Dies.Add(die);  // Die 객체를 Dies ObservableCollection에 추가
                }

                if (line.StartsWith("DefectList"))
                {
                    inDefectListSection = true;
                    continue;
                }
                else if (inDefectListSection && line.Contains(";"))
                {
                    inDefectListSection = false;
                    break;
                }
                else if (inDefectListSection)
                {
                    Defect defect = new Defect(line);
                    Defects.Add(defect);
                    
                }
            }
        }

        


    }
}
