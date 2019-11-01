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
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public HistoryRemovePreparationDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(HistoryRemovePreparationDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            try
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

                if (existingDailyOperationSizingDocument == null)
                {
                    throw Validator.ErrorValidation(("SizingHistory", "Tidak ada Id History yang Cocok dengan " + request.HistoryId));
                }
                
                //Get Daily Operation History
                var existingDailyOperationSizingHistories =
                    existingDailyOperationSizingDocument
                            .SizingHistories
                            .OrderByDescending(o => o.DateTimeMachine);
                var lastHistory =
                    existingDailyOperationSizingHistories
                        .Where(o => o.Identity.Equals(request.HistoryId))
                        .FirstOrDefault();
                lastHistory.Remove();

                existingDailyOperationSizingDocument.Remove();

                await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);

                _storage.Save();

                return existingDailyOperationSizingDocument;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }
    }
}
