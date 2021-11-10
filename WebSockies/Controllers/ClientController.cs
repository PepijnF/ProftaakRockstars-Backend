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
        public ClientController(UserContainer userContainer, LobbyContainer lobbyContainer)
        {
            _userContainer = userContainer;
            _lobbyContainer = lobbyContainer;
        }

        



        
    }
}