using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DAL;
using MyShop.Models;
using MyShop.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyShop.Controllers
{
   public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }
          public IActionResult About()
        {
        return View();
        }

        // Action to display Products.cshtml
        public IActionResult Products()
        {
            return View("Products");
        }

        // Action to display items in grid layout with pagination
         public async Task<IActionResult> Grid(int page = 1, int pageSize = 6)
        {
            var items = await _itemRepository.GetAll();
            if (items == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }

            var totalItems = items.Count();
            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new ItemsViewModel
            {
                Items = pagedItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                CurrentPage = page
            };

            // Check for AJAX request to return only the items as partial view
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ItemCardsPartial", viewModel);
            }

            // For full page load
            return View(viewModel);
        }

        // Partial view action for table view
        public async Task<IActionResult> TablePartial()
        {
            var items = await _itemRepository.GetAll();
            if (items == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }

            var viewModel = new ItemsViewModel
            {
                Items = items
            };

            return PartialView("Table", viewModel); 
        }

        // Action to display items in table layout
        public async Task<IActionResult> Table()
        {
            var items = await _itemRepository.GetAll();
            if (items == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }

            var itemsViewModel = new ItemsViewModel(items, "Table");
            return View(itemsViewModel);
        }

        // Action to display the details of a specific item by its ID
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                return NotFound("Item not found for the ItemId");
            }

            return PartialView("Details", item);
        }

        // Action to return the form for creating a new item (GET request)
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // Action to handle the form submission for creating a new item (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Item item, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", ImageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    item.ImageUrl = "/images/" + ImageFile.FileName;
                }

                bool returnOk = await _itemRepository.Create(item);

                if (returnOk)
                    return RedirectToAction(nameof(Products));
            }

            _logger.LogWarning("[ItemController] Item creation failed {@item}", item);
            return View(item);
        }

        // Action to return the form for updating an existing item (GET request)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                _logger.LogError("[ItemController] Item not found when updating the ItemId {ItemId:0000}", id);
                return BadRequest("Item not found for the ItemId");
            }
            return View(item);
        }

        // Action to handle the form submission for updating an existing item (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(Item item)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _itemRepository.Update(item);
                if (returnOk)
                    return RedirectToAction(nameof(Products));
            }

            _logger.LogWarning("[ItemController] Item update failed {@item}", item);
            return View(item);
        }

        // Action to display the confirmation page for deleting an item
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
                return BadRequest("Item not found for the ItemId");
            }
            return View(item);
        }

        // Action to handle the deletion of an item (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool returnOk = await _itemRepository.Delete(id);

            if (!returnOk)
            {
                _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
                return BadRequest("Item deletion failed");
            }

            return RedirectToAction(nameof(Products));
        }
    }
    
    
}
