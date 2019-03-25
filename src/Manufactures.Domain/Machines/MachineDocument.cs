using Infrastructure.Domain;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Machines
{
    public class MachineDocument : AggregateRoot<MachineDocument, 
                                   MachineDocumentReadModel>
    {
        public string MachineNumber { get; private set; }
        public string Location { get; private set; }
        public MachineTypeId MachineTypeId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }

        public MachineDocument(Guid identity,
                               string machineNumber,
                               string location,
                               MachineTypeId machineTypeId,
                               UnitId weavingUnitId) : base(identity)
        {
            Identity = identity;
            MachineNumber = machineNumber;
            Location = location;
            MachineTypeId = machineTypeId;
            WeavingUnitId = weavingUnitId;

            this.MarkTransient();
            
            ReadModel = new MachineDocumentReadModel(Identity)
            {
                MachineNumber = this.MachineNumber,
                Location = this.Location,
                MachineTypeId = this.MachineTypeId.Value,
                WeavingUnitId = this.WeavingUnitId.Value
            };
        }

        public MachineDocument(MachineDocumentReadModel readModel): 
            base(readModel)
        {
            this.MachineNumber = readModel.MachineNumber;
            this.Location = readModel.Location;
            this.MachineTypeId =
                readModel.MachineTypeId.HasValue ? 
                    new MachineTypeId(readModel.MachineTypeId.Value) : null;
            this.WeavingUnitId =
                readModel.WeavingUnitId.HasValue ? 
                    new UnitId(readModel.WeavingUnitId.Value) : null;
        }

        public void SetLocation(string value)
        {
            if(!Location.Equals(value))
            {
                Location = value;
                ReadModel.Location = Location;

                MarkModified();
            }
        }

        public void SetMachineTypeId(MachineTypeId value)
        {
            if (MachineTypeId != value)
            {
                MachineTypeId = value;
                ReadModel.MachineTypeId = MachineTypeId.Value;

                MarkModified();
            }
        }

        public void SetWeavingUnitId(UnitId value)
        {
            if(WeavingUnitId != value)
            {
                WeavingUnitId = value;
                ReadModel.WeavingUnitId = WeavingUnitId.Value;

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
