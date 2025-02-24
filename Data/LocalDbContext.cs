using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data;

public class LocalDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
    {
        
    }
    

    
}