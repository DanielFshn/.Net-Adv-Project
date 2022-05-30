using Microsoft.VisualStudio.TestTools.UnitTesting;
using Course_Store.Controllers;
using ProgressDemo.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Course_Store.Models.Requests;

namespace ProgressDemoUnitTest
{

    [TestClass]
    public class HomeTests
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [TestMethod]
        //public void HomeIndexTest()
        //{
        //    HomeController controller = new HomeController();
        //    var result = controller.Index() as ViewResult;
        //    Assert.IsNotNull(result);
        //}
        [TestMethod]
        public void HomeAboutTest()
        {
            HomeController controller = new HomeController();
            var result = controller.About() as ViewResult;
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }
        [TestMethod]
        public void HomeContactTest()
        {
            HomeController controller = new HomeController();
            var result = controller.Contact() as ViewResult;
            Assert.IsNotNull(result);
        }
    }   
}
