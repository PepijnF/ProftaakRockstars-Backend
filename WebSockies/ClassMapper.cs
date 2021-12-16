using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockies.Data;

namespace WebSockies
{
    public class ClassMapper
    {
        public ClassMapper() {
            //Too much being mapped atm
            BsonClassMap.RegisterClassMap<Answer>();
            BsonClassMap.RegisterClassMap<Question>();
            BsonClassMap.RegisterClassMap<Quiz>();
        }
        
            
    }
}
