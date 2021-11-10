using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fleck;
using System.Reflection;
using System.Text.Json;

namespace WebSockies
{
    class Program
    {
        public class WebSockets
            {
                WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8001");
                List<User> connections = new List<User>();
                private UserContainer _userContainer;
        
                public WebSockets(UserContainer userContainer)
                {
                    _userContainer = userContainer;
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

                            var controllerInst = Activator.CreateInstance(Type.GetType("WebSockies." + messageModel.Controller), _userContainer);
                            List<Object> parameters = new List<Object>();
                            parameters.Add(_userContainer.users.Find(u => u.SocketConnection.ConnectionInfo.Id == socket.ConnectionInfo.Id));
                            parameters.Add(messageModel.Parameters);
                            if (messageModel.Parameters != null)
                            {
                                mi.Invoke(controllerInst, parameters.ToArray());
                            }
                            else
                            {
                                mi.Invoke(controllerInst, null);
                            }
                        };
                        socket.OnClose = () =>
                        {
                            User User = connections.Find(u => u.Id == socket.ConnectionInfo.Id.ToString());
                            Console.WriteLine(User.Username + " Disconnected");
                        };
                    });
                }
            }
        static void Main(string[] args)
        {
            WebSockets webSockets = new WebSockets(new UserContainer());
            webSockets.StartServer();
            Console.ReadKey();
        }
    }
}