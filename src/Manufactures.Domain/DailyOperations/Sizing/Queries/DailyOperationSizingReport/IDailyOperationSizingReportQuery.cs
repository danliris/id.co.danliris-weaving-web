using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Sizing.Queries.DailyOperationSizingReport
{
    public interface IDailyOperationSizingReportQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetReports(string machineId,
                                             string orderId,
                                             int unitId,
                                             DateTimeOffset? dateFrom,
                                             DateTimeOffset? dateTo,
                                             string operationStatus,
                                             int page,
                                             int size);
    }
}
