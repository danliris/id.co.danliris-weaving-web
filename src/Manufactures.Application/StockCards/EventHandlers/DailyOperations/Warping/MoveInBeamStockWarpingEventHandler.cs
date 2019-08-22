using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Events;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.EventHandlers.Warping
{
    public class MoveInBeamStockWarpingEventHandler : IManufactureEventHandler<MoveInBeamStockWarpingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;
        private bool IsSucceed;

        public MoveInBeamStockWarpingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
            IsSucceed = false;
        }

        public async Task Handle(MoveInBeamStockWarpingEvent notification, CancellationToken cancellationToken)
        {
            // Update for MoveOut StockCard Warping
            var moveOutStockCardWarpings =
                _stockCardRepository.Find(o => o.BeamId.Equals(notification.BeamId.Value) &&
                                               o.Expired.Equals(false));

            foreach( var stockCard in moveOutStockCardWarpings)
            {
                if (stockCard.StockStatus.Equals(StockCardStatus.MOVEOUT_STOCK) &&
                   stockCard.StockType.Equals(StockCardStatus.WARPING_STOCK))
                {
                    stockCard.UpdateExpired(true);
                    await _stockCardRepository.Update(stockCard);
                }
            }

            //Add MoveIn StockCard
            var newStockCard =
                 new StockCardDocument(Guid.NewGuid(),
                                       notification.StockNumber,
                                       notification.DailyOperationId,
                                       notification.DateTimeOperation,
                                       notification.BeamId,
                                       true,
                                       false,
                                       StockCardStatus.WARPING_STOCK,
                                       StockCardStatus.MOVEIN_STOCK);

            await _stockCardRepository.Update(newStockCard);

            _storage.Save();
            IsSucceed = true;
        }

        //This function only for testing
        public bool ReturnResult()
        {
            return IsSucceed;
        }
    }
}
