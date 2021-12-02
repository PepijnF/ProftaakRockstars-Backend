using System.Text.Json;
using WebSockies.Data;
using WebSockies.Data.Models;

namespace WebSockies.Logic
{
    public class WebSockiesUtils
    {
        public static void Broadcast(Lobby lobby, ResponseModel message)
        {
            lobby.Users.ForEach(u => u.SocketConnection.Send(JsonSerializer.Serialize(message)));
        }
    }
}