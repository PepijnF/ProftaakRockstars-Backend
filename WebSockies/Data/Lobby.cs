using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebSockies;
using WebSockies.Logic;

namespace WebSockies.Data
{
    public class Lobby
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string InviteCode { get; set; }
        public List<User> Users { get; set; }
        public bool IsOpen { get; set; }
        public List<User> HasAnswered { get; set;} 
        public LobbySettings Settings { get; set; }

        public Quiz Quiz { get; set; }

        public int CurrentQuestion
        {
            get
            {
                int num = 0;
                Quiz.Questions.ForEach(q =>
                {
                    if (q.Answered == true)
                    {
                        num += 1;
                    }
                });
                return num;
            }
        }


        public Lobby(User user)
        {
            Id = Guid.NewGuid().ToString();
            OwnerId = user.Id;

            Random rand = new Random();
            InviteCode = rand.Next(1000,9999).ToString();
            
            Users = new List<User>();
            Users.Add(user);

            Settings = new LobbySettings() { LobbyType = LobbySettings.LobbyTypeEnum.Standard };
            
            IsOpen = true;

            HasAnswered = new List<User>();
            
            // TODO change this
            Quiz = QuizMaster.GetQuiz();
        }

        public void NewOwnerRandom()
        {
            User user = Users[new Random().Next(0, Users.Count - 1)];
            Users.Insert(0, user);
        }
    }
}