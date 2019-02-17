using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Machines.ReadModels
{
    public class MachineDocumentReadModel : ReadModelBase
    {
        public MachineDocumentReadModel(Guid identity) : base(identity) { }
    }
}
