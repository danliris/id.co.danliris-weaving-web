using Infrastructure.Domain;
using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manufactures.Domain.Entities
{
    public class GoodsConstruction : EntityBase<GoodsConstruction>
    {
        public GoodsConstruction(Guid identity) : base(identity)
        {

        }

        public GoodsConstruction(Guid identity, MaterialIds materialIds) : base(identity)
        {
            Identity = identity;
            MaterialIds = materialIds;
        }

        [NotMapped]
        public MaterialIds MaterialIds { get; private set; }
        public void SetMaterialIds(MaterialIds newMaterialIds)
        {
            if(newMaterialIds != MaterialIds)
            {
                this.MaterialIds = newMaterialIds;

                this.MarkModified();
            }
        }

        public string MaterialIdsJson
        {
            get => MaterialIds.Serialize();
            set => MaterialIds = value.Deserialize<MaterialIds>();
        }

        protected override GoodsConstruction GetEntity()
        {
            return this;
        }
    }
}
