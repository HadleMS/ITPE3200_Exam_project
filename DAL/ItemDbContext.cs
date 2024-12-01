using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Exam.Models;

namespace Exam.DAL;

// Database context class for managing Item entities and user identities, derived from IdentityDbContext.
public class ItemDbContext : IdentityDbContext
{

    // Constructor to configure the database context with the provided options.
    public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
    {
    }

    // Ensure that Items is either nullable or initialized
    public DbSet<Item> Items { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
