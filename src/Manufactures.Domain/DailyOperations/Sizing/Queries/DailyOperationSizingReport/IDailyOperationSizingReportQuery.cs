using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Sizing.Queries.DailyOperationSizingReport
{
    public interface IDailyOperationSizingReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(string machineId,
                                                    string orderId,
                                                    string operationStatus,
                                                    int unitId,
                                                    DateTimeOffset? dateFrom,
                                                    DateTimeOffset? dateTo,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
