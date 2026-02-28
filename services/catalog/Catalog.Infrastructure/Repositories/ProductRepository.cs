using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products.Find(p => p.Name == name).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsByBrandAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products.Find(p => p.Brand.Name == name).ToListAsync(cancellationToken);
    }

    public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        var result = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Products.DeleteOneAsync(p => p.Id == id, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> DeleteAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.Products.DeleteManyAsync(_ => true, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
