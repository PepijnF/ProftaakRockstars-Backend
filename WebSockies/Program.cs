﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fleck;
using System.Reflection;
using System.Text.Json;
using WebSockies.Containers;
using WebSockies.Data;

namespace WebSockies
{
    class Program
    {
        public class WebSockets
            {
                WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8001");
                List<User> connections = new List<User>();
                private UserContainer _userContainer;
                private LobbyContainer _lobbyContainer;
                private QuestionContainer _questionContainer;
        
                public WebSockets(UserContainer userContainer, LobbyContainer lobbyContainer, QuestionContainer questionContainer)
                {
                    _userContainer = userContainer;
                    _lobbyContainer = lobbyContainer;
                    _questionContainer = questionContainer;
                }
        
                public void StartServer()
                {
                    server.Start(socket =>
                    {
                        socket.OnOpen = () =>
                        {
                            User user = new User(socket);
                            _userContainer.users.Add(user);
                            Console.WriteLine("Connection opened " + user.Username);
                        };
                        socket.OnMessage = message =>
                        {
                            try
                            {
                                var messageModel = JsonSerializer.Deserialize<MessageModel>(message.Replace("\n", "").Replace("\t", ""));

                                MethodInfo mi = Type.GetType("WebSockies." + messageModel.Controller)
                                    .GetMethod(messageModel.Method);

                                object? controllerInst;

                                if (messageModel.Controller == "QuizController")
                                {
                                    controllerInst = Activator.CreateInstance(Type.GetType("WebSockies." + messageModel.Controller), _userContainer, _lobbyContainer, _questionContainer);
                                }
                                else
                                {
                                    controllerInst = Activator.CreateInstance(Type.GetType("WebSockies." + messageModel.Controller), _userContainer, _lobbyContainer);
                                }

                                List<Object> parameters = new List<Object>();
                                parameters.Add(_userContainer.users.Find(u => u.SocketConnection.ConnectionInfo.Id == socket.ConnectionInfo.Id));
                                if (messageModel.Parameters.Length != 0)
                                {
                                    parameters.Add(messageModel.Parameters);
                                }
                                mi.Invoke(controllerInst, parameters.ToArray());
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Oepsie woepsie iets in de code is hewemaal mwis gegaan UwU");
                                throw;
                            }
                            
                            
                        };
                        socket.OnClose = () =>
                        {
                            User user = _userContainer.users.Find(u => u.Id == socket.ConnectionInfo.Id.ToString());
                            Console.WriteLine(user.Username + " Disconnected");
                            _userContainer.users.Remove(user);
                            if (_lobbyContainer.Lobbies.Exists(l => l.OwnerId == user.Id))
                            {
                                Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.OwnerId == user.Id);
                                lobby.NewOwnerRandom();
                            }
                        };
                        socket.OnError = exception =>
                        {
                            User user = _userContainer.users.Find(u => u.Id == socket.ConnectionInfo.Id.ToString());
                            Console.WriteLine(user.Username + " Disconnected because of an error");
                            Console.WriteLine(exception.Message);
                            _userContainer.users.Remove(user);
                            if (_lobbyContainer.Lobbies.Exists(l => l.OwnerId == user.Id))
                            {
                                Lobby lobby = _lobbyContainer.Lobbies.Find(l => l.OwnerId == user.Id);
                                lobby.NewOwnerRandom();
                            }
                        };
                    });
                }
            }
        static void Main(string[] args)
        {
            WebSockets webSockets = new WebSockets(new UserContainer(), new LobbyContainer(), new QuestionContainer());

            webSockets.StartServer();
            Console.ReadKey();
        }
    }
}