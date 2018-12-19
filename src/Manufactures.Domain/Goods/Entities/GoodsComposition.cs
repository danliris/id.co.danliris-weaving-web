using Infrastructure.Domain;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Products.ReadModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manufactures.Domain.Goods.Entities
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

        public ProductGoodsReadModel Goods { get; set; }

        public Guid GoodsId { get; set; }

        protected override GoodsComposition GetEntity()
        {
            return this;
        }
    }
}