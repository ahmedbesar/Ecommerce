using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;

namespace Ordering.Application.Mappers
{
    [Mapper]
    public partial class OrderMapper
    {
        public partial OrderResponseDto ToResponseDto(Order order);
        public partial IReadOnlyList<OrderResponseDto> ToResponseListDto(IReadOnlyList<Order> orders);
        public partial IEnumerable<OrderResponseDto> ToResponseListDtoFallback(IEnumerable<Order> orders);

        public partial Order ToEntity(CreateOrderCommand command);
        public partial Order ToEntity(UpdateOrderCommand command);
        public partial void Map(UpdateOrderCommand command, Order order);
    }
}
