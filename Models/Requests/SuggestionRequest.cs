using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Requests
{
    public class SuggestionRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter your suggestion")]
        public string SuggestionContent { get; set; }
    }
}