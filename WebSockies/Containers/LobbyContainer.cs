using System.Collections.Generic;
using System.Text.Json;
using WebSockies.Data;
using WebSockies.Data.Models;
using WebSockies.Logic;

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
            WebSockiesUtils.Broadcast(lobby, new ResponseModel("LobbyResponse", "Failed", "Owner left the lobby"));
            
            Lobbies.Remove(lobby);
        }
    }
}