namespace Identity.Infrastructure.Entities.Interfaces
{
    public interface IEntityWithTypedId<TypeId>
    {
        TypeId Id { get; }
    }
}
