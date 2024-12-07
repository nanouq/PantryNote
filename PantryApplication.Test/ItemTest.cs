using Microsoft.EntityFrameworkCore;
using PantryApplication.Data;
using PantryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryApplication.Test
{
    public class ItemTest
    {
        [Fact]
        public void ItemMethods_ValidData_PerishableReturnsCorrectExpiration()
        {
            var item = new PerishableItem
            {
                Id = 1,
                Name = "Milk",
                ExpirationDate = DateTime.Now.AddDays(-1),
                Category = "Dairy",
                Location = "Fridge",
                Unit = "Carton"
            };

            var isExpired = item.IsExpired();

            Assert.Equal("Milk is past expiration and could be expired. Check for signs of spoilage before consuming.", isExpired);
        }

        [Fact]
        public void ItemMethods_ValidData_NonPerishableReturnsCorrectExpiration()
        {
            var item = new NonPerishableItem
            {
                Id = 1,
                Name = "Pasta",
                ExpirationDate = DateTime.Now.AddDays(-1),
                Category = "Dry Goods",
                Location = "Pantry",
                Unit = "Box",
                IsSealed = true
            };

            var isExpired = item.IsExpired();

            Assert.Equal("Pasta is past expiration date, but still sealed. This item should be safe to eat, but check for signs of spoilage before consuming.", isExpired);
        }

        [Fact]
        public void ItemMethods_ValidData_PerishableReturnsCorrectStorage()
        {
            var item = new PerishableItem
            {
                Id = 1,
                Name = "Milk",
                ExpirationDate = DateTime.Now,
                Category = "Dairy",
                Location = "Fridge",
                Unit = "Carton",
                RequiresRefrigeration = true
            };

            var storage = item.GetStorageInstructions();

            Assert.Equal("Store Milk in the refrigerator for optimal freshness.", storage);
        }

        [Fact]
        public void ItemMethods_ValidData_NonPerishableReturnsCorrectStorage()
        {
            var item = new NonPerishableItem
            {
                Id = 1,
                Name = "Pasta",
                ExpirationDate = DateTime.Now.AddDays(-1),
                Category = "Dry Goods",
                Location = "Pantry",
                Unit = "Box",
                IsSealed = true
            };

            var storage = item.GetStorageInstructions();

            Assert.Equal("Store Pasta in a cool, dry place.", storage);
        }
    }
}
