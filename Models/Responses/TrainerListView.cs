using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class TrainerListView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Skills { get; set; }
        public byte YearsOfExperience { get; set; }
    }
}