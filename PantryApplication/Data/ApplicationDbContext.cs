using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PantryApplication.Models;

namespace PantryApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Pantry> Pantry { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pantry>().HasData(
                new Pantry { Id = 1, UserId = "39d77c7b-2212-4ded-85cf-9f86cb9d18df" }
                );

            modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<PerishableItem>("Perishable")
                .HasValue<NonPerishableItem>("NonPerishable");

            modelBuilder.Entity<PerishableItem>().HasData(
                new PerishableItem { 
                    Id = 1, 
                    Name = "Milk", 
                    Brand = "Fairlife", 
                    Category = "Dairy", 
                    ExpirationDate = DateTime.Today.AddDays(5), 
                    DateAdded = DateTime.Today, 
                    Quantity = 2, 
                    Unit = "Liters", 
                    Location = "Fridge", 
                    PantryId = 1, 
                    RequiresRefrigeration = true, 
                    RequiresFreezing = false
                });

            modelBuilder.Entity<NonPerishableItem>().HasData(
                new NonPerishableItem
                {
                    Id = 2,
                    Name = "Baked Beans",
                    Brand = "Buschs",
                    Category = "Canned Goods",
                    ExpirationDate = DateTime.Today.AddMonths(12),
                    DateAdded = DateTime.Today,
                    Quantity = 3,
                    Unit = "Cans",
                    Location = "Pantry",
                    PantryId = 1,
                    IsSealed = true
                });
        }
    }
}
