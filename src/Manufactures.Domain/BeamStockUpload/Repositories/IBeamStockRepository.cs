using Infrastructure.Domain.Repositories;
using Manufactures.Domain.BeamStockUpload.Entities;
using Manufactures.Domain.BeamStockUpload.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BeamStockUpload.Repositories
{
    public interface IBeamStockRepository : IAggregateRepository<BeamStock, BeamStockReadModel>
    {
    }
}
