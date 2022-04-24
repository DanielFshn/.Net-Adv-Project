using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Requests
{
    public class CourseAddRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter curse name")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter curse description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter curse objectives")]
        public string Objectives { get; set; }
        [Required(ErrorMessage = "Please enter curse price")]
        public decimal Price { get; set; }
        public bool IsPublish { get; set; }
        public CourseCategory Category { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase UploadImage { get; set; }
        public string TrainerName { get; set; }
    }
}