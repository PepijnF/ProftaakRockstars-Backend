using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Proftaak.Data;

namespace Proftaak.Controllers
{
    public class LobbyController : Controller
    {
        private LobbyContainer _lobbyContainer;
        
        public LobbyController()
        {
            _lobbyContainer = new LobbyContainer();
        }
    
        // GET
        [HttpGet]
        public void CreateLobby(int userId, string userName)
        {
            _lobbyContainer.Lobbies.Add(new Lobby()
            {
                Id = Guid.NewGuid().ToString(),
                InviteCode = "123",
                OwnerId = userId,
                Users = new List<User>()
            });
        }
    }
}