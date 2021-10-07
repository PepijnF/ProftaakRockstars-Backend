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

namespace Proftaak.Services
{
    public class WebSockets
    {
        WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8001");
        List<WsUser> connections = new List<WsUser>();

        public void StartServer()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    WsUser wsUser = new WsUser(socket);
                    connections.Add(wsUser);
                    Console.WriteLine("Connection opened " + wsUser.Username + " in room " + wsUser.RoomNumber);
                };
                socket.OnMessage = message =>
                {
                    
                    WsUser sendUser = connections.Find(u => u.Id == socket.ConnectionInfo.Id);
                    List<WsUser> roomMembers = connections.FindAll(u => u.RoomNumber == sendUser.RoomNumber);

                    char[] delimChars = {':', '}', '{', '[', ']', ',', '"'};
                    var words = message.Split(delimChars);
                    if(words[5] == "IsReady")
                    {
                        ListUsersModel listUsers = new ListUsersModel();
                        foreach(var user in roomMembers)
                        {
                            listUsers.Users.Add(user.Username);
                        }

                        foreach(var user in roomMembers)
                        {
                            user.SocketConnection.Send(JsonSerializer.Serialize(listUsers));
                        }
                    }


                    //foreach (var wsUser in roomMembers)
                    //{
                    //    wsUser.SocketConnection.Send(sendUser.Username + " " + message);
                    //}

                    
                };
                socket.OnClose = () =>
                {
                    WsUser wsUser = connections.Find(u => u.Id == socket.ConnectionInfo.Id);
                    Console.WriteLine(wsUser.Username + " Disconnected");
                };
            });
        }
    }
}