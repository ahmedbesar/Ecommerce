using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly ICatalogContext _context;

    public TypeRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductType>> GetAllTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Types.Find(_ => true).ToListAsync(cancellationToken);
    }
}
