using Course_Store.Controllers;
using Course_Store.Models;
using Course_Store.Models.Requests;
using Course_Store.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Course_Store.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace ProgressDemoUnitTest
{
    [TestClass]
    public class CourseControllerTests_v
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [TestMethod]
        public void CourseInstanceNotEqual()
        {
            var course = db.Courses.Find(11);
            var result = new Course();
            Assert.AreNotEqual(course, result);
        }
        [TestMethod]
        public void CourseExistiongId()
        {
            var courses = db.Courses.ToList();
            var result = db.Courses.FirstOrDefault(x=> x.Id ==11);
            foreach (var item in courses)
            {
                if (item.Id == result.Id)
                    Assert.AreEqual(item.Id, result.Id);
            }
        }
        [TestMethod]
        public void CreatinNullCourseFails()
        {
            CourseController controller = new CourseController();
            var result = controller.Create() as ViewResult;
            if(result == null)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void CourseCreateTest()
        {
            var controller = new CourseController();
            var result = controller.Create() as ViewResult;
            //var product = (Course)result.ViewData.Model;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CourseDeleteTest()
        {
            var controller = new CourseController();
            var result = controller.Delete(null) as ViewResult;
            Assert.IsNull(result);
        }
        [TestMethod]
        public void CourseDetailsTest()
        {
            var controller = new CourseController();
            var result = controller.Details(null) as ViewResult;
            if (result != null)
                Assert.IsNotNull(result);
            else
                Assert.Fail();
        }
        [TestMethod]
        public void Course_Delete_Test()
        {
            var controller = new CourseController();
            var result = controller.Delete(null) as ViewResult;
            Assert.IsNull(result);
        }

    }
}
