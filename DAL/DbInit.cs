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
                    Energi_Kcal = "264 kj",
                    Fett = "3.5 g",
                    Protein = "3.4 g",
                    Karbohydrat = "4.5 g",
                    Salt = "0.1 g",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/helmelk.jpg"
                },
                new Item
                {
                    Name = "Kjøttdeig",
                    Food_Group = "Meat",
                    Energi_Kcal = "791 kj",
                    Fett = "14 g",
                    Protein = "18 g",
                    Karbohydrat = "0 g",
                    Salt = "0.8 g",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/kjøttdeig.jpg"
                },
                new Item
                {
                    Name = "Kylling",
                    Food_Group = "Meat",
                    Energi_Kcal = "632 kj",
                    Fett = "8.8 g",
                    Protein = "18 g",
                    Karbohydrat = "0 g",
                    Salt = "0.2 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/kylling.jpg"
                },
                new Item
                {
                    Name = "Tine Lettmelk",
                    Food_Group = "Dairy",
                    Energi_Kcal = "155 kj",
                    Fett = "0.5 g",
                    Protein = "3.5 g",
                    Karbohydrat = "4.5 g",
                    Salt = "0.1 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/lettmelk.jpg"
                },
                new Item
                {
                    Name = "Peanøtter",
                    Food_Group = "Nuts",
                    Energi_Kcal = "2634 kj",
                    Fett = "52 g",
                    Protein = "24 g",
                    Karbohydrat = "14 g",
                    Salt = "1.2 g",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/peanøtter.jpg"
                },
                new Item
                {
                    Name = "Polpa",
                    Food_Group = "Sauce",
                    Energi_Kcal = "110 kj",
                    Fett = "0.2 g",
                    Protein = "1.2 g",
                    Karbohydrat = "3.9 g",
                    Salt = "0.3 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/polpa.jpg"
                },
                new Item
                {
                    Name = "Blåbær",
                    Food_Group = "Berries",
                    Energi_Kcal = "181 kj",
                    Fett = "0.5 g",
                    Protein = "0.5 g",
                    Karbohydrat = "7,6 g",
                    Salt = "0 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/blåbær.jpg"
                },
                new Item
                {
                    Name = "Brokkoli",
                    Food_Group = "Vegetables",
                    Energi_Kcal = "180 kj",
                    Fett = "0.6 g",
                    Protein = "4.3 g",
                    Karbohydrat = "3.1 g",
                    Salt = "0 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/brokkoli.jpg"
                },
                new Item
                {
                    Name = "Kremgo",
                    Food_Group = "Creamcheese",
                    Energi_Kcal = "1063 kj",
                    Fett = "24 g",
                    Protein = "6.7 g",
                    Karbohydrat = "2.8 g",
                    Salt = "1 g",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/kremgo.jpg"
                },
                new Item
                {
                    Name = "Kokt skinke",
                    Food_Group = "Toppings",
                    Energi_Kcal = "415 kj",
                    Fett = "1.8 g",
                    Protein = "19 g",
                    Karbohydrat = "1.7 g",
                    Salt = "1.9 g",
                    HasGreenKeyhole = true,
                    ImageUrl = "/images/skinke.jpg"
                }, 
                new Item
                {
                    Name = "Norvegia 26% Skivet",
                    Food_Group = "Dairy",
                    Energi_Kcal = "1400 kJ (336 kcal)",
                    Fett = "26.0 g",
                    Protein = "27.0 g",
                    Karbohydrat = "0.0 g",
                    Salt = "1.2 g",
                    HasGreenKeyhole = false,
                    ImageUrl = "/images/norvegia_skivet.jpg"},
                    
                new Item
                    {
                        Name = "Gulrot 400g",
                        Food_Group = "Vegetables",
                        Energi_Kcal = "150 kJ (36 kcal)",
                        Fett = "0.2 g",
                        Protein = "0.6 g",
                        Karbohydrat = "6.7 g",
                        Salt = "0.1 g",
                        HasGreenKeyhole = true,
                        ImageUrl = "/images/gulrot.jpg"
                        },
                    
                        new Item
                        {
                            Name = "Pepsi Max 1,5l",
                            Food_Group = "Beverages",
                            Energi_Kcal = "1.7 kJ (0.4 kcal)",
                            Fett = "0.0 g",
                            Protein = "0.0 g",
                            Karbohydrat = "0.0 g",
                            Salt = "0.02 g",
                            HasGreenKeyhole = false,
                            ImageUrl = "/images/pepsi_max.jpg"
                            }               
                        };

            context.AddRange(items);
            context.SaveChanges();
        }
    }
}
