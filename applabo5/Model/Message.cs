using System;

namespace HomeSnailHome.Model
{
    public class Message
    {
        public int ID { get; set; }
        public User Sender { get; set; }
        public User Reciever { get; set; }
        public DateTime SendDate { get; set; }
        public String Content { get; set; }
        public Housing Housing { get; set; }
    }
}
