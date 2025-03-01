using Core.CoreDtos;

namespace Application.MenuService
{
    public class SearchMenuQuery : PaggingDto
    {
        public Guid? StoreId { get; set; }
    }

    public class SearchMenuDto
    {
        public Guid? ProductId { get; set; }
        public string? Name { get; set; }
        public string? LogoLink { get; set; }
        public decimal? Price { get; set; }
    }

    public class AddProductCommand
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateProductCommand
    {
        public Guid? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }

    public class DeleteProductCommand
    {
        public Guid? ProductId { get; set; }
    }
}
