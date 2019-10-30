using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemovePauseOrResumeOrFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
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

            if (lastHistory.Identity.Equals(request.HistoryId))
            {
                if (request.HistoryStatus.Equals(Helpers.MachineStatus.ONFINISH))
                {
                    existingDailyOperationSizingDocument.SetMachineSpeed(0);
                    existingDailyOperationSizingDocument.SetVisco(0);
                    existingDailyOperationSizingDocument.SetTexSQ(0);
                    existingDailyOperationSizingDocument.SetOperationStatus(OperationStatus.ONPROCESS);

                    existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                }
                else
                {
                    existingDailyOperationSizingDocument.RemoveDailyOperationSizingHistory(lastHistory.Identity);
                }

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);

                _storage.Save();

                return existingDailyOperationSizingDocument;
            }
            else
            {
                throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
            }
        }
    }
}
