using Course_Store.Models;
using Course_Store.Models.Requests;
using Course_Store.Models.Responses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Store.Controllers
{
    public class TrainerPannelController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Trainer")]
        // GET: TrainerPannel
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var trainer = db.Trainers.FirstOrDefault(x => x.User_Id == id);
            var user = db.Users.Find(id);
            var trainerPannelView = new TrainerPannel()
            {
                Birthday = user.Birthday,
                Email = user.Email,
                Id = trainer.TrainderId,
                Image = user.Photo,
                Name = user.Name,
                Skills = trainer.Skills,
                Surname = user.Surname,
                YearOfExperience = trainer.YearOfExperience
            };
            return View(trainerPannelView);
        }
        public PartialViewResult Courses()
        {
            var id = User.Identity.GetUserId();
            var trainer = db.Trainers.FirstOrDefault(x => x.User_Id == id);
            var courses = db.Courses.Where(x => x.TrainerId == trainer.TrainderId).ToList();
            var coursesView = new List<CourseAddRequest>();
            foreach (var item in courses)
            {
                if (!item.IsPublish)
                {
                    var category = db.CourseCategories.Find(item.CategoryId);
                    coursesView.Add(new CourseAddRequest()
                    {
                        Category = category,
                        Description = item.Description,
                        Id = item.Id,
                        Image = item.Image,
                        IsPublish = item.IsPublish,
                        Objectives = item.Objectives,
                        Price = item.Price,
                        Title = item.Title,
                    });
                }
            }
            return PartialView("_Courses", coursesView);
        }
        public PartialViewResult PubishedCourses()
        {
            var id = User.Identity.GetUserId();
            var trainer = db.Trainers.FirstOrDefault(x => x.User_Id == id);
            var courses = db.Courses.Where(x => x.TrainerId == trainer.TrainderId).ToList();
            var coursesView = new List<CourseAddRequest>();
            foreach (var item in courses)
            {
                if (item.IsPublish)
                {
                    var category = db.CourseCategories.Find(item.CategoryId);
                    coursesView.Add(new CourseAddRequest()
                    {
                        Category = category,
                        Description = item.Description,
                        Id = item.Id,
                        Image = item.Image,
                        IsPublish = item.IsPublish,
                        Objectives = item.Objectives,
                        Price = item.Price,
                        Title = item.Title,
                    });
                }
            }
            return PartialView("_PubishedCourses", coursesView);
        }
    }
}