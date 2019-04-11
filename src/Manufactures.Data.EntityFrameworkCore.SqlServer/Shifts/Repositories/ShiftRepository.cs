using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.ReadModels;
using Manufactures.Domain.Shifts.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Shifts.Repositories
{
    public class ShiftRepository : AggregateRepostory<ShiftDocument, ShiftReadModel>, IShiftRepository
    {
        protected override ShiftDocument Map(ShiftReadModel readModel)
        {
            return new ShiftDocument(readModel);
        }
    }
}
