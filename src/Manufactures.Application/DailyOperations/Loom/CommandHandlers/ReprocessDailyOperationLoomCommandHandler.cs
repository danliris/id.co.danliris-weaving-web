using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class ReprocessDailyOperationLoomCommandHandler : ICommandHandler<ReprocessDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;

        public ReprocessDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository =
                _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(ReprocessDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            var loomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(s => s.Identity == request.Id)
                        .FirstOrDefault();

            if (loomDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Loom: " + request.Id));
            }

            var lastLoomHistory =
                _dailyOperationLoomHistoryRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId == request.ReprocessBeamUsedId)
                    .OrderByDescending(d => d.DateTimeMachine)
                    .FirstOrDefault();

            var lastLoomBeamUsed =
                _dailyOperationLoomBeamUsedRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId == request.ReprocessBeamUsedId)
                    .OrderByDescending(d => d.LastDateTimeProcessed)
                    .FirstOrDefault();

            //Reformat DateTime
            var year = request.ReprocessDate.Year;
            var month = request.ReprocessDate.Month;
            var day = request.ReprocessDate.Day;
            var hour = request.ReprocessTime.Hours;
            var minutes = request.ReprocessTime.Minutes;
            var seconds = request.ReprocessTime.Seconds;
            var dateTimeLoom =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastLoomHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var reprocessDateMachineLogUtc = new DateTimeOffset(request.ReprocessDate.Date, new TimeSpan(+7, 0, 0));

            if (reprocessDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("StartDate", "Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya"));
            }
            else
            {
                if (dateTimeLoom <= lastLoomHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("StartTime", "Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya"));
                }
                else
                {
                    if (lastLoomHistory.MachineStatus == MachineStatus.ONSTART || lastLoomHistory.MachineStatus == MachineStatus.ONPROCESSBEAM)
                    {
                        var newLoomHistory =
                                new DailyOperationLoomHistory(Guid.NewGuid(),
                                                              request.ReprocessBeamUsedId,
                                                              request.ReprocessBeamUsedNumber,
                                                              lastLoomHistory.LoomMachineId,
                                                              request.ReprocessLoomOperatorDocumentId,
                                                              0,
                                                              //request.StartCounterPerOperator,
                                                              dateTimeLoom,
                                                              request.ReprocessShiftDocumentId,
                                                              MachineStatus.REPROCESS,
                                                              loomDocument.Identity);
                        newLoomHistory.SetTyingOperatorId(request.ReprocessTyingOperatorDocumentId);
                        newLoomHistory.SetInformation(request.Information);

                        await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                        lastLoomBeamUsed.SetLastTyingOperatorDocumentId(request.ReprocessTyingOperatorDocumentId);
                        lastLoomBeamUsed.SetLastTyingOperatorName(request.ReprocessTyingOperatorName);
                        lastLoomBeamUsed.SetLastDateTimeProcessed(dateTimeLoom);
                        lastLoomBeamUsed.SetBeamUsedStatus(BeamStatus.REPROCESS);

                        await _dailyOperationLoomBeamUsedRepository.Update(lastLoomBeamUsed);

                        await _dailyOperationLoomDocumentRepository.Update(loomDocument);
                        _storage.Save();

                        return loomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Tidak Dapat Memulai, Status Terakhir Mesin Beam " + request.ReprocessBeamUsedNumber + " Harus ON-START atau ON-PROCESS-BEAM"));
                    }
                }
            }
        }
    }
}
