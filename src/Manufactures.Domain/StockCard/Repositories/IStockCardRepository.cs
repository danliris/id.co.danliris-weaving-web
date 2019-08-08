using Infrastructure.Domain.Repositories;
using Manufactures.Domain.StockCard.ReadModels;

namespace Manufactures.Domain.StockCard.Repositories
{
    public interface IStockCardRepository : IAggregateRepository<StockCardDocument, StockCardReadModel>
    {
    }
}
