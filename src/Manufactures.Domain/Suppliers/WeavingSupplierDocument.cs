﻿using Infrastructure.Domain;
using Manufactures.Domain.Suppliers.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manufactures.Domain.Suppliers
{
    public class WeavingSupplierDocument : AggregateRoot<WeavingSupplierDocument, WeavingSupplierDocumentReadModel>, IValidatableObject
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string CoreSupplierId { get; private set; }

        public WeavingSupplierDocument(Guid id, 
                                       string code,
                                       string name,
                                       string coreSupplierId) : base(id)
        {
            this.MarkTransient();

            // Set Properties
            Identity = id;
            Code = code;
            Name = name;
            CoreSupplierId = coreSupplierId;

            ReadModel = new WeavingSupplierDocumentReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                CoreSupplierId = this.CoreSupplierId
            };
        }

        public WeavingSupplierDocument(WeavingSupplierDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.CoreSupplierId = readModel.CoreSupplierId;
        }


        public void SetCode(string code)
        {
            if(code != Code)
            {
                Code = code;
                ReadModel.Code = Code;

                MarkModified();
            }
        }

        public void SetName(string name)
        {
            if(name != Name)
            {
                Name = name;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetCoreSupplierId(string id)
        {
            if(id != CoreSupplierId)
            {
                CoreSupplierId = id;
                ReadModel.CoreSupplierId = CoreSupplierId;

                MarkModified();
            }
        }

        protected override WeavingSupplierDocument GetEntity()
        {
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
