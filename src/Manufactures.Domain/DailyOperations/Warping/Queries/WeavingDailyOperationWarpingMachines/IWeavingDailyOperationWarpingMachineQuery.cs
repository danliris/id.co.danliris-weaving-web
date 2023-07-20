using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Infrastructure.Domain.Queries;

namespace Manufactures.Domain.DailyOperations.Warping.Queries.WeavingDailyOperationWarpingMachines
{
    public interface IWeavingDailyOperationWarpingMachineQuery<TModel> : IQueries<TModel>
    {
        Task<bool> Upload(ExcelWorksheets sheet,string month,int year,int monthId);
        List<TModel> GetReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code);
        List<TModel> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code);
    }
}
