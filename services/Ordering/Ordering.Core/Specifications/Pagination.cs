using System.Collections.Generic;
using System.Linq;

namespace Ordering.Core.Specifications
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        public Pagination(int pageIndex, int pageSize, long totalCount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
