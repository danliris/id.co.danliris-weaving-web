using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.StockCard;
using Manufactures.Domain.StockCard.Commands;
using Manufactures.Domain.StockCard.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.StockCards.CommandHandlers
{
    public class CreateStockAdjustmentCommandHandler : ICommandHandler<CreateStockAdjustmentCommand, StockCardDocument>
    {
        private readonly IStorage _storage;
        private readonly IStockCardRepository
            _stockCardRepository;
        private readonly IBeamRepository
            _beamRepository;

        public CreateStockAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _stockCardRepository =
                _storage.GetRepository<IStockCardRepository>();
            _beamRepository =
               _storage.GetRepository<IBeamRepository>();
        }

        public async Task<StockCardDocument> Handle(CreateStockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var existingAdjustmentStock = _stockCardRepository
                .Find(o => o.BeamId
                            .Equals(request.BeamId.Value) &&
                           o.Expired.Equals(false))
                .FirstOrDefault();

            if (existingAdjustmentStock == null)
            {
                throw Validator.ErrorValidation(("BeamDocument", "not have existing stock to adjustment"));
            }

            var beamDocument =
                _beamRepository
                    .Find(o => o.Identity.Equals(request.BeamId.Value))
                    .FirstOrDefault();

            if (beamDocument == null)
            {
                throw Validator.ErrorValidation(("BeamId", "Not found beam"));
            }

            var beam = new BeamDocumentValueObject(beamDocument);
            beam.SetEmptyWeight(request.EmptyWeight);
            beam.SetYarnLength(request.YarnLength);
            beam.SetYarnStrands(request.YarnStrands);

            var dateTimeOperation = DateTimeOffset.UtcNow.AddHours(7);
            StringBuilder stockNumber = new StringBuilder();
            stockNumber.Append(dateTimeOperation.Second);
            stockNumber.Append("/");
            stockNumber.Append(dateTimeOperation.Hour);
            stockNumber.Append("/adjustment/");
            stockNumber.Append(dateTimeOperation.Date);

            var stockStatus = "";

            if (request.IsMoveIn == true)
            {
                stockStatus = StockCardStatus.MOVEIN_STOCK;
            } else
            {
                stockStatus = StockCardStatus.MOVEOUT_STOCK;
            }

            var stockDocument = new StockCardDocument(Guid.NewGuid(),
                                                      stockNumber.ToString(),
                                                      request.DailyOperationId,
                                                      request.BeamId,
                                                      beam,
                                                      stockStatus);

            await _stockCardRepository.Update(stockDocument);

            _storage.Save();

            return stockDocument;
        }
    }
}
