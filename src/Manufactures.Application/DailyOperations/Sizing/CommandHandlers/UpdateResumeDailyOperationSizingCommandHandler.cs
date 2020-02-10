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
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;

        public UpdateResumeDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingHistoryRepository =
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateResumeDailyOperationSizingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Sizing
            var existingSizingDocument =
                _dailyOperationSizingDocumentRepository
                        .Find(o=>o.Identity == request.Id)
                        .FirstOrDefault();

            //Get Daily Operation History
            var existingSizingHistories =
                _dailyOperationSizingHistoryRepository
                    .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                    .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingSizingHistories.FirstOrDefault();

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
                existingSizingDocument.OperationStatus;

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
                    if (existingSizingHistories.FirstOrDefault().MachineStatus == MachineStatus.ONSTOP)
                    {
                        var newHistory = 
                            new DailyOperationSizingHistory(Guid.NewGuid(),
                                                            request.ResumeShift,
                                                            request.ResumeOperator,
                                                            dateTimeOperation,
                                                            MachineStatus.ONRESUME,
                                                            "",
                                                            lastHistory.BrokenPerShift,
                                                            lastHistory.MachineTroubled,
                                                            lastHistory.SizingBeamNumber,
                                                            existingSizingDocument.Identity);

                        await _dailyOperationSizingHistoryRepository.Update(newHistory);

                        _storage.Save();

                        return existingSizingDocument;
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
