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

        
    }
}