using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Model.Models;
using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products{get; set;}
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts{get; set;}
        public DbSet<OrderDetails> orderDetails{get; set;}
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category {Id = 1, Name = "Action", DisplayOrder = 1},
                new Category {Id = 2, Name = "SciFi", DisplayOrder = 2},
                new Category {Id = 3, Name = "History", DisplayOrder = 3}
            );

            modelBuilder.Entity<Company>().HasData(
                new Company {Id = 1, Name = "Test", City = "Test", StreetAddress = "Test", State = "Test", PostalCode = "Test"},
                new Company {Id = 2, Name = "Test", City = "Test", StreetAddress = "Test", State = "Test", PostalCode = "Test"},
                new Company {Id = 3, Name = "Test", City = "Test", StreetAddress = "Test", State = "Test", PostalCode = "Test"}
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1, 
                    Title = "Fortune of Time", 
                    Author="Billy Spark", 
                    Description= "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN="SWD9999001",
                    ListPrice=99,
                    Price=90,
                    Price50=85,
                    Price100=80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 1,
                    ImageUrl = ""
                }
            );
        }
    }
}