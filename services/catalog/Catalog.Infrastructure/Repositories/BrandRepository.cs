using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ICatalogContext _context;

    public BrandRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Brands.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<ProductBrand?> GetBrandByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Brands.Find(b => b.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
