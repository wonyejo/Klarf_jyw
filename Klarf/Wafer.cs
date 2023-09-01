using System;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace Klarf.Model
{
    public class Wafer
    {


        public string FileTimestamp;
        public string LotID;
        public string WaferID;
        public Tuple<int, int> GridCoordinate { get; set; }
         public int GridWidth
        {
            get
            {
                if (Dies == null || Dies.Count == 0)
                    return 0;

                int maxX = Dies.Max(die => die.X);
                int minX = Dies.Min(die => die.X);

                return maxX - minX + 1;
            }
        }

        public int GridHeight
        {
            get
            {
                if (Dies == null || Dies.Count == 0)
                    return 0;

                int maxY = Dies.Max(die => die.Y);
                int minY = Dies.Min(die => die.Y);

                return maxY - minY + 1;
            }
        }
        public List<Die> Dies { get; set; }
  
        public Wafer()
        {

        }
        public Wafer(string fileTimestamp, string lotID, string waferID)
        {
            FileTimestamp = fileTimestamp;
            LotID = lotID;
            WaferID = waferID;
           
        }

        public void setDies(List<Die> dies)
        {
            Dies = dies;
        }

       


    }
}
