using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class RemoveDailyOperationLoomCommandHandler 
        : ICommandHandler<RemoveDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository 
            _dailyOperationalDocumentRepository;

        public RemoveDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository = 
                _storage.GetRepository<IDailyOperationLoomRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(RemoveDailyOperationLoomCommand request, 
                                                             CancellationToken cancellationToken)
        {
            var query = 
                _dailyOperationalDocumentRepository.Query
                                                   .Include(d => d.DailyOperationMachineDetails)
                                                   .Where(entity => entity.Identity.Equals(request.Id));
            var existingOperation = _dailyOperationalDocumentRepository.Find(query).FirstOrDefault();

            if(existingOperation == null)
            {
                Validator.ErrorValidation(("Daily Production Document", "Unavailable existing Daily Production Document with Id " + request.Id));
            }

            existingOperation.Remove();
            await _dailyOperationalDocumentRepository.Update(existingOperation);
            _storage.Save();

            return existingOperation;
        }
    }
}
