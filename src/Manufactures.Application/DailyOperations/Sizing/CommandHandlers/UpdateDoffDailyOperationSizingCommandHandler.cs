using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateDoffDailyOperationSizingCommandHandler : ICommandHandler<UpdateDoffFinishDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMovementRepository
            _movementRepository;
        private readonly IBeamRepository
            _beamRepository;

        public UpdateDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _movementRepository =
              _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateDoffFinishDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query
                                                               .Include(d => d.SizingDetails)
                                                               .Where(detail => detail.Identity.Equals(request.Id))
                                                               .Include(b => b.SizingBeamDocuments)
                                                               .Where(beamDocument => beamDocument.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamDocuments.OrderByDescending(b => b.DateTimeBeamDocument);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingDetails.OrderByDescending(e => e.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperation
                    .SizingBeamDocuments
                    .Where(e => e.SizingBeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (countBeamStatus != 0)
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Finish. There's ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Machine Status
            var currentMachineStatus = lastDetail.MachineStatus;

            if (currentMachineStatus != MachineStatus.ONCOMPLETE)
            {
                throw Validator.ErrorValidation(("MachineStatus", "Can't Finish. This Machine's Operation is not ONCOMPLETE"));
            }

            //Validation for Started Operation Status
            var operationStartStatus = 
                existingDailyOperation
                .SizingDetails
                .Where(e => e.MachineStatus == MachineStatus.ONSTART)
                .Count();

            if (operationStartStatus == 0)
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation is not Started yet"));
            }

            //Validation for Finished Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

            if (currentOperationStatus == OperationStatus.ONFINISH)
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.Details.FinishDate.Year;
            var month = request.Details.FinishDate.Month;
            var day = request.Details.FinishDate.Day;
            var hour = request.Details.FinishTime.Hours;
            var minutes = request.Details.FinishTime.Minutes;
            var seconds = request.Details.FinishTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for DoffFinish Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var doffFinishDateMachineLogUtc = new DateTimeOffset(request.Details.FinishDate.Date, new TimeSpan(+7, 0, 0));

            if (doffFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("DoffDate", "Finish date cannot less than latest date log"));
            } else
            {
                if (dateTimeOperation < lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("DoffTime", "Finish time cannot less than latest time log"));
                }
                else
                {
                    existingDailyOperation.SetMachineSpeed(request.MachineSpeed);
                    existingDailyOperation.SetTexSQ(request.TexSQ);
                    existingDailyOperation.SetVisco(request.Visco);
                    existingDailyOperation.SetOperationStatus(OperationStatus.ONFINISH);

                    //Add New Detail on Document
                    var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);
                    var newOperation =
                                new DailyOperationSizingDetail(Guid.NewGuid(),
                                                               new ShiftId(request.Details.ShiftId.Value),
                                                               new OperatorId(request.Details.OperatorDocumentId.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONCOMPLETE,
                                                               "-",
                                                               new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                               //lastDetail.SizingBeamNumber
                                                               " ");

                    existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                    await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);

                    _storage.Save();

                    return existingDailyOperation;
                }

            }
        }
    }
}
