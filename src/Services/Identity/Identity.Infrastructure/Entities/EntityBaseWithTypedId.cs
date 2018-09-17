namespace Identity.Infrastructure.Entities
{
    using Interfaces;

    public abstract class EntityBaseWithTypedId<TypeId> : IEntityWithTypedId<TypeId>
    {
        public virtual TypeId Id { get; protected set; }
    }
}
