using Manufactures.Domain.Suppliers;
using System;

namespace Manufactures.Dtos
{
    public class SupplierDto
    {
        public SupplierDto(WeavingSupplierDocument weavingSupplierDocument)
        {
            Id = weavingSupplierDocument.Identity;
            Code = weavingSupplierDocument.Code;
            Name = weavingSupplierDocument.Name;
            CoreSupplierId = weavingSupplierDocument.CoreSupplierId;
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string CoreSupplierId { get; private set; }
    }
}
