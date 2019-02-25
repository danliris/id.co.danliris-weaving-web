using Infrastructure.Domain;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Machines.ValueObjects;
using Moonlay;
using System;

namespace Manufactures.Domain.Machines
{
    public class MachineDocument : AggregateRoot<MachineDocument, MachineDocumentReadModel>
    {
        public string MachineNumber { get; private set; }
        public string Location { get; private set; }
        public MachineTypeValueObject MachineType { get; private set; }
        public WeavingUnit Unit { get; private set; }

        public MachineDocument(Guid identity,
                               string machineNumber,
                               string location,
                               MachineTypeValueObject machineType,
                               WeavingUnit unit) : base(identity)
        {
            Validator.ThrowIfNullOrEmpty(() => machineNumber);
            Validator.ThrowIfNullOrEmpty(() => location);
            Validator.ThrowIfNullOrEmpty(() => machineType.Name);
            Validator.ThrowIfNullOrEmpty(() => machineType.Unit);
            Validator.ThrowIfNullOrEmpty(() => unit._id);
            Validator.ThrowIfNullOrEmpty(() => unit.Code);
            Validator.ThrowIfNullOrEmpty(() => unit.Name);

            this.MarkTransient();

            Identity = identity;
            MachineNumber = machineNumber;
            Location = location;
            MachineType = MachineType;
            Unit = unit;

            ReadModel = new MachineDocumentReadModel(Identity)
            {
                MachineNumber = this.MachineNumber,
                Location = this.Location,
                MachineType = this.MachineType.Serialize(),
                Unit = this.Unit.Serialize()
            };
        }

        public MachineDocument(MachineDocumentReadModel readModel): base(readModel)
        {
            this.MachineNumber = readModel.MachineNumber;
            this.Location = readModel.Location;
            this.MachineType = readModel.MachineType.Deserialize<MachineTypeValueObject>();
            this.Unit = readModel.Unit.Deserialize<WeavingUnit>();
        }

        public void SetLocation(string value)
        {
            Validator.ThrowIfNullOrEmpty(() => value);

            if(!Location.Equals(value))
            {
                Location = value;
                ReadModel.Location = Location;

                MarkModified();
            }
        }

        public void SetMachineType(MachineTypeValueObject value)
        {
            Validator.ThrowIfNullOrEmpty(() => value.Name);

            if (!MachineType.Name.Equals(value.Name))
            {
                MachineType = value;
                ReadModel.MachineType = MachineType.Serialize();

                MarkModified();
            }
        }

        public void SetUnit(WeavingUnit value)
        {
            Validator.ThrowIfNullOrEmpty(() => value._id);
            Validator.ThrowIfNullOrEmpty(() => value.Code);
            Validator.ThrowIfNullOrEmpty(() => value.Name);

            if(!value._id.Equals(value._id))
            {
                Unit = value;
                ReadModel.Unit = Unit.Serialize();

                MarkModified();
            }
        }

        public void SetMachineNumber(string value)
        {
            if(!MachineNumber.Equals(value))
            {
                MachineNumber = value;
                ReadModel.MachineNumber = MachineNumber;

                MarkModified();
            }
        }

        protected override MachineDocument GetEntity()
        {
            return this;
        }
    }
}
