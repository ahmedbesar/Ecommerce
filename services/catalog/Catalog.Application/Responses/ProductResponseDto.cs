namespace Catalog.Application.Responses
{
    public sealed record  ProductResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public ProductBrandResponseDto Brand { get; set; }
        public ProductTypeResponseDto Type { get; set; }
    }
}
