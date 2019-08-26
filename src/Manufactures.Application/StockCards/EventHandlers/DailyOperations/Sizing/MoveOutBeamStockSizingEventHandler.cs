using ExtCore.Data.Abstractions;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.Events;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Sizing;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.EventHandlers.DailyOperations.Sizing
{
    public class MoveOutBeamStockSizingEventHandler : IManufactureEventHandler<MoveOutBeamStockSizingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;
        private readonly IBeamRepository
            _beamRepository;
        private bool IsSucceed;

        public MoveOutBeamStockSizingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            IsSucceed = false;
        }

        public async Task Handle(MoveOutBeamStockSizingEvent notification, CancellationToken cancellationToken)
        {
            //Get MoveIn stok from latest beam id (same id)
            await Task.Yield();
            var moveInStockCardSizings =
                _stockCardRepository.Find(o => o.BeamDocument
                                                .Deserialize<BeamDocumentValueObject>()
                                                .Identity
                                                .Equals(notification.BeamId.Value) &&
                                               o.Expired.Equals(false));
            //Get BeamDocument
            await Task.Yield();
            var beamDocument =
                _beamRepository
                    .Find(o => o.Identity.Equals(notification.BeamId.Value))
                    .FirstOrDefault();

            //Check for set expired movein sizing stock card
            foreach (var stockCard in moveInStockCardSizings)
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
                                       new BeamDocumentValueObject(beamDocument),
                                       false,
                                       true,
                                       StockCardStatus.WARPING_STOCK,
                                       StockCardStatus.MOVEOUT_STOCK);

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
