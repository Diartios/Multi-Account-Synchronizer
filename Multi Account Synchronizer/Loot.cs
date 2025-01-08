﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class Loot
    {
        public int Vnum;
        public int Id;
        public Point Pos;
        public int Quantity;
        public int Owner;
        public DateTime Time;

        public Loot(int vnum, int id, Point pos, int quantity, int owner, DateTime time)
        {
            this.Vnum = vnum;
            this.Id = id;
            this.Pos = pos;
            this.Quantity = quantity;
            this.Owner = owner;
            this.Time = time;
        }

    }
}
