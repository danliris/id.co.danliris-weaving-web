using Moonlay.Domain;
using System;
using Weaving.Domain.Entities;
using Weaving.Domain.ReadModels;
using Weaving.Domain.ValueObjects;

namespace Weaving.Domain
{
    public class ManufactureOrder : Entity, IAggregateRoot
    {
        private ManufactureOrderReadModel _ReadModel;

        public enum Status
        {
            Draft = 10,
            Active = 20,
            Finished = 30,
        }

        public ManufactureOrder(Guid id, DateTime orderDate, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsConstruction construction, Blended blended, MachineId machineId)
        {
            Identity = id;
            OrderDate = orderDate;
            UnitDepartmentId = unitId;
            YarnCodes = yarnCodes;
            Construction = construction;
            Blended = blended;
            MachineId = machineId;

            State = Status.Draft;
        }

        public ManufactureOrder(ManufactureOrderReadModel readModel)
        {
            this._ReadModel = readModel;

            this.Identity = _ReadModel.Identity;
            this.MachineId = new MachineId(_ReadModel.MachineId);
            this.Blended = _ReadModel.BlendedJson.Deserialize<Blended>();

            this.OrderDate = _ReadModel.OrderDate;
            this.State = _ReadModel.State;
            this.UnitDepartmentId = new UnitDepartmentId(_ReadModel.UnitDepartmentId);
            this.UserId = _ReadModel.UserId;
            this.YarnCodes = _ReadModel.YarnCodesJson.Deserialize<YarnCodes>();
        }

        public ManufactureOrderReadModel ReadModel => _ReadModel ?? new ManufactureOrderReadModel(Identity)
        {
            MachineId = this.MachineId.Value,
            UnitDepartmentId = this.UnitDepartmentId.Value,
            YarnCodesJson = this.YarnCodes.Serialize(),
            State = this.State,
            OrderDate = this.OrderDate,
            BlendedJson = this.Blended.Serialize(),
            UserId = this.UserId,
        };

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitDepartmentId { get; private set; }

        public void SetUnitDepartment(UnitDepartmentId newUnit)
        {
            if(newUnit != UnitDepartmentId)
            {
                UnitDepartmentId = newUnit;
                ReadModel.UnitDepartmentId = UnitDepartmentId.Value;
            }
        }

        public YarnCodes YarnCodes { get; private set; }
        public void SetYarnCodes(YarnCodes newCodes)
        {
            if(newCodes != YarnCodes)
            {
                YarnCodes = newCodes;
                ReadModel.YarnCodesJson = YarnCodes.Serialize();
            }
        }

        public GoodsConstruction Construction { get; }


        public Blended Blended { get; private set; }
        public void SetBlended(Blended newBlended)
        {
            if(newBlended != Blended)
            {
                Blended = newBlended;
                ReadModel.BlendedJson = Blended.Serialize();
            }
        }

        public MachineId MachineId { get; private set; }
        public void SetMachineId(MachineId newMachine)
        {
            if(newMachine != MachineId)
            {
                MachineId = newMachine;
                ReadModel.MachineId = MachineId.Value;
            }
        }

        public Status State { get; private set; }
        public void SetState(Status newState)
        {
            if(newState != State)
            {
                State = newState;
                ReadModel.State = State;
            }
        }

        /// <summary>
        /// Owner
        /// </summary>
        public string UserId { get; private set; }

        public void SetUserId(string userId)
        {
            UserId = userId;
        }
    }
}
