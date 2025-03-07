using Core.CoreDtos;

namespace Application.PaymentServices
{
    public class PaymentHistoryQuery : PaggingDto
    {
        public DateTime StartPaymentDate { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndPaymentDate { get; set; } = DateTime.Now;
        public Guid? StoreId { get; set; }
    }

    public class PaymentHistoryDto
    {
        public string? StoreName { get; set; }
        public DateTime? PaymentDate {  get; set; }
        public decimal? Amount { get; set; }
        public string? BankTransaction { get; set; }
    }
}
