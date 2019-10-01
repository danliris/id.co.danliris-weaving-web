using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;

namespace Manufactures.Domain.Defects.YarnDefect.Repositories
{
    public interface IYarnDefectRepository : IAggregateRepository<YarnDefectDocument, YarnDefectReadModel>
    {
    }
}
