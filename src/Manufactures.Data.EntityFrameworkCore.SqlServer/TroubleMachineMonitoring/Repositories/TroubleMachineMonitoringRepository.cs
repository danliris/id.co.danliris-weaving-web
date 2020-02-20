using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.TroubleMachineMonitoring;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.TroubleMachineMonitoring
{
    public class TroubleMachineMonitoringRepository : AggregateRepostory<TroubleMachineMonitoringDocument, TroubleMachineMonitoringReadModel>, ITroubleMachineMonitoringRepository
    {
        protected override TroubleMachineMonitoringDocument Map(TroubleMachineMonitoringReadModel readModel)
        {
            return new TroubleMachineMonitoringDocument(readModel);
        }
    }
}
