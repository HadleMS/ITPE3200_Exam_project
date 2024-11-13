using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public class ItemDbContext : IdentityDbContext
{
	public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
	{
        
	}

	public DbSet<Item> Items { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}

