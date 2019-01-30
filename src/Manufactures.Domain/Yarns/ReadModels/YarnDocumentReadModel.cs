using Infrastructure.Domain.ReadModels;
using System;

namespace Manufactures.Domain.Yarns.ReadModels
{
    public class YarnDocumentReadModel : ReadModelBase
    {
        public YarnDocumentReadModel(Guid identity) : base(identity) { }

        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string Tags { get; internal set; }
        public string CoreCurrency { get; internal set; }
        public string CoreUom { get; internal set; }
        public string MaterialTypeDocument { get; internal set; }
        public string RingDocument { get; internal set; }
        public double Price { get; internal set; }
    }
}
