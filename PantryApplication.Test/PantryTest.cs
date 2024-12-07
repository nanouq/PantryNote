using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Moq.EntityFrameworkCore;
using PantryApplication.Controllers;
using PantryApplication.Data;
using PantryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PantryApplication.Test
{
    public class PantryTest
    {

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                );
        }
        [Fact]
        public void SearchItems_ReturnsCorrectResults()
        {
            var userId = "test";
            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);

            context.Item.AddRange(
                    new PerishableItem { Id = 1, Name = "Milk", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Cartons"},
                    new NonPerishableItem { Id = 2, Name = "Pasta", PantryId = 1, Category = "Dry Goods", Location = "Pantry", Unit = "Box" }
                );
            context.SaveChanges();

            var controller = new PantryController(context, mockUserManager.Object);
            var result = controller.Search("Milk");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Item>>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Single(model);
            Assert.Equal("Milk", model.First().Name);

        }

        [Fact]
        public void SearchItems_NoMatch_ReturnsEmptyResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase_EmptySearch")
               .Options;

            using var context = new ApplicationDbContext(options);

            context.Item.AddRange(
                    new PerishableItem { Id = 1, Name = "Milk", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Cartons" },
                    new NonPerishableItem { Id = 2, Name = "Pasta", PantryId = 1, Category = "Dry Goods", Location = "Pantry", Unit = "Box" }
                );
            context.SaveChanges();

            var userId = "test";
            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var controller = new PantryController(context, mockUserManager.Object);

            var result = controller.Search("Apple");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Item>>(viewResult.Model);

            Assert.Empty(model);
        }

        [Fact]
        public void AddItem_NonPerishableItemAddedSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase_AddNonPerishableItem")
              .Options;

            using var context = new ApplicationDbContext(options);

            var itemToAdd = new NonPerishableItem { Id = 1, Name = "Bread", PantryId = 1, Category = "Dry Goods", Location = "Cupboard", Unit = "Loaf"};

            context.Item.Add(itemToAdd);
            context.SaveChanges();

            var item = context.Item.FirstOrDefault(i => i.Id == 1);
            Assert.NotNull(item);
            Assert.Equal("Bread", item.Name);
        }

        [Fact]
        public void AddItem_PerishableItemAddedSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase_AddPerishableItem")
              .Options;

            using var context = new ApplicationDbContext(options);

            var itemToAdd = new PerishableItem { Id = 1, Name = "Butter", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Stick" };

            context.Item.Add(itemToAdd);
            context.SaveChanges();

            var item = context.Item.FirstOrDefault(i => i.Id == 1);
            Assert.NotNull(item);
            Assert.Equal("Butter", item.Name);
        }

        [Fact]
        public void Report_ReturnItemsExpiringSoonSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReportReturnExpirationTest")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userId = "test";

            var pantry = new Pantry { Id = 1, UserId = userId };
            context.Pantry.Add(pantry);

            context.Item.AddRange(
                    new PerishableItem { Id = 1, Name = "Milk", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Cartons", ExpirationDate = DateTime.Now.AddDays(1)},
                    new NonPerishableItem { Id = 2, Name = "Pasta", PantryId = 1, Category = "Dry Goods", Location = "Pantry", Unit = "Box", ExpirationDate = DateTime.Now.AddDays(1) }
                );

            context.SaveChanges();

            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var controller = new PantryController(context, mockUserManager.Object);

            var result = controller.Report("1") as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<Tuple<string, List<string>, List<List<string>>>>(result.Model);
            Assert.Equal("Items Expiring Soon Report", model.Item1);
            Assert.Equal("Milk", model.Item3.First()[0]);
        }

        [Fact]
        public void Report_ReturnCategoryCountSuccessfully()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReportReturnCategoryTest")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userId = "test";

            var pantry = new Pantry { Id = 1, UserId = userId };
            context.Pantry.Add(pantry);

            context.Item.AddRange(
                    new PerishableItem { Id = 1, Name = "Milk", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Cartons", ExpirationDate = DateTime.Now.AddDays(1) },
                    new NonPerishableItem { Id = 2, Name = "Pasta", PantryId = 1, Category = "Dry Goods", Location = "Pantry", Unit = "Box", ExpirationDate = DateTime.Now.AddDays(1) }
                );

            context.SaveChanges();

            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var controller = new PantryController(context, mockUserManager.Object);

            // Act
            var result = controller.Report("2") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<Tuple<string, List<string>, List<List<string>>>>(result.Model);
            Assert.Equal("Category Count Report", model.Item1);
            Assert.Equal("Dry Goods", model.Item3.First()[0]);
        }

        [Fact]
        public void Report_MissingPantryNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReportMissingTest")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userId = "test";

            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var controller = new PantryController(context, mockUserManager.Object);

            // Act
            var result = controller.Report("1");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Pantry not found for current user", notFoundResult.Value);
        }

        [Fact]
        public void Report_InvalidReportRedirect()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ReportInvalidTest")
                .Options;

            using var context = new ApplicationDbContext(options);
            var userId = "test";

            var pantry = new Pantry { Id = 1, UserId = userId };
            context.Pantry.Add(pantry);

            context.Item.AddRange(
                    new PerishableItem { Id = 1, Name = "Milk", PantryId = 1, Category = "Dairy", Location = "Fridge", Unit = "Cartons", ExpirationDate = DateTime.Now.AddDays(1) },
                    new NonPerishableItem { Id = 2, Name = "Pasta", PantryId = 1, Category = "Dry Goods", Location = "Pantry", Unit = "Box", ExpirationDate = DateTime.Now.AddDays(1) }
                );

            context.SaveChanges();

            var mockUserManager = GetMockUserManager();
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            var controller = new PantryController(context, mockUserManager.Object);

            var tempData = new Mock<ITempDataDictionary>();
            controller.TempData = tempData.Object;

            // Act
            var result = controller.Report("invalidType");

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            tempData.VerifySet(t => t["error"] = "Invalid report type selected", Times.Once);        }
    }

}
