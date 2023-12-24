using DirectoryApp.Data;
using DirectoryApp.Models.Entities;
using DirectoryApp.Repository.Abstracts;

namespace DirectoryApp.Repository
{
    public class ContactRepo : RepositoryBase<Contact, int>
    {
        public ContactRepo(MyContext myContext) : base(myContext)
        {
        }
    }
}
