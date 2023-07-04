using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Productions;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class DailyOperationMachineSizingDocumentRepository : AggregateRepostory<DailyOperationMachineSizingDocument, DailyOperationMachineSizingDocumentReadModel>, IDailyOperationMachineSizingDocumentRepository
    {
        protected override DailyOperationMachineSizingDocument Map(DailyOperationMachineSizingDocumentReadModel readModel)
        {
            return new DailyOperationMachineSizingDocument(readModel);
        }

        //public async Task<string> GetEstimationNumber()
        //{
        //    DateTimeOffset now = DateTimeOffset.Now;
        //    var year = now.Year.ToString();
        //    var month = now.Month.ToString();
        //    var estimationNumber = (this.dbSet.Where(o => o.Period.Deserialize<Period>().Year.Contains(year))).Count() + 1.ToString();
        //    estimationNumber = estimationNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + year;

        //    return await Task.FromResult(estimationNumber);
        //}
    }
}
