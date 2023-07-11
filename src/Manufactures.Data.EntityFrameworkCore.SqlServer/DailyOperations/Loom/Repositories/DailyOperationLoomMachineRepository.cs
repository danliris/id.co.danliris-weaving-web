using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.DailyOperations.Loom.Repositories
{
    public class DailyOperationLoomMachineRepository : AggregateRepostory<DailyOperationLoomMachine, DailyOperationLoomMachineReadModel>, IDailyOperationLoomMachineRepository
    {
        protected override DailyOperationLoomMachine Map(DailyOperationLoomMachineReadModel readModel)
        {
            return new DailyOperationLoomMachine(readModel);
        }
    }
}