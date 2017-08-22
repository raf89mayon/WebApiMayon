﻿using System;

namespace HomeSnailHome.Model
{
    public class UserPost
    {
        public String ID { get; set; }
/**/    public String PassWord { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Number { get; set; }
        public int PostBox { get; set; }
        public int ZipCode { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
/**/    public String EmailAddress { get; set; }
        public String Picture { get; set; }
        public int RoleID { get; set; }
    }
}