using System;
using System.Collections.Generic;

namespace WebSockies.Data
{
    public class Lobby
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string InviteCode { get; set; }
        public List<User> Users { get; set; }
        public bool IsOpen { get; set; }

        public Lobby(string userId, string userName)
        {
            Id = Guid.NewGuid().ToString();
            OwnerId = userId;

            Random rand = new Random();
            InviteCode = rand.Next(1000,9999).ToString();
            
            Users = new List<User>();
            Users.Add(new User(){Id = userId, Name = userName, Score = 0});

            IsOpen = true;
        }
    }
}