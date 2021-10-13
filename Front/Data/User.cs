using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proftaak.Data
{
    public class User
    {
        public string Username { get; set; }
        public string RoomNumber { get; set; }
        public string Id { get; set; }
        public IWebSocketConnection SocketConnection { get; set; }
        public string Name { get; set; }
        public int Score {get; set;}
        private string _getValue(string key)
        {
            string path = this.SocketConnection.ConnectionInfo.Path;
            int index = path.IndexOf("?");
            var value = path.Substring(index + 1)
                .Split("&")
                .Single(s => s.StartsWith(key + "="))
                .Substring(key.Length + 1);
            return value;
        }

        public User (IWebSocketConnection socketConnection)
        {
            SocketConnection = socketConnection;
            Username = _getValue("name");
            RoomNumber = _getValue("room");
            Id = socketConnection.ConnectionInfo.Id.ToString();
        }

        public User(){}
    }
}
