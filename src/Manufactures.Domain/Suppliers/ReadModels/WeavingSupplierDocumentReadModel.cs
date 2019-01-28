using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Suppliers.ReadModels
{
    public class WeavingSupplierDocumentReadModel : ReadModelBase
    {
        public WeavingSupplierDocumentReadModel(Guid identity) : base(identity) { }
        
        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public string CoreSupplierId { get; internal set; }
    }
}
