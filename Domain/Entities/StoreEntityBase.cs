using Core.Entities;

namespace Domain.Entities
{
    public class StoreEntityBase : EntityBase
    {
        public Guid? StoreId { get; set; }
    }
}
