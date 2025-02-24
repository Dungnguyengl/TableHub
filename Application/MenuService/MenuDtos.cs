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

    public class CreateMenuCommand
    {

    }

    public class CreateMenuDto
    {

    }

    public class UpdateMenuCommand
    {

    }

    public class UpdateMenuDto
    {

    }

    public class DeleteMenuCommand
    {

    }
}
