using Microsoft.EntityFrameworkCore;
using PantryApplication.Models;

namespace PantryApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Pantry> Pantry { get; set; }
    }
}
