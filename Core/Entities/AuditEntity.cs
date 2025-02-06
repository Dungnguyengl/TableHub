namespace Core.Entities
{
    public class AuditEntity
    {
        public Guid? InsUserCode { get; set; }
        public DateTime? InsDate { get; set; }
        public Guid? UpdUserCode { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
