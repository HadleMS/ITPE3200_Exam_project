using MyShop.Models;

namespace MyShop.ViewModels
{
    // ViewModel to pass item data and view information to the view
    public class ItemsViewModel
    {
        
        public IEnumerable<Item> Items;
        
        
        public string? CurrentViewName;

        // Constructor to initialize the ViewModel with items and the current view name
        public ItemsViewModel(IEnumerable<Item> items, string? currentViewName)
        {
            Items = items; // Assigns the passed items to the ViewModel
            CurrentViewName = currentViewName; // Sets the current view name
        }
    }
}


