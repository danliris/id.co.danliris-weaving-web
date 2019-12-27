using Manufactures.Domain.Suppliers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.DataTransferObjects.WeavingSupplier
{
    public class SupplierListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        public SupplierListDto(WeavingSupplierDocument weavingSupplierDocument)
        {
            Id = weavingSupplierDocument.Identity;
            Code = weavingSupplierDocument.Code;
            Name = weavingSupplierDocument.Name;
        }
    }
}
