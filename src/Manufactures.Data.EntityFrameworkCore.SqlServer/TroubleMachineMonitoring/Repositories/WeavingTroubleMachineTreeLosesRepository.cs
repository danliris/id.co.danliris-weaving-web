using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.TroubleMachineMonitoring.Entities;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.TroubleMachineMonitoring.Repositories
{
    public class WeavingTroubleMachineTreeLosesRepository : AggregateRepostory<WeavingTroubleMachineTreeLoses, WeavingTroubleMachineTreeLosesReadModel>, IWeavingTroubleMachineTreeLosesRepository
    {
        protected override WeavingTroubleMachineTreeLoses Map(WeavingTroubleMachineTreeLosesReadModel readModel)
        {
            return new WeavingTroubleMachineTreeLoses(readModel);
        }
    }
}