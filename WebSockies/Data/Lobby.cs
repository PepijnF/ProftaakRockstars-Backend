using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebSockies;

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


        public Lobby(string userId, string userName)
        {
            Id = Guid.NewGuid().ToString();
            OwnerId = userId;

            Random rand = new Random();
            InviteCode = rand.Next(1000,9999).ToString();
            
            Users = new List<User>();
            Users.Add(new User(){Id = userId, Username = userName, Score = 0});

            Settings.LobbyType = LobbySettings.LobbyTypeEnum.Standard;
            
            IsOpen = true;
        }

        public void NewOwnerRandom()
        {
            User user = Users[new Random().Next(0, Users.Count - 1)];
            Users.Insert(0, user);
        }
    }
}