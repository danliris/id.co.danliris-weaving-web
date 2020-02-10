using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
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
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        //private readonly IMovementRepository
        //    _movementRepository;
        private readonly IBeamRepository
            _beamRepository;

        public FinishDoffDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository =
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            //_movementRepository =
            //  _storage.GetRepository<IMovementRepository>();
            _beamRepository =
              _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(FinishDoffDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Sizing
            var existingSizingDocument =
                _dailyOperationSizingDocumentRepository
                        .Find(o=>o.Identity == request.Id)
                        .FirstOrDefault();

            //Get Daily Operation History
            var existingSizingHistories =
                _dailyOperationSizingHistoryRepository
                    .Find(o=>o.Identity == existingSizingDocument.Identity)
                    .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingSizingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            //var existingDailyOperationBeamProducts =
            //    existingDailyOperationSizingDocument
            //            .SizingBeamProducts
            //            .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            //var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatus =
                existingSizingDocument
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
                existingSizingDocument.OperationStatus;

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
                    //Update Sizing Document Properties
                    existingSizingDocument.SetMachineSpeed(request.MachineSpeed);
                    existingSizingDocument.SetTexSQ(request.TexSQ);
                    existingSizingDocument.SetVisco(request.Visco);
                    existingSizingDocument.SetOperationStatus(OperationStatus.ONFINISH);

                    await _dailyOperationSizingDocumentRepository.Update(existingSizingDocument);

                    //Add New History
                    var newHistory =
                        new DailyOperationSizingHistory(Guid.NewGuid(),
                                                        request.FinishDoffShift,
                                                        request.FinishDoffOperator,
                                                        dateTimeOperation,
                                                        MachineStatus.ONFINISH,
                                                        "",
                                                        lastHistory.BrokenPerShift,
                                                        lastHistory.MachineTroubled,
                                                        "",
                                                        existingSizingDocument.Identity);

                    await _dailyOperationSizingHistoryRepository.Update(newHistory);

                    _storage.Save();

                    return existingSizingDocument;
                }

            }
        }
    }
}
