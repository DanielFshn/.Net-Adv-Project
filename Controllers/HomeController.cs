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
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var categories = db.CourseCategories.ToList();
            ViewBag.Categories = categories;
            var userId = User.Identity.GetUserId();
            var courses = db.Courses.Where(x => x.IsPublish == true).OrderByDescending(x => x.CreatedOn).ToList();
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
            try
            {
                if (courses != null && courses.Count() >= 6)
                {
                    for (int i = 0; i < courses.Count; i++)
                    {
                        var orderDet = orderDetails.FirstOrDefault(x => x.CourseId == courses[i].Id);
                        if (orderDet == null)
                        {
                            courseView.Add(new CourseAddRequest()
                            {
                                Id = courses[i].Id,
                                Category = courses[i].CourseCategory,
                                Description = courses[i].Description,
                                Image = courses[i].Image,
                                IsPublish = courses[i].IsPublish,
                                Objectives = courses[i].Objectives,
                                Points = (int)courses[i].Points,
                                Price = courses[i].Price,
                                Title = courses[i].Title
                            });
                            if (courseView.Count() >= 6)
                            {
                                break;
                            }
                        }
                    }
                }
                ViewBag.Courses = courseView;
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "User")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}