using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Rings.ReadModels
{
    public class RingDocumentReadModel : ReadModelBase
    {
        public RingDocumentReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public int Number { get; internal set; }
        public string Description { get; internal set; }
    }
}
