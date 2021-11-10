using System;
using System.Collections.Generic;
using System.Text.Json;

namespace WebSockies 
{
    public class ClientController
    {
        private UserContainer _userContainer;
        public void GetAllUsers(User user, List<User> users)
        {
            List<User> roomMembers = users.FindAll(u => u.RoomNumber == user.RoomNumber);
           
           foreach(var roomUser in roomMembers) 
           {
               roomUser.SocketConnection.Send("Dik");
           }
        }

        public void JoinLobby(User user, string[] paramStrings)
        {
            _userContainer.users.Find(u => u.Username == user.Username).RoomNumber = paramStrings[0];
            user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel() {Status = "OK"}));
        }
        
        public void CreateLobby(User user)

        public void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }

        public ClientController(UserContainer userContainer)
        {
            _userContainer = userContainer;
        }
    }
}