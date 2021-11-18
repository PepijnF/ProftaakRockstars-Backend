using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockies.Data
{
    public class Question
    {
        public string Id;
        public string QuestionString;
        public List<Answer> Answers;
        public DateTime TimeStarted;
        public bool Answered;
    }
}
