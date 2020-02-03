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
    public class OrderDocumentRepository : AggregateRepostory<OrderDocument, OrderReadModel>, IOrderRepository
    {
        protected override OrderDocument Map(OrderReadModel readModel)
        {
            return new OrderDocument(readModel);
        }
    }
}
