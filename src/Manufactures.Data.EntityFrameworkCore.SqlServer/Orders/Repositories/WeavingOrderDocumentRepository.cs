using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.Orders.Repositories
{
    public class WeavingOrderDocumentRepository : AggregateRepostory<WeavingOrderDocument, WeavingOrderDocumentReadModel>, IWeavingOrderDocumentRepository
    {
        protected override WeavingOrderDocument Map(WeavingOrderDocumentReadModel readModel)
        {
            return new WeavingOrderDocument(readModel);
        }
    }
}
