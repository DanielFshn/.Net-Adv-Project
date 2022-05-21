using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Course_Store.Models;
using Course_Store.Models.Requests;
using Course_Store.Models.Responses;
using Loggers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Course_Store.Controllers
{
    //[Authorize(Roles = "Trainer,User")]
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private LoggerService logger;
        public CourseController()
        {
            logger = new LoggerService();
        }
        // GET: Course
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Id = new SelectList(db.CourseCategories, "Id", "CategotyType");


            var userId = User.Identity.GetUserId();
            if (userId != null && User.IsInRole("Admin"))
            {
                var user = db.Users.Find(userId);
                var role = db.Roles.FirstOrDefault(x => x.Name == "Admin");
                if (user.Roles.Any(x => x.RoleId == role.Id))
                {
                    var courseView = new List<CourseAddRequest>();
                    var courses = db.Courses.Include(c => c.CourseCategory).Include(c => c.CourseTrainer).ToList();
                    foreach (var item in courses.ToList())
                    {
                        var trainer = db.Trainers.FirstOrDefault(c => c.TrainderId == item.TrainerId);
                        var publisherName = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
                        courseView.Add(new CourseAddRequest()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Category = item.CourseCategory,
                            Description = item.Description,
                            Objectives = item.Objectives,
                            Price = item.Price,
                            Image = item.Image,
                            IsPublish = item.IsPublish,
                            TrainerName = publisherName.Name
                        });
                    }
                    return View(courseView);
                }
            }
            if (userId == null)
            {
                var courseView = new List<CourseAddRequest>();
                var courses = db.Courses.Include(c => c.CourseCategory).Include(c => c.CourseTrainer).ToList();
                foreach (var item in courses.ToList())
                {
                    if (item.IsPublish)
                    {
                        var trainer = db.Trainers.FirstOrDefault(c => c.TrainderId == item.TrainerId);
                        var publisherName = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
                        courseView.Add(new CourseAddRequest()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Category = item.CourseCategory,
                            Description = item.Description,
                            Objectives = item.Objectives,
                            Price = item.Price,
                            Image = item.Image,
                            IsPublish = item.IsPublish,
                            TrainerName = publisherName.Name
                        });
                    }
                }
                return View(courseView);
            }
            else
            {
                var courseView = new List<CourseAddRequest>();
                var orders = db.Orders.Where(o => o.UserId == userId).ToList();
                if (orders.Count() == 0)
                {
                    var courses = db.Courses.Where(x => x.IsPublish == true).ToList();
                    foreach (var c in courses)
                    {
                        courseView.Add(new CourseAddRequest()
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Category = c.CourseCategory,
                            Description = c.Description,
                            Objectives = c.Objectives,
                            Price = c.Price,
                            Image = c.Image,
                            IsPublish = c.IsPublish
                        });
                    }
                    return View(courseView);
                }
                else
                {
                    var courses = db.Courses.Where(p => p.IsPublish == true).ToList();
                    //var courseView = new List<CourseAddRequest>();
                    var orders2 = db.Orders.Where(x => x.UserId == userId).ToList();
                    var orderDetails = new List<OrderDetailList>();
                    foreach(var item in orders2)
                    {
                        var orderD = db.OrderDetails.FirstOrDefault(x => x.OrderId == item.Id);
                        orderDetails.Add(new OrderDetailList()
                        {
                            OrderDetailId = orderD.Id,
                            CourseId = orderD.CourseId
                        });
                    }
                    foreach (var item in courses)
                    {
                        try
                        {
                            //var orderDet = db.OrderDetails.FirstOrDefault(x => x.CourseId == item.Id);
                            var orderDet = orderDetails.FirstOrDefault(x => x.CourseId == item.Id);
                            if (orderDet == null)
                            {
                                courseView.Add(new CourseAddRequest()
                                {
                                    Id = item.Id,
                                    Title = item.Title,
                                    Category = item.CourseCategory,
                                    Description = item.Description,
                                    Objectives = item.Objectives,
                                    Price = item.Price,
                                    Image = item.Image,
                                    IsPublish = item.IsPublish
                                });
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    return View(courseView);
                }
            }
        }
        [HttpPost]
        public ActionResult Index(int Id)
        {
            ViewBag.Id = new SelectList(db.CourseCategories, "Id", "CategotyType");
            var userId = User.Identity.GetUserId();
            if (userId != null && User.IsInRole("Admin"))
            {
                var user = db.Users.Find(userId);
                var role = db.Roles.FirstOrDefault(x => x.Name == "Admin");
                if (user.Roles.Any(x => x.RoleId == role.Id))
                {
                    var courseView = new List<CourseAddRequest>();
                    var courses = db.Courses.Where(x => x.CategoryId == Id).ToList();
                    foreach (var item in courses.ToList())
                    {
                        var trainer = db.Trainers.FirstOrDefault(c => c.TrainderId == item.TrainerId);
                        var publisherName = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
                        courseView.Add(new CourseAddRequest()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Category = item.CourseCategory,
                            Description = item.Description,
                            Objectives = item.Objectives,
                            Price = item.Price,
                            Image = item.Image,
                            IsPublish = item.IsPublish,
                            TrainerName = publisherName.Name
                        });
                    }
                    return View(courseView);
                }
            }
            if (userId == null)
            {
                var courseView = new List<CourseAddRequest>();
                var courses = db.Courses.Where(x => x.CategoryId == Id).ToList();
                foreach (var item in courses.ToList())
                {
                    if (item.IsPublish)
                    {
                        var trainer = db.Trainers.FirstOrDefault(c => c.TrainderId == item.TrainerId);
                        var publisherName = db.Users.FirstOrDefault(u => u.Id == trainer.User_Id);
                        courseView.Add(new CourseAddRequest()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Category = item.CourseCategory,
                            Description = item.Description,
                            Objectives = item.Objectives,
                            Price = item.Price,
                            Image = item.Image,
                            IsPublish = item.IsPublish,
                            TrainerName = publisherName.Name
                        });
                    }
                }
                return View(courseView);
            }
            else
            {
                var courses = db.Courses.Where(x => x.CategoryId == Id).Where(p => p.IsPublish == true).ToList();
                var courseView = new List<CourseAddRequest>();
                var orders = db.Orders.Where(x => x.UserId == userId).ToList();
                var orderDetails = new List<OrderDetailList>();
                orderDetails = (from o in db.Orders
                                where o.UserId == userId
                                join od in db.OrderDetails on o.Id equals od.OrderId
                                select new OrderDetailList
                                {
                                   OrderDetailId = od.Id,
                                   CourseId = od.CourseId
                                }).ToList();

                foreach (var item in courses)
                {
                    try
                    {
                        //var orderDet = db.OrderDetails.FirstOrDefault(x => x.CourseId == item.Id);
                        var orderDet = orderDetails.FirstOrDefault(x => x.CourseId == item.Id);
                        if (orderDet == null)
                        {
                            courseView.Add(new CourseAddRequest()
                            {
                                Id = item.Id,
                                Title = item.Title,
                                Category = item.CourseCategory,
                                Description = item.Description,
                                Objectives = item.Objectives,
                                Price = item.Price,
                                Image = item.Image,
                                IsPublish = item.IsPublish
                            });
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                return View(courseView);
            }
        }
        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Error/BadRequest");
            }
            //Course course = db.Courses.Find(id);
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            var category = db.CourseCategories.FirstOrDefault(cat => cat.Id == course.CategoryId);
            var courseView = new CourseAddRequest()
            {
                Id = (int)id,
                Category = category,
                Description = course.Description,
                Image = course.Image,
                IsPublish = course.IsPublish,
                Objectives = course.Objectives,
                Price = course.Price,
                Title = course.Title,
            };
            if (course == null)
            {
                return View("~/Error/PageNotFound");
            }
            return View(courseView);
        }
        [Authorize(Roles = "Trainer")]
        // GET: Course/Create
        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var categories = db.CourseCategories.ToList();
            foreach (var cat in categories)
            {
                list.Add(new SelectListItem() { Value = cat.Id.ToString(), Text = cat.CategotyType });
            }
            ViewBag.Category = list;
            
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseAddRequest model)
        {
            var category = db.CourseCategories.FirstOrDefault(c => c.Id == model.Category.Id);
            var fileName = "".Trim();
            try {
                if (model.UploadImage != null)
                    fileName = "~/Content/img/logo/" + Guid.NewGuid() + "_" + model.UploadImage.FileName;
                else
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    var categories = db.CourseCategories.ToList();
                    foreach (var cat in categories)
                    {
                        list.Add(new SelectListItem() { Value = cat.Id.ToString(), Text = cat.CategotyType });
                    }
                    ViewBag.Category = list;
                    ViewBag.ErrorMessage = "Please upload the image";
                    return View(model);
                }
            }
            catch (Exception) { return View(model); }
            var id = User.Identity.GetUserId();
            var trainerId = db.Trainers.FirstOrDefault(x => x.User_Id == id);
            if (ModelState.IsValid)
            {
                var courseModel = db.Courses.FirstOrDefault(x => x.Title == model.Title);
                if(courseModel!=null)
                {
                    ViewBag.ErrorMessage = "There is a course with this name";
                    List<SelectListItem> list = new List<SelectListItem>();
                    var categories = db.CourseCategories.ToList();
                    foreach (var cat in categories)
                    {
                        list.Add(new SelectListItem() { Value = cat.Id.ToString(), Text = cat.CategotyType });
                    }
                    ViewBag.Category = list;
                    return View(model);
                }
                var course = new Course()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Image = fileName,
                    Price = model.Price,
                    Objectives = model.Objectives,
                    IsPublish = model.IsPublish,
                    CreatedOn = DateTime.Now,
                    CategoryId = category.Id,
                    TrainerId = trainerId.TrainderId,
                    Points = model.Points
                };
                try
                {
                    model.UploadImage.SaveAs(Server.MapPath(fileName));
                }
                catch (Exception) { throw; }
                logger.Info(db.Users.Find(id).Email, "Course Added Sucessfuly", "Course/Create");
                ViewBag.Category = new SelectList(db.CourseCategories, "Id", "CategoryType", category.Id);
                db.Courses.Add(course);
                db.SaveChanges();
                //ViewBag.CategoryId = new SelectList(db.CourseCategories, "Id", "CategotyType", course.CategoryId);
                //ViewBag.TrainerId = new SelectList(db.Trainers, "TrainderId", "TrainderId", course.TrainerId);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            var category = db.CourseCategories.FirstOrDefault(cat => cat.Id == course.CategoryId);
            var courseView = new CourseAddRequest()
            {
                Id = (int)id,
                Category = category,
                Description = course.Description,
                Image = course.Image,
                IsPublish = course.IsPublish,
                Objectives = course.Objectives,
                Price = course.Price,
                Title = course.Title,
                Points = (int)course.Points
            };
            if (course == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CategoryId = new SelectList(db.CourseCategories, "Id", "CategotyType", course.CategoryId);
            //ViewBag.TrainerId = new SelectList(db.Trainers, "TrainderId", "Skills", course.TrainerId);
            List<SelectListItem> list = new List<SelectListItem>();
            var categories = db.CourseCategories.ToList();
            foreach (var cat in categories)
            {
                list.Add(new SelectListItem() { Value = cat.CategotyType, Text = cat.CategotyType });
            }
            ViewBag.Category = list;
            return View(courseView);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseAddRequest model)
        {
            //var fileName = "/CoursePhotos/" + Guid.NewGuid() + "_" + model.UploadImage.FileName;
            var fileName = ("~/CoursePhotos/" + model.Image).Trim();
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var trainerId = db.Trainers.FirstOrDefault(t => t.User_Id == userId);
                //var course = db.Courses.FirstOrDefault(c => c.TrainerId == trainerId.TrainderId);
                var course = db.Courses.Where(x => x.TrainerId == trainerId.TrainderId && x.Id == model.Id).FirstOrDefault();
                var categoryType = model.Category.CategotyType;
                var category = db.CourseCategories.FirstOrDefault(c => c.CategotyType == categoryType);
                if (model.Image != null)
                {
                    course.Image = fileName;
                }
                course.UpdatedById = trainerId.TrainderId;
                course.UpdatedOn = DateTime.Now;
                course.Title = model.Title;
                course.Price = model.Price;
                course.IsPublish = model.IsPublish;
                course.Objectives = model.Objectives;
                course.Description = model.Description;
                course.CategoryId = category.Id;
                course.Points = model.Points;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CategoryId = new SelectList(db.CourseCategories, "Id", "CategotyType", course.CategoryId);
            //ViewBag.TrainerId = new SelectList(db.Trainers, "TrainderId", "Skills", course.TrainerId);
            return View(model);
        }
        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            //var category = db.CourseCategories.FirstOrDefault(c => c.Id == course.CategoryId);
            var courseView = new CourseAddRequest()
            {
                Id = (int)id,
                Category = course.CourseCategory,
                Description = course.Description,
                Image = course.Image,
                IsPublish = course.IsPublish,
                Objectives = course.Objectives,
                Price = course.Price,
                Title = course.Title
            };
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(courseView);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            var userId = User.Identity.GetUserId();
            var trainer = db.Trainers.FirstOrDefault(t => t.User_Id == userId);
            course.DeletedById = trainer.TrainderId;
            course.DeletedOn = DateTime.Now;
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles="Admin")]
        public ActionResult PublishedCourses()
        {
            var courses = db.Courses.Where(x => x.IsPublish == true).ToList();
            var courseView = new List<CourseAddRequest>();
            foreach (var item in courses)
            {
                courseView.Add(new CourseAddRequest()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Category = item.CourseCategory,
                    Description = item.Description,
                    Objectives = item.Objectives,
                    Price = item.Price,
                    Image = item.Image,
                    IsPublish = item.IsPublish,
                });
            }
            return View(courseView);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UnpublishedCourses()
        {
            var courses = db.Courses.Where(x => x.IsPublish == false).ToList();
            var courseView = new List<CourseAddRequest>();
            foreach (var item in courses)
            {
                courseView.Add(new CourseAddRequest()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Category = item.CourseCategory,
                    Description = item.Description,
                    Objectives = item.Objectives,
                    Price = item.Price,
                    Image = item.Image,
                    IsPublish = item.IsPublish,
                });
            }
            return View(courseView);
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
