using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSockies.Data
{
    public class Answer
    {
        public string QuestionId { get; set; }
        public string AnswerString { get; set; }
        public bool IsCorrect { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}