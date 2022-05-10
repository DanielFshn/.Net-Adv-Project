using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Store.Repositories
{
    public interface IContact
    {
        void Edit(Contact contact);
        Contact Details(int id);
        void Delete(Contact contact);
        void Create(Contact contact);
        void Save();
        List<Contact> Index();
    }
}
