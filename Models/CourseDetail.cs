using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string VideoPath { get; set; }
        public int Course_Id { get; set; }
        [ForeignKey("Course_Id")]
        public Course Course { get; set; }
    }
}