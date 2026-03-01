using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Core.Specifications.Products;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams specParams)
    {
        var builder = Builders<Product>.Filter;

        // Build filters
        if (!string.IsNullOrEmpty(specParams.Search))
            ApplyFilter(builder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(specParams.Search, "i")));

        if (!string.IsNullOrEmpty(specParams.BrandId))
            ApplyFilter(builder.Eq(p => p.BrandId, specParams.BrandId));

        if (!string.IsNullOrEmpty(specParams.TypeId))
            ApplyFilter(builder.Eq(p => p.TypeId, specParams.TypeId));

        // Apply sorting
        switch (specParams.Sort?.ToLower())
        {
            case "priceasc":
                ApplyOrderBy(p => p.Price);
                break;
            case "pricedesc":
                ApplyOrderByDescending(p => p.Price);
                break;
            default:
                ApplyOrderBy(p => p.Name);
                break;
        }

        // Apply pagination
        ApplyPaging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }
}
