using Course_Store.Models;
using Course_Store.Models.Responses;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Course_Store.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Role
        public ActionResult Index()
        {
            var models = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
                models.Add(new RoleViewModel(role));
            return View(models);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var role = RoleManager.Roles.FirstOrDefault(x => x.Id == id);
            var roleView = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleView);
        }
        [HttpPost]
        public ActionResult Edit(string id, RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var r = new IdentityRole()
                    {
                        Id = role.Id,
                        Name = role.Name
                    };
                    //RoleManager.UpdateAsync(r);
                    db.Entry(r).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(role);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new IdentityRole()
            {
                Name = model.Name
            };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);
                await RoleManager.DeleteAsync(role);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}