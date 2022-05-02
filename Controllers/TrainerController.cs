using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Course_Store.Models;
using Course_Store.Models.Requests;
using Course_Store.Models.Responses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Course_Store.Controllers
{
    [Authorize(Roles = "Admin,Trainer")]
    public class TrainerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public TrainerController()
        {
        }

        public TrainerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Trainers
        public ActionResult Index()
        {
            var trainer = db.Trainers.ToList();
            var trainerView = new List<TrainerPannel>();
            foreach (var item in trainer)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == item.User_Id);
                trainerView.Add(new TrainerPannel()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    YearOfExperience = item.YearOfExperience,
                    Skills = item.Skills,
                    Id = item.TrainderId,
                    Image = user.Photo
                });
            }
            return View(trainerView);
        }

        // GET: Trainers/Details/5
        public ActionResult Details(int? id)
        {
            var trainer = db.Trainers.Include(t => t.User).FirstOrDefault();
            var user = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
            var trainerView = new TrainerPannel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                YearOfExperience = trainer.YearOfExperience,
                Skills = trainer.Skills,
                Id = trainer.TrainderId,
                Image = user.Photo
            };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainerView);
        }

        // GET: Trainers/Create
        public ActionResult Create()
        {
            return View(new TrainerRegisterRequest());
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, TrainerRegisterRequest model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var appuser = UserManager.Users.FirstOrDefault(e => e.Id == id);
                    var trainerRole = db.Roles.Where(n => n.Name == "Trainer").FirstOrDefault();
                    try
                    {
                        UserManager.RemoveFromRole(id, "User");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    UserManager.AddToRole(id, trainerRole.Name);
                    var trainer = new Trainer()
                    {
                        YearOfExperience = model.YearOfExperience,
                        Skills = model.Skills,
                        User_Id = appuser.Id
                    };

                    db.Trainers.Add(trainer);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                { throw; }
            }
            return View();
        }
        [Authorize(Roles= "Admin,Trainer")]
        // GET: Trainers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Error/BadRequest");
            }
            Trainer trainer = db.Trainers.Find(id);
            var user = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
            if(user == null || trainer == null)
            {
                return View("~/Error/PageNotFound");
            }
            var trainerEditReq = new TrainerEditRequest()
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.UserName,
                Birthday = user.Birthday,
                Email = user.Email,
                Skills = trainer.Skills,
                YearsOfExperience = trainer.YearOfExperience,
                Photo = user.Photo
            };
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainerEditReq);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrainerEditRequest model,int id)
        {
            if (ModelState.IsValid)
            {
                var trainer = db.Trainers.Find(id);
                var user = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
                try
                {
                    var filename = "~/UserPhotos/";
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    user.Birthday = model.Birthday;
                    trainer.Skills = model.Skills;
                    trainer.YearOfExperience = model.YearsOfExperience;
                    if (model.Photo != null)
                        user.Photo = (filename + model.Photo).Trim();
                    user.UpdatedById = trainer.User_Id;
                    user.UpdatedOn = DateTime.Now;
                }
                catch (Exception)
                {
                    throw;
                }
                db.Entry(user).State = EntityState.Modified;
                db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Trainers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            var user = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
            var trainerView = new TrainerEditRequest()
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.UserName,
                Birthday = user.Birthday,
                Email = user.Email,
                Skills = trainer.Skills,
                YearsOfExperience = trainer.YearOfExperience
            };
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainerView);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainer trainer = db.Trainers.Find(id);
            db.Trainers.Remove(trainer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
