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
            //var currentBeamStatus = lastBeamProduct.BeamStatus;

            //if (!currentBeamStatus.Equals(BeamStatus.ONPROCESS))
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Resume. There isn't ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Operation Status
            var currentOperationStatus =
                existingDailyOperationSizingDocument.OperationStatus;

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
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var resumeDateMachineLogUtc = new DateTimeOffset(request.ResumeDate.Date, new TimeSpan(+7, 0, 0));

            if (resumeDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than or equal latest operation"));
                }
                else
                {
                    if (existingDailyOperationSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTOP)
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

                        var newHistory =
                                    new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                   new ShiftId(request.ResumeShift.Value),
                                                                   new OperatorId(request.ResumeOperator.Value),
                                                                   dateTimeOperation,
                                                                   MachineStatus.ONRESUME,
                                                                   "-",
                                                                   lastHistory.BrokenBeam,
                                                                   lastHistory.MachineTroubled,
                                                                   lastHistory.SizingBeamNumber);

                        existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newHistory);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);
                        _storage.Save();

                        return existingDailyOperationSizingDocument;
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
