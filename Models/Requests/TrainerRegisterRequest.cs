using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Requests
{
    public class TrainerRegisterRequest
    {
        //[Required(ErrorMessage = "Please enter trainer name")]
        //public string Name { get; set; }
        //[Required(ErrorMessage = "Please enter trainer surname")]
        //public string Surname { get; set; }
        //[Required(ErrorMessage = "Please enter trainer email")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
        //[Required(ErrorMessage = "Please enter trainer password")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must have at least 8 characters")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        //[Required(ErrorMessage = "Please enter trainer confirm password")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Please enter trainer birthday")]
        //[DataType(DataType.Date)]
        //public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Please enter trainer skills")]
        public string Skills { get; set; }
        [Required(ErrorMessage = "Please enter trainer year of experience")]
        public byte YearOfExperience { get; set; }
        public HttpPostedFileBase UploadPhoto { get; set; }
    }
}