using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public string Serialize()
        {
            List<string> answers = new List<string>();
            foreach (var answer in Answers)
            {
                answers.Add(answer.Serialize());
            }

            return JsonSerializer.Serialize(answers);
        }
    }
}
