﻿using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Orders.ReadModels;

namespace Manufactures.Domain.Orders.Repositories
{
    public interface IWeavingOrderDocumentRepository : IAggregateRepository<WeavingOrderDocument, WeavingOrderDocumentReadModel>
    {
    }
}
