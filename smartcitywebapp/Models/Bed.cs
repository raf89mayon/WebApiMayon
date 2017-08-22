using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCityWebApp.Models
{
  
    public class Bed
    {
        [Key]
        public int Code { get; set; }
        [Required]
        public String Name { get; set; }
    }
}