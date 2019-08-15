using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Events;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.EventHandlers.DailyOperations.Warping
{
    public class MoveOutBeamStockWarpingEventHandler : IManufactureEventHandler<MoveOutBeamStockWarpingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;

        public MoveOutBeamStockWarpingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
        }

        public async Task Handle(MoveOutBeamStockWarpingEvent notification, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var existingStockCard =
                _stockCardRepository
                    .Find(entity => entity.BeamId.Equals(notification.BeamId.Value) &&
                                    entity.StockType.Equals(StockCardStatus.WARPING_STOCK) &&
                                    entity.StockStatus.Equals(StockCardStatus.MOVEIN_STOCK) &&
                                    entity.Expired.Equals(false))
                    .FirstOrDefault();

            await Task.Yield();
            var redundantStockCard =
                _stockCardRepository
                    .Find(entity => entity.BeamId.Equals(notification.BeamId.Value) &&
                                    entity.StockType.Equals(StockCardStatus.WARPING_STOCK) &&
                                    entity.StockStatus.Equals(StockCardStatus.MOVEOUT_STOCK) &&
                                    entity.Expired.Equals(false))
                    .FirstOrDefault();

            if (redundantStockCard == null)
            {
                //Update move in to expired
                if (existingStockCard != null)
                {
                    existingStockCard.UpdateExpired(true);

                    await _stockCardRepository.Update(existingStockCard);
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
}
