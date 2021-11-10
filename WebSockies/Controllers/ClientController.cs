using System;
using System.Collections.Generic;
using System.Linq;
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
                user.SocketConnection.Send(JsonSerializer.Serialize(roomMembers.Select(u => u.Username).ToList()));
            }
        }

        public void JoinLobby(User user, string[] paramStrings)
        {
            if (_lobbyContainer.Lobbies.Exists(l => l.InviteCode == paramStrings[0]))
            {
                _userContainer.users[_userContainer.users.IndexOf(user)].RoomNumber = paramStrings[0];
                user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel() {Status = "OK", Content = "Lobby joined"}));
                
                SendAllLobbyUsers(paramStrings[0]);
            }
            else
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel()
                    {Status = "Failed", Content = "Lobby doesn't exist"}));
            }
        }

        public void CreateLobby(User user)
        {
            Lobby lobby = new Lobby(user.SocketConnection.ConnectionInfo.Id.ToString(), user.Username);
            user.SocketConnection.Send(JsonSerializer.Serialize(new StatusResponseModel() {Status = "OK", Content = lobby.InviteCode}));
            _lobbyContainer.Lobbies.Add(lobby);
        } 

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