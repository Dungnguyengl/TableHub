using Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Payment
    {
        [Key]
        public long OrderCode { get; set; }

        public decimal? Amount { get; set; }

        public string? Description { get; set; }

        public string? Reference { get; set; }

        public PaymentStatus Status { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? StoreId { get; set; }
    }
}
