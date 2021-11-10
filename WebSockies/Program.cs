using System;
using System.Collections.Generic;
using Fleck;
using System.Reflection;

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
                            
                            char[] delimChars = {':', '}', '{', '[', ']', ',', '"'};
                            var words = message.Split(delimChars);
        
                            // 5 is controller
                            // 11 is method
                            //message converten naar class zodat deze troep niet hoeft
                            string controller = "WebSockies" + words[5];
                            string methodName = words[11];
        
                            //Get the method information using the method info class
                            MethodInfo mi = Type.GetType(controller).GetMethod(methodName);
        
        
                            //Pieter's code
                            var controllerInst = Activator.CreateInstance(Type.GetType(controller));
                            //Invoke the method
                            // (null- no parameter for the method call
                            // or you can pass the array of parameters...)
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