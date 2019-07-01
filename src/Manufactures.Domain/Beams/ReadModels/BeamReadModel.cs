using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Beams.ReadModels
{
    public class BeamReadModel : ReadModelBase
    {
        public BeamReadModel(Guid identity) : base(identity) { }

        public string Number { get; internal set; }
        public string Type { get; internal set; }
        public double EmtpyWeight { get; internal set; }
        public double? YarnLength { get; internal set; }
        public Guid? ContructionId { get; internal set; }
    }
}
