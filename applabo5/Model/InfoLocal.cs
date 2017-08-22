using System;

namespace HomeSnailHome.Model
{
    public class InfoLocal
    {
        public String Status { get; set; }

        public double location_lat { get; set; }
        public double location_lon { get; set; }

        public double northeast_lat { get; set; }
        public double northeast_lon { get; set; }

        public double southwest_lat { get; set; }
        public double southwest_lon { get; set; }
    }
}
