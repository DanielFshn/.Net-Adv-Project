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
    public class TrainerPannelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Trainer")]
        // GET: TrainerPannels
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var user = db.Users.Find(id);
            var trainer = db.Trainers.FirstOrDefault(u => u.User_Id == id);
            var trainerView = new TrainerPannel()
            {
                Name = user.Name,
                Birthday = user.Birthday,
                Email = user.Email,
                Id = trainer.TrainderId,
                Image = user.Photo,
                Skills = trainer.Skills,
                Surname = user.Surname,
                YearOfExperience = trainer.YearOfExperience
            };
            ViewData["courses"] = TempData["courses"];
            return View(trainerView);
        }
        [HttpPost]
        public ActionResult Courses(int id)
        {
            var courseView = new List<CourseAddRequest>();
            var courses = db.Courses.Where(x => x.TrainerId == id).ToList();
            foreach (var item in courses)
            {
                var category = db.CourseCategories.Find(item.CategoryId);
                courseView.Add(new CourseAddRequest()
                {
                    Category = category,
                    Description = item.Description,
                    Id = item.Id,
                    Image = item.Image,
                    IsPublish = item.IsPublish,
                    Objectives = item.Objectives,
                    Price = item.Price,
                    Title = item.Title
                });
            }
            TempData["courses"] = courseView;
            return RedirectToAction("Index");
        }
        //// GET: TrainerPannels/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //TrainerPannel trainerPannel = db.TrainerPannels.Find(id);
        //    if (trainerPannel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trainerPannel);
        //}

        //// GET: TrainerPannels/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TrainerPannels/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,Surname,Email,YearOfExperience,Skills,Image")] TrainerPannel trainerPannel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TrainerPannels.Add(trainerPannel);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(trainerPannel);
        //}

        //// GET: TrainerPannels/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TrainerPannel trainerPannel = db.TrainerPannels.Find(id);
        //    if (trainerPannel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trainerPannel);
        //}

        //// POST: TrainerPannels/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,Surname,Email,YearOfExperience,Skills,Image")] TrainerPannel trainerPannel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(trainerPannel).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(trainerPannel);
        //}

        //// GET: TrainerPannels/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TrainerPannel trainerPannel = db.TrainerPannels.Find(id);
        //    if (trainerPannel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trainerPannel);
        //}

        //// POST: TrainerPannels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TrainerPannel trainerPannel = db.TrainerPannels.Find(id);
        //    db.TrainerPannels.Remove(trainerPannel);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
