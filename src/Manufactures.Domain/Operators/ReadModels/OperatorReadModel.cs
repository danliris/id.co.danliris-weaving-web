using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Operators.ReadModels
{
    public class OperatorReadModel : ReadModelBase
    {
        public OperatorReadModel(Guid identity) : base(identity) { }

        public string CoreAccount { get; internal set; }
        public string Group { get; internal set; }
        public string Assignment { get; internal set; }
        public string Type { get; internal set; }
    }
}
