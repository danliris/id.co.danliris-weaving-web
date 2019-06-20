using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Movements.ReadModels;

namespace Manufactures.Domain.Movements.Repositories
{
    public interface IMovementRepository : IAggregateRepository<MovementDocument, MovementReadModel>
    {
    }
}
