using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCityWebApp.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public String Title { get; set; }
        public Boolean UserRight { get; set; }
        public Boolean HousingRight { get; set; }
    }
}
