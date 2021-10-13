using Proftaak.Data;
using System;
using System.Collections.Generic;

namespace Proftaak.WebSocketModels 
{
    public class ClientController 
    {
        public void GetAllUsers(User user, List<User> users)
        {
            List<User> roomMembers = users.FindAll(u => u.RoomNumber == user.RoomNumber);
           
           foreach(var roomUser in roomMembers) 
           {
               roomUser.SocketConnection.Send("Dik");
           }
        }

        public void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }
    }
}