using Infrastructure.Domain;
using Manufactures.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manufactures.Domain.Entities
{
    public class GoodsComposition : EntityBase<GoodsComposition>
    {
        public GoodsComposition(Guid identity) : base(identity)
        {
        }

        public GoodsComposition(Guid identity, MaterialIds materialIds) : base(identity)
        {
            Identity = identity;
            MaterialIds = materialIds;
        }

        [NotMapped]
        public MaterialIds MaterialIds { get; private set; }

        public void SetMaterialIds(MaterialIds newMaterialIds)
        {
            if (newMaterialIds != MaterialIds)
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

        protected override GoodsComposition GetEntity()
        {
            return this;
        }
    }
}