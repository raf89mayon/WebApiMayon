using System;

namespace HomeSnailHome.Model
{
    public class Locality
    {
        public String Name { get; set; }
        public int Zip { get; set; }
        public String Display {
            get
            {
                return Zip + " - " + Name;
            }
            set
            {
                Display = value;
            }
        }
    }
}
