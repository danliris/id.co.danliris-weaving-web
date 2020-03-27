using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class HistoryRemoveStartOrProduceBeamDailyOperationWarpingCommandHandler : ICommandHandler<HistoryRemoveStartOrProduceBeamDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingDocumentRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IBeamRepository
            _beamRepository;

        public HistoryRemoveStartOrProduceBeamDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingDocumentRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(HistoryRemoveStartOrProduceBeamDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Sizing
                var existingWarpingDocument =
                    _dailyOperationWarpingDocumentRepository
                            .Find(o => o.Identity == request.Id)
                            .FirstOrDefault();

                //Get Daily Operation History
                var existingWarpingHistories =
                    _dailyOperationWarpingHistoryRepository
                        .Find(o => o.DailyOperationWarpingDocumentId == existingWarpingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory = existingWarpingHistories.Where(o => o.Identity == request.HistoryId).FirstOrDefault();
                var lastSecondHistory = existingWarpingHistories.ElementAt(1);

                //Get Daily Operation Beam Product
                var existingWarpingBeamProducts =
                    _dailyOperationWarpingBeamProductRepository
                        .Find(o => o.DailyOperationWarpingDocumentId == existingWarpingDocument.Identity)
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                var lastBeamProduct = existingWarpingBeamProducts.Where(o=>o.Identity == request.BeamProductId).FirstOrDefault();

                switch (request.HistoryStatus)
                {
                    case "START":
                        existingWarpingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationWarpingDocumentRepository.Update(existingWarpingDocument);
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _dailyOperationWarpingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                        {
                            lastBeamProduct.Remove();

                            await _dailyOperationWarpingBeamProductRepository.Update(lastBeamProduct);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingBeamProduct", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                        }
                        break;
                    case "COMPLETED":
                        existingWarpingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationWarpingDocumentRepository.Update(existingWarpingDocument);

                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _dailyOperationWarpingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                        {  
                            lastBeamProduct.SetLatestDateTimeBeamProduct(lastSecondHistory.DateTimeMachine);

                            await _dailyOperationWarpingBeamProductRepository.Update(lastBeamProduct);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingBeamProduct", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                        }
                        break;
                }

                _storage.Save();

                return existingWarpingDocument;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
