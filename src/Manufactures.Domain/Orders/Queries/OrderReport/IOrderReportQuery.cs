using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.Orders.Queries.OrderReport
{
    public interface IOrderReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(int weavingUnitId,
                                                    DateTimeOffset? dateFrom,
                                                    DateTimeOffset? dateTo,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
