using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Events;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Sizing;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.EventHandlers.DailyOperations.Sizing
{
    public class MoveOutBeamStockSizingEventHandler : IManufactureEventHandler<MoveOutBeamStockSizingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;

        public MoveOutBeamStockSizingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
        }

        public async Task Handle(MoveOutBeamStockSizingEvent notification, CancellationToken cancellationToken)
        {
            var existingStockCards =
                _stockCardRepository
                    .Find(entity => entity.BeamId.Equals(notification.BeamId.Value) &&
                                    entity.Expired.Equals(false));

            //Check for set expired movein sizing stock card
            foreach (var stockCard in existingStockCards)
            {
                if (stockCard.StockType.Equals(StockCardStatus.SIZING_STOCK) &&
                    stockCard.StockStatus.Equals(StockCardStatus.MOVEIN_STOCK))
                {
                    stockCard.UpdateExpired(true);

                    await _stockCardRepository.Update(stockCard);
                }
            }

            var newStockCard =
                 new StockCardDocument(Guid.NewGuid(),
                                       notification.StockNumber,
                                       notification.DailyOperationId,
                                       notification.DateTimeOperation,
                                       notification.BeamId,
                                       false,
                                       true,
                                       StockCardStatus.WARPING_STOCK,
                                       StockCardStatus.MOVEOUT_STOCK);

            await _stockCardRepository.Update(newStockCard);

            _storage.Save();
        }
    }
}
