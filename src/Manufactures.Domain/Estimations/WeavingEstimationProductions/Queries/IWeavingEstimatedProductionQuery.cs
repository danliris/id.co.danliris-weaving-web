using Infrastructure.Domain.Queries;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.Estimations.WeavingEstimationProductions.Queries
{
    public interface IWeavingEstimatedProductionQuery<TModel> : IQueries<TModel>
    {
        Task<bool> Upload(ExcelWorksheets sheet, string month, int year, int monthId);
        // List<TModel> GetReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code);
        List<TModel> GetDataByFilter(string month, string yearPeriode);

    }
}
