using Core.Entities;

namespace Domain.Entities
{
    public class Product : StoreEntityBase
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? ImageLink { get; set; }
    }
}
