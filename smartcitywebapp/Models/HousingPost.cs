using System;

namespace SmartCityWebApp.Models
{
    public class HousingPost
    {
        public int PostBox { get; set; }
        public int ZipCode { get; set; }
        public int BedType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Number { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String SpaceLocalization { get; set; }
        public String Description { get; set; }
        public String Pictures { get; set; }
        public Boolean Wifi { get; set; }
        public Boolean Kitchen { get; set; }
        public Boolean Office { get; set; }
        public Boolean Toilet { get; set; }
        public Boolean Shower { get; set; }
    }
}
