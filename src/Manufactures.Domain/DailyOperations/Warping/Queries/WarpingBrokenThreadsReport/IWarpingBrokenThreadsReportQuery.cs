using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport
{
    public interface IWarpingBrokenThreadsReportQuery<TModel>
    {
        TModel GetReports(int month, int year, string weavingId);
    }
}
