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
        private readonly IDailyOperationSizingBeamProductRepository
            _dailyOperationSizingBeamProductRepository;
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
            _dailyOperationSizingBeamProductRepository =
                _storage.GetRepository<IDailyOperationSizingBeamProductRepository>();
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
                    .Find(o=>o.DailyOperationSizingDocumentId == existingSizingDocument.Identity && request.SizingBeamNumber == o.SizingBeamNumber)
                    .OrderByDescending(x => x.DateTimeMachine);
            var lastHistory = existingSizingHistories.FirstOrDefault();

            ////Validation for Beam Status
            //var countBeamStatus =
            //    existingSizingDocument
            //        .SizingBeamProducts
            //        .Where(e => e.BeamStatus == BeamStatus.ONPROCESS)
            //        .Count();

            //if (countBeamStatus != 0)
            //{
            //    throw Validator.ErrorValidation(("BeamStatus", "Can't Finish. There's ONPROCESS Sizing Beam on this Operation"));
            //}

            //Validation for Machine Status
            //var currentMachineStatus = lastHistory.MachineStatus;

            //if (currentMachineStatus != MachineStatus.ONCOMPLETE)
            //{
            //    throw Validator.ErrorValidation(("MachineStatus", "Can't Finish. This Machine's Operation is not ONCOMPLETE"));
            //}

            //Validation for Finished Operation Status
            var currentOperationStatus =
                existingSizingDocument.OperationStatus;

            if (currentOperationStatus == OperationStatus.ONFINISH)
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Finish. This Operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.ProduceBeamDate.Year;
            var month = request.ProduceBeamDate.Month;
            var day = request.ProduceBeamDate.Day;
            var hour = request.ProduceBeamTime.Hours;
            var minutes = request.ProduceBeamTime.Minutes;
            var seconds = request.ProduceBeamTime.Seconds;
            var dateTimeOperation =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for DoffFinish Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var doffFinishDateMachineLogUtc = new DateTimeOffset(request.ProduceBeamDate.Date, new TimeSpan(+7, 0, 0));

            if (doffFinishDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ProduceBeamDate", "Tanggal Tidak Boleh Lebih Awal Dari Tanggal Sebelumnya"));
            }
            else
            {
                if (dateTimeOperation <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ProduceBeamTime", "Waktu Tidak Boleh Lebih Awal Dari Waktu Sebelumnya"));
                }
                else
                {
                    //Update Sizing Document Properties
                    if (request.IsFinishFlag == true)
                    {
                        existingSizingDocument.SetOperationStatus(OperationStatus.ONFINISH);
                    }
                    else
                    {
                        existingSizingDocument.SetOperationStatus(OperationStatus.ONPROCESS);
                    }

                    existingSizingDocument.SetMachineSpeed(request.MachineSpeed);
                    existingSizingDocument.SetTexSQ(request.TexSQ);
                    existingSizingDocument.SetVisco(request.Visco);

                    await _dailyOperationSizingDocumentRepository.Update(existingSizingDocument);

                    //Get Daily Operation Beam Product
                    var existingDailyOperationBeamProducts =
                        _dailyOperationSizingBeamProductRepository
                            .Find(o => o.DailyOperationSizingDocumentId == existingSizingDocument.Identity && o.SizingBeamId.ToString() == request.SizingBeamId)
                            .OrderByDescending(o => o.LatestDateTimeBeamProduct);
                    var lastBeamProduct = existingDailyOperationBeamProducts.FirstOrDefault();

                    var totalBrokenHistories =
                        existingSizingHistories
                            .Where(o => o.SizingBeamNumber == lastHistory.SizingBeamNumber)
                            .Sum(x => x.BrokenPerShift) + request.BrokenPerShift;

                    //Set Beam Product Value on Daily Operation Sizing Beam Document
                    var theoriticalLimit = Math.Round(request.WeightTheoritical, 2);
                    var spuLimit = Math.Round(request.SPU, 2);

                    lastBeamProduct.SetSizingBeamStatus(BeamStatus.ROLLEDUP);
                    lastBeamProduct.SetLatestDateTimeBeamProduct(dateTimeOperation);
                    lastBeamProduct.SetCounterFinish(request.CounterFinish);
                    lastBeamProduct.SetWeightNetto(request.WeightNetto);
                    lastBeamProduct.SetWeightBruto(request.WeightBruto);
                    lastBeamProduct.SetWeightTheoritical(theoriticalLimit);
                    lastBeamProduct.SetPISMeter(request.PISMeter);
                    lastBeamProduct.SetSPU(spuLimit);
                    lastBeamProduct.SetTotalBroken(totalBrokenHistories);

                    await _dailyOperationSizingBeamProductRepository.Update(lastBeamProduct);

                    //Add New History
                    var newHistory =
                        new DailyOperationSizingHistory(Guid.NewGuid(),
                                                        request.ProduceBeamShift,
                                                        request.ProduceBeamOperator,
                                                        dateTimeOperation,
                                                        MachineStatus.ONCOMPLETE,
                                                        existingSizingDocument.Identity);
                    newHistory.SetBrokenPerShift(request.BrokenPerShift);
                    newHistory.SetSizingBeamNumber(lastHistory.SizingBeamNumber);

                    await _dailyOperationSizingHistoryRepository.Update(newHistory);

                    _storage.Save();

                    return existingSizingDocument;
                }

            }
        }
    }
}
