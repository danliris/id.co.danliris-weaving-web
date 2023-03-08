using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.TroubleMachineMonitoring.Entities;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.TroubleMachineMonitoring.Repositories
{
    public interface IWeavingTroubleMachineTreeLosesRepository : IAggregateRepository<WeavingTroubleMachineTreeLoses, WeavingTroubleMachineTreeLosesReadModel>
    {
       
    }
}