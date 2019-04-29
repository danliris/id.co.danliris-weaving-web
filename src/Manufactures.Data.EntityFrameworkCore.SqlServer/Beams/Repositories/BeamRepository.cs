using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Beams.Repositories
{
    public class BeamRepository : AggregateRepostory<BeamDocument, BeamReadModel>, IBeamRepository
    {
        protected override BeamDocument Map(BeamReadModel readModel)
        {
            return new BeamDocument(readModel);
        }
    }
}
