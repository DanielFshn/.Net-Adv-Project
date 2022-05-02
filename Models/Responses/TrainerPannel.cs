using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class TrainerPannel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int YearOfExperience { get; set; }
        public string Skills { get; set; }
        public string Image { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}