using Infrastructure.Domain.Repositories;
using Manufactures.Domain.BeamStockMonitoring.ReadModels;

namespace Manufactures.Domain.BeamStockMonitoring.Repositories
{
    public interface IBeamStockMonitoringRepository : IAggregateRepository<BeamStockMonitoringDocument, BeamStockMonitoringReadModel>
    {
    }
}
