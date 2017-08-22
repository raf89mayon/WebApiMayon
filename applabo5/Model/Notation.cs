using System;

namespace HomeSnailHome.Model
{
    public class Notation
    {
        public int ID { get; set; }
        public User Origin { get; set; }
        public Housing Housing { get; set; }
        public DateTime DateNotation { get; set; }
        public String Comment { get; set; }
        public float Quotation { get; set; }
    }
}
