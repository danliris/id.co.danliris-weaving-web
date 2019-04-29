using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Beams.ReadModels
{
    public class BeamReadModel : ReadModelBase
    {
        public BeamReadModel(Guid identity) : base(identity) { }

        public string BeamNumber { get; internal set; }
        public string BeamType { get; internal set; }
    }
}
