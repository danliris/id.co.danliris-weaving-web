using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.YarnNumbers.ReadModels
{
    public class YarnNumberDocumentReadModel : ReadModelBase
    {
        public YarnNumberDocumentReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public int Number { get; internal set; }
        public string Description { get; internal set; }
        public int? AdditionalNumber { get; internal set; }
        public string RingType { get; internal set; }
    }
}
