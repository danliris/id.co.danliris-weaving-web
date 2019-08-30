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
                .Find(o => o.BeamDocument
                            .Deserialize<BeamDocumentValueObject>()
                            .Identity
                            .Equals(request.BeamId.Value) &&
                           o.StockStatus.Equals(StockCardStatus.ADJUSTMENT) &&
                           o.Expired.Equals(false))
                .Any();

            if (existingAdjustmentStock == true)
            {
                throw Validator.ErrorValidation(("BeamDocument", "Has stock Adjustment with beam Number " + request.BeamNumber));
            }

            var beamDocument =
                _beamRepository
                    .Find(o => o.Identity.Equals(request.BeamId.Value))
                    .FirstOrDefault();

            if (beamDocument == null)
            {
                throw Validator.ErrorValidation(("BeamId", "Not found beam"));
            }

            var stockType = "";
            var beam = new BeamDocumentValueObject(beamDocument);

            if (beamDocument.Type.Equals(BeamStatus.WARPING))
            {
                stockType = StockCardStatus.WARPING_STOCK;
            }
            else if (beamDocument.Type.Equals(BeamStatus.SIZING))
            {
                stockType = StockCardStatus.SIZING_STOCK;
            }

            beam.SetEmptyWeight(request.EmptyWeight);
            beam.SetYarnLength(request.YarnLength);
            beam.SetYarnStrands(request.YarnStrands);

            var dateTimeOperation = DateTimeOffset.UtcNow.AddHours(7);
            StringBuilder stockNumber = new StringBuilder();
            stockNumber.Append(dateTimeOperation.Second);
            stockNumber.Append("/");
            stockNumber.Append(dateTimeOperation.Hour);
            stockNumber.Append("/Adjustment/");
            stockNumber.Append(dateTimeOperation.Date);

            var stockDocument = new StockCardDocument(Guid.NewGuid(), 
                                                      stockNumber.ToString(), 
                                                      request.DailyOperationId, 
                                                      request.DateTimeOperation, 
                                                      beam, 
                                                      true,
                                                      false,
                                                      stockType,
                                                      StockCardStatus.ADJUSTMENT);

            await _stockCardRepository.Update(stockDocument);

            _storage.Save();

            return stockDocument;
        }
    }
}
