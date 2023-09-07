using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Klarf.Model
{
    public class Die
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsDefectInDie { get; set; }
        public List<Defect> Defects { get; set; }
        public Tuple<int, int> GridCoordinate { get; set; }
        public Tuple<int, int> Coordinate { get; set; }

        public Die(int x, int y)
        {
            X = x;
            Y = y;
            Coordinate = new Tuple<int, int>(x, y);
            Defects = new List<Defect>();
        }
        public void AddDefect(Defect defect)
        {
            Defects.Add(defect);
            if (!IsDefectInDie) IsDefectInDie = true;
        }
        
    }
}
