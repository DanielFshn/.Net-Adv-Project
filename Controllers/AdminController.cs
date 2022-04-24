using Course_Store.Models;
using Course_Store.Models.Requests;
using Course_Store.Models.Responses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Store.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult CreateTrainer(string id)
        //{
        //    return View(new TrainerRegisterRequest());
        //}
        //[HttpPost]
        //public ActionResult CreateTrainer(string id, TrainerRegisterRequest model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var appuser = UserManager.Users.FirstOrDefault(e => e.Id == id);
        //            var trainerRole = db.Roles.Where(n => n.Name == "Trainer").FirstOrDefault();
        //            try
        //            {
        //                UserManager.RemoveFromRole(id, "User");
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //            UserManager.AddToRole(id, trainerRole.Name);
        //            var trainer = new Trainer()
        //            {
        //                YearOfExperience = model.YearOfExperience,
        //                Skills = model.Skills,
        //                User_Id = appuser.Id
        //            };
                   
        //            db.Trainers.Add(trainer);
        //            db.SaveChanges();
        //            return RedirectToAction("Index", "Home");
        //        }
        //        catch (Exception)
        //        { throw; }
        //    }
        //    return View();
        //}
        //public ActionResult TrainerDetails(int? id)
        //{

        //    return View();
        //}
        public ActionResult ListOfUsers()
        {
            var users = UserManager.Users.ToList();
            var userList = new List<UserListView>();
            foreach (var item in users)
            {
                var role = item.Roles.Select(r => r.RoleId).FirstOrDefault();
                if (role != "Trainer")
                {
                var userListView = new UserListView()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Surname = item.Surname,
                    Username = item.UserName,
                    Email = item.Email
                };
                    userList.Add(userListView);
                }
            }
            return View(userList);
        }
    }
}