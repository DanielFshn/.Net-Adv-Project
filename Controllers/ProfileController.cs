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
    [Authorize]
    public class ProfileController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        //ky funnksioni eshte sa per ceshtje testimi qe ta populloje databazen
        public ActionResult Populate()
        {
            try
            {
                var category = new CourseCategory()
                {
                    CategotyType = "programming"
                };
                db.CourseCategories.Add(category);

                var id = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == id);
                var trainer = new Trainer()
                {
                    YearOfExperience = 10,
                    Skills = ".Net"
                };
                db.Trainers.Add(trainer);

                var course = new Course()
                {
                    Image = "DSC_4642.jpg",
                    Points = 10,
                    Title = "c#",
                    Price = 100,
                    CreatedOn = DateTime.Now,
                    IsPublish = true,
                    CategoryId = category.Id,
                    TrainerId = trainer.TrainderId
                };
                db.Courses.Add(course);

                var course1 = new Course()
                {
                    Image = "DSC_4642.jpg",
                    Points = 10,
                    Title = ".net",
                    Price = 100,
                    CreatedOn = DateTime.Now,
                    IsPublish = true,
                    CategoryId = category.Id,
                    TrainerId = trainer.TrainderId
                };
                db.Courses.Add(course1);

                var order = new Order()
                {
                    UserId = user.Id,
                    CreatedOn = DateTime.Now,
                    PaymentMethod = PaymentMethod.None
                };
                db.Orders.Add(order);

                var order2 = new Order()
                {
                    UserId = user.Id,
                    CreatedOn = DateTime.Now,
                    PaymentMethod = PaymentMethod.None
                };
                db.Orders.Add(order2);
                db.SaveChanges();
                var detail = new OrderDetail()
                {
                    CourseId = course.Id,
                    OrderId = order.Id,
                    IsHidden = false,
                    Price = 100,
                    Order = order
                };
                db.OrderDetails.Add(detail);
                var detail1 = new OrderDetail()
                {
                    CourseId = course1.Id,
                    OrderId = order2.Id,
                    IsHidden = false,
                    Price = 100,
                    Order = order2
                };
                db.OrderDetails.Add(detail1);
                db.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("Index");

        }
        // GET: Profile
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
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
                    Points = user.Points,
                    Courses = new List<Course>()
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("login", "account");
            }

        }
        public ActionResult Courses()
        {
            var id = User.Identity.GetUserId();

            var user = db.Users.FirstOrDefault(u => u.Id == id);
            var model = new List<Course>();
            if (user != null)
            {
                var orders = db.Orders.Where(o => o.UserId == id).ToList();
                foreach (var item in orders)
                {
                    var orderDetails = db.OrderDetails.Where(o => o.OrderId == item.Id && o.IsHidden == false).ToList();
                    foreach (var course in orderDetails)
                    {
                        var courses = db.Courses.FirstOrDefault(c => c.Id == course.CourseId);
                        if (!model.Contains(courses))
                        {
                            model.Add(courses);
                        }
                    }
                }
            }
            return View(model);
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
                //Photo = applicationUser.Photo,
                
            };
            return View(user);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase Photo,UserEditRequest model)
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == id);
                if (TempData.ContainsKey("username"))
                {
                    var username = TempData["username"].ToString();
                    if (username != user.UserName && db.Users.FirstOrDefault(u => u.UserName == model.Username) != null)
                    {
                        ViewBag.message = "Username qe doni te vendosni ekziston ne sistem";
                        return View(model);
                    }
                    else
                    {
                        user.UserName = model.Username;
                    }

                }
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UpdatedById = id;
                user.UpdatedOn = DateTime.Now;
                if (Photo != null)
                {
                    Photo.SaveAs(HttpContext.Server.MapPath("~/UserPhotos/")
                                                          + Photo.FileName);
                    user.Photo = Photo.FileName;
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
