namespace ZaposliMe.Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid(); 
        }

        protected Entity(Guid id)
        {
            Id = id;
        }

        // Optional: Equality by Id
        public override bool Equals(object obj)
        {
            if (obj is not Entity other) return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public bool Equals(Entity? other)
        {
            return Id == other.Id;
        }
    }
}
