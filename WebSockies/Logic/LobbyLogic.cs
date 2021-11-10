using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockies.Containers;

namespace WebSockies.Logic
{
    public class LobbyLogic
    {
        private LobbyContainer _lobbyContainer;
        private UserContainer _userContainer;
        public LobbyLogic(LobbyContainer lobbyContainer, UserContainer userContainer) {
            _lobbyContainer = lobbyContainer;
        }
        public List<User> GetAllUsersInRoom(string roomNumber)
        {
            List<User> roomMembers = _userContainer.users.FindAll(u => u.RoomNumber == roomNumber);
            return roomMembers;

        }
        public void SendAllLobbyUsers(string roomNumber)
        {
            List<User> roomMembers = GetAllUsersInRoom(roomNumber);
            ResponseModel responsemodel = new ResponseModel("UserList", "OK", JsonSerializer.Serialize(roomMembers.Select(u => u.Username).ToList()));

            foreach (User user in roomMembers)
            {
                user.SocketConnection.Send(JsonSerializer.Serialize(responsemodel));
            }
        }
    }
}
