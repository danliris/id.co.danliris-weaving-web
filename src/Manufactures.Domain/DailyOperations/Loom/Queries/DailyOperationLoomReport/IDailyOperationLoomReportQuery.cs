using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Loom.Queries.DailyOperationLoomReport
{
    public interface IDailyOperationLoomReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(string orderId,
                                                    string constructionId,
                                                    string operationStatus,
                                                    int unitId,
                                                    DateTimeOffset? dateFrom,
                                                    DateTimeOffset? dateTo,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
