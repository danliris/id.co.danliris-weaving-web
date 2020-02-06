using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;

        public HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
                        .Find(o=>o.DailyOperationSizingDocumentId==existingSizingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory = existingSizingHistories.FirstOrDefault();

                if (lastHistory.Identity.Equals(request.HistoryId))
                {
                    if (request.HistoryStatus.Equals(MachineStatus.ONFINISH))
                    {
                        existingSizingDocument.SetMachineSpeed(0);
                        existingSizingDocument.SetVisco(0);
                        existingSizingDocument.SetTexSQ(0);
                        existingSizingDocument.SetOperationStatus(OperationStatus.ONPROCESS);

                        //existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                        lastHistory.Remove();
                    }
                    else
                    {
                        //existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                        lastHistory.Remove();
                    }

                    await _dailyOperationSizingDocumentRepository.Update(existingSizingDocument);

                    _storage.Save();

                    return existingSizingDocument;
                }
                else
                {
                    throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
