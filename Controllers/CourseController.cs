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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Course_Store.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Course
        public ActionResult Index()
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

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return HttpNotFound();
            }
            return View(courseView);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var categories = db.CourseCategories.ToList();
            foreach (var cat in categories)
            {
                list.Add(new SelectListItem() { Value = cat.CategotyType, Text = cat.CategotyType });
            }
            ViewBag.Category = list;
            //ViewBag.Category = new SelectList(db.CourseCategories, "Id", "CategotyType");
            //ViewBag.TrainerId = new SelectList(db.Trainers, "TrainderId", "TrainderId");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseAddRequest model)
        {
            var fileName = "/CoursePhotos/" + Guid.NewGuid() + "_" + model.UploadImage.FileName;
            var id = User.Identity.GetUserId();
            var trainerId = db.Trainers.FirstOrDefault(x => x.User_Id == id);
            if (ModelState.IsValid)
            {
                var course = new Course()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Image = fileName,
                    Price = model.Price,
                    Objectives = model.Objectives,
                    IsPublish = model.IsPublish,
                    CourseCategory = model.Category,
                    CreatedOn = DateTime.Now,
                    CategoryId = model.Category.Id,
                    TrainerId = trainerId.TrainderId
                };
                try
                {
                    model.UploadImage.SaveAs(Server.MapPath(fileName));
                }
                catch(Exception) { throw; }
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
                Title = course.Title
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
            var fileName = "/CoursePhotos/" + Guid.NewGuid() + "_" + model.UploadImage.FileName;
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var trainerId = db.Trainers.FirstOrDefault(t => t.User_Id == userId);
                var course = db.Courses.FirstOrDefault(c => c.TrainerId == trainerId.TrainderId);
                var categoryType = model.Category.CategotyType;
                var category = db.CourseCategories.FirstOrDefault(c => c.CategotyType == categoryType);
                if(model.UploadImage != null)
                {
                    try
                    {
                        model.UploadImage.SaveAs(Server.MapPath(fileName));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                course.UpdatedById = trainerId.TrainderId;
                course.UpdatedOn = DateTime.Now;
                course.Title = model.Title;
                course.Price = model.Price;
                course.IsPublish = model.IsPublish;
                course.Objectives = model.Objectives;
                course.Description = model.Description;
                course.CategoryId = category.Id;
                course.Image = fileName;
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
