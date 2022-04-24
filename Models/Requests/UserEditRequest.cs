using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Requests
{
    public class UserEditRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public DateTime Birthday { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }
    }
}