using Infrastructure.Domain.Queries;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Queries
{
    public interface IWeavingDailyOperationMachineSizingQuery<TModel> : IQueries<TModel>
    {
        Task<bool> Upload(ExcelWorksheets sheet, string month, string year, int monthId);
        // List<TModel> GetReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code);
        List<TModel> GetDataByFilter(string month, string yearPeriode);

    }
}
