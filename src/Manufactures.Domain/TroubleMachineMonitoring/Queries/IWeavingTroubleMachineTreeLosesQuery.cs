using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks; 
using Infrastructure.Domain.Queries;


namespace Manufactures.Domain.TroubleMachineMonitoring.Queries
{
    public interface IWeavingTroubleMachineTreeLosesQuery<TModel>: IQueries<TModel>
    {
       Task<bool> Upload(ExcelWorksheets sheet,string month,int year,int monthId);
        //List<TModel> GetReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string threadNo, string code);
        List<TModel> GetDataByFilter(string month, string yearPeriode);
    }
}
