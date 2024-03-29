﻿using Manufactures.Domain.Suppliers;
using Newtonsoft.Json;
using System;

namespace Manufactures.DataTransferObjects.WeavingSupplier
{
    public class SupplierDocumentDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "CoreSupplierId")]
        public string CoreSupplierId { get; private set; }

        public SupplierDocumentDto(WeavingSupplierDocument weavingSupplierDocument)
        {
            Id = weavingSupplierDocument.Identity;
            Code = weavingSupplierDocument.Code;
            Name = weavingSupplierDocument.Name;
            CoreSupplierId = weavingSupplierDocument.CoreSupplierId;
        }
    }
}
