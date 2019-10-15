using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateResumeDailyOperationSizingCommandHandler : ICommandHandler<UpdateResumeDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingDocumentRepository;

        public UpdateResumeDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateResumeDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            var query = _dailyOperationSizingDocumentRepository.Query
                                                               .Include(d => d.SizingHistories)
                                                               .Include(b => b.SizingBeamProducts)
                                                               .Where(sizingDoc => sizingDoc.Identity.Equals(request.Id));
            var existingDailyOperation = _dailyOperationSizingDocumentRepository.Find(query).FirstOrDefault();
            var existingBeamdocuments = existingDailyOperation.SizingBeamProducts.OrderByDescending(b => b.LatestDateTimeBeamProduct);
            var lastBeamDocument = existingBeamdocuments.FirstOrDefault();
            var existingDetails = existingDailyOperation.SizingHistories.OrderByDescending(d => d.DateTimeMachine);
            var lastDetail = existingDetails.FirstOrDefault();

            //Validation for Beam Status
            var currentBeamStatus = lastBeamDocument.BeamStatus;

            if (!currentBeamStatus.Equals(BeamStatus.ONPROCESS))
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Resume. There isn't ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperation.OperationStatus;

            if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Resume. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ResumeDate.Year;
            var month = request.ResumeDate.Month;
            var day = request.ResumeDate.Day;
            var hour = request.ResumeTime.Hours;
            var minutes = request.ResumeTime.Minutes;
            var seconds = request.ResumeTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Resume Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastDetail.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var resumeDateMachineLogUtc = new DateTimeOffset(request.ResumeDate.Date, new TimeSpan(+7, 0, 0));

            if (resumeDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastDetail.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than or equal latest operation"));
                }
                else
                {
                    if (existingDetails.FirstOrDefault().MachineStatus == MachineStatus.ONSTOP)
                    {
                        //var updateBeamDocument = new DailyOperationSizingBeamProduct(lastBeamDocument.Identity,
                        //                                                             new BeamId(lastBeamDocument.SizingBeamId),
                        //                                                             dateTimeOperation,
                        //                                                             lastBeamDocument.CounterStart ?? 0,
                        //                                                             lastBeamDocument.CounterFinish ?? 0,
                        //                                                             lastBeamDocument.WeightNetto ?? 0,
                        //                                                             lastBeamDocument.WeightBruto ?? 0,
                        //                                                             lastBeamDocument.WeightTheoritical ?? 0,
                        //                                                             lastBeamDocument.PISMeter ?? 0,
                        //                                                             lastBeamDocument.SPU ?? 0,
                        //                                                             BeamStatus.ONPROCESS);

                        //existingDailyOperation.UpdateDailyOperationSizingBeamDocument(updateBeamDocument);

                        var newOperation =
                                    new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                   new ShiftId(request.ResumeShift.Value),
                                                                   new OperatorId(request.ResumeOperator.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONRESUME,
                                                                   "-",
                                                                   lastDetail.BrokenBeam,
                                                                   lastDetail.MachineTroubled,
                                                                   lastDetail.SizingBeamNumber);

                        existingDailyOperation.AddDailyOperationSizingDetail(newOperation);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperation);
                        _storage.Save();

                        return existingDailyOperation;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't Resume. This current Operation status isn't ONSTOP"));
                    }
                }
            }
        }
    }
}
