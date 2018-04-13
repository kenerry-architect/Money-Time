using MoneyTime.Domain.Abstractions.Aggregate;
using MoneyTime.Domain.Abstractions.UoW;

namespace MoneyTime.Domain.Abstractions.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
