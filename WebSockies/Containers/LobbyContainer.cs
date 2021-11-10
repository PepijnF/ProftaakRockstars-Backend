using System.Collections.Generic;

namespace Proftaak.Data
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