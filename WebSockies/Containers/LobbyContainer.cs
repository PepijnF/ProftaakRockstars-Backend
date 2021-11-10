using System.Collections.Generic;
using WebSockies.Data;

namespace WebSockies.Containers
{
    public class LobbyContainer
    {
        public List<Lobby> Lobbies { get; set; }

        public LobbyContainer()
        {
            Lobbies = new List<Lobby>();
        }
    }
}