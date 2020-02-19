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
    public class UpdateStartDailyOperationSizingCommandHandler : ICommandHandler<UpdateStartDailyOperationSizingCommand, DailyOperationSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationSizingDocumentRepository
            _dailyOperationSizingDocumentRepository;
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
        private readonly IDailyOperationSizingHistoryRepository
            _dailyOperationSizingHistoryRepository;
        private readonly IBeamRepository
            _beamDocumentRepository;

        public UpdateStartDailyOperationSizingCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationSizingDocumentRepository = 
                _storage.GetRepository<IDailyOperationSizingDocumentRepository>();
            _dailyOperationSizingBeamProductRepository =
                _storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
            _dailyOperationSizingHistoryRepository = 
                _storage.GetRepository<IDailyOperationSizingHistoryRepository>();
            _beamDocumentRepository = _storage.GetRepository<IBeamRepository>();
        }

        public async Task<DailyOperationSizingDocument> Handle(UpdateStartDailyOperationSizingCommand request, CancellationToken cancellationToken)
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
            var existingSizingBeamProduct =
                _dailyOperationSizingBeamProductRepository
                    .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity)
                    .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            var lastBeamProduct = existingSizingBeamProduct.FirstOrDefault();

            //Validation for Beam Id
            var countBeamId =
                existingSizingBeamProduct
                    .Where(o => o.SizingBeamId.Equals(request.SizingBeamId.Value))
                    .Count();

            if (!countBeamId.Equals(0))
            {
                throw Validator.ErrorValidation(("SizingBeamId", "No. Beam Sizing ini Sudah Digunakan dalam Operasi Harian Sizing Ini"));
            }

            //Validation for Beam Status
            var countBeamStatus = 
                existingSizingBeamProduct
                    .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (!countBeamStatus.Equals(0))
            {
                throw Validator.ErrorValidation(("BeamStatus", "Can't Start. There's ONPROCESS Sizing Beam on this Operation"));
            }

            //Validation for Operation Status
            var operationCompleteStatus =
                existingSizingDocument.OperationStatus;

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
                throw Validator.ErrorValidation(("StartDate", "Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONENTRY || lastHistory.MachineStatus == MachineStatus.ONCOMPLETE)
                    {
                        var newBeamProduct = 
                            new DailyOperationSizingBeamProduct(Guid.NewGuid(),
                                                                request.SizingBeamId,
                                                                request.CounterStart,
                                                                BeamStatus.ONPROCESS,
                                                                dateTimeOperation,
                                                                existingSizingDocument.Identity);

                        await _dailyOperationSizingBeamProductRepository.Update(newBeamProduct);

                        var newHistory =
                                new DailyOperationSizingHistory(Guid.NewGuid(),
                                                                request.StartShift,
                                                                request.StartOperator,
                                                                dateTimeOperation,
                                                                MachineStatus.ONSTART,
                                                                existingSizingDocument.Identity);
                        newHistory.SetSizingBeamNumber(request.SizingBeamNumber);

                        await _dailyOperationSizingHistoryRepository.Update(newHistory);
                        
                        _storage.Save();

                        return existingSizingDocument;
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
