using Infrastructure.Domain.Events;
using Moonlay;
using Moonlay.Domain;
using System;
using Manufactures.Domain.Entities;
using Manufactures.Domain.Events;
using Manufactures.Domain.ReadModels;
using Manufactures.Domain.ValueObjects;

namespace Manufactures.Domain
{
    public class ManufactureOrder : Entity, IAggregateRoot
    {
        private bool _Changed = false;

        public enum Status
        {
            Draft = 10,
            Active = 20,
            Finished = 30,
        }

        public ManufactureOrder(Guid id, DateTime orderDate, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsConstruction construction, Blended blended, MachineId machineId, string userId)
        {
            // Validate Mandatory Properties
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => machineId);
            Validator.ThrowIfNullOrEmpty(() => yarnCodes);
            Validator.ThrowIfNullOrEmpty(() => blended);
            Validator.ThrowIfNullOrEmpty(() => userId);


            // Set initial value
            Identity = id;
            OrderDate = orderDate;
            UnitDepartmentId = unitId;
            YarnCodes = yarnCodes;
            Construction = construction;
            Blended = blended;
            MachineId = machineId;
            UserId = userId;
            State = Status.Draft;

            ReadModel = new ManufactureOrderReadModel(Identity)
            {
                MachineId = this.MachineId.Value,
                UnitDepartmentId = this.UnitDepartmentId.Value,
                YarnCodesJson = this.YarnCodes.Serialize(),
                State = this.State,
                OrderDate = this.OrderDate,
                BlendedJson = this.Blended.Serialize(),
                UserId = this.UserId,
            };

            ReadModel.AddDomainEvent(new OnEntityCreated<ManufactureOrder>(this));
            ReadModel.AddDomainEvent(new OnManufactureOrderPlaced(this.Identity));
        }

        public ManufactureOrder(ManufactureOrderReadModel readModel)
        {
            this.ReadModel = readModel;

            this.Identity = ReadModel.Identity;
            this.MachineId = new MachineId(ReadModel.MachineId);
            this.Blended = ReadModel.BlendedJson.Deserialize<Blended>();

            this.OrderDate = ReadModel.OrderDate;
            this.State = ReadModel.State;
            this.UnitDepartmentId = new UnitDepartmentId(ReadModel.UnitDepartmentId);
            this.UserId = ReadModel.UserId;
            this.YarnCodes = ReadModel.YarnCodesJson.Deserialize<YarnCodes>();

            _Changed = false;
        }

        public ManufactureOrderReadModel ReadModel { get; }

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitDepartmentId { get; private set; }
        public void SetUnitDepartment(UnitDepartmentId newUnit)
        {
            Validator.ThrowIfNull(() => newUnit);

            if(newUnit != UnitDepartmentId)
            {
                UnitDepartmentId = newUnit;
                ReadModel.UnitDepartmentId = UnitDepartmentId.Value;

                _Changed = true;
            }
        }

        public YarnCodes YarnCodes { get; private set; }
        public void SetYarnCodes(YarnCodes newCodes)
        {
            Validator.ThrowIfNullOrEmpty(() => newCodes);

            if (newCodes != YarnCodes)
            {
                YarnCodes = newCodes;
                ReadModel.YarnCodesJson = YarnCodes.Serialize();

                _Changed = true;
            }
        }

        public GoodsConstruction Construction { get; }


        public Blended Blended { get; private set; }
        public void SetBlended(Blended newBlended)
        {
            Validator.ThrowIfNullOrEmpty(() => newBlended);

            if (newBlended != Blended)
            {
                Blended = newBlended;
                ReadModel.BlendedJson = Blended.Serialize();

                _Changed = true;
            }
        }

        public MachineId MachineId { get; private set; }
        public void SetMachineId(MachineId newMachine)
        {
            Validator.ThrowIfNull(() => newMachine);

            if (newMachine != MachineId)
            {
                MachineId = newMachine;
                ReadModel.MachineId = MachineId.Value;

                _Changed = true;
            }
        }

        public Status State { get; private set; }
        public void SetState(Status newState)
        {
            if(newState != State)
            {
                State = newState;
                ReadModel.State = State;

                _Changed = true;
            }
        }

        /// <summary>
        /// Owner
        /// </summary>
        public string UserId { get; private set; }

        public void SetUserId(string newUser)
        {
            Validator.ThrowIfNullOrEmpty(() => newUser);

            if (newUser != UserId)
            {
                UserId = newUser;
                ReadModel.UserId = UserId;

                _Changed = true;
            }
        }

        public void MarkChanged()
        {
            if (_Changed)
                ReadModel.AddDomainEvent(new OnEntityUpdated<ManufactureOrder>(this));
        }

        public void MarkRemoved()
        {
            ReadModel.AddDomainEvent(new OnEntityDeleted<ManufactureOrder>(this));
        }
    }
}
