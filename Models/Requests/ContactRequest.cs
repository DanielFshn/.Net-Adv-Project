using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Requests
{
    public class ContactRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter your message")]
        public string Message { get; set; }
    }
}