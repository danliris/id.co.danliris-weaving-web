using System;
using Moonlay.Domain;
using Weaving.Domain.Entities;
using Weaving.Domain.ValueObjects;

namespace Weaving.Domain
{
    public class ManufactureOrder : Entity, IAggregateRoot
    {
        public enum Status
        {
            Draft = 10,
            Active = 20,
            Finished = 30,
        }

        public ManufactureOrder(Guid id, DateTime date, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsConstruction construction, Blended blended, MachineId machineId)
        {
            Identity = id;
            OrderDate = date;
            UnitId = unitId;
            YarnCodes = yarnCodes;
            Construction = construction;
            Blended = blended;
            MachineId = machineId;

            State = Status.Draft;

            Created = DateTime.Now;
        }

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitId { get; }

        public YarnCodes YarnCodes { get; }

        public GoodsConstruction Construction { get; }

        public Blended Blended { get; }

        public MachineId MachineId { get; }

        public Status State { get; }

        public DateTimeOffset Created { get; }
    }
}
