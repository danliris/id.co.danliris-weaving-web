using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Dtos.Machine
{
    public class MachineDocumentDto
    {
        public string MachineNumber { get; private set; }
        public string Location { get; private set; }
        public MachineTypeId MachineTypeId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }

        public MachineDocumentDto(MachineDocument document)
        {
            MachineNumber = document.MachineNumber;
            Location = document.Location;
            MachineTypeId = document.MachineTypeId;
            WeavingUnitId = document.WeavingUnitId;
        }

        public MachineDocumentDto(MachineDocument machine, 
                                  MachineTypeValueObject machineTypeValueObject, 
                                  WeavingUnit weavingUnitValueObject)
        {

        }
    }
}
