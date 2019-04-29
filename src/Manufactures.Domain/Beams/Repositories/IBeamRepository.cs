using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Beams.ReadModels;

namespace Manufactures.Domain.Beams.Repositories
{
    public interface IBeamRepository : IAggregateRepository<BeamDocument, BeamReadModel>
    {
    }
}
