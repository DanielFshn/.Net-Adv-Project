using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class UserOrderDetailsView
    {
        public string CourseName { get; set; }
        public decimal Price { get; set; }
    }
    public class UserOrdersListView
    {
        public int OrderId { get; set; }
        public string CourseName { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Price { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string UserId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        //public List<UserOrderDetailsView> Details { get; set; }
    }
}