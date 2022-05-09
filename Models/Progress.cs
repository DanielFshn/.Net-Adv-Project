using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public enum ProgressStatus
    {
        None = 0,
        Started = 1,
        Ended = 2
    }
    public class Progress
    {
        public int Id { get; set; }
        public DateTime StaretdTime { get; set; }
        public DateTime EndTime { get; set; }
        public ProgressStatus ProgressStatus { get; set; }
        public int Points { get; set; }
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser User { get;set;}
        public int CourseDetail_Id { get; set; }
        [ForeignKey("CourseDetail_Id")]
        public CourseDetail CourseDetail { get; set; }
    }
}