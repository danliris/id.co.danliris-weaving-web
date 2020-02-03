using Infrastructure.Domain.Repositories;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.FabricConstructions.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.Repositories
{
    public interface IConstructionYarnDetailRepository : IAggregateRepository<ConstructionYarnDetail, ConstructionYarnDetailReadModel>
    {
    }
}
