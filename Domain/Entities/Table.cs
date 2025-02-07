using Core.Entities;
using Core.Enum;

namespace Domain.Entities
{
    public class Table : StoreEntityBase
    {
        public string? Name { get; set; }
        public TableStatus? Status { get; set; }
    }
}
