using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class HistoryRemoveStartOrProduceBeamDailyOperationReachingCommandHandler : ICommandHandler<HistoryRemoveStartOrProduceBeamDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingDocumentRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _daislyOperationReachingHistoryRepository;
        private readonly IBeamRepository
            _beamRepository;

        public HistoryRemoveStartOrProduceBeamDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingDocumentRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _daislyOperationReachingHistoryRepository =
                _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationReachingDocument> Handle(HistoryRemoveStartOrProduceBeamDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Sizing
                var existingReachingDocument =
                    _dailyOperationReachingDocumentRepository
                            .Find(o => o.Identity == request.Id)
                            .FirstOrDefault();

                //Get Daily Operation History
                var existingReachingHistories =
                    _daislyOperationReachingHistoryRepository
                        .Find(o => o.DailyOperationReachingDocumentId == existingReachingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory = existingReachingHistories.FirstOrDefault();
                var lastSecondHistory = existingReachingHistories.ElementAt(1);

                switch (request.HistoryStatus)
                {
                    case "REACHING-IN-START":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("ReachingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }
                        break;
                    case "COMB-START":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);
                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("ReachingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }
                        break;
                    case "REACHING-IN-FINISH":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        break;
                    case "COMB-FINISH":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        break;
                    case "REACHING-IN-CHANGE-OPERATOR":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        break;
                    case "COMB-CHANGE-OPERATOR":
                        existingReachingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                        await _dailyOperationReachingDocumentRepository.Update(existingReachingDocument);

                        if (lastHistory.Identity.Equals(request.HistoryId))
                        {
                            lastHistory.Remove();

                            await _daislyOperationReachingHistoryRepository.Update(lastHistory);
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("WarpingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                        }

                        break;
                }

                _storage.Save();

                return existingReachingDocument;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
