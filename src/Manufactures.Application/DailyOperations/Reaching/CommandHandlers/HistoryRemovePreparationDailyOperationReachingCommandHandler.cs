using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.CommandHandlers
{
    public class HistoryRemovePreparationDailyOperationReachingCommandHandler : ICommandHandler<HistoryRemovePreparationDailyOperationReachingCommand, DailyOperationReachingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public HistoryRemovePreparationDailyOperationReachingCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationReachingRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationReachingHistoryRepository =
                _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
        }
        public async Task<DailyOperationReachingDocument> Handle(HistoryRemovePreparationDailyOperationReachingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Warping
                var existingReachingDocument =
                    _dailyOperationReachingRepository
                            .Find(o => o.Identity == request.Id)
                            .FirstOrDefault();

                if (existingReachingDocument == null)
                {
                    throw Validator.ErrorValidation(("WarpingDocument", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                }

                //Get Daily Operation History
                var existingReachingHistories =
                    _dailyOperationReachingHistoryRepository
                        .Find(o => o.DailyOperationReachingDocumentId == existingReachingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);

                var lastHistory =
                    existingReachingHistories
                        .Where(o => o.Identity.Equals(request.HistoryId))
                        .FirstOrDefault();
                lastHistory.Remove();

                existingReachingDocument.Remove();

                await _dailyOperationReachingRepository.Update(existingReachingDocument);

                _storage.Save();

                return existingReachingDocument;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }
    }
}
