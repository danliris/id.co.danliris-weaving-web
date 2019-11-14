using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Reaching.Queries.DailyOperationReachingReport
{
    public interface IDailyOperationReachingReportQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetReports(string machineId,
                                             string orderId,
                                             string constructionId,
                                             string beamId,
                                             string operationStatus,
                                             int unitId,
                                             DateTimeOffset? dateFrom,
                                             DateTimeOffset? dateTo,
                                             int page,
                                             int size,
                                             string order);
    }
}
