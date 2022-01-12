using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace WebSockies.Data
{
    public class Quiz
    {
        public ObjectId _id { get; set; }
        public List<Question> Questions { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}