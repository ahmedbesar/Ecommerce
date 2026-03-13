using Ordering.Core.Entities;

namespace Ordering.Core.Specifications.Orders
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(OrderSpecificationParams specParams)
            : base(o =>
                (string.IsNullOrEmpty(specParams.UserName) || o.UserName == specParams.UserName))
        {
            // Apply sorting
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "priceasc":
                        ApplyOrderBy(o => o.TotalPrice);
                        break;
                    case "pricedesc":
                        ApplyOrderByDescending(o => o.TotalPrice);
                        break;
                    case "dateasc":
                        ApplyOrderBy(o => o.CreatedDate);
                        break;
                    case "datedesc":
                        ApplyOrderByDescending(o => o.CreatedDate);
                        break;
                    default:
                        ApplyOrderByDescending(o => o.CreatedDate);
                        break;
                }
            }
            else
            {
                ApplyOrderByDescending(o => o.CreatedDate);
            }

            // Apply pagination
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        public OrderSpecification(string userName)
            : base(o => o.UserName == userName)
        {
        }
    }
}
