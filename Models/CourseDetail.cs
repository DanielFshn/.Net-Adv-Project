using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string VideoPath { get; set; }
        public Progress Progress { get; set; }
    }
}