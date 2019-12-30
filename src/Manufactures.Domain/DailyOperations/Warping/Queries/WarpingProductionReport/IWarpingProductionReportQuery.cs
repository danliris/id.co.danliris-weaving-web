using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport
{
    public interface IWarpingProductionReportQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReports(int month,
                                                    int page,
                                                    int size,
                                                    string order);
    }
}
