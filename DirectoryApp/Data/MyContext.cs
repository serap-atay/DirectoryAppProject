using DirectoryApp.Models.Entities;
using DirectoryApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Emit;

namespace DirectoryApp.Data
{
    public class MyContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>().HasQueryFilter(c => !c.isDeleted);
            builder.Entity<Contact>()
            .Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Contact>()
                .Property(x => x.Name).IsRequired();
            builder.Entity<Contact>()
            .Property(x => x.SurName).IsRequired();
            builder.Entity<Contact>()
            .Property(x => x.Phone).IsRequired();


        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
