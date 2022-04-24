using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public decimal Price { get; set; }
        public bool IsHidden { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}