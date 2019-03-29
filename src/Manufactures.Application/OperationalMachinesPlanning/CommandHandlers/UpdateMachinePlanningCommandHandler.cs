using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.OperationalMachinesPlanning;
using Manufactures.Domain.OperationalMachinesPlanning.Commands;
using Manufactures.Domain.OperationalMachinesPlanning.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.OperationalMachinesPlanning.CommandHandlers
{
    public class UpdateEnginePlanningCommandHandler : ICommandHandler<UpdateEnginePlanningCommand, MachinesPlanningDocument>
    {

        private readonly IStorage _storage;
        private readonly IMachinesPlanningRepository _enginePlanningRepository;

        public UpdateEnginePlanningCommandHandler(IStorage storage)
        {
            _storage = storage;
            _enginePlanningRepository = _storage.GetRepository<IMachinesPlanningRepository>();
        }

        public async Task<MachinesPlanningDocument> Handle(UpdateEnginePlanningCommand request, CancellationToken cancellationToken)
        {
            var enginePlanningDocument = _enginePlanningRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (enginePlanningDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Document Not Available: with request Id" + request.Id));
            }

            enginePlanningDocument.ChangeArea(request.Area);
            enginePlanningDocument.ChangeBlok(request.Blok);
            enginePlanningDocument.ChangeBlokKaizen(request.BlokKaizen);
            enginePlanningDocument.ChangeMachineId(request.MachineId);
            enginePlanningDocument.ChangeUnitDepartmentId(request.UnitDepartementId);
            enginePlanningDocument.ChangeUserMaintenanceId(request.UserMaintenanceId);
            enginePlanningDocument.ChangeUserOperatorId(request.UserOperatorId);

            await _enginePlanningRepository.Update(enginePlanningDocument);

            _storage.Save();

            return enginePlanningDocument;
        }
    }
}
