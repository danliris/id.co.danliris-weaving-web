using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateDailyOperationLoomCommandHandler
        : ICommandHandler<UpdateDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationalDocumentRepository;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;

        public UpdateDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationalDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationSizingRepository =
               _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateDailyOperationLoomCommand request,
                                                             CancellationToken cancellationToken)
        {
            var query =
                _dailyOperationalDocumentRepository
                    .Query
                    .Include(d => d.DailyOperationLoomDetails)
                    .Where(entity => entity.Identity.Equals(request.Id));
            var existingDailyOperation =
                _dailyOperationalDocumentRepository
                    .Find(query)
                    .FirstOrDefault();

            var newDailyOperationHistory = new DailyOperationLoomHistory();
            newDailyOperationHistory.SetTimeOnMachine(request.Detail.DailyOperationLoomHistory.TimeOnMachine);

            if (request.Detail.DailyOperationLoomHistory.MachineStatus == DailyOperationMachineStatus.ONSTOP)
            {
                newDailyOperationHistory.SetMachineStatus(DailyOperationMachineStatus.ONSTOP);
                newDailyOperationHistory.SetInformation(request.Detail.DailyOperationLoomHistory.Information);
            }
            else if (request.Detail.DailyOperationLoomHistory.MachineStatus == DailyOperationMachineStatus.ONRESUME)
            {
                newDailyOperationHistory.SetMachineStatus(DailyOperationMachineStatus.ONRESUME);
                newDailyOperationHistory.SetInformation(request.Detail.DailyOperationLoomHistory.Information);
            }
            else if (request.Detail.DailyOperationLoomHistory.MachineStatus == DailyOperationMachineStatus.ONFINISH)
            {
                newDailyOperationHistory.SetMachineStatus(DailyOperationMachineStatus.ONFINISH);
                newDailyOperationHistory.SetInformation(request.Detail.DailyOperationLoomHistory.Information);
            }

            if (request.DailyOperationSizingId.Value != null)
            {
                var sizingOperatorDocumentId =
               _dailyOperationSizingRepository
                   .Query
                   .Where(o => o.Identity == request.DailyOperationSizingId.Value)
                   .FirstOrDefault()
                   .Identity;

                var newOperation =
                  new DailyOperationLoomDetail(Guid.NewGuid(),
                                                    request.Detail.OrderDocumentId,
                                                    request.Detail.WarpOrigin,
                                                    request.Detail.WeftOrigin,
                                                    request.Detail.BeamId,
                                                    newDailyOperationHistory,
                                                    request.Detail.ShiftId,
                                                    request.Detail.BeamOperatorId,
                                                    new OperatorId(sizingOperatorDocumentId));

                existingDailyOperation.AddDailyOperationMachineDetail(newOperation);

            }
            else
            {
                var list = existingDailyOperation.DailyOperationMachineDetails.ToList();
                var lastDetail = list[list.Count - 1];
                var sizingOperatorDocumentId = new OperatorId(lastDetail.SizingOperatorId);

                var newOperation =
                  new DailyOperationLoomDetail(Guid.NewGuid(),
                                                    request.Detail.OrderDocumentId,
                                                    request.Detail.WarpOrigin,
                                                    request.Detail.WeftOrigin,
                                                    request.Detail.BeamId,
                                                    newDailyOperationHistory,
                                                    request.Detail.ShiftId,
                                                    request.Detail.BeamOperatorId,
                                                    sizingOperatorDocumentId);

                existingDailyOperation.AddDailyOperationMachineDetail(newOperation);
            }
            
            await _dailyOperationalDocumentRepository.Update(existingDailyOperation);
            _storage.Save();

            return existingDailyOperation;
        }
    }
}
