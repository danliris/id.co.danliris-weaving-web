using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.Goods.Entities;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Products.ReadModels
{
    public class ProductGoodsReadModel : ReadModelBase
    {
        public ProductGoodsReadModel(Guid identity) : base(identity)
        {
            Compositions = new List<GoodsComposition>();
        }

        public int ProductId { get; set; }

        public List<GoodsComposition> Compositions { get; set; }
    }
}