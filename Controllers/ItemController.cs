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
    public class ItemController : Controller // Defines a controller 
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
        {
            _itemRepository = itemRepository; // Responsible for accessing data
            _logger = logger; // Used for logging events
        }

        public async Task<IActionResult> Table() // Action to fetch and display items in a table layout    
        {
            var items = await _itemRepository.GetAll(); //Fetch all items asynchronously
            if (items == null) // If no items are found, log the error and return a NotFound response
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }
            // If items are found, create a ViewModel for "Table" layout and return it
            var itemsViewModel = new ItemsViewModel(items, "Table");
            return View(itemsViewModel);
        }

        // Action to fetch and display items in grid layout
        public async Task<IActionResult> Grid(int page = 1, int pageSize = 6)
        {
            var items = await _itemRepository.GetAll();

            if (items == null)
            {
                _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
                return NotFound("Item list not found");
            }

            // Calculate pagination details
            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            // Get items for the current page
            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Create and pass the paginated ViewModel
            var viewModel = new ItemsViewModel
            {
                Items = pagedItems,
                TotalPages = totalPages,
                CurrentPage = page
            };

            return View(viewModel);
        }

        // Action to fetch and display the details of a specific item by its ID
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            
            if (item == null)
            {
                return NotFound("Item not found for the ItemId");
            }

            // Return the partial view for AJAX requests
            return PartialView("Details", item);
        }

        // Action to return the form for creating a new item (GET request)
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            // Return a blank form for item creation
            return View();
        }

        // Action to handle item creation (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Item item, IFormFile ImageFile)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Check if an image file was uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Define the path to save the file (adjust path as needed)
                    var filePath = Path.Combine("wwwroot/images", ImageFile.FileName);

                    // Save the file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Set the ImageUrl to the relative path for serving the image
                    item.ImageUrl = "/images/" + ImageFile.FileName;
                }

                // Attempt to create the item in the repository
                bool returnOk = await _itemRepository.Create(item);

                // Redirect to the Table view if successful
                if (returnOk)
                    return RedirectToAction(nameof(Table));
            }

            // If creation fails or the model is invalid, log a warning and return the form
            _logger.LogWarning("[ItemController] Item creation failed {@item}", item);
            return View(item);
        }

        // Action to return the form for updating an existing item by its ID (GET request)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            // Fetch item by ID asynchronously
            var item = await _itemRepository.GetItemById(id);
            
            // If item is not found, log the error and return BadRequest response
            if (item == null)
            {
                _logger.LogError("[ItemController] Item not found when updating the ItemId {ItemId:0000}", id);
                return BadRequest("Item not found for the ItemId");
            }
            // If the item is found, return the item to the update form
            return View(item);
        }

        // Action to handle item update (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(Item item)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Try to update the item in the repository
                bool returnOk = await _itemRepository.Update(item);
                
                // If the update is successful, redirect to the Table view
                if (returnOk)
                    return RedirectToAction(nameof(Table));
            }

            // If update fails or the model is invalid, log a warning and return the form
            _logger.LogWarning("[ItemController] Item update failed {@item}", item);
            return View(item);
        }

        // Action to return the confirmation view for deleting an item by its ID (GET request)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            // Fetch item by ID asynchronously
            var item = await _itemRepository.GetItemById(id);
            
            // If item is not found, log the error and return BadRequest response
            if (item == null)
            {
                _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
                return BadRequest("Item not found for the ItemId");
            }
            
            // If the item is found, return the item to the delete confirmation view
            return View(item);
        }

        // Action to handle confirmed deletion of an item (POST request)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Delete the item from the repository
            bool returnOk = await _itemRepository.Delete(id);
            
            // If deletion fails, log the error and return BadRequest response
            if (!returnOk)
            {
                _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
                return BadRequest("Item deletion failed");
            }

            // If deletion is successful, redirect to the Table view
            return RedirectToAction(nameof(Table));
        }
    }
}