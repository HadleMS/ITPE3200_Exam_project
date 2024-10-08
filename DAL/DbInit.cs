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
                    Food_Group = ":))",
                    Energi_Kcal = "Energi: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
                    ImageUrl = "/images/helmelk.jpg"
                },
                new Item
                {
                    Name = "Kjøttdeig",
                    Food_Group = ":)",
                    Energi_Kcal = "Protein: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
                    ImageUrl = "/images/kjøttdeig.jpg"
                },
                new Item
                {
                    Name = "Kylling",
                    Food_Group = ":)",
                    Energi_Kcal = "Protein: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
                    ImageUrl = "/images/kylling.jpg"
                },
                new Item
                {
                    Name = "Tine Lettmelk",
                    Food_Group = "250",
                    Energi_Kcal = "Protein: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
                    ImageUrl = "/images/lettmelk.jpg"
                },
                new Item
                {
                    Name = "Peanøtter",
                    Food_Group = "150",
                    Energi_Kcal = "Protein: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
                    ImageUrl = "/images/peanøtter.jpg"
                },
                new Item
                {
                    Name = "Polpa",
                    Food_Group = "180",
                    Energi_Kcal = "Protein: ",
                    Fett = "Protein: ",
                    Protein = "Protein: ",
                    Karbohydrat = "Protein: ",
                    Salt = "Protein: ",
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
