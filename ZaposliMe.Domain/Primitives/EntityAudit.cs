namespace ZaposliMe.Domain.Primitives
{
    public abstract class EntityAudit : Entity
    {
        protected EntityAudit()
        { }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
