using System.Collections.Generic;

namespace HomeSnailHome.Model
{
    public class Conversation
    {
        public User Correspondant { get; set; }
        public List<Message> Messages { get; set; }
        public Housing Subject { get; set; }
    }
}
