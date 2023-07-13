
using System;
using System.Collections.Generic;

using Infrastructure.Domain.Queries;

namespace Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines
{
    public interface IWeavingDailyOperationSpuMachineQuery<TModel> : IQueries<TModel>
    {
       
        List<TModel> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code);
    }
}
