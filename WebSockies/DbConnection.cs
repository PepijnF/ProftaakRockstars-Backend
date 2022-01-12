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

namespace TestProjecie
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
            var Quizzes = _db.GetCollection<BsonDocument>("Quizzes");

            var filter = Builders<BsonDocument>.Filter.Eq("Quizname", quizName);
            var Quiz = Quizzes.Find(filter).FirstOrDefault();

            Quiz SelectedQuiz = BsonSerializer.Deserialize<Quiz>(Quiz);

            return SelectedQuiz;
        }
        
        
    }
}
