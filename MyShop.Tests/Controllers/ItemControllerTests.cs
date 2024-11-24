using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using Exam.Controllers;

namespace Exam.Tests 
{     
   public class ItemControllerTests     
   {         
       private readonly Mock<IItemRepository> _mockRepo;         
       private readonly Mock<ILogger<ItemController>> _mockLogger;         
       private readonly ItemController _controller;         
       private readonly List<Item> _testItems;         

       public ItemControllerTests()     
       {         
           _mockRepo = new Mock<IItemRepository>();         
           _mockLogger = new Mock<ILogger<ItemController>>();                  
           
           _testItems = new List<Item>         
           {             
               new Item { ItemId = 1, Name = "Item 1" },             
               new Item { ItemId = 2, Name = "Item 2" },             
               new Item { ItemId = 3, Name = "Item 3" },             
               new Item { ItemId = 4, Name = "Item 4" },             
               new Item { ItemId = 5, Name = "Item 5" },             
               new Item { ItemId = 6, Name = "Item 6" },             
               new Item { ItemId = 7, Name = "Item 7" }         
           };          
           
           _mockRepo.Setup(repo => repo.GetAll())                 
               .ReturnsAsync(_testItems);          
           
           _controller = new ItemController(_mockRepo.Object, _mockLogger.Object)         
           {             
               ControllerContext = new ControllerContext             
               {                 
                   HttpContext = new DefaultHttpContext()             
               }         
           };     
       }

       [Fact]
       public async Task Grid_ReturnsNotFound_WhenRepositoryReturnsNull()
       {
           // Arrange
           _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync((List<Item>?)null);

           // Act
           var result = await _controller.Grid();

           // Assert
           Assert.IsType<NotFoundObjectResult>(result);
       }

       [Fact]
       public async Task Grid_ReturnsPaginatedItems_WithCorrectPageSize()
       {
           // Arrange
           _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_testItems);
           const int page = 1;
           const int pageSize = 3;

           // Act
           var result = await _controller.Grid(page, pageSize) as ViewResult;
           var model = result?.Model as ItemsViewModel;

           // Assert
           Assert.NotNull(model);
           Assert.Equal(pageSize, model.Items.Count());
           Assert.Equal(3, model.TotalPages);
           Assert.Equal(page, model.CurrentPage);
       }

       [Fact]
       public async Task Grid_ReturnsPartialView_WhenAjaxRequest()
       {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_testItems);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            // Act
            var result = await _controller.Grid();
            // Assert
            var partialView = result as PartialViewResult;
            // Ensure partialView is not null before accessing ViewName
            Assert.NotNull(partialView);  // Ensure partialView is not null
            Assert.Equal("_ItemCardsPartial", partialView.ViewName);  // Now safe to access ViewName
            }

       [Fact]
       public async Task Grid_ReturnsCorrectItems_ForLastPage()
       {
           // Arrange
           _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_testItems);
           const int page = 2;
           const int pageSize = 6;

           // Act
           var result = await _controller.Grid(page, pageSize) as ViewResult;
           var model = result?.Model as ItemsViewModel;

           // Assert
           Assert.NotNull(model);
           Assert.Single(model.Items);
           Assert.Equal(2, model.TotalPages);
           Assert.Equal(page, model.CurrentPage);
       }
   }
}