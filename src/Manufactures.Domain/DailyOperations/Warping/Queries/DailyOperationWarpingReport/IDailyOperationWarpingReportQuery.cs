using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport
{
    public interface IDailyOperationWarpingReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(string orderId, 
                                                    string materialTypeId,
                                                    string operationStatus,
                                                    int unitId,
                                                    DateTimeOffset? dateFrom, 
                                                    DateTimeOffset? dateTo, 
                                                    int page, 
                                                    int size,
                                                    string order);
    }
}
