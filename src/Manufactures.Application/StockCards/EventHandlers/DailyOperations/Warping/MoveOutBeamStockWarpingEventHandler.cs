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
                                    entity.DailyOperationId.Equals(notification.DailyOperationId.Value) &&
                                    entity.StockType.Equals(StockCardStatus.WARPING_STOCK) &&
                                    entity.IsEmpty.Equals(false) &&
                                    entity.Expired.Equals(false))
                    .FirstOrDefault();

            //Update move in to expired
            if (existingStockCard != null)
            {
                existingStockCard.UpdateExpired(true);

                //Update will failed while existing same stock not null
                await _stockCardRepository.Update(existingStockCard);
            }

            await Task.Yield();
            var existingSameStock =
                _stockCardRepository
                    .Find(entity => entity.BeamId.Equals(notification.BeamId.Value) &&
                                    entity.DailyOperationId.Equals(notification.DailyOperationId.Value) &&
                                    entity.StockType.Equals(StockCardStatus.WARPING_STOCK) &&
                                    entity.IsEmpty.Equals(true) &&
                                    entity.Expired.Equals(false))
                    .FirstOrDefault();

            //Check if have same stock (move out) on same beamId, same daily Operation and not expired 
            if (existingSameStock == null)
            {
                var newStockCard =
                 new StockCardDocument(Guid.NewGuid(),
                                       notification.StockNumber,
                                       notification.DailyOperationId,
                                       notification.DateTimeOperation,
                                       notification.BeamId,
                                       false,
                                       true,
                                       StockCardStatus.WARPING_STOCK);

                await _stockCardRepository.Update(newStockCard);

                _storage.Save();
            }
        }
    }
}
