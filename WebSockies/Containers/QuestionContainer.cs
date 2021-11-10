using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockies.Data;

namespace WebSockies.Containers
{
    public class QuestionContainer
    {
        public QuestionContainer() {
            Questions = new List<Question>();
        }
        public List<Question> Questions = new List<Question>();
    }
}
