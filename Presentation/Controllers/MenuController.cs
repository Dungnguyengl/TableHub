﻿using Application.MenuService;
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
    //[Authorize]
    public class MenuController(TableHubDbContext context) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;

        [HttpGet]
        public async Task<PaggingResultDto<SearchMenuDto>> SearchMenu([FromQuery] SearchMenuQuery query)
        {
            var result = await _context.Products.AsNoTracking()
                .TakeAvailable()
                .TakeByStore(User, query.StoreId)
                .Pagging(query, out var total)
                .Select(x => new SearchMenuDto
                {
                    ProductId = x.Key,
                    Name = x.Name,
                    Price = x.Price,
                    LogoLink = x.ImageLink
                })
                .ToListAsync();

            return new PaggingResultDto<SearchMenuDto>
            {
                Total = total,
                PageSize = query.PageSize,
                Sequence = query.Sequence,
                Results = result
            };
        }

        [HttpPost]
        public Task<CreateMenuDto> Create([FromBody] CreateMenuCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<UpdateMenuDto> Update([FromBody] UpdateMenuCommand command)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task Delete([FromBody] DeleteMenuCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
