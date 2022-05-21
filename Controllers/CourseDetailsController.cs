using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Course_Store.Models;
using Microsoft.AspNet.Identity;

namespace Course_Store.Controllers
{
    public class CourseDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: CourseDetails
        public ActionResult Index()
        {
            var courseDetails = db.CourseDetails.Include(c => c.Course);
            return View(courseDetails.ToList());
        }
        [Authorize(Roles ="User")]
        public ActionResult SeeContent(int id)
        {
            var userId = User.Identity.GetUserId();
            var detail = db.CourseDetails.FirstOrDefault(x => x.Course_Id == id);
            var orders = db.Orders.Where(u => u.UserId == userId).ToList();
            List<int> courseId = new List<int>();
            foreach(var item in orders)
            {
                var orderDetails = db.OrderDetails.Where(x => x.OrderId == item.Id);
                foreach(var od in orderDetails)
                {
                    courseId.Add(od.CourseId);
                }
            }
            if(!courseId.Contains(id))
            {
                return PartialView("_NotAllowed",detail);
            }
            var progress = new Progress()
            {
                ProgressStatus = ProgressStatus.Started,
                CourseDetail_Id = detail.Id,
                StaretdTime = DateTime.Now,
            };
            Session["progress"] = progress;
            return View(detail);
        }
        public PartialViewResult AddPoints(int id)
        {
            var userId = User.Identity.GetUserId();
            var detail = db.CourseDetails.Find(id);
            var courseId = detail.Course_Id;
            var course = db.Courses.Find(courseId);
            try
            {
                //var prog = db.Progresses.FirstOrDefault(x => x.CourseDetail_Id == id);
                var prog = db.Progresses.Where(x => x.CourseDetail_Id == id && x.User_Id == userId).FirstOrDefault();
                if(prog != null)
                {
                    return PartialView("_UpdateProgress");
                }
            }
            catch (Exception)
            { }
            var progress = (Progress)Session["progress"];
            progress.EndTime = DateTime.Now;
            progress.Points = (int)course.Points;
            progress.User_Id = userId;
            db.Progresses.Add(progress);
            db.SaveChanges();
            return PartialView("_AddPoints");
        }
        // GET: CourseDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            return View(courseDetail);
        }
        [Authorize(Roles ="Trainer,Admin")]
        // GET: CourseDetails/Create
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: CourseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseDetail courseDetail,int id)
        {
            try
            {
                var course = db.Courses.Find(id);
                if(course.IsPublish)
                {
                    return RedirectToAction("MethodNotAllowed", "Error");
                }
            }
            catch{}
            if (ModelState.IsValid)
            {
                var course = db.Courses.Find(id);
                course.IsPublish = true;
                courseDetail.Course_Id = course.Id;
                course.Details.Add(courseDetail);
                db.CourseDetails.Add(courseDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.Course_Id = new SelectList(db.Courses, "Id", "Title", courseDetail.Course_Id);
            return View(courseDetail);
        }

        // GET: CourseDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Id = new SelectList(db.Courses, "Id", "Title", courseDetail.Course_Id);
            return View(courseDetail);
        }

        // POST: CourseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VideoPath,Course_Id")] CourseDetail courseDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course_Id = new SelectList(db.Courses, "Id", "Title", courseDetail.Course_Id);
            return View(courseDetail);
        }

        // GET: CourseDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            return View(courseDetail);
        }

        // POST: CourseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            db.CourseDetails.Remove(courseDetail);
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
