using System.Collections.Generic;

namespace Proftaak.Data
{
    public class Lobby
    {
        public string Id { get; set; }
        public int OwnerId { get; set; }
        public string InviteCode { get; set; }
        public List<User> Users { get; set; }
    }
}