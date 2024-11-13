using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Exam.Models;

namespace Exam.DAL;

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

