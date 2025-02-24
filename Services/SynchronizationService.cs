using Demo.Data;
using Demo.Interfaces;
using Demo.Models;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services;

public class SynchronizationService
{
    private readonly LocalDbContext _localContext;
    private readonly RemoteDbContext _remoteContext;

    public SynchronizationService(LocalDbContext localContext, RemoteDbContext remoteContext)
    {
        _localContext = localContext;
        _remoteContext = remoteContext;
    }

    // Sync local changes to remote
public async Task SyncLocalToRemoteAsync()
{
    var localProducts = await _localContext.Products.ToListAsync();  // Get all products

    // Get all remote products
    var remoteProducts = await _remoteContext.Products.ToListAsync();

    foreach (var product in localProducts)
    {
        try
        {
            var existingProduct = remoteProducts
                .FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                // Resolve conflict before updating
                await ResolveConflict(product, existingProduct);

                // After resolving the conflict, update the remote product
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.LastModified = product.LastModified;
                _remoteContext.Products.Update(existingProduct);
            }
            else
            {
                // If the product doesn't exist in the remote database, create it
                _remoteContext.Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error syncing product {product.Id}: {ex.Message}");
        }
    }

    // Now delete products from the remote database that no longer exist in the local database
    foreach (var remoteProduct in remoteProducts)
    {
        var productInLocal = localProducts.FirstOrDefault(p => p.Id == remoteProduct.Id);
        if (productInLocal == null)
        {
            // Product no longer exists in local, delete from remote
            _remoteContext.Products.Remove(remoteProduct);
        }
    }

    await _remoteContext.SaveChangesAsync();
}


    // Sync remote changes to local
    public async Task SyncRemoteToLocalAsync()
    {
        var remoteProducts = await _remoteContext.Products.ToListAsync();  // Get all products

        foreach (var product in remoteProducts)
        {
            try
            {
                var existingProduct = await _localContext.Products
                    .FirstOrDefaultAsync(p => p.Id == product.Id);

                if (existingProduct != null)
                {
                    // Resolve conflict before updating
                    await ResolveConflict(existingProduct, product);

                    // After resolving the conflict, update the local product
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.LastModified = product.LastModified;
                    _localContext.Products.Update(existingProduct);
                }
                else
                {
                    // If the product doesn't exist in the local database, create it
                    _localContext.Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing product {product.Id}: {ex.Message}");
            }
        }

        await _localContext.SaveChangesAsync();
    }

    // Resolve conflict between local and remote products
    public async Task ResolveConflict(Product localProduct, Product remoteProduct)
    {
        if (localProduct.LastModified > remoteProduct.LastModified)
        {
            // Local changes should overwrite remote
            remoteProduct.Name = localProduct.Name;
            remoteProduct.Description = localProduct.Description;
            remoteProduct.Price = localProduct.Price;
            remoteProduct.LastModified = localProduct.LastModified;
        }
        else
        {
            // Remote changes should overwrite local
            localProduct.Name = remoteProduct.Name;
            localProduct.Description = remoteProduct.Description;
            localProduct.Price = remoteProduct.Price;
            localProduct.LastModified = remoteProduct.LastModified;
        }
    }

    // Sync both databases
    public async Task SyncDatabases()
    {
        try
        {
            await SyncLocalToRemoteAsync();
            await SyncRemoteToLocalAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during sync: {ex.Message}");
        }
    }
}
