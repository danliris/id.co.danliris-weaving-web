using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MachineTypes.ReadModels
{
    public class MachineTypeReadModel : ReadModelBase
    {
        public MachineTypeReadModel(Guid id) : base(id) { }
    }
}
