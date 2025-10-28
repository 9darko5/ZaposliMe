namespace ZaposliMe.Domain.Primitives
{
    public abstract class EntityAudit : Entity
    {
        protected EntityAudit()
        { }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
