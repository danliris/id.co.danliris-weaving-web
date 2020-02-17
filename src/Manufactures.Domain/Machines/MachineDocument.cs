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
        public string CutmarkUom { get; private set; }
        public string Process { get; private set; }
        public string Area { get; private set; }
        public int Block { get; private set; }

        public MachineDocument(Guid identity,
                               string machineNumber,
                               string location,
                               MachineTypeId machineTypeId,
                               UnitId weavingUnitId,
                               string process,
                               string area, 
                               int block) : base(identity)
        {
            Identity = identity;
            MachineNumber = machineNumber;
            Location = location;
            MachineTypeId = machineTypeId;
            WeavingUnitId = weavingUnitId;
            Process = process;
            Area = area;
            Block = block;

            this.MarkTransient();

            ReadModel = new MachineDocumentReadModel(Identity)
            {
                MachineNumber = this.MachineNumber,
                Location = this.Location,
                MachineTypeId = this.MachineTypeId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                Cutmark = this.Cutmark,
                CutmarkUom = this.CutmarkUom,
                Process = this.Process,
                Area = this.Area,
                Block = this.Block
            };
        }

        public MachineDocument(MachineDocumentReadModel readModel) :
            base(readModel)
        {
            this.MachineNumber = readModel.MachineNumber;
            this.Location = readModel.Location;
            this.MachineTypeId = readModel.MachineTypeId.HasValue ? new MachineTypeId(readModel.MachineTypeId.Value) : null;
            this.WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            this.Cutmark = readModel.Cutmark ?? 0;
            this.CutmarkUom = readModel.CutmarkUom ?? "";
            this.Process = readModel.Process;
            this.Area = readModel.Area;
            this.Block = readModel.Block;
        }

        public void SetLocation(string location)
        {
            if (Location != location)
            {
                Location = location;
                ReadModel.Location = Location;

                MarkModified();
            }
        }

        public void SetMachineTypeId(MachineTypeId machineTypeId)
        {
            if (MachineTypeId != machineTypeId)
            {
                MachineTypeId = machineTypeId;
                ReadModel.MachineTypeId = MachineTypeId.Value;

                MarkModified();
            }
        }

        public void SetWeavingUnitId(UnitId unitId)
        {
            if (WeavingUnitId != unitId)
            {
                WeavingUnitId = unitId;
                ReadModel.WeavingUnitId = WeavingUnitId.Value;

                MarkModified();
            }
        }

        public void SetMachineNumber(string machineNumber)
        {
            if (MachineNumber != machineNumber)
            {
                MachineNumber = machineNumber;
                ReadModel.MachineNumber = MachineNumber;

                MarkModified();
            }
        }

        public void SetCutmark(int cutmark)
        {
            if (Cutmark != cutmark)
            {
                Cutmark = cutmark;
                ReadModel.Cutmark = Cutmark;

                MarkModified();
            }
        }

        public void SetCutmarkUom(string cutmarkUom)
        {
            if (CutmarkUom != cutmarkUom)
            {
                CutmarkUom = cutmarkUom;
                ReadModel.CutmarkUom = CutmarkUom;

                MarkModified();
            }
        }

        public void SetProcess(string process)
        {
            if (Process != process)
            {
                Process = process;
                ReadModel.Process = Process;

                MarkModified();
            }
        }

        public void SetArea(string area)
        {
            if (Area != area)
            {
                Area = area;
                ReadModel.Area = Area;

                MarkModified();
            }
        }

        public void SetBlock(int block)
        {
            if (Block != block)
            {
                Block = block;
                ReadModel.Block = Block;

                MarkModified();
            }
        }

        protected override MachineDocument GetEntity()
        {
            return this;
        }
    }
}
