using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Construction.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Construction.Repositories
{
    public interface IConstructionDocumentRepository : IAggregateRepository<ConstructionDocument, ConstructionDocumentReadModel>
    {
    }
}
