using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using WebSockies.Data;
using MongoDB.Bson.IO;

namespace WebSockies.Db
{
    public class DbConnection
    {
        MongoClient _dbClient;
        IMongoDatabase _db;
        public DbConnection() {
            _dbClient = new MongoClient("mongodb://127.0.0.1:27017");
            _db = _dbClient.GetDatabase("Proftaak");
        }
        public Quiz GetQuiz(string quizName) {
            var Quizzes = _db.GetCollection<Quiz>("Quizzes");

            var filter = Builders<Quiz>.Filter.Eq("Name", quizName);
            var SelectedQuiz = Quizzes.Find(filter).FirstOrDefault();

            return SelectedQuiz;
        }

        public Quiz GetRandomQuiz()
        {
            var quizzes = _db.GetCollection<Quiz>("Quizzes");
            var selectedQuiz = quizzes.Find(Builders<Quiz>.Filter.Empty).ToList();

            var rand = new Random();

            return selectedQuiz[rand.Next(0, 1)];
        }
        

    }
}
