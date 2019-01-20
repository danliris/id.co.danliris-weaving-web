using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Construction;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Construction.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Construction.Repositories
{
    public class ConstructionDocumentRepository : AggregateRepostory<ConstructionDocument, ConstructionDocumentReadModel>, IConstructionDocumentRepository
    {
        protected override ConstructionDocument Map(ConstructionDocumentReadModel readModel)
        {
            return new ConstructionDocument(readModel);
        }
    }
}
