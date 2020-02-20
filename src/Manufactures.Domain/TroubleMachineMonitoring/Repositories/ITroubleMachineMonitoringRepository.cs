using Infrastructure.Domain.Repositories;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;

namespace Manufactures.Domain.TroubleMachineMonitoring.Repositories
{
    public interface ITroubleMachineMonitoringRepository : IAggregateRepository<TroubleMachineMonitoringDocument, TroubleMachineMonitoringReadModel>
    {
    }
}
