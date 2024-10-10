using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Items.Any())
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Tine Helmelk",
                    Food_Group = "Dairy",
                    Energi_Kcal = "264 kj (63 kcal) ",
                    Fett = "3,5 g ",
                    Protein = "3,4 g ",
                    Karbohydrat = "4,5 g ",
                    Salt = "0,1 g ",
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
                    ImageUrl = "/images/polpa.jpg"
                },
             
            };
            context.AddRange(items);
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