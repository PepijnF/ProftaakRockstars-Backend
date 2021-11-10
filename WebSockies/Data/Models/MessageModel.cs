using System;

namespace WebSockies
{
    public class MessageModel
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public string[] Parameters { get; set; }
    }
}