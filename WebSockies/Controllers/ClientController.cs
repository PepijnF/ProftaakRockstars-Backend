using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;
using WebSockies.Data.Models;

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
        private void SendAllLobbyUsers(string roomNumber) {
            List<User> roomMembers = _userContainer.users.FindAll(u => u.RoomNumber == roomNumber);
            ResponseModel responsemodel = new ResponseModel("UserList", "OK", roomMembers.Select(u => u.Username).ToList());
            
            foreach (User user in roomMembers)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(responsemodel));
            }
        }

        public void JoinLobby(User user, string[] paramStrings)
        {
            if (_lobbyContainer.Lobbies.Exists(l => l.InviteCode == paramStrings[0]))
            {
                _userContainer.users[_userContainer.users.IndexOf(user)].RoomNumber = paramStrings[0];
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Lobby Joined")));
                
                SendAllLobbyUsers(paramStrings[0]);
            }
            else
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "Failed", "Lobby doesn't exist")));
            }
        }

        public void CreateLobby(User user)
        {
            Lobby lobby = new Lobby(user.SocketConnection.ConnectionInfo.Id.ToString(), user.Username);
            user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("InviteCode", "OK", lobby.InviteCode)));
            _lobbyContainer.Lobbies.Add(lobby);

            _userContainer.users[_userContainer.users.IndexOf(user)].RoomNumber = lobby.InviteCode;
            SendAllLobbyUsers(lobby.InviteCode);
        }

        public void StartQuiz(User user)
        {
            var lobby = _lobbyContainer.Lobbies.Find(l => l.InviteCode == user.RoomNumber);
            if (user.Id == lobby.OwnerId)
            {
                _lobbyContainer.Lobbies[_lobbyContainer.Lobbies.IndexOf(lobby)].IsOpen = false;
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Quiz started")));
            }
        }
        
        public ClientController(UserContainer userContainer, LobbyContainer lobbyContainer)
        {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
        }

    }
}