using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
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
    public class RemoveDailyOperationSizingCommandHandler : ICommandHandler<RemoveDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public RemoveDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(RemoveDailyOperationSizingCommand request,
                                                             CancellationToken cancellationToken)
        {
            var query =
                _dailyOperationSizingDocumentRepository.Query
                                                   .Include(d => d.DailyOperationSizingDetails)
                                                   .Where(entity => entity.Identity.Equals(request.Id));
            var existingOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();

            if (existingOperation == null)
            {
                Validator.ErrorValidation(("Daily Production Document", "Unavailable existing Daily Production Document with Id " + request.Id));
            }

            existingOperation.Remove();
            await _dailyOperationSizingDocumentRepository.Update(existingOperation);
            _storage.Save();

            return existingOperation;
        }
    }
}
