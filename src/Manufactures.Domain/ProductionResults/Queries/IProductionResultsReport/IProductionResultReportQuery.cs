using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.ProductionResults.Queries.IProductionResultsReport
{
    public interface IProductionResultReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(string machineId,
                                                    string orderId,
                                                    string shiftId,
                                                    int weavingUnitId,
                                                    DateTimeOffset? dateFrom,
                                                    DateTimeOffset? dateTo,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
