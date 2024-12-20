using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Exam.Controllers;
using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;

namespace MyShop.Test.Controllers
{
    // Unit tests for the ItemController, verifying the behavior of CRUD operations
    public class ItemControllerTests
    {
        // Tests the Table action to ensure it returns the correct view and data.
        [Fact]
        public async Task TestTable()
        {
            // Arrange
            var itemList = new List<Item>()
            {
                new Item
                {
                    Name = "Tine Helmelk",
                    Food_Group = "Dairy",
                    Energi_Kj = 264,
                    Fett = 3.5,
                    Protein = 3.4,
                    Karbohydrat = 4.5,
                    Salt = 0.1,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/helmelk.jpg"
                },
                new Item
                {
                    Name = "Kjøttdeig",
                    Food_Group = "Meat",
                    Energi_Kj = 791,
                    Fett = 14,
                    Protein = 18,
                    Karbohydrat = 0,
                    Salt = 0.8,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/kjøttdeig.jpg"
                },
                new Item
                {
                    Name = "Kylling",
                    Food_Group = "Meat",
                    Energi_Kj = 632,
                    Fett = 8.8,
                    Protein = 18,
                    Karbohydrat = 0,
                    Salt = 0.2,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/kylling.jpg"
                },
                new Item
                {
                    Name = "Tine Lettmelk",
                    Food_Group = "Dairy",
                    Energi_Kj = 155,
                    Fett = 0.5,
                    Protein = 3.5,
                    Karbohydrat = 4.5,
                    Salt = 0.1,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/lettmelk.jpg"
                },
                new Item
                {
                    Name = "Peanøtter",
                    Food_Group = "Nuts",
                    Energi_Kj = 2634,
                    Fett = 52,
                    Protein = 24,
                    Karbohydrat = 14,
                    Salt = 1.2,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/peanøtter.jpg"
                },
                new Item
                {
                    Name = "Polpa",
                    Food_Group = "Sauce",
                    Energi_Kj = 110,
                    Fett = 0.2,
                    Protein = 1.2,
                    Karbohydrat = 3.9,
                    Salt = 0.3,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/polpa.jpg"
                },
                new Item
                {
                    Name = "Blåbær",
                    Food_Group = "Berries",
                    Energi_Kj = 181,
                    Fett = 0.5,
                    Protein = 0.5,
                    Karbohydrat = 7.6,
                    Salt = 0,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/blåbær.jpg"
                },
                new Item
                {
                    Name = "Brokkoli",
                    Food_Group = "Vegetables",
                    Energi_Kj = 180,
                    Fett = 0.6,
                    Protein = 4.3,
                    Karbohydrat = 3.1,
                    Salt = 0,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/brokkoli.jpg"
                },
                new Item
                {
                    Name = "Kremgo",
                    Food_Group = "Dairy",
                    Energi_Kj = 1063,
                    Fett = 24,
                    Protein = 6.7,
                    Karbohydrat = 2.8,
                    Salt = 1,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/kremgo.jpg"
                },
                new Item
                {
                    Name = "Kokt skinke",
                    Food_Group = "Meat",
                    Energi_Kj = 415,
                    Fett = 1.8,
                    Protein = 19,
                    Karbohydrat = 1.7,
                    Salt = 1.9,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/skinke.jpg"
                },
                new Item
                {
                    Name = "Norvegia 26% Skivet",
                    Food_Group = "Dairy",
                    Energi_Kj = 1400,
                    Fett = 26,
                    Protein = 27,
                    Karbohydrat = 0.0,
                    Salt = 1.2,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/norvegia_skivet.jpg"},

                new Item
                {
                    Name = "Gulrot 400g",
                    Food_Group = "Vegetables",
                    Energi_Kj = 150,
                    Fett = 0.2,
                    Protein = 0.6,
                    Karbohydrat = 6.7,
                    Salt = 0.1,
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/gulrot.jpg"
                },

                new Item
                {
                    Name = "Pepsi Max 1,5l",
                    Food_Group = "Beverages",
                    Energi_Kj = 1.7,
                    Fett = 0.0,
                    Protein = 0.0,
                    Karbohydrat = 0.0,
                    Salt = 0.02,
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/pepsi_max.jpg"
                }

            };

            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.GetAll()).ReturnsAsync(itemList);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.Table();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var itemsViewModel = Assert.IsAssignableFrom<ItemsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(13, itemsViewModel.Items.Count());
            Assert.Equal(itemList, itemsViewModel.Items);
        }

        // Tests creating a valid item to ensure it redirects to the Products page.
        [Fact]
        public async Task Create_ValidItem_ReturnsRedirectToAction() //Positiv
        {
            // Arrange
            var item = new Item { ItemId = 1, Name = "Test Item" };
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.Create(It.IsAny<Item>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.Create(item, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ActionName);
        }

        // Tests creating an invalid item to ensure it returns the view with the original model.
        [Fact]
        public async Task Create_InvalidItem_ReturnsView() //Negativ
        {
            // Arrange
            var item = new Item { ItemId = 1, Name = "" };
            var mockItemRepository = new Mock<IItemRepository>();
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Create(item, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(item, viewResult.Model);
        }

        // Tests retrieving details of an existing item to ensure it returns a partial view.
        [Fact]
        public async Task Details_ExistingItem_ReturnsPartialView() //Positiv
        {
            // Arrange
            var item = new Item { ItemId = 1, Name = "Test Item" };
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.GetItemById(1)).ReturnsAsync(item);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsType<Item>(viewResult.Model);
            Assert.Equal(1, model.ItemId);
        }

        // Tests retrieving details of a non-existing item to ensure it returns a NotFound result.
        [Fact]
        public async Task Details_NonExistingItem_ReturnsNotFound() //Negativ
        {
            // Arrange
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.GetItemById(999)).ReturnsAsync((Item?)null);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.Details(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Tests updating a valid item to ensure it redirects to the Products page.
        [Fact]
        public async Task Update_ValidItem_ReturnsRedirectToAction() //Positiv
        {
            // Arrange
            var item = new Item { ItemId = 1, Name = "Updated Item" };
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.Update(It.IsAny<Item>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.Update(item, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ActionName);
        }

        // Tests updating an invalid item to ensure it returns the view with the original model.
        [Fact]
        public async Task Update_InvalidItem_ReturnsView() //Negativ
        {
            // Arrange
            var item = new Item { ItemId = 1 };
            var mockItemRepository = new Mock<IItemRepository>();
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Update(item, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(item, viewResult.Model);
        }

        // Tests deleting a valid item to ensure it redirects to the Products page.
        [Fact]
        public async Task DeleteConfirmed_ValidId_ReturnsRedirectToAction() //Positiv
        {
            // Arrange
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ActionName);
        }

        // Tests deleting an invalid item to ensure it returns a BadRequest result.
        [Fact]
        public async Task DeleteConfirmed_InvalidId_ReturnsBadRequest() //Negativ
        {
            // Arrange
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.Delete(999)).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteConfirmed(999);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}