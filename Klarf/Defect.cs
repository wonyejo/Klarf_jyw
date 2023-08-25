using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarf.Model
{
    class Defect
    {
        public int DefectId { get; set; }
        public double XRel { get; set; }
        public double YRel { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int DefectArea { get; set; }
        public int DSize { get; set; }
        public int ClassNumber { get; set; }
        public int Test { get; set; }
     
        public int ClusterNumber { get; set; }
        public int RoughBinNumber { get; set; }
        public int FineBinNumber { get; set; }
        public int ReviewSample { get; set; }
        public int ImageCount { get; set; }
        public int ImageList { get; set; }

        // 생성자로 데이터를 초기화하는 메서드
        public Defect(string line)
        {
            string[] parts = line.Split(' ');
            DefectId = int.Parse(parts[0]);
            XRel = double.Parse(parts[1]);
            YRel = double.Parse(parts[2]);
            XIndex = int.Parse(parts[3]);
            YIndex = int.Parse(parts[4]);
            XSize = int.Parse(parts[5]);
            YSize = int.Parse(parts[6]);
            DefectArea = int.Parse(parts[7]);
            DSize = int.Parse(parts[8]);
            ClassNumber = int.Parse(parts[9]);
            Test = int.Parse(parts[10]);
            ClusterNumber = int.Parse(parts[11]);
            RoughBinNumber = int.Parse(parts[12]);
            FineBinNumber = int.Parse(parts[13]);
            ReviewSample = int.Parse(parts[14]);
            ImageCount = int.Parse(parts[15]);
            ImageList = int.Parse(parts[16]);
       
        }
    }
}
