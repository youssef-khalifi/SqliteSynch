using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data;

public class RemoteDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public RemoteDbContext(DbContextOptions<RemoteDbContext> options) : base(options)
    {
        
    }
    
}