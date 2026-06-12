using Microsoft.EntityFrameworkCore;
using SmartLeadAI.Models;

namespace SmartLeadAI.Data
{
    public class SmartLeadContext : DbContext
    {
        public SmartLeadContext(DbContextOptions<SmartLeadContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Interaction> Interactions { get; set; }
    }
}