using System.Collections.Generic;
using WebSockies.Data;

namespace WebSockies.Containers
{
    public class LobbyContainer
    {
        public LobbyContainer()
        {
            Lobbies = new List<Lobby>();
        }
        public List<Lobby> Lobbies { get; set; }

        public Lobby GetLobbyById(string id) {
            return Lobbies.Find(l => l.InviteCode == id);
        }

        public void RemoveLobby(Lobby lobby)
        {
            // TODO send lobby closed message

            Lobbies.Remove(lobby);
        }
    }
}