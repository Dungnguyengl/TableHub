using Application.PaymentServices;
using Core.Extentions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController(TableHubDbContext context) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;

        [HttpGet("payment-history")]
        public IEnumerable<PaymentHistoryDto> GetPaymentHistory([FromQuery] PaymentHistoryQuery query)
        {
            var storeQuery = _context.Stores.TakeAvailable();

            var paymentQuery = _context.Payments.AsNoTracking()
                .Where(x => x.Status == Core.Enum.PaymentStatus.Paid)
                .Where(x => x.PaidAt.Value.Date >= query.StartPaymentDate.Date)
                .Where(x => x.PaidAt.Value.Date <= query.EndPaymentDate.Date);

            var resultQuery = from payment in paymentQuery
                              join store in storeQuery on payment.StoreId equals store.Key
                              select new PaymentHistoryDto
                              {
                                  StoreName = store.Name,
                                  PaymentDate = payment.PaidAt,
                                  Amount = payment.Amount,
                                  BankTransaction = payment.Reference
                              };

            return resultQuery.OrderBy(x => x.StoreName)
                .ThenBy(x => x.PaymentDate);
        }
    }
}
