using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class HistoryRemovePreparationDailyOperationWarpingCommandHandler : ICommandHandler<HistoryRemovePreparationDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;

        public HistoryRemovePreparationDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(HistoryRemovePreparationDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Warping
                var existingWarpingDocument =
                    _dailyOperationWarpingRepository
                            .Find(o => o.Identity == request.Id)
                            .FirstOrDefault();

                if (existingWarpingDocument == null)
                {
                    throw Validator.ErrorValidation(("WarpingDocument", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                }

                //Get Daily Operation History
                var existingWarpingHistories =
                    _dailyOperationWarpingHistoryRepository
                        .Find(o => o.DailyOperationWarpingDocumentId == existingWarpingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);

                var lastHistory =
                    existingWarpingHistories
                        .Where(o => o.Identity.Equals(request.HistoryId))
                        .FirstOrDefault();
                lastHistory.Remove();

                existingWarpingDocument.Remove();

                await _dailyOperationWarpingRepository.Update(existingWarpingDocument);

                _storage.Save();

                return existingWarpingDocument;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }
    }
}
