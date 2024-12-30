using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class WalkPoint
    {
        public int X;
        public int Y;
        public bool Kill;
        public WalkPoint(int x, int y, bool kill)
        {
            X = x;
            Y = y;
            Kill = kill;
        }
    }
}
