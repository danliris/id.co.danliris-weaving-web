using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class HistoryRemoveStartOrCompletedDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand, DailyOperationSizingDocument>
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

        public HistoryRemoveStartOrCompletedDailyOperationSizingCommandHandler(IStorage storage)
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

                var beamSizingIdGuid = _dailyOperationSizingBeamProductRepository.Find(y => y.Identity == request.BeamProductId).FirstOrDefault();

                //Get Beam Number by Identity
                var beamSizing =
                    _beamRepository
                        .Find(o =>
                            o.Identity == beamSizingIdGuid.SizingBeamId.Value
                        ).FirstOrDefault();

                //Get Daily Operation History
                var existingSizingHistories =
                    _dailyOperationSizingHistoryRepository
                        .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity && o.SizingBeamNumber == beamSizing.Number)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory = existingSizingHistories.FirstOrDefault();
                

                //Get Daily Operation Beam Product
                var existingSizingBeamProducts =
                    _dailyOperationSizingBeamProductRepository
                        .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity && o.Identity == request.BeamProductId)
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                var lastBeamProduct = existingSizingBeamProducts.FirstOrDefault();

                switch (request.HistoryStatus)
                {
                    case "START":
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _dailyOperationSizingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        if (lastBeamProduct.Identity.Equals(request.BeamProductId))
                        {
                            lastBeamProduct.Remove();

                            await _dailyOperationSizingBeamProductRepository.Update(lastBeamProduct);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingBeamProduct", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
                        }
                        break;
                    case "COMPLETED":
                        var lastSecondHistory = existingSizingHistories.ElementAt(1);
                        existingSizingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationSizingDocumentRepository.Update(existingSizingDocument);
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _dailyOperationSizingHistoryRepository.Update(lastHistory);
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
                            lastBeamProduct.SetTotalBroken(0);
                            lastBeamProduct.SetSizingBeamStatus(BeamStatus.ONPROCESS);
                            lastBeamProduct.SetLatestDateTimeBeamProduct(lastSecondHistory.DateTimeMachine);

                            await _dailyOperationSizingBeamProductRepository.Update(lastBeamProduct);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("SizingBeamProduct", "Tidak ada Id Produk Beam yang Cocok dengan " + request.BeamProductId));
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
