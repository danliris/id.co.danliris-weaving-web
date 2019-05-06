using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Shifts.ReadModels;

namespace Manufactures.Domain.Shifts.Repositories
{
    public interface IShiftRepository : IAggregateRepository<ShiftDocument, ShiftReadModel>
    {
    }
}
