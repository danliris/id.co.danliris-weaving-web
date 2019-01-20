using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Orders.Repositories
{
    public class WeavingOrderDocumentRepository : AggregateRepostory<WeavingOrderDocument, WeavingOrderDocumentReadModel>, IWeavingOrderDocumentRepository
    {
        protected override WeavingOrderDocument Map(WeavingOrderDocumentReadModel readModel)
        {
            return new WeavingOrderDocument(readModel);
        }

        public async Task<string> GetWeavingOrderNumber()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            var today = now.Year.ToString();
            var month = now.Month.ToString();
            var orderNumber = (this.dbSet.Where(order => order.Period.Deserialize<Period>().Year.Contains(today)).Count() + 1).ToString();
            orderNumber = orderNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + today;

            return await Task.FromResult(orderNumber);
        }
    }
}
