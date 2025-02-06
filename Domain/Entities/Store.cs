using Core.Entities;

namespace Domain.Entities
{
    public class Store : EntityBase
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LogoLink { get; set; }
    }
}
