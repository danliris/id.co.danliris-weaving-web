using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport
{
    public interface IDailyOperationWarpingReportQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetReports(string orderId, 
                                             int weavingUnitId, 
                                             string materialTypeId, 
                                             DateTimeOffset? dateFrom, 
                                             DateTimeOffset? dateTo, 
                                             string operationStatus, 
                                             int page, 
                                             int size);
    }
}
