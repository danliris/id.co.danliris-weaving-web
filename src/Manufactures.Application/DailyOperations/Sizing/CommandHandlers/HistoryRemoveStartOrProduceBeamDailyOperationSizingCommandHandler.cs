using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamRepository;

        public HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            //_movementRepository =
            //  _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Sizing
            var sizingQuery =
                _dailyOperationSizingDocumentRepository
                        .Query
                        .Include(d => d.SizingHistories)
                        .Include(b => b.SizingBeamProducts)
                        .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationSizingDocument =
                _dailyOperationSizingDocumentRepository
                        .Find(sizingQuery)
                        .FirstOrDefault();

            //Get Daily Operation History
            var existingDailyOperationSizingHistories =
                existingDailyOperationSizingDocument
                        .SizingHistories
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationSizingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationBeamProducts =
                existingDailyOperationSizingDocument
                        .SizingBeamProducts
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

            switch (request.HistoryStatus)
            {
                case "START":
                    if (lastHistory.Identity.Equals(request.HistoryId))
                    {
                        existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                    }

                    if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                    {
                        existingDailyOperationSizingDocument.RemoveDailyOperationSizingBeamProduct(lastBeamProduct.Identity);
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                    }
                    break;
                case "COMPLETED":
                    if (lastHistory.Identity.Equals(request.HistoryId))
                    {
                        existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                    }

                    if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                    {
                        lastBeamProduct.SetCounterFinish(0);
                        lastBeamProduct.SetWeightNetto(0);
                        lastBeamProduct.SetWeightBruto(0);
                        lastBeamProduct.SetWeightTheoritical(0);
                        lastBeamProduct.SetPISMeter(0);
                        lastBeamProduct.SetSPU(0);
                        lastBeamProduct.SetSizingBeamStatus(Helpers.BeamStatus.ONPROCESS);

                        existingDailyOperationSizingDocument.UpdateDailyOperationSizingBeamProduct(lastBeamProduct);
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                    }
                    break;
            }

            await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);

            _storage.Save();

            return existingDailyOperationSizingDocument;
        }
    }
}
