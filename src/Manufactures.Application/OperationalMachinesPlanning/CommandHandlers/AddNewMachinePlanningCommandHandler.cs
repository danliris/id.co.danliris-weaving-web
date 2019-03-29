using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.OperationalMachinesPlanning;
using Manufactures.Domain.OperationalMachinesPlanning.Commands;
using Manufactures.Domain.OperationalMachinesPlanning.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.OperationalMachinesPlanning.CommandHandlers
{
    public class AddNewEnginePlanningCommandHandler 
        : ICommandHandler<AddNewEnginePlanningCommand, MachinesPlanningDocument>
    {
        private readonly IStorage _storage;
        private readonly IMachinesPlanningRepository _enginePlanningRepository;

        public AddNewEnginePlanningCommandHandler(IStorage storage)
        {
            _storage = storage;
            _enginePlanningRepository = _storage.GetRepository<IMachinesPlanningRepository>();
        }

        public async Task<MachinesPlanningDocument> Handle(AddNewEnginePlanningCommand request, 
                                                   CancellationToken cancellationToken)
        {
            var enginePlanningDocument = new MachinesPlanningDocument(Guid.NewGuid(),
                                                                    request.Area,
                                                                    request.Blok,
                                                                    request.BlokKaizen,
                                                                    new UnitId(request.UnitDepartementId),
                                                                    new MachineId(request.MachineId),
                                                                    new UserId(request.UserMaintenanceId),
                                                                    new UserId(request.UserOperatorId));

            await _enginePlanningRepository.Update(enginePlanningDocument);

            _storage.Save();

            return enginePlanningDocument;
        }
    }
}
