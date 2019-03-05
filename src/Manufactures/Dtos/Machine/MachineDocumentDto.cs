using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ValueObjects;

namespace Manufactures.Dtos.Machine
{
    public class MachineDocumentDto
    {
        public MachineDocumentDto(MachineDocument document)
        {

        }

        public MachineDocumentDto(MachineDocument machine, 
                                  MachineTypeValueObject machineTypeValueObject, 
                                  WeavingUnit weavingUnitValueObject)
        {

        }
    }
}
