using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public int? Points { get; set; }
        public string Photo { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}