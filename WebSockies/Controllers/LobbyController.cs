using System;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;
using WebSockies.Data.Models;
using WebSockies.Logic;

namespace WebSockies
{
    public class LobbyController
    {
        private LobbyContainer _lobbyContainer;
        private UserContainer _userContainer;
        private LobbyLogic _lobbyLogic;
        public LobbyController(UserContainer userContainer, LobbyContainer lobbyContainer) {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
            _lobbyLogic = new LobbyLogic(_lobbyContainer, _userContainer);
        
        }
        public void JoinLobby(User user, string[] paramStrings)
        {
            Lobby lobby = _lobbyContainer.GetLobbyById(paramStrings[0]);
            User userobj = _userContainer.users[_userContainer.users.IndexOf(user)];

            if (lobby != null)
            {
                userobj.LobbyInviteCode = paramStrings[0];
                userobj.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Lobby Joined")));

                Console.WriteLine(user.Username + " joined lobby " + user.LobbyInviteCode);
                lobby.Users.Add(user);
                _lobbyLogic.SendAllLobbyUsers(paramStrings[0]);
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

            Console.WriteLine(user.Username + " created a new lobby with invite code " + lobby.InviteCode);
            _userContainer.users[_userContainer.users.IndexOf(user)].LobbyInviteCode = lobby.InviteCode;
            _lobbyLogic.SendAllLobbyUsers(lobby.InviteCode);
        }

        
        
    }
}