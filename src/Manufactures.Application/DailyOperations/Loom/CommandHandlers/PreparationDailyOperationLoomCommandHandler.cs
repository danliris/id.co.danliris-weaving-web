using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockMonitoring.Repositories;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class PreparationDailyOperationLoomCommandHandler : ICommandHandler<PreparationDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage 
            _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IBeamStockMonitoringRepository
             _beamStockMonitoringRepository;
        private readonly IDailyOperationLoomBeamUsedRepository
            _dailyOperationLoomBeamUsedRepository;
        private readonly IDailyOperationLoomHistoryRepository
            _dailyOperationLoomHistoryRepository;
        //private readonly IDailyOperationLoomBeamProductRepository 
        //    _dailyOperationLoomProductRepository;

        public PreparationDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _dailyOperationLoomDocumentRepository =
                _storage.GetRepository<IDailyOperationLoomRepository>();
            _beamStockMonitoringRepository =
                _storage.GetRepository<IBeamStockMonitoringRepository>();
            _dailyOperationLoomBeamUsedRepository =
                _storage.GetRepository<IDailyOperationLoomBeamUsedRepository>();
            _dailyOperationLoomHistoryRepository =
                 _storage.GetRepository<IDailyOperationLoomHistoryRepository>();
            //_dailyOperationLoomProductRepository = 
            //    _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(PreparationDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            var newLoomDocument =
                new DailyOperationLoomDocument(Guid.NewGuid(),
                                               request.OrderDocumentId,
                                               request.BeamProcessed,
                                               OperationStatus.ONPROCESS);
            newLoomDocument.SetTotalCounter(0);
            await _dailyOperationLoomDocumentRepository.Update(newLoomDocument);

            foreach (var item in request.LoomItems)
            {
                var year = item.PreparationDate.Year;
                var month = item.PreparationDate.Month;
                var day = item.PreparationDate.Day;
                var hour = item.PreparationTime.Hours;
                var minutes = item.PreparationTime.Minutes;
                var seconds = item.PreparationTime.Seconds;
                var dateTimeLoom =
                    new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

                var newBeamUsed = new DailyOperationLoomBeamUsed(Guid.NewGuid(),
                                                                 item.BeamOrigin,
                                                                 item.BeamDocumentId,
                                                                 item.BeamNumber,
                                                                 dateTimeLoom,
                                                                 BeamStatus.ONPROCESS,
                                                                 newLoomDocument.Identity);
                newBeamUsed.SetTyingMachineDocumentId(item.TyingMachineId);
                newBeamUsed.SetTyingMachineNumber(item.TyingMachineNumber);
                newBeamUsed.SetLoomMachineNumber(item.LoomMachineNumber);
                newBeamUsed.SetLastTyingOperatorDocumentId(item.TyingOperatorId);
                newBeamUsed.SetLastTyingOperatorName(item.TyingOperatorName);
                await _dailyOperationLoomBeamUsedRepository.Update(newBeamUsed);

                var newHistory = new DailyOperationLoomHistory(Guid.NewGuid(),
                                                               item.BeamDocumentId,
                                                               item.BeamNumber,
                                                               item.LoomMachineId,
                                                               item.LoomOperatorId,
                                                               0,
                                                               dateTimeLoom,
                                                               item.PreparationShift,
                                                               MachineStatus.ONENTRY,
                                                               newLoomDocument.Identity);
                newHistory.SetTyingMachineId(item.TyingMachineId);
                newHistory.SetTyingOperatorId(item.TyingOperatorId);
                await _dailyOperationLoomHistoryRepository.Update(newHistory);
            }

            //foreach (var beamProduct in request.LoomBeamProducts)
            //{
            //    var year = beamProduct.DateBeamProduct.Year;
            //    var month = beamProduct.DateBeamProduct.Month;
            //    var day = beamProduct.DateBeamProduct.Day;
            //    var hour = beamProduct.TimeBeamProduct.Hours;
            //    var minutes = beamProduct.TimeBeamProduct.Minutes;
            //    var seconds = beamProduct.TimeBeamProduct.Seconds;
            //    var dateTimeBeamProduct =
            //        new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //    var newBeamProduct =
            //        new DailyOperationLoomBeamUsed(Guid.NewGuid(),
            //                                       beamProduct.BeamOrigin,
            //                                       new BeamId(beamProduct.BeamDocumentId.Value),
            //                                       beamProduct.CombNumber,
            //                                       new MachineId(beamProduct.MachineDocumentId.Value),
            //                                       dateTimeBeamProduct,
            //                                       beamProduct.LoomProcess,
            //                                       BeamStatus.ONPROCESS,
            //                                       dailyOperationLoomDocument.Identity);

            //    //dailyOperationLoomDocument.AddDailyOperationLoomBeamProduct(newBeamProduct);
            //    await _dailyOperationLoomProductRepository.Update(newBeamProduct);
            //}

            //foreach (var beamHistory in request.LoomBeamHistories)
            //{
            //    var beamNumber = beamHistory.BeamNumber;

            //    var machineNumber = beamHistory.MachineNumber;

            //    var year = beamHistory.DateMachine.Year;
            //    var month = beamHistory.DateMachine.Month;
            //    var day = beamHistory.DateMachine.Day;
            //    var hour = beamHistory.TimeMachine.Hours;
            //    var minutes = beamHistory.TimeMachine.Minutes;
            //    var seconds = beamHistory.TimeMachine.Seconds;
            //    var dateTimeBeamHistory =
            //        new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //    var newLoomHistory =
            //            new DailyOperationLoomHistory(Guid.NewGuid(),
            //                                              beamNumber,
            //                                              machineNumber,
            //                                              new OperatorId(beamHistory.OperatorDocumentId.Value),
            //                                              dateTimeBeamHistory,
            //                                              new ShiftId(beamHistory.ShiftDocumentId.Value),
            //                                              MachineStatus.ONENTRY,
            //                                              dailyOperationLoomDocument.Identity);

            //    newLoomHistory.SetWarpBrokenThreads(0);
            //    newLoomHistory.SetWeftBrokenThreads(0);
            //    newLoomHistory.SetLenoBrokenThreads(0);
            //    newLoomHistory.SetInformation(beamHistory.Information ?? "");

            //    //dailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);
            //    await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);
            //}

            _storage.Save();

            return newLoomDocument;
        }
    }
}
