using System.Collections.Generic;

namespace WebSockies.Data
{
    public class Quiz
    {
        public List<Question> Questions { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}