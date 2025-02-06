using Core.CoreDtos;
using Core.Enum;

namespace Application.StoreService
{
    public class SearchStoreQuery : PaggingDto
    {
    }

    public class SearchStoreDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AvailableTableQuantity { get; set; }
        public string? LogoLink { get; set; }
    }

    public class CreateStoreCommand
    {

    }

    public class CreateStoreDto
    {

    }

    public class UpdateStoreCommand
    {

    }

    public class UpdateStoreDto
    {

    }

    public class DeleteStoreCommand
    {

    }
}
