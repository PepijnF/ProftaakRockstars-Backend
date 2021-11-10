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
            Questions.Add(new Question()
            {
                QuestionString = "Hoeveel commits heeft Ebe vandaag gepushed?",
                Id = "test",
                Answers = new List<Answer>() { new Answer() { IsCorrect = true, QuestionId = "test", AnswerString = "Te veel" } }

            });
            Questions.Add(new Question()
            {
                QuestionString = "Hoeveel commits heeft Ebe vandaag gepushed?",
                Id = "test1",
                Answers = new List<Answer>() { new Answer() { IsCorrect = true, QuestionId = "test1", AnswerString = "Te veel" } }

            });
            Questions.Add(new Question()
            {
                QuestionString = "Hoeveel commits heeft Ebe vandaag gepushed?",
                Id = "test2",
                Answers = new List<Answer>() { new Answer() { IsCorrect = true, QuestionId = "test2", AnswerString = "Te veel" } }

            });


        }
        public List<Question> Questions = new List<Question>();
        
    }
}
