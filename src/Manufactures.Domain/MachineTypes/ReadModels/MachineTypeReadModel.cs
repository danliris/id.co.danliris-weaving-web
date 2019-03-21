using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MachineTypes.ReadModels
{
    public class MachineTypeReadModel : ReadModelBase
    {
        public MachineTypeReadModel(Guid id) : base(id) { }

        public string TypeName { get; internal set; }
        public int Speed { get; internal set; }
        public string MachineUnit { get; internal set; }
    }
}
