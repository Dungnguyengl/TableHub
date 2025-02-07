using Core.Entities;
using Core.Enum;

namespace Domain.Entities
{
    public class Order : StoreEntityBase
    {
        public Guid? TableId { get; set; }
        public Guid? CustomerId { get; set; }
        public OrderStatus? Status { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
