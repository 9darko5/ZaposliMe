namespace ZaposliMe.Domain.Primitives
{
    public abstract class EntityAudit : Entity
    {
        protected EntityAudit()
        { }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UpdatedBy { get; set; }
    }
}
