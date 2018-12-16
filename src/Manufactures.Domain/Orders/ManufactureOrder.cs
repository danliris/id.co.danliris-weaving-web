using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Orders
{
    public class ManufactureOrder : AggregateRoot<ManufactureOrder, ManufactureOrderReadModel>
    {
        public enum Status
        {
            Draft = 10,
            Active = 20,
            Finished = 30,
        }

        public ManufactureOrder(Guid id, DateTimeOffset orderDate, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsCompositionId compositionId, Blended blended, MachineId machineId, string userId) : base(id)
        {
            // Validate Mandatory Properties
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => machineId);
            Validator.ThrowIfNullOrEmpty(() => yarnCodes);
            Validator.ThrowIfNullOrEmpty(() => blended);
            Validator.ThrowIfNullOrEmpty(() => userId);

            this.MarkTransient();

            // Set initial value
            Identity = id;
            OrderDate = orderDate;
            UnitDepartmentId = unitId;
            YarnCodes = yarnCodes;
            CompositionId = compositionId;
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

            if (this.CompositionId != null)
                ReadModel.CompositionId = Guid.Parse(CompositionId.Value);

            ReadModel.AddDomainEvent(new OnManufactureOrderPlaced(this.Identity));
        }

        public ManufactureOrder(ManufactureOrderReadModel readModel) : base(readModel)
        {
            this.MachineId = new MachineId(ReadModel.MachineId);
            this.Blended = ReadModel.BlendedJson.Deserialize<Blended>();

            this.OrderDate = ReadModel.OrderDate;
            this.State = ReadModel.State;
            this.UnitDepartmentId = new UnitDepartmentId(ReadModel.UnitDepartmentId);
            this.UserId = ReadModel.UserId;
            this.YarnCodes = ReadModel.YarnCodesJson.Deserialize<YarnCodes>();
            this.CompositionId = ReadModel.CompositionId.HasValue ? new GoodsCompositionId(ReadModel.CompositionId.Value.ToString()) : null;
        }

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitDepartmentId { get; private set; }

        public void SetUnitDepartment(UnitDepartmentId newUnit)
        {
            Validator.ThrowIfNull(() => newUnit);

            if (newUnit != UnitDepartmentId)
            {
                UnitDepartmentId = newUnit;
                ReadModel.UnitDepartmentId = UnitDepartmentId.Value;

                MarkModified();
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

                MarkModified();
            }
        }

        public GoodsCompositionId CompositionId { get; }

        public Blended Blended { get; private set; }

        public void SetBlended(Blended newBlended)
        {
            Validator.ThrowIfNullOrEmpty(() => newBlended);

            if (newBlended != Blended)
            {
                Blended = newBlended;
                ReadModel.BlendedJson = Blended.Serialize();

                MarkModified();
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

                MarkModified();
            }
        }

        public Status State { get; private set; }

        public void SetState(Status newState)
        {
            if (newState != State)
            {
                State = newState;
                ReadModel.State = State;

                MarkModified();
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

                MarkModified();
            }
        }

        protected override ManufactureOrder GetEntity()
        {
            return this;
        }
    }
}