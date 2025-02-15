using Core.CoreDtos;
using Core.Enum;

namespace Application.OrderService
{
    public class SearchOrderQuery : PaggingDto
    {
        public Guid? StoreId { get; set; }
    }

    public class SearchOrderDto
    {
        public string? CustommerName { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public IEnumerable<SearchOrderItemDto>? Items { get; set; }
    }

    public class SearchOrderItemDto
    {
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
    }

    public class CreateOrderCommand
    {
    }

    public class CreateOrderDto
    {

    }

    public class UpdateOrderCommand
    {

    }

    public class UpdateOrderDto
    {

    }

    public class DeleteOrderCommand
    {

    }
}
