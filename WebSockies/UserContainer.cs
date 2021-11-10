using System.Collections.Generic;

namespace WebSockies 
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