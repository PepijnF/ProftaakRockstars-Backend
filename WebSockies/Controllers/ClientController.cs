using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;

namespace WebSockies 
{
    public class ClientController
    {
        private UserContainer _userContainer;
        private LobbyContainer _lobbyContainer;
        public void GetAllUsers(User user, List<User> users)
        {
            List<User> roomMembers = users.FindAll(u => u.RoomNumber == user.RoomNumber);
           
           foreach(var roomUser in roomMembers) 
           {
               roomUser.SocketConnection.Send(JsonSerializer.Serialize(roomMembers));
           }
        }
        public void SendAllLobbyUsers(string roomNumber) {
            List<User> roomMembers = _userContainer.users.FindAll(u => u.RoomNumber == roomNumber);
            foreach (User user in roomMembers)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(roomMembers));
            }
        }

        public void JoinLobby(User user, string[] paramStrings)
        {
            if (_lobbyContainer.Lobbies.Exists(l => l.InviteCode == paramStrings[0]))
            {
                _userContainer.users.Find(u => u.Username == user.Username).RoomNumber = paramStrings[0];
                user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel() {Status = "OK", Description = "Lobby joined"}));
                
                SendAllLobbyUsers(user.RoomNumber);
            }
            else
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel()
                    {Status = "Failed", Description = "Lobby doesn't exist"}));
            }
        }
        
        public void CreateLobby(User user)

        public void CreateLobby(User user) { }

        public void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }

        public ClientController(UserContainer userContainer, LobbyContainer lobbyContainer)
        {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
        }

    }
}