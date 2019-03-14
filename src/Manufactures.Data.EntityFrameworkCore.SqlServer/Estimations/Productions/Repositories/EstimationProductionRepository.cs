using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class EstimationProductionRepository : AggregateRepostory<EstimatedProductionDocument, EstimatedProductionDocumentReadModel>, IEstimationProductRepository
    {
        protected override EstimatedProductionDocument Map(EstimatedProductionDocumentReadModel readModel)
        {
            return new EstimatedProductionDocument(readModel);
        }

        public async Task<string> GetEstimationNumber()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString();
            var estimationNumber = (this.dbSet.Where(o => o.Period.Deserialize<Period>().Year.Contains(year))).Count() + 1.ToString();
            estimationNumber = estimationNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + year;

            return await Task.FromResult(estimationNumber);
        }
    }
}
