using System;

namespace WebSockies
{
    public class MessageModel
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public object[] Parameters { get; set; }
    }
}