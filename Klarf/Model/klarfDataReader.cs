using System;
using System.IO;
using System.Collections.ObjectModel;
namespace Klarf.Model
{
    class KlarfDataReader
    { 
       
        public ObservableCollection<Defect> Defects { get; private set; }
        public ObservableCollection<Die> Dies { get; private set; }
        public Wafer WaferInfo { get; private set; }

        public KlarfDataReader(string klarfFilePath)
        {
            Defects = new ObservableCollection<Defect>();
            Dies = new ObservableCollection<Die>();
            bool inDefectListSection = false;
            bool inSampleTestPlanSection = false;
            string[] lines = File.ReadAllLines(klarfFilePath);

            foreach (string line in lines)
            {

               if (line.StartsWith("FileTimestamp"))
                {
                    // 파일 타임스탬프 정보 추출
                    string[] parts = line.Split(' ');
                    string date = parts[1];
                    string time = parts[2];
                    string fileTimestamp = $"{date} {time}";
                    // 파일 타임스탬프 정보로 Wafer 객체 초기화
                    WaferInfo.FileTimestamp = fileTimestamp;
                }
                else if (line.StartsWith("LotID"))
                {
                    // LotID 정보 추출
                    string[] parts = line.Split(' ');
                    string lotID = parts[1].Trim('"'); // 따옴표 제거
                    // LotID 정보로 Wafer 객체 초기화
                    WaferInfo.LotID = lotID;
                }
                else if (line.StartsWith("WaferID"))
                {
                    // WaferID 정보 추출
                    string[] parts = line.Split(' ');
                    string waferID = parts[1].Trim('"'); // 따옴표 제거
                    // WaferID 정보로 Wafer 객체 초기화
                    WaferInfo.WaferID = waferID;
                }

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
                  
                   
                }
            }
        }

        


    }
}
