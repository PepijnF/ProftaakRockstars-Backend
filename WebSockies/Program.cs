using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fleck;
using System.Reflection;
using System.Text.Json;
using WebSockies.Containers;

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
        
                public WebSockets(UserContainer userContainer, LobbyContainer lobbyContainer)
                {
                    _userContainer = userContainer;
                    _lobbyContainer = lobbyContainer;
                }
        
                public void StartServer()
                {
                    server.Start(socket =>
                    {
                        socket.OnOpen = () =>
                        {
                            User user = new User(socket);
                            _userContainer.users.Add(user);
                            Console.WriteLine("Connection opened " + user.Username + " in room " + user.RoomNumber);
                        };
                        socket.OnMessage = message =>
                        {
                            var messageModel = JsonSerializer.Deserialize<MessageModel>(message);

                            MethodInfo mi = Type.GetType("WebSockies." + messageModel.Controller)
                                .GetMethod(messageModel.Method);

                            var controllerInst = Activator.CreateInstance(Type.GetType("WebSockies." + messageModel.Controller), _userContainer, _lobbyContainer);
                            List<Object> parameters = new List<Object>();
                            parameters.Add(_userContainer.users.Find(u => u.SocketConnection.ConnectionInfo.Id == socket.ConnectionInfo.Id));
                            if (messageModel.Parameters.Length != 0)
                            {
                                parameters.Add(messageModel.Parameters);
                            }
                            mi.Invoke(controllerInst, parameters.ToArray());
                            
                        };
                        socket.OnClose = () =>
                        {
                            User user = _userContainer.users.Find(u => u.Id == socket.ConnectionInfo.Id.ToString());
                            Console.WriteLine(user.Username + " Disconnected");
                            _userContainer.users.Remove(user);
                        };
                    });
                }
            }
        static void Main(string[] args)
        {
            WebSockets webSockets = new WebSockets(new UserContainer(), new LobbyContainer());

            webSockets.StartServer();
            Console.ReadKey();
        }
    }
}