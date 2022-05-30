using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Course_Store.Models;
using Course_Store.Models.Responses;
using Loggers;
using Microsoft.AspNet.Identity;

namespace Course_Store.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private LoggerService logger;
        public OrderController()
        {
            logger = new LoggerService();
        }
        // GET: Order
        public ActionResult Index()
        {
            var orderView = new List<UserOrdersListView>();
            var id = User.Identity.GetUserId();
            orderView = (from o in db.Orders
                         where o.UserId == id
                         where o.UserId == id
                         join d in db.OrderDetails on o.Id equals d.OrderId
                         join c in db.Courses on d.CourseId equals c.Id
                         select new UserOrdersListView
                         {
                             CreatedOn = o.CreatedOn,
                             OrderId = o.Id,
                             Price = d.Price,
                             CourseName = c.Title,
                             PaymentMethod = o.PaymentMethod,
                             OrderStatus = OrderStatus.Completed
                         }).ToList();
            return View(orderView.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            var orderDetails = db.OrderDetails.FirstOrDefault(od => od.OrderId == order.Id);
            var course = db.Courses.Find(orderDetails.CourseId);
            var orderView = new UserOrdersListView()
            {
                CourseName = course.Title,
                CreatedOn = order.CreatedOn,
                OrderId = order.Id,
                OrderStatus = OrderStatus.Created,
                PaymentMethod = PaymentMethod.None,
                Price = orderDetails.Price
            };
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(orderView);
        }
        [HttpPost]
        public string AddToCart(int id)
        {
            var course = db.Courses.Find(id);
            var courseView = new CourseListView()
            {
                Description = course.Description,
                Image = course.Image,
                Price = course.Price,
                Title = course.Title,
                Id = course.Id
            };
            var list = (List<CourseListView>)Session["cart"];
            if (list != null)
            {
                if (list.Count() == 0)
                {
                    list.Add(courseView);
                    Session["cart"] = list;
                    return "The course was successfully added";
                }
                else
                {
                    foreach (var item in list)
                    {
                        if (item.Id != courseView.Id)
                        {
                            list.Add(courseView);
                            Session["cart"] = list;
                            return "The course was successfully added";
                        }
                    }
                }
            }
            return "Course is added once";
        }
        public ActionResult Cart()
        {
            var courseView = (List<CourseListView>)Session["cart"];
            return View(courseView);
        }
        public ActionResult RemoveFromCart(int id)
        {
            try
            {
                var list = (List<CourseListView>)Session["cart"];
                var newList = list.Where(x => x.Id != id).ToList();
                Session["cart"] = newList;
                return RedirectToAction("Cart");
            }
            catch (Exception)
            {
            }
            return View();
        }
        public ActionResult PlaceOrder()
        {
            var userId = User.Identity.GetUserId();
            var order = new Order()
            {
                UserId = userId,
                PaymentMethod = PaymentMethod.CreditCard,
                CreatedOn = DateTime.Now
            };
            if ((List<CourseListView>)Session["cart"] != null)
            {
                foreach (var item in (List<CourseListView>)Session["cart"])
                {
                    var orderDet = new OrderDetail()
                    {
                        CourseId = item.Id,
                        Price = item.Price,
                        OrderId = order.Id
                    };
                    db.OrderDetails.Add(orderDet);
                    order.Details.Add(orderDet);
                }
                db.Orders.Add(order);
                db.SaveChanges();
                logger.Info(db.Users.Find(userId).Email, "PlaceOrder Succesfuly", "Order/PlaceOrder");
                return RedirectToAction("Index", "Order", new { message = "The order was successfully completed" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { message = "The order is not completed" });
            }
            //Session.Clear();
        }
        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,AdminId,CreatedOn,UpdatedOn,DeletedOn,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,AdminId,CreatedOn,UpdatedOn,DeletedOn,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
