namespace Core.Entities
{
    public class EntityBase : AuditEntity
    {
        public Guid Key { get; set; }

        public bool IsDelete { get; set; }
    }
}
