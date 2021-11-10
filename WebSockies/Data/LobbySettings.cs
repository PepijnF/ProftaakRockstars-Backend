using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockies.Data
{
    public class LobbySettings
    {
        public LobbySettings()
        {
            TimePerQuestion = 10;
        }
        
        public List<Question> Questions;
        public int TimePerQuestion;
        

    }
}
