using Course_Store.Models.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class Trainer
    {
        [Key]
        public int TrainderId { get; set; }
        public string Skills { get; set; }
        public byte YearOfExperience { get; set; }
        List<Course> Courses { get; set; } = new List<Course>();
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser User { get; set; }
        
    }
}