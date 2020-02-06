using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
        private readonly IBeamRepository
            _beamRepository;

        public HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _dailyOperationSizingBeamProductRepository =
                _storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
            //_movementRepository =
            //  _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Sizing
                var existingSizingDocument =
                    _dailyOperationSizingDocumentRepository
                            .Find(o=>o.Identity == request.Id)
                            .FirstOrDefault();

                //Get Daily Operation History
                var existingSizingHistories =
                    _dailyOperationSizingHistoryRepository
                        .Find(o=>o.Identity == existingSizingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory = existingSizingHistories.FirstOrDefault();

                //Get Daily Operation Beam Product
                var existingSizingBeamProducts =
                    _dailyOperationSizingBeamProductRepository
                        .Find(o=>o.Identity == existingSizingDocument.Identity)
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                var lastBeamProduct = existingSizingBeamProducts.FirstOrDefault();

                switch (request.HistoryStatus)
                {
                    case "START":
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                        {
                            lastBeamProduct.Remove();
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                        }
                        break;
                    case "COMPLETED":
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();
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

                            await _dailyOperationSizingBeamProductRepository.Update(lastBeamProduct);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                        }
                        break;
                }

                _storage.Save();

                return existingSizingDocument;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
