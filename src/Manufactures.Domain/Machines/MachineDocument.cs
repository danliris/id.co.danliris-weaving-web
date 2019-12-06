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
        public int Cutmark { get; private set; }
        public UomId CutmarkUomId { get; private set; }

        public MachineDocument(Guid identity,
                               string machineNumber,
                               string location,
                               MachineTypeId machineTypeId,
                               UnitId weavingUnitId,
                               int cutmark,
                               UomId cutmarkUomId) : base(identity)
        {
            Identity = identity;
            MachineNumber = machineNumber;
            Location = location;
            MachineTypeId = machineTypeId;
            WeavingUnitId = weavingUnitId;
            Cutmark = cutmark;
            CutmarkUomId = cutmarkUomId;

            this.MarkTransient();

            ReadModel = new MachineDocumentReadModel(Identity)
            {
                MachineNumber = this.MachineNumber,
                Location = this.Location,
                MachineTypeId = this.MachineTypeId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                Cutmark = this.Cutmark,
                CutmarkUomId = this.CutmarkUomId.Value
            };
        }

        public MachineDocument(MachineDocumentReadModel readModel) :
            base(readModel)
        {
            this.MachineNumber = readModel.MachineNumber;
            this.Location = readModel.Location;
            this.MachineTypeId = readModel.MachineTypeId.HasValue ? new MachineTypeId(readModel.MachineTypeId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.Cutmark = readModel.Cutmark.HasValue ? readModel.Cutmark.Value : 0;
            this.CutmarkUomId = readModel.CutmarkUomId.HasValue ? new UomId(readModel.CutmarkUomId.Value) : null;
        }

        public void SetLocation(string value)
        {
            if (!Location.Equals(value))
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
            if (WeavingUnitId != value)
            {
                WeavingUnitId = value;
                ReadModel.WeavingUnitId = WeavingUnitId.Value;

                MarkModified();
            }
        }

        public void SetMachineNumber(string value)
        {
            if (!MachineNumber.Equals(value))
            {
                MachineNumber = value;
                ReadModel.MachineNumber = MachineNumber;

                MarkModified();
            }
        }

        public void SetCutmark(int cutmark)
        {
            if (!Cutmark.Equals(cutmark))
            {
                Cutmark = cutmark;
                ReadModel.Cutmark = Cutmark;

                MarkModified();
            }
        }

        public void SetCutmarkUomId(UomId cutmarkUomId)
        {
            if (!CutmarkUomId.Equals(cutmarkUomId))
            {
                CutmarkUomId = cutmarkUomId;
                ReadModel.CutmarkUomId = CutmarkUomId.Value;

                MarkModified();
            }
        }

        protected override MachineDocument GetEntity()
        {
            return this;
        }
    }
}
