namespace Ordering.Core.Specifications.Orders
{
    public class OrderSpecificationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Sort { get; set; }
        public string? UserName { get; set; }
    }
}
