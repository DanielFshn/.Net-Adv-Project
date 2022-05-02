using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Models
{
    public enum PaymentMethod : byte
    {
        None = 0,
        CreditCard = 1
    }
    public enum OrderStatus : byte
    {
        None = 0,
        Created = 1,
        Processing = 2,
        Completed = 3
    }
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string AdminId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }
}