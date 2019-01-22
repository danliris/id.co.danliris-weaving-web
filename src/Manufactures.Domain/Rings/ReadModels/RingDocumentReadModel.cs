using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Rings.ReadModels
{
    public class RingDocumentReadModel : ReadModelBase
    {
        public RingDocumentReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }
}
