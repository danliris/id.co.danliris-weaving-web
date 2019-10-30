using ExtCore.Data.Abstractions;
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
        //private readonly IMovementRepository
        //    _movementRepository;
        private readonly IBeamRepository
            _beamRepository;

        public FinishDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            //_movementRepository =
            //  _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(FinishDoffDailyOperationSizingCommand request, CancellationToken cancellationToken)
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

            //Get Daily Operation History
            var existingDailyOperationSizingHistories =
                existingDailyOperationSizingDocument
                        .SizingHistories
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationSizingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            //var existingDailyOperationBeamProducts =
            //    existingDailyOperationSizingDocument
            //            .SizingBeamProducts
            //            .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            //var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperationSizingDocument
                    .SizingBeamProducts
                    .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (countBeamStatus != 0)
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Finish. There's ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Machine Status
            var currentMachineStatus = lastHistory.MachineStatus;

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
                existingDailyOperationSizingDocument.OperationStatus;

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
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var doffFinishDateMachineLogUtc = new DateTimeOffset(request.FinishDoffDate.Date, new TimeSpan(+7, 0, 0));

            if (doffFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("FinishDoffDate", "Finish date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("FinishDoffTime", "Finish time cannot less than or equal latest time log"));
                }
                else
                {
                    existingDailyOperationSizingDocument.SetMachineSpeed(request.MachineSpeed);
                    existingDailyOperationSizingDocument.SetTexSQ(request.TexSQ);
                    existingDailyOperationSizingDocument.SetVisco(request.Visco);
                    existingDailyOperationSizingDocument.SetOperationStatus(OperationStatus.ONFINISH);

                    //Add New Detail on Document
                    //var causes = JsonConvert.DeserializeObject<DailyOperationSizingCauseValueObject>(lastDetail.Causes);
                    var newHistory =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                               new ShiftId(request.FinishDoffShift.Value),
                                                               new OperatorId(request.FinishDoffOperator.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONFINISH,
                                                               "-",
                                                               //new DailyOperationSizingCauseValueObject(causes.BrokenBeam, causes.MachineTroubled),
                                                               lastHistory.BrokenBeam,
                                                               lastHistory.MachineTroubled,
                                                               "");

                    existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newHistory);

                    await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);

                    _storage.Save();

                    return existingDailyOperationSizingDocument;
                }

            }
        }
    }
}
