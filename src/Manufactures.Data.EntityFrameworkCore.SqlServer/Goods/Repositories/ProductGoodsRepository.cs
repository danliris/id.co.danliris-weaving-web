using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Goods;
using Manufactures.Domain.Goods.Repositories;
using Manufactures.Domain.Products.ReadModels;

namespace Manufactures.Data.EntityFrameworkCore.Goods.Repositories
{
    public class ProductGoodsRepository : AggregateRepostory<ProductGoods, ProductGoodsReadModel>, IProductGoodsRepository
    {
        protected override ProductGoods Map(ProductGoodsReadModel readModel)
        {
            return new ProductGoods(readModel);
        }
    }
}
