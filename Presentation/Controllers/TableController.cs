﻿using Application.TableService;
using Core.CoreDtos;
using Core.Extentions;
using Domain.Constants;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TableController(TableHubDbContext context) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;

        [HttpGet]
        public async Task<PaggingResultDto<SearchTableDto>> SearchTable([FromQuery] SearchTableQuery query)
        {
            var result = await _context.Tables.AsNoTracking()
                .TakeByStore(User, query.StoreId)
                .TakeAvailable()
                .Pagging(query, out var total)
                .Select(x => new SearchTableDto
                {
                    TableId = x.Key,
                    TableName = x.Name,
                    Status = x.Status
                })
                .ToListAsync();

            return new PaggingResultDto<SearchTableDto>
            {
                Total = total,
                PageSize = query.PageSize,
                Sequence = query.Sequence,
                Results = result
            };
        }

        [HttpPost]
        public Task<CreateTableDto> Create([FromBody] CreateTableCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<UpdateTableDto> Update([FromBody] UpdateTableCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpPut("change-status")]
        [Authorize(Roles = Roles.STAFF)]
        public async Task<CommandDto> ChangeStatus([FromBody] ChangeStatusCommand command)
        {
            var table = await _context.Tables
                .TakeByStore(User)
                .TakeAvailable()
                .FirstOrDefaultAsync(x => x.Key == command.TableId);

            if (table == null)
            {
                return CommandDto.NotFound("Table", command.TableId?.ToString());
            }

            table.Status = command.Status ?? table.Status;
            _context.Tables.Update(table, User);
            await _context.SaveChangesAsync();

            return CommandDto.Success();
        }

        [HttpDelete]
        public Task Delete([FromBody] DeleteTableCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
