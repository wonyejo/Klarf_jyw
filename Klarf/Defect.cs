using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klarf.Model
{
    public class Defect
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
public Defect(int defectId, double xRel, double yRel, int xIndex, int yIndex, int xSize, int ySize, int defectArea, int dSize, int classNumber, int test, int clusterNumber, int roughBinNumber, int fineBinNumber, int reviewSample, int imageCount, int imageList)
{
    DefectId = defectId;
    XRel = xRel;
    YRel = yRel;
    XIndex = xIndex;
    YIndex = yIndex;
    XSize = xSize;
    YSize = ySize;
    DefectArea = defectArea;
    DSize = dSize;
    ClassNumber = classNumber;
    Test = test;
    ClusterNumber = clusterNumber;
    RoughBinNumber = roughBinNumber;
    FineBinNumber = fineBinNumber;
    ReviewSample = reviewSample;
    ImageCount = imageCount;
    ImageList = imageList;
}

    }
}
