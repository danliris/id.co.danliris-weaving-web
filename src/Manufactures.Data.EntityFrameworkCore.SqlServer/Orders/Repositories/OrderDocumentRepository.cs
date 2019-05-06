using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Orders.Repositories
{
    public class OrderDocumentRepository : AggregateRepostory<OrderDocument, OrderDocumentReadModel>, IWeavingOrderDocumentRepository
    {
        protected override OrderDocument Map(OrderDocumentReadModel readModel)
        {
            return new OrderDocument(readModel);
        }

        public async Task<string> GetWeavingOrderNumber()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString();
            var orderNumber = (this.dbSet.Where(order => order.Period.Deserialize<Period>().Year.Contains(year)).Count() + 1).ToString();
            orderNumber = orderNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + year;

            return await Task.FromResult(orderNumber);
        }
    }
}
