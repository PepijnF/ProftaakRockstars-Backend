using System;
using NUnit.Framework;
using WebSockies.Data;
using WebSockies.Db;

namespace TestProjecie
{
    public class DbTesting
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DbConnectionTest()
        {
            DbConnection dbConnection = new DbConnection();
            Quiz quiz = dbConnection.GetQuiz("Test");
            Console.WriteLine("");
        }

        [Test]
        public void RandomQuiz()
        {
            DbConnection dbConnection = new DbConnection();
            Quiz quiz = dbConnection.GetRandomQuiz();
            Console.WriteLine("");
        }
    }
}