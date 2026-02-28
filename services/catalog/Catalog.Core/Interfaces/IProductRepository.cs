using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllProductsByBrandAsync(string name, CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAllProductsAsync(CancellationToken cancellationToken = default);
    }
}
