
using System;
using System.Collections.Generic;

using Infrastructure.Domain.Queries;

namespace Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines
{
    public interface IWeavingDailyOperationSpuMachineQuery<TModel> : IQueries<TModel>
    {
       
        List<TModel> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code);
        List<TModel> GetDailySizingReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string groupui,  string name, string code,string sp);
        //DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui, string name, string code,string sp
    }
}
