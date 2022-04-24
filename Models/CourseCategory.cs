using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }
        public string CategotyType { get; set; }
    }
}