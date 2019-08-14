using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Events;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Sizing;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.EventHandlers.DailyOperations.Sizing
{
    public class MoveInBeamStockSizingEventHandler : IManufactureEventHandler<MoveInBeamStockSizingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;

        public MoveInBeamStockSizingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
        }

        public async Task Handle(MoveInBeamStockSizingEvent notification, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var existingStockCard =
                _stockCardRepository
                    .Find(entity => entity.BeamId.Equals(notification.BeamId.Value) &&
                                    entity.DailyOperationId.Equals(notification.DailyOperationId.Value) &&
                                    entity.StockType.Equals(StockCardStatus.SIZING_STOCK) &&
                                    entity.IsEmpty.Equals(false) &&
                                    entity.Expired.Equals(false))
                    .FirstOrDefault();

            if (existingStockCard == null)
            {
                var newStockCard =
                 new StockCardDocument(Guid.NewGuid(),
                                       notification.StockNumber,
                                       notification.DailyOperationId,
                                       notification.DateTimeOperation,
                                       notification.BeamId,
                                       true,
                                       false,
                                       StockCardStatus.SIZING_STOCK);

                await _stockCardRepository.Update(newStockCard);

                _storage.Save();
            }
        }
    }
}
