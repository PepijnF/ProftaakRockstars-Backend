using Proftaak.Data;
using System.Collections.Generic;

namespace Proftaak.WebSocketModels 
{
    public class UserContainer 
    {
        public List<User> users {get; set;}

        public UserContainer()
        {
            users = new List<User>();
        }
    }
}