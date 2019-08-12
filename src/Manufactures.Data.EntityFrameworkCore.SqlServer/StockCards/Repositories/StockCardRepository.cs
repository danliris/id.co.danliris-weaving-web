using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.ReadModels;
using Manufactures.Domain.StockCard.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.StockCards.Repositories
{
    public class StockCardRepository : AggregateRepostory<StockCardDocument, StockCardReadModel>, IStockCardRepository
    {
        protected override StockCardDocument Map(StockCardReadModel readModel)
        {
            return new StockCardDocument(readModel);
        }
    }
}
