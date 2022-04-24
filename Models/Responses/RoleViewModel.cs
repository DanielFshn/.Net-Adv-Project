using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Course_Store.Models.Responses
{
    public class RoleViewModel
    {
        public RoleViewModel(IdentityRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }
        public RoleViewModel() { }

        public string Id { get; set; }
        public string Name { get; set; }
        
    }
}