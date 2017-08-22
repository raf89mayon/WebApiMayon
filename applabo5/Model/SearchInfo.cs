using System;

namespace HomeSnailHome.Model
{
    public class SearchInfo
    {
        public Boolean Wifi { get; set; }
        public Boolean Shower { get; set; }
        public Boolean Kitchen { get; set; }
        public Boolean Toilet { get; set; }
        public Boolean Office { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int BedType { get; set; }
    }
}
