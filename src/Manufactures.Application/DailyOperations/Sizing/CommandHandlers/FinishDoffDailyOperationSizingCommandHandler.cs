﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Movements.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class FinishDoffDailyOperationSizingCommandHandler : ICommandHandler<FinishDoffDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IMovementRepository
            _movementRepository;
        private readonly IBeamRepository
            _beamRepository;

        public FinishDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _movementRepository =
              _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(FinishDoffDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query
                                                               .Include(d => d.SizingHistories)
                                                               .Include(b => b.SizingBeamProducts)
                                                               .Where(sizingDoc => sizingDoc.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamProducts.OrderByDescending(b => b.LatestDateTimeBeamProduct);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingHistories.OrderByDescending(e => e.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperation
                    .SizingBeamProducts
                    .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
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
            //var sizingOperationStartStatus = 
            //    existingDailyOperation
            //    .SizingDetails
            //    .Where(e => e.MachineStatus == MachineStatus.ONSTART)
            //    .Count();

            //if (sizingOperationStartStatus == 0)
            //{
            //    throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation is not Started yet"));
            //}

            //Validation for Finished Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

            if (currentOperationStatus == OperationStatus.ONFINISH)
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.FinishDoffDate.Year;
            var month = request.FinishDoffDate.Month;
            var day = request.FinishDoffDate.Day;
            var hour = request.FinishDoffTime.Hours;
            var minutes = request.FinishDoffTime.Minutes;
            var seconds = request.FinishDoffTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for DoffFinish Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var doffFinishDateMachineLogUtc = new DateTimeOffset(request.FinishDoffDate.Date, new TimeSpan(+7, 0, 0));

            if (doffFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("DoffDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("DoffTime", "Finish time cannot less than or equal latest time log"));
                }
                else
                {
                    existingDailyOperation.SetMachineSpeed(request.MachineSpeed);
                    existingDailyOperation.SetTexSQ(request.TexSQ);
                    existingDailyOperation.SetVisco(request.Visco);
                    existingDailyOperation.SetOperationStatus(OperationStatus.ONFINISH);

                    //Add New Detail on Document
                    //var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);
                    var newOperation =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                               new ShiftId(request.FinishDoffShift.Value),
                                                               new OperatorId(request.FinishDoffOperator.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONFINISH,
                                                               "-",
                                                               //new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                               lastDetail.BrokenBeam,
                                                               lastDetail.MachineTroubled,
                                                               "");

                    existingDailyOperation.AddDailyOperationSizingHistory(newOperation);

                    await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);

                    _storage.Save();

                    return existingDailyOperation;
                }

            }
        }
    }
}
