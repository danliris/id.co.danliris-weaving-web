using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Defects.FabricDefect;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.Defects.FabricDefect.Repositories
{
    public class FabricDefectRepository : AggregateRepostory<FabricDefectDocument, FabricDefectReadModel>, IFabricDefectRepository
    {
        protected override FabricDefectDocument Map(FabricDefectReadModel readModel)
        {
            return new FabricDefectDocument(readModel);
        }
    }
}
