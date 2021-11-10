using System;
using System.Collections.Generic;
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
                private ClientController _clientController;
        
                public WebSockets(UserContainer userContainer, ClientController clientController)
                {
                    _userContainer = userContainer;
                    _clientController = clientController;
                }
        
                public void StartServer()
                {
                    server.Start(socket =>
                    {
                        socket.OnOpen = () =>
                        {
                            User User = new User(socket);
                            connections.Add(User);
                            Console.WriteLine("Connection opened " + User.Username + " in room " + User.RoomNumber);
                        };
                        socket.OnMessage = message =>
                        {
                            var messageModel = JsonSerializer.Deserialize<MessageModel>(message);

                            MethodInfo mi = Type.GetType("WebSockies." + messageModel.Controller)
                                .GetMethod(messageModel.Method);

                            var controllerInst = Activator.CreateInstance(Type.GetType("WebSockies." + messageModel.Controller));
                            mi.Invoke(controllerInst, null);
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
            WebSockets webSockets = new WebSockets(new UserContainer(), new ClientController());
            webSockets.StartServer();
            Console.ReadKey();
        }
    }
}