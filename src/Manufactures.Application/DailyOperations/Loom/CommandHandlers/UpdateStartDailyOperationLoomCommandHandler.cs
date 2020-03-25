using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateStartDailyOperationLoomCommandHandler : ICommandHandler<UpdateStartDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;
        //private readonly IDailyOperationLoomBeamProductRepository 
        //    _dailyOperationLoomProductRepository;

        public UpdateStartDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage =
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository =
                _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
            //_dailyOperationLoomProductRepository = 
            //    _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateStartDailyOperationLoomCommand request, CancellationToken cancellationToken)
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
                    .Where(o => o.BeamDocumentId == request.StartBeamUsedId)
                    .OrderByDescending(d => d.DateTimeMachine)
                    .FirstOrDefault();

            var lastLoomBeamUsed =
                _dailyOperationLoomBeamUsedRepository
                    .Find(s => s.DailyOperationLoomDocumentId == loomDocument.Identity)
                    .Where(o => o.BeamDocumentId == request.StartBeamUsedId)
                    .OrderByDescending(d => d.LastDateTimeProcessed)
                    .FirstOrDefault();

            //Get Daily Operation Loom History
            //var loomHistories =
            //    loomHistories
            //            .Where(o => o.BeamNumber.Equals(request.StartBeamNumber))
            //            .OrderByDescending(o => o.DateTimeMachine);
            //var lastHistory = loomHistories.FirstOrDefault();

            ////Get Daily Operation Loom Beam Product
            //var loomBeamsUsed =
            //    loomBeamsUsed
            //            .Where(o => o.Identity.Equals(request.StartBeamDocumentId))
            //            .OrderByDescending(o => o.LastDateTimeProcessed);
            //var lastBeamProduct = loomBeamsUsed.FirstOrDefault();

            //Validation for Prevent Beam Product with End Status Processed Again
            if (lastLoomBeamUsed.BeamUsedStatus.Equals(BeamStatus.COMPLETED))
            {
                throw Validator.ErrorValidation(("StartBeamUsedId", "Status Beam Ini Sudah Completed, Tidak Dapat Diproses Kembali"));
            }

            //Reformat DateTime
            var year = request.StartDate.Year;
            var month = request.StartDate.Month;
            var day = request.StartDate.Day;
            var hour = request.StartTime.Hours;
            var minutes = request.StartTime.Minutes;
            var seconds = request.StartTime.Seconds;
            var dateTimeLoom =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastLoomHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var startDateMachineLogUtc = new DateTimeOffset(request.StartDate.Date, new TimeSpan(+7, 0, 0));

            if (startDateMachineLogUtc < lastDateMachineLogUtc)
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
                    if (lastLoomHistory.MachineStatus == MachineStatus.ONENTRY)
                    {
                        var newLoomHistory =
                                new DailyOperationLoomHistory(Guid.NewGuid(),
                                                              request.StartBeamUsedId,
                                                              request.StartBeamUsedNumber,
                                                              lastLoomHistory.LoomMachineId,
                                                              request.StartLoomOperatorDocumentId,
                                                              0,
                                                              //request.StartCounterPerOperator,
                                                              dateTimeLoom,
                                                              request.StartShiftDocumentId,
                                                              MachineStatus.ONSTART,
                                                              loomDocument.Identity);
                        newLoomHistory.SetTyingOperatorId(request.StartTyingOperatorDocumentId);

                        await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                        //lastLoomBeamUsed.SetStartCounter(request.StartCounterPerOperator);
                        lastLoomBeamUsed.SetLastTyingOperatorDocumentId(request.StartTyingOperatorDocumentId);
                        lastLoomBeamUsed.SetLastTyingOperatorName(request.StartTyingOperatorName);
                        lastLoomBeamUsed.SetLastDateTimeProcessed(dateTimeLoom);
                        lastLoomBeamUsed.SetBeamUsedStatus(BeamStatus.ONPROCESS);

                        await _dailyOperationLoomBeamUsedRepository.Update(lastLoomBeamUsed);

                        await _dailyOperationLoomDocumentRepository.Update(loomDocument);
                        _storage.Save();

                        return loomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Tidak Dapat Memulai, Status Terakhir Mesin Beam " + request.StartBeamUsedNumber + " Harus ON-ENTRY"));
                    }
                }
            }
        }
    }
}
