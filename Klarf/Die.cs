using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Klarf.Model
{
    class Die
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsDefectInDie { get; set; } // 색상 필드 추가

        public Die(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
