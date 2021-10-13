using System;
using System.Collections.Generic;

namespace Proftaak.WebSocketModels 
{
    public class UserList
    {
        public string Type {get; set;} = "UserList";
        public List<string> Users {get; set;}

        public UserList()
        {
            Users = new List<string>();
        }
    }
}