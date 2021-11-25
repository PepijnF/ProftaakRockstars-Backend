using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSockies.Containers;
using WebSockies.Data.Models;

namespace WebSockies.Logic
{
    public class LobbyLogic
    {
        private LobbyContainer _lobbyContainer;
        private UserContainer _userContainer;
        public LobbyLogic(LobbyContainer lobbyContainer, UserContainer userContainer) {
            _lobbyContainer = lobbyContainer;
            _userContainer = userContainer;
        }
        public List<User> GetAllUsersInRoom(string roomNumber)
        {
            List<User> lobbyMembers = _userContainer.users.FindAll(u => u.LobbyInviteCode == roomNumber);
            return lobbyMembers;

        }
        public void SendAllLobbyUsers(string roomNumber)
        {
            List<User> lobbyMembers = GetAllUsersInRoom(roomNumber);
            ResponseModel responseModel = new ResponseModel("UserList", "OK", lobbyMembers.Select(u => u.Username).ToList());

            foreach (User user in lobbyMembers)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(responseModel));
            }
        }
    }
}
