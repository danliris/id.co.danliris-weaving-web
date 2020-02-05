using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
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
    public class UpdateStartDailyOperationSizingCommandHandler : ICommandHandler<UpdateStartDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public UpdateStartDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationSizingDocumentRepository = _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateStartDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
            //var existingDailyOperationSizingBeamProduct =
            //    existingDailyOperationSizingDocument
            //        .SizingBeamProducts
            //        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            //var lastBeamProduct = existingDailyOperationSizingBeamProduct.FirstOrDefault();

            //Validation for Beam Id
            var countBeamId =
                existingDailyOperationSizingDocument
                    .SizingBeamProducts
                    .Where(o => o.SizingBeamId.Equals(request.SizingBeamId.Value))
                    .Count();

            if (!countBeamId.Equals(0))
            {
                throw Validator.ErrorValidation(("SizingBeamId", "No. Beam Sizing ini Sudah Digunakan dalam Operasi Harian Sizing Ini"));
            }

            //Validation for Beam Status
            var countBeamStatus =
                existingDailyOperationSizingDocument
                    .SizingBeamProducts
                    .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (!countBeamStatus.Equals(0))
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Start. There's ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Operation Status
            var operationCompleteStatus =
                existingDailyOperationSizingDocument.OperationStatus;

            if (operationCompleteStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.StartDate.Year;
            var month = request.StartDate.Month;
            var day = request.StartDate.Day;
            var hour = request.StartTime.Hours;
            var minutes = request.StartTime.Minutes;
            var seconds = request.StartTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Start date cannot less than latest date log"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Start time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONENTRY || lastHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        var sizingBeamDocument = 
                            _beamDocumentRepository
                                .Find(b => b.Identity.Equals(request.SizingBeamId.Value))
                                .FirstOrDefault();
                        var sizingBeamNumber = sizingBeamDocument.Number;

                        var newBeamProduct = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                                   new BeamId(sizingBeamDocument.Identity),
                                                                                   dateTimeOperation,
                                                                                   request.CounterStart,
                                                                                   0,
                                                                                   0,
                                                                                   0,
                                                                                   0,
                                                                                   0,
                                                                                   0,
                                                                                   BeamStatus.ONPROCESS);
                        existingDailyOperationSizingDocument.AddDailyOperationSizingBeamProduct(newBeamProduct);

                        var newHistory =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                               new ShiftId(request.StartShift.Value),
                                                               new OperatorId(request.StartOperator.Value),
                                                               dateTimeOperation,
                                                               MachineStatus.ONSTART,
                                                               "-",
                                                               lastHistory.BrokenBeam,
                                                               lastHistory.MachineTroubled,
                                                               sizingBeamNumber);
                        existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newHistory);

                        await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);
                        _storage.Save();

                        return existingDailyOperationSizingDocument;
                        //}
                        //else if (lastHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                        //{
                        //Get Daily Operation Beam Product
                        //var existingDailyOperationBeamProducts =
                        //    existingDailyOperationSizingDocument
                        //            .SizingBeamProducts
                        //            .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                        //var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

                        //if (lastBeamProduct.BeamStatus == BeamStatus.ROLLEDUP)
                        //{
                        //var sizingBeamDocument = _beamDocumentRepository.Find(b => b.Identity.Equals(request.SizingBeamId.Value)).FirstOrDefault();
                        //var sizingBeamNumber = sizingBeamDocument.Number;

                        //var newBeamDocument = new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                        //                                                           new BeamId(sizingBeamDocument.Identity),
                        //                                                           dateTimeOperation,
                        //                                                           request.CounterStart,
                        //                                                           0,
                        //                                                           0,
                        //                                                           0,
                        //                                                           0,
                        //                                                           0,
                        //                                                           0,
                        //                                                           BeamStatus.ONPROCESS);
                        //existingDailyOperationSizingDocument.AddDailyOperationSizingBeamProduct(newBeamDocument);

                        //var newOperationDetail =
                        //        new DailyOperationSizingHistory(Guid.NewGuid(),
                        //                                       new ShiftId(request.StartShift.Value),
                        //                                       new OperatorId(request.StartOperator.Value),
                        //                                       dateTimeOperation,
                        //                                       MachineStatus.ONSTART,
                        //                                       "-",
                        //                                       lastHistory.BrokenBeam,
                        //                                       lastHistory.MachineTroubled,
                        //                                       sizingBeamNumber);
                        //existingDailyOperationSizingDocument.AddDailyOperationSizingHistory(newOperationDetail);

                        //await _dailyOperationSizingDocumentRepository.Update(existingDailyOperationSizingDocument);
                        //_storage.Save();

                        //return existingDailyOperationSizingDocument;
                        //}
                        //else
                        //{
                        //    throw Validator.ErrorValidation(("BeamStatus", "Can't start, latest beam status must ROLLED-UP"));
                        //}
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't start, latest machine status must ONENTRY or ONCOMPLETE"));
                    }
                }
            }
        }
    }
}
