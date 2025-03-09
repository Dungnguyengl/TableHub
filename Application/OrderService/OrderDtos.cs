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
        public Guid? OrderId { get; set; }
        public string? CustommerName { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? TableName { get; set; }
        public IEnumerable<SearchOrderItemDto>? Items { get; set; }
    }

    public class SearchOrderItemDto
    {
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
    }

    public class CreateOrderCommand
    {
        public Guid? StoreId { get; set; }
        public Guid? TableId { get; set; }
        public bool IsOnlinePayment { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
        public List<CreateOrderItemCommand> Items { get; set;}
    }

    public class CreateOrderItemCommand
    {
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
    }

    public class CreateOrderDto
    {
        public Guid? OrderId;
        public string? PaymentUrl { get; set; }
        public string? RawQrCode { get; set; }
    }

    public class CheckinCommand
    {
        public string CheckinCode { get; set; }
    }

    public class CheckinDto
    {
        public string? TableName { get; set; }
        public string? CustomerName { get; set; }
        public IEnumerable<CheckinOrderItemDto> Items { get; set; }
    }

    public class CheckinOrderItemDto
    {
        public string? Name { get; set; }
        public int? Quantity { get; set; } 
    }

    public class DeleteOrderCommand
    {

    }
}
