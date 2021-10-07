using System.Collections.Generic;

namespace Proftaak.Data 
{
    public class ListUsersModel
    {
        public string Type {get; set;} = "UserList";
        public List<string> Users {get; set;}

        public ListUsersModel()
        {
            Users = new List<string>();
        }
    }
}