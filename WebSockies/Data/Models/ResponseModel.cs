using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSockies.Data.Models
{
    public class ResponseModel
    {
        public ResponseModel(string type, string status, string content)
        {
            Type = type;
            Status = status;
            Content = content;

        }
        public string Type;
        public string Status;
        public object Content;

    }
}
