using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Course_Store.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult Unauthorized()
        {
            return View();
        }
        public ActionResult Forbidden()
        {
            return View();
        }
        public ActionResult MethodNotAllowed()
        {
            return View();
        }
        public ActionResult NotAcceptable()
        {
            return View();
        }
        public ActionResult PreconditionFailed()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }
        public ActionResult NotImplemented()
        {
            return View();
        }
        public ActionResult BadGateway()
        {
            return View();
        }
    }
}