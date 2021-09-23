using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proftaak.Data;

namespace Proftaak.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        private LobbyContainer _lobbyContainer;
        
        public LobbyController(LobbyContainer lobbyContainer)
        {
            _lobbyContainer = lobbyContainer;
        }
    
        /// <summary>
        /// Creates a new lobby
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns>A newly created lobby</returns>
        /// <response code="201">Lobby has been created succesfully</response>
        [HttpGet("CreateLobby")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Lobby> CreateLobby(string userId, string userName)
        {
            Lobby createdLobby = new Lobby(userId, userName);
            _lobbyContainer.Lobbies.Add(createdLobby);
            
            // Geen idee wat uri doet dus laat maar leeg
            return Created("", createdLobby);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="inviteCode"></param>
        /// <returns></returns>
        /// <response code="202">Joined the lobby</response>
        /// <response code="404">Lobby code is invalid</response>
        [HttpGet("JoinLobby")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Lobby> JoinLobby(string userId, string userName, string inviteCode)
        {
            foreach (Lobby lobby in _lobbyContainer.Lobbies)
            {
                if (lobby.IsOpen && lobby.InviteCode == inviteCode)
                {
                    lobby.Users.Add(new User()
                    {
                        Id = userId,
                        Name = userName,
                        Score = 0
                    });
                    // Mogelijk data meegeven
                    return Accepted(lobby);
                }
            }

            return NotFound("Lobby not found");
        }

        [HttpGet("StartLobby")]
        public ActionResult<Lobby> StartLobby(string userId, string lobbyId)
        {
            foreach (Lobby lobby in _lobbyContainer.Lobbies)
            {
                if (lobby.OwnerId == userId && lobby.Id == lobbyId)
                {
                    lobby.IsOpen = false;
                    return Ok(lobby);
                }
            }

            return NotFound("Lobby not found");
        }
    }
}