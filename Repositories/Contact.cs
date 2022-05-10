using Course_Store.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Course_Store.Repositories
{
    public class Contact : IContact
    {
        public int Id { get; set; }
        public string Email{ get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser User { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

        public void Edit(Contact contact)
        {
            db.Entry(contact).State = System.Data.Entity.EntityState.Modified;
        }

        public Contact Details(int id)
        {
            return db.Contacts.Find(id);
        }

        public void Delete(Contact contact)
        {
            db.Contacts.Remove(contact);
        }

        public void Create(Contact contact)
        {
            db.Contacts.Add(contact);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public List<Contact> Index()
        {
            return db.Contacts.ToList();
        }
    }
}