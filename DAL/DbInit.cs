using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public static class DBInit
{

    // Seed method to initialize the database with default data
    public static void Seed(IApplicationBuilder app)
    {
         // Create a service scope to access the database context (ItemDbContext)
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();

        // Ensures that the database is deleted and recreated
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Check if there are any items in the Items table
        if (!context.Items.Any())
        {

             // Define a list of sample Item objects to populate the database
            var items = new List<Item>
            {
                // Each item represents a product with its attributes
                new Item
                {
                    Name = "Tine Helmelk",
                    Food_Group = "Dairy",
                    Energi_Kcal = "264 kj (63 kcal) ",
                    Fett = "3,5 g ",
                    Protein = "3,4 g ",
                    Karbohydrat = "4,5 g ",
                    Salt = "0,1 g ",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/helmelk.jpg"

                },
                new Item
                {
                    Name = "Kjøttdeig",
                    Food_Group = "Meat",
                    Energi_Kcal = "791 kj (190 kcal) ",
                    Fett = "14 g ",
                    Protein = "18 g ",
                    Karbohydrat = "0 g ",
                    Salt = "0,8 g ",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/kjøttdeig.jpg"
                },
                new Item
                {
                    Name = "Kylling",
                    Food_Group = "Meat",
                    Energi_Kcal = "632 kj (151 kcal) ",
                    Fett = "8,8 g ",
                    Protein = "18 g ",
                    Karbohydrat = "0 g ",
                    Salt = "0,2 g ",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/kylling.jpg"
                },
                new Item
                {
                    Name = "Tine Lettmelk",
                    Food_Group = "Dairy",
                    Energi_Kcal = "155 kj (37 kcal) ",
                    Fett = "0,5 g ",
                    Protein = "3,5 g ",
                    Karbohydrat = "4,5 g ",
                    Salt = "0,1 g ",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/lettmelk.jpg"
                },
                new Item
                {
                    Name = "Peanøtter",
                    Food_Group = "Nuts",
                    Energi_Kcal = "2634 kj (628 kcal) ",
                    Fett = "52 g ",
                    Protein = "24 g ",
                    Karbohydrat = "14 g ",
                    Salt = "1,2 g ",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/peanøtter.jpg"
                },
                new Item
                {
                    Name = "Polpa",
                    Food_Group = "Sauce",
                    Energi_Kcal = "110 kj (26 kcal) ",
                    Fett = "0,2 g ",
                    Protein = "1,2 g ",
                    Karbohydrat = "3,9 g ",
                    Salt = "0,3 g ",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/polpa.jpg"
                },
             
            };

            // Add the list of items to the database
            context.AddRange(items);

            // Save the changes to the database
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Alice Hansen", Address = "Osloveien 1"},
                new Customer { Name = "Bob Johansen", Address = "Oslomet gata 2"},
            };
            context.AddRange(customers);
            context.SaveChanges();
        }
       
}}