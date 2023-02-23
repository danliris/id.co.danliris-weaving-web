using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.BeamStockMonitoring;
using Manufactures.Domain.BeamStockMonitoring.ReadModels;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.BeamStockMonitoring.Repositories
{
    public class BeamStockMonitoringRepository : AggregateRepostory<BeamStockMonitoringDocument, BeamStockMonitoringReadModel>, IBeamStockMonitoringRepository
    {
        protected override BeamStockMonitoringDocument Map(BeamStockMonitoringReadModel readModel)
        {
            return new BeamStockMonitoringDocument(readModel);
        }
    }
}
