using System.Collections.Generic;

namespace WebSockies.Containers 
{
    public class UserContainer 
    {
        public List<User> users {get; set;}

        public UserContainer()
        {
            users = new List<User>();
        }

        public List<User> GetUserByLobbyID(string id) {
            return users.FindAll(u => u.LobbyInviteCode == id);
        }
        
    }
}