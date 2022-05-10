using Course_Store.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Course_Store.Controllers
{
    [Authorize(Roles ="User")]
    public class ContactsController : Controller
    {
        private IContact contact;
        public ContactsController()
        {
            this.contact = new Contact();
        }
        
        // GET: Contacts
        public ActionResult Index()
        {
            return View(contact.Index());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var c = contact.Details((int)id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                contact.User_Id = userId;
                contact.CreatedOn = DateTime.Now;
                IContact c = new Contact();
                c.Create(contact);
                c.Save();
                return RedirectToAction("Index","Home");
            }

            return View(contact);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var c = contact.Details((int)id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                this.contact.Edit(contact);
                this.contact.Save();
                return RedirectToAction("Index");
            }
            return View(contact);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var c = this.contact.Details((int)id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var c = this.contact.Details(id);
            contact.Delete(c);
            contact.Save();
            return RedirectToAction("Index");
        }

    }
}