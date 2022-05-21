using Course_Store.Models;
using Course_Store.Models.Responses;
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
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UsersList()
        {
            var role = db.Roles.Where(x => x.Name == "User").FirstOrDefault();
            var users = db.Users.Where(x => x.Roles.Any(r => r.RoleId == role.Id));
            var userList = new List<UserListView>();
            foreach (var item in users)
            {
                userList.Add(new UserListView()
                {
                    Email = item.Email,
                    Id = item.Id,
                    Name = item.Name,
                    Surname = item.Surname,
                    Username = item.UserName
                });
            }
            return View(userList);
        }
        public ActionResult TrainersList()
        {
            var trainerList = new List<TrainerListView>();
            trainerList = (from t in db.Trainers
                           join u in db.Users on t.User_Id equals u.Id
                           select new TrainerListView
                           {
                               Id = t.TrainderId,
                               Email = u.Email,
                               Name = u.Name,
                               Skills = t.Skills,
                               Surname = u.Surname,
                               YearsOfExperience = t.YearOfExperience
                           }).ToList();
            return View(trainerList);
        }
        public ActionResult AllOrders()
        {
            var orders = new List<UserOrdersListView>();
            orders = (from o in db.Orders
                      join od in db.OrderDetails on o.Id equals od.OrderId
                      select new UserOrdersListView()
                      {
                          OrderId = o.Id,
                          CourseName = od.CourseId.ToString(),
                          CreatedOn = o.CreatedOn,
                          PaymentMethod = o.PaymentMethod,
                          Price = od.Price,
                          UserId = o.UserId,
                          UpdatedOn = o.UpdatedOn
                      }).ToList();

            return View(orders);
        }
    }
}