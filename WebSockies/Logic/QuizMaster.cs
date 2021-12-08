using System;
using System.Collections.Generic;
using WebSockies.Data;

namespace WebSockies.Logic
{
    public class QuizMaster
    {
        private static Question CreateQuestion()
        {
            List<Answer> answers = new List<Answer>();
            answers.Add(new Answer(){AnswerString = "Ja", IsCorrect = true, QuestionId = "123"});
            answers.Add(new Answer(){AnswerString = "Nee", IsCorrect = false, QuestionId = "123"});

            return new Question()
                { Answered = false, Id = "123", QuestionString = "Werkt t", TimeStarted = new DateTime(), Answers = answers};
        }
        
        public static Quiz GetQuiz()
        {
            return new Quiz()
                { Description = "Test", Name = "test", Questions = new List<Question>() { CreateQuestion() } };
        }
    }
}