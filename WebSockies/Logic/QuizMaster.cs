using System;
using System.Collections.Generic;
using WebSockies.Data;
using WebSockies.Db;

namespace WebSockies.Logic
{
    public class QuizMaster
    {
        private static Question CreateQuestion()
        {
            List<Answer> answers = new List<Answer>();
            answers.Add(new Answer(){AnswerString = "Ja", IsCorrect = true});
            answers.Add(new Answer(){AnswerString = "Nee", IsCorrect = false});
            answers.Add(new Answer(){AnswerString = "Misschien", IsCorrect = false});
            answers.Add(new Answer(){AnswerString = "Beetje", IsCorrect = false});

            return new Question()
                { Answered = false, QuestionString = "Werkt t", TimeStarted = new DateTime(), Answers = answers};
        }
        
        public static Quiz GetQuiz()
        {
            var dbConnection = new DbConnection();
            return dbConnection.GetRandomQuiz();
        }
    }
}