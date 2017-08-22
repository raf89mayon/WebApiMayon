using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCityWebApp.Models
{
    public class User
    {
        [Key]
        public String ID { get; set; }
        [Required]
        public String PassWord { get; set; }
        [Required]
        public String FirstName { get; set; }
        [Required]
        public String LastName { get; set; }
        public String Number { get; set; }
        public int PostBox { get; set; }
        public int ZipCode { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        [EmailAddress]
        public String EmailAddress { get; set; }
        [Url]
        public String Picture { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastSignInDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}