using Application.MenuService;
using Core.CoreDtos;
using Core.Extentions;
using Core.Services.FileService;
using Core.Services.UserService;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MenuController(TableHubDbContext context, IImageStorageService imageStorageService, IUserService userService) : ControllerBase
    {
        private readonly TableHubDbContext _context = context;
        private readonly IImageStorageService _imageStorageService = imageStorageService;
        private readonly IUserService _userService = userService;

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
        public async Task<CommandDto> Create([FromForm] AddProductCommand command)
        {
            var product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                ImageLink = command.Image != null ? await _imageStorageService.StoreFileAsync(command.Image) : null,
                StoreId = _userService.GetUserStoreId(),
            };

            _context.Products.CreateWithTracking(product);

            await _context.SaveChangesAsync();

            return CommandDto.Success();
        }

        [HttpPut]
        public async Task<CommandDto> Update([FromForm] UpdateProductCommand command)
        {
            var product = await _context.Products.TakeAvailable()
                .FirstOrDefaultAsync(x => x.Key == command.ProductId) ?? throw new Exception("Product Not found!");

            product.Name = command.Name;
            product.Price = command.Price;
            var currentImageLink = product.ImageLink;

            if (command.Image == null && currentImageLink != null)
            {
                await _imageStorageService.DeleteForeverAsync(currentImageLink);
                product.ImageLink = null;
            }
            else if (command.Image != null)
            {
                product.ImageLink = currentImageLink != null
                    ? await _imageStorageService.ChangeImageAsync(currentImageLink, command.Image)
                    : await _imageStorageService.StoreFileAsync(command.Image);
            }

            _context.Products.UpdateWithTracking(product);
            await _context.SaveChangesAsync();

            return CommandDto.Success();
        }

        [HttpDelete]
        public async Task<CommandDto> Delete([FromBody] DeleteProductCommand command)
        {
            var product = await _context.Products.TakeAvailable()
                  .FirstOrDefaultAsync(x => x.Key == command.ProductId) ?? throw new Exception("Product Not found!");

            _context.Products.DeleteWithTracking(product);
            await _context.SaveChangesAsync();
            return CommandDto.Success();
        }
    }
}
