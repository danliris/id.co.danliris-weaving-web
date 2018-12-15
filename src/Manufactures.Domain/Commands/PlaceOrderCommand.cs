using Infrastructure.Domain.Commands;
using Manufactures.Domain.ValueObjects;
using System;

namespace Manufactures.Domain.Commands
{
    public class PlaceOrderCommand : ICommand<ManufactureOrder>
    {
        public PlaceOrderCommand(DateTime orderDate, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsCompositionId compositionId, Blended blended, MachineId machineId, string userId)
        {
            OrderDate = orderDate;
            UnitDepartmentId = unitId;
            YarnCodes = yarnCodes;
            CompositionId = compositionId;
            Blended = blended;
            MachineId = machineId;
            UserId = userId;
        }

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitDepartmentId { get; }

        public YarnCodes YarnCodes { get; }

        public GoodsCompositionId CompositionId { get; }

        public Blended Blended { get; }

        public MachineId MachineId { get; }

        public string UserId { get;}
    }
}
