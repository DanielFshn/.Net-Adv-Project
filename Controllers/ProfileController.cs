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

namespace Course_Store.Controllers
{
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult Index()
        {
            int sumOfPoints = 0;
            var id = User.Identity.GetUserId();
            var user_courses = new List<CourseAddRequest>();
            var orders = db.Orders.Where(x => x.UserId == id).ToList();
            foreach (var item in orders)
            {
                var orderDet = db.OrderDetails.Where(x => x.OrderId == item.Id).ToList();
                foreach (var od in orderDet)
                {
                    var course = db.Courses.Find(od.CourseId);
                    var category = db.CourseCategories.Find(course.CategoryId);
                    user_courses.Add(new CourseAddRequest()
                    {
                        Id = course.Id,
                        Category = category,
                        Description = course.Description,
                        Image = course.Image,
                        IsPublish = course.IsPublish,
                        Objectives = course.Objectives,
                        Price = course.Price,
                        Title = course.Title
                    });
                }
            }
            ViewBag.Points = 0;
            var progesses = db.Progresses.Where(x => x.User_Id == id).ToList();
            foreach (var item in progesses)
            {
                sumOfPoints += item.Points;
            }
            ViewBag.Points = sumOfPoints;
            ViewData["userCourses"] = user_courses;
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                UserProfile model = new UserProfile()
                {
                    Birthday = user.Birthday,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Id = user.Id,
                    Photo = user.Photo,
                    Username = user.UserName,
                    Courses = new List<Course>()
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("login", "account");
            }

        }
        public PartialViewResult Courses()
        {
            var id = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            var model = new List<CourseAddRequest>();
            var orders = db.Orders.Where(o => o.UserId == id).ToList();
            foreach (var item in orders)
            {
                var orderDetails = db.OrderDetails.Where(o => o.OrderId == item.Id && o.IsHidden == false).ToList();
                foreach (var course in orderDetails)
                {
                    var c = db.Courses.Find(course.CourseId);
                    var category = db.CourseCategories.Find(c.CategoryId);
                    model.Add(new CourseAddRequest()
                    {
                        Description = c.Description,
                        Id = c.Id,
                        Category = category,
                        Image = c.Image,
                        IsPublish = c.IsPublish,
                        Objectives = c.Objectives,
                        Price = c.Price,
                        Title = c.Title
                    });
                }
            }
            return PartialView("_Courses", model);
        }
        public ActionResult Hide(int id)
        {
            var UserId = User.Identity.GetUserId();

            var user = db.Users.FirstOrDefault(u => u.Id == UserId);
            if (user != null)
            {
                var orders = db.Orders.Where(o => o.UserId == UserId).ToList();
                foreach (var item in orders)
                {
                    var orderDetails = db.OrderDetails.FirstOrDefault(o => o.OrderId == item.Id && o.CourseId == id);
                    if (orderDetails != null)
                    {
                        orderDetails.IsHidden = true;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return View("~Error/BadRequest");
            }
            ApplicationUser applicationUser = db.Users.FirstOrDefault(u => u.Id == id);
            if (applicationUser == null)
            {
                return View("~Error/PageNotFound");
            }
            TempData["username"] = applicationUser.UserName;

            var user = new UserEditRequest()
            {
                Birthday = applicationUser.Birthday,
                Name = applicationUser.Name,
                Surname = applicationUser.Surname,
                Username = applicationUser.UserName,
                Photo = applicationUser.Photo,

            };
            return View(user);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditRequest model)
        {
            if (ModelState.IsValid)
            {
                var filename = "~/UserPhotos/";
                var id = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == id);
                try
                {
                    var username = db.Users.FirstOrDefault(x => x.UserName == model.Username);
                    if (username != null)
                    {
                        ViewBag.message = "Username qe doni te vendosni ekziston ne sistem";
                        return View(model);
                    }
                    else
                        user.UserName = model.Username;
                }
                catch (Exception) { }
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UpdatedById = id;
                user.UpdatedOn = DateTime.Now;
                if (model.Photo != null)
                {
                    user.Photo = (filename + model.Photo).Trim();
                }
                if (model.Birthday != null)
                {
                    user.Birthday = model.Birthday;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return View("~Error/BadRequest");
            }
            ApplicationUser applicationUser = db.Users.FirstOrDefault(u => u.Id == id);
            if (applicationUser == null)
            {
                return View("~Error/PageNotFound");
            }
            var user = new UserProfile()
            {
                Birthday = applicationUser.Birthday,
                Email = applicationUser.Email,
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                Surname = applicationUser.Surname,
                Photo = applicationUser.Photo,
            };
            return View(user);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.FirstOrDefault(u => u.Id == id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
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
