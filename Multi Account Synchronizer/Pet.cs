using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Account_Synchronizer
{
    internal class Pet
    {
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
        public int Id { get; set; } = -1;
        public Dictionary<int, bool> Skills = new Dictionary<int, bool>();
        public Pet()
        {

        }
    }
}
