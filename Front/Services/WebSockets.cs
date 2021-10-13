using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Fleck;
using Proftaak.Data;
using System.Text.Json;
using Proftaak.WebSocketModels;
using System.Reflection;

namespace Proftaak.Services
{
    public class WebSockets
    {
        WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8001");
        List<User> connections = new List<User>();
        private UserContainer _userContainer;
        private ClientController _clientController;

        public WebSockets(ClientController clientController, UserContainer userContainer)
        {
            _clientController = clientController;
            _userContainer = userContainer;

            this.StartServer();
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
                    string controller = "Proftaak.WebSocketControllers." + words[5];
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
}