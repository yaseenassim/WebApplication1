using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> products { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Order> Orders { get; set; }

        

    }
}
