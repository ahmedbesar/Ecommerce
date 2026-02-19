using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
    }
}
