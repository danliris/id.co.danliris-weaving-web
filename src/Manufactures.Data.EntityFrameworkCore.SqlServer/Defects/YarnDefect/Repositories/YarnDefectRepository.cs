using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Defects.YarnDefect;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.Defects.YarnDefect.Repositories
{
    public class YarnDefectRepository : AggregateRepostory<YarnDefectDocument, YarnDefectReadModel>, IYarnDefectRepository
    {
        protected override YarnDefectDocument Map(YarnDefectReadModel readModel)
        {
            return new YarnDefectDocument(readModel);
        }
    }
}
