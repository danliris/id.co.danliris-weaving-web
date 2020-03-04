using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Sizing.Queries.SizePickupReport
{
    public interface ISizePickupReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReportsAsync(string shiftId,
                                                    string spuStatus,
                                                    int unitId,
                                                    DateTimeOffset? date,
                                                    DateTimeOffset? dateFrom,
                                                    DateTimeOffset? dateTo,
                                                    int? month,
                                                    int? year,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
