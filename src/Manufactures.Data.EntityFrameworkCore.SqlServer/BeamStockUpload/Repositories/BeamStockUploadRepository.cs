using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.BeamStockUpload.Entities;
using Manufactures.Domain.BeamStockUpload.ReadModels;
using Manufactures.Domain.BeamStockUpload.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.BeamStockUpload.Repositories
{
    public class BeamStockUploadRepository : AggregateRepostory<BeamStock, BeamStockReadModel>, IBeamStockRepository
    {
        protected override BeamStock Map(BeamStockReadModel readModel)
        {
            return new BeamStock(readModel);
        }
    }
}
