using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.MachineTypes.ReadModels
{
    public class MachineTypeReadModel : ReadModelBase
    {
        public MachineTypeReadModel(Guid identity) : base(identity) { }

        public string TypeName { get; internal set; }
        public int Speed { get; internal set; }
        public string MachineUnit { get; internal set; }
    }
}
