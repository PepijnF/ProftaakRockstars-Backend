using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSockies.Data.Models
{
    public class ResponseModel
    {
        public ResponseModel(string type, string status, object content)
        {
            Type = type;
            Status = status;
            if (content is string)
            {
                Content = (string)content;
            }
            else
            {
                Content = JsonSerializer.Serialize(content);
            }

        }
        
        public ResponseModel(){}
        
        public string Type { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }

    }
}
