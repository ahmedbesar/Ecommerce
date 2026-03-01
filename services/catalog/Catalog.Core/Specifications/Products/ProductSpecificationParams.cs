namespace Catalog.Core.Specifications.Products;

public class ProductSpecificationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageIndex { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? Sort { get; set; }
    public string? BrandId { get; set; }
    public string? TypeId { get; set; }

    private string? _search;
    public string? Search
    {
        get => _search;
        set => _search = value?.Trim().ToLower();
    }
}
