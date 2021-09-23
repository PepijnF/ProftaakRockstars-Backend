using System.Collections.Generic;

namespace Proftaak.Data
{
    public class Question
    {
        public string Id { get; set; }
        public string QuestionString { get; set; }
        public List<Answer> Answers { get; set; }

        public Question(string questionString)
        {
            QuestionString = questionString;
            Answers = new List<Answer>();
        }
    }
}