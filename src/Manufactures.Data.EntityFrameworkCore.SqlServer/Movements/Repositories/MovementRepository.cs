using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Movements;
using Manufactures.Domain.Movements.ReadModels;
using Manufactures.Domain.Movements.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.Movements.Repositories
{
    public class MovementRepository : AggregateRepostory<MovementDocument, MovementReadModel>, IMovementRepository
    {
        protected override MovementDocument Map(MovementReadModel readModel)
        {
            return new MovementDocument(readModel);
        }
    }
}
