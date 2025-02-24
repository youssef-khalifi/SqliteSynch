using Demo.Data;
using Demo.Dtos;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Repositories.LocalRepository;

public class LocalRepository : IProductRepositoryLocal
{
    
    private readonly LocalDbContext _context;

    public LocalRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Create(Product request)
    {
        await _context.Products.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<List<Product>> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product;
    }

    public async Task<Product?> DeleteAsync(int id)
    {
        var existProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (existProduct == null)
        {
            return null;
        }
        _context.Products.Remove(existProduct);
        await _context.SaveChangesAsync();
        return existProduct;
    }
}