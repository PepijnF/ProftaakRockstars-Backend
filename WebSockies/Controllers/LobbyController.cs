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
            if (_lobbyContainer.Lobbies.Exists(l => l.InviteCode == paramStrings[0]))
            {
                _userContainer.users[_userContainer.users.IndexOf(user)].RoomNumber = paramStrings[0];
                user.SocketConnection.Send(JsonSerializer.Serialize(new ResponseModel("LobbyResponse", "OK", "Lobby Joined")));

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

            _userContainer.users[_userContainer.users.IndexOf(user)].RoomNumber = lobby.InviteCode;
            _lobbyLogic.SendAllLobbyUsers(lobby.InviteCode);
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

    }

}