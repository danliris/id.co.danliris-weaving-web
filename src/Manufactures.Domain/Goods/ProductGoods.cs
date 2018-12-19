using Infrastructure.Domain;
using Manufactures.Domain.Goods.Entities;
using Manufactures.Domain.Products.ReadModels;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Goods
{
    public class ProductGoods : AggregateRoot<ProductGoods, ProductGoodsReadModel>
    {
        public ProductGoods(Guid identity, int productId) : base(identity)
        {
            ProductId = productId;

            Compositions = new List<GoodsComposition>().AsReadOnly();
        }

        public ProductGoods(ProductGoodsReadModel readModel) : base(readModel)
        {
            ProductId = readModel.ProductId;
            Compositions = readModel.Compositions.AsReadOnly();
        }

        public int ProductId { get; private set; }

        public IReadOnlyCollection<GoodsComposition> Compositions { get; private set; }

        protected override ProductGoods GetEntity()
        {
            return this;
        }
    }
}