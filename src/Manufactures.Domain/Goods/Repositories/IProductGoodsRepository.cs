using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Products.ReadModels;

namespace Manufactures.Domain.Goods.Repositories
{
    public interface IProductGoodsRepository : IAggregateRepository<ProductGoods, ProductGoodsReadModel>
    {
    }
}
