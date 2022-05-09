    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int? UpdatedById { get; set; }
        public int? DeletedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsPublish { get; set; }
        public int? Points { get; set; }
        public string Objectives { get; set; }
        public List<CourseDetail> Details { get; set; } = new List<CourseDetail>();
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CourseCategory CourseCategory { get; set; }
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer CourseTrainer { get; set; }
    }
}