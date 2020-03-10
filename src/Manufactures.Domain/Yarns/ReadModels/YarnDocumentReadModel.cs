using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Yarns.ReadModels
{
    public class YarnDocumentReadModel : ReadModelBase
    {
        public YarnDocumentReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public string Tags { get; internal set; }
        public Guid MaterialTypeId { get; internal set; }
        public Guid YarnNumberId { get; internal set; }
    }
}
