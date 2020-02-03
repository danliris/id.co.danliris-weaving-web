using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.FabricConstructions.Repositories
{
    public class ConstructionYarnDetailRepository : AggregateRepostory<ConstructionYarnDetail, ConstructionYarnDetailReadModel>, IConstructionYarnDetailRepository
    {
        protected override ConstructionYarnDetail Map(ConstructionYarnDetailReadModel readModel)
        {
            return new ConstructionYarnDetail(readModel);
        }
    }
}
