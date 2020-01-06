using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport
{
    public interface IWarpingProductionReportQuery<TModel>
    {
        Task<TModel> GetReports(int month, int year);
    }
}