using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class UserListView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}