using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int MinDelay;
        public int MaxDelay;
        public WalkPoint(int x, int y, bool kill,int minDelay, int maxDelay)
        {
            X = x;
            Y = y;
            Kill = kill;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }
    }
}
