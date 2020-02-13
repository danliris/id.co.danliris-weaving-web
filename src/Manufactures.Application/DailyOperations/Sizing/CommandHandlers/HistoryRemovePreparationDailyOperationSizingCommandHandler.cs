using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class HistoryRemovePreparationDailyOperationSizingCommandHandler : ICommandHandler<HistoryRemovePreparationDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;

        public HistoryRemovePreparationDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemovePreparationDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Get Daily Operation Document Sizing
                var existingSizingDocument =
                    _dailyOperationSizingDocumentRepository
                            .Find(o=>o.Identity == request.Id)
                            .FirstOrDefault();

                if (existingSizingDocument == null)
                {
                    throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                }
                
                //Get Daily Operation History
                var existingSizingHistories =
                    _dailyOperationSizingHistoryRepository
                        .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                        .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory =
                    existingSizingHistories
                        .Where(o => o.Identity.Equals(request.HistoryId))
                        .FirstOrDefault();
                lastHistory.Remove();

                existingSizingDocument.Remove();

                await _dailyOperationSizingDocumentRepository.Update(existingSizingDocument);

                _storage.Save();

                return existingSizingDocument;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }
    }
}
