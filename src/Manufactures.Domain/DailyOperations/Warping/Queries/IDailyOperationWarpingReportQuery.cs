using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries
{
    public interface IDailyOperationWarpingReportQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetReports(string orderId, 
                                             int weavingUnitId, 
                                             string materialTypeId, 
                                             DateTimeOffset? startDate, 
                                             DateTimeOffset? endDate, 
                                             string operationStatus, 
                                             int page, 
                                             int size);
    }
}
