using Application.StoreService;
using Core.CoreDtos;
using Core.Extentions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreController(TableHubDbContext context) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;

        [HttpGet]
        public async Task<PaggingResultDto<SearchStoreDto>> SearchStore([FromQuery] SearchStoreQuery query)
        {
            var store = await _context.Stores.AsNoTracking()
                .TakeAvailable()
                .Pagging(query, out var total)
                .ToListAsync();

            var storeIds = store.Select(x => x.Key)
                .ToList();

            var tables = await _context.Tables.AsNoTracking()
                .TakeAvailable()
                .Where(x => x.StoreId.HasValue && storeIds.Contains(x.StoreId.Value))
                .ToListAsync();

            var tableQuantities = tables.GroupBy(x => x.StoreId)
                .Select(x => new
                {
                    StoreId = x.Key,
                    Quantity = x.Count()
                });

            var result = store.Select(x => new SearchStoreDto
            {
                Name = x.Name,
                Address = x.Address,
                AvailableTableQuantity = tableQuantities.FirstOrDefault(q => q.StoreId == x.Key)?.Quantity ?? 0
            });

            return new PaggingResultDto<SearchStoreDto>
            {
                Total = total,
                PageSize = query.PageSize,
                Sequence = query.Sequence,
                Results = result.ToList(),
            };
        }

        [HttpPost]
        public Task<CreateStoreDto> Create([FromBody] CreateStoreCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<UpdateStoreDto> Update([FromBody] UpdateStoreCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task Delete([FromBody] DeleteStoreCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
