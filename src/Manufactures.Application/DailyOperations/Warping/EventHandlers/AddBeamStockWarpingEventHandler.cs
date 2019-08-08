using ExtCore.Data.Abstractions;
using Manufactures.Domain.Events;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Events.Warping;
using Manufactures.Domain.StockCard.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.EventHandlers
{
    public class AddBeamStockWarpingEventHandler : IManufactureEventHandler<AddBeamStockWarpingEvent>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;

        public AddBeamStockWarpingEventHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
        }

        public async Task Handle(AddBeamStockWarpingEvent notification, CancellationToken cancellationToken)
        {

            var newStockCard =
                new StockCardDocument(Guid.NewGuid(),
                                      notification.StockNumber,
                                      notification.DailyOperationId,
                                      notification.DateTimeOperation,
                                      notification.BeamId,
                                      notification.Length.HasValue ? notification.Length.Value : 0,
                                      notification.YarnStrands.HasValue ? notification.YarnStrands.Value : 0 , 
                                      true, 
                                      false);

            await _stockCardRepository.Update(newStockCard);

            _storage.Save();
        }
    }
}
