using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.OperationalMachinesPlanning;
using Manufactures.Domain.OperationalMachinesPlanning.Commands;
using Manufactures.Domain.OperationalMachinesPlanning.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.OperationalMachinesPlanning.CommandHandlers
{
    public class RemoveEnginePlanningCommandHandler
        : ICommandHandler<RemoveEnginePlanningCommand, MachinesPlanningDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachinesPlanningRepository _enginePlanningRepository;

        public RemoveEnginePlanningCommandHandler(IStorage storage)
        {
            _storage = storage;
            _enginePlanningRepository = _storage.GetRepository<IMachinesPlanningRepository>();
        }

        public async Task<MachinesPlanningDocument> Handle(RemoveEnginePlanningCommand request, CancellationToken cancellationToken)
        {
            var enginePlanningDocument = _enginePlanningRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (enginePlanningDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Document Not Available: " + request.Id));
            }

            enginePlanningDocument.Remove();

            await _enginePlanningRepository.Update(enginePlanningDocument);

            _storage.Save();

            return enginePlanningDocument;
        }
    }
}
