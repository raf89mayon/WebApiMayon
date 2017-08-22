using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCityWebApp.Models
{
    public class Locality
    {
        [Key]
        public String Name { get; set; }
        [Required]
        public int Zip { get; set; }
    }
}