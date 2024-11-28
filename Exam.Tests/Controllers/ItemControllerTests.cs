using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Exam.Controllers;
using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MyShop.Test.Controllers
{
    public class ItemControllerTests
    {
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
                    ImageUrl = "/images/norvegia_skivet.jpg"
                },
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

        [Fact]
        public async Task Create_ValidItem_ReturnsRedirectToAction() // Positive
        {
            // Arrange
            var item = new Item { ItemId = 1, Name = "Test Item" };
            var mockItemRepository = new Mock<IItemRepository>();
            mockItemRepository.Setup(repo => repo.Create(It.IsAny<Item>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);

            // Create a mock IFormFile
            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(content);
            writer.Flush();
            memoryStream.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(_ => _.FileName).Returns(string.Empty);
            fileMock.Setup(_ => _.Length).Returns(memoryStream.Length);

            var mockFile1 = fileMock.Object; // Renamed to avoid conflict

            var mockFile = fileMock.Object; // Renamed to avoid conflict

            // Act
            var result = await controller.Update(item, mockFile ?? Mock.Of<IFormFile>());

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Products", redirectResult.ActionName);
        }

        [Fact]
        public async Task Update_InvalidItem_ReturnsView() // Negative
        {
            // Arrange
            var item = new Item { ItemId = 1 };
            var mockItemRepository = new Mock<IItemRepository>();
            var mockLogger = new Mock<ILogger<ItemController>>();
            var controller = new ItemController(mockItemRepository.Object, mockLogger.Object);
            controller.ModelState.AddModelError("Name", "Required");

            // Create a mock IFormFile
            var fileMock = new Mock<IFormFile>();
            var mockFile = fileMock.Object; // Renamed to avoid conflict

            // Act
            var result = await controller.Update(item, mockFile ?? Mock.Of<IFormFile>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(item, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_ReturnsRedirectToAction() // Positive
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

        [Fact]
        public async Task DeleteConfirmed_InvalidId_ReturnsBadRequest() // Negative
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
