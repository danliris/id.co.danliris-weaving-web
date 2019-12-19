using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.BeamStockMonitoring.Commands;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class ProduceBeamDailyOperationWarpingCommandHandler
        : ICommandHandler<ProduceBeamsDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public ProduceBeamDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(ProduceBeamsDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var warpingQuery =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(x => x.WarpingHistories)
                    .Include(x => x.WarpingBeamProducts)
                    .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(warpingQuery)
                    .FirstOrDefault();

            //Get Daily Operation Detail
            var existingDailyOperationWarpingHistories = existingDailyOperationWarpingDocument
                    .WarpingHistories
                    .OrderByDescending(detail => detail.DateTimeMachine);
            var lastWarpingHistory = existingDailyOperationWarpingHistories.FirstOrDefault();

            //Get Daily Operation Beam Product
            var existingDailyOperationWarpingBeamProduct = existingDailyOperationWarpingDocument
                    .WarpingBeamProducts
                    .OrderByDescending(beamProduct => beamProduct.LatestDateTimeBeamProduct);
            var lastWarpingBeamProduct = existingDailyOperationWarpingBeamProduct.FirstOrDefault();

            //Validation for Beam Status
            var countBeamStatusOnProcess =
                existingDailyOperationWarpingDocument
                    .WarpingBeamProducts
                    .Where(beamProduct => beamProduct.BeamStatus == BeamStatus.ONPROCESS)
                    .Count();

            if (!countBeamStatusOnProcess.Equals(0))
            {
                //Validation for Machine Status (History)
                var currentMachineStatus = lastWarpingHistory.MachineStatus;

                if (currentMachineStatus.Equals(MachineStatus.ONCOMPLETE))
                {
                    throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam. This current Operation status already ONCOMPLETE"));
                }

                //Validation for Operation Status
                var currentOperationStatus = existingDailyOperationWarpingDocument.OperationStatus;

                if (currentOperationStatus.Equals(OperationStatus.ONFINISH))
                {
                    throw Validator.ErrorValidation(("OperationStatus", "Can't Produce Beam. This operation's status already FINISHED"));
                }

                //Reformat DateTime
                var year = request.ProduceBeamsDate.Year;
                var month = request.ProduceBeamsDate.Month;
                var day = request.ProduceBeamsDate.Day;
                var hour = request.ProduceBeamsTime.Hours;
                var minutes = request.ProduceBeamsTime.Minutes;
                var seconds = request.ProduceBeamsTime.Seconds;
                var warpingDateTime =
                    new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

                //Validation for Start Date
                var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
                var warpingProduceBeamsDateLogUtc = new DateTimeOffset(request.ProduceBeamsDate.Date, new TimeSpan(+7, 0, 0));

                if (warpingProduceBeamsDateLogUtc < lastWarpingDateLogUtc)
                {
                    throw Validator.ErrorValidation(("ProduceBeamsDate", "Produce Beams date cannot less than latest date log"));
                }
                else
                {
                    if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                    {
                        throw Validator.ErrorValidation(("ProduceBeamsTime", "Produce Beams time cannot less than or equal latest time log"));
                    }
                    else
                    {
                        if (lastWarpingHistory.MachineStatus == MachineStatus.ONSTART || lastWarpingHistory.MachineStatus == MachineStatus.ONRESUME)
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.ProduceBeamsShift.Value),
                                                                              new OperatorId(request.ProduceBeamsOperator.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONCOMPLETE,
                                                                              lastWarpingHistory.WarpingBeamNumber);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //Assign Value to Warping Beam Product and Add to Warping Document
                            //var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(), 
                            //                                                          request.WarpingBeamLength, 
                            //                                                          request.Tention, 
                            //                                                          request.MachineSpeed, 
                            //                                                          request.PressRoll);
                            lastWarpingBeamProduct.SetWarpingBeamId(lastWarpingBeamProduct.WarpingBeamId);

                            lastWarpingBeamProduct.SetWarpingTotalBeamLength(request.WarpingBeamLength);
                            lastWarpingBeamProduct.SetWarpingBeamLengthUomId(request.WarpingBeamLengthUOMId);
                            lastWarpingBeamProduct.SetTention(request.Tention);
                            lastWarpingBeamProduct.SetMachineSpeed(request.MachineSpeed);
                            lastWarpingBeamProduct.SetPressRoll(request.PressRoll);

                            lastWarpingBeamProduct.SetBeamStatus(BeamStatus.ROLLEDUP);
                            lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);
                            //existingDailyOperationWarpingDocument.UpdateDailyOperationWarpingBeamProduct(newBeamProduct);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            //var sizingStock = new SizingBeamStockMonitoringCommand
                            //{
                            //    BeamDocumentId = new BeamId(existingDailyOperationWarpingDocument.Identity),
                            //    SizingEntryDate = request.ProduceBeamsDate,
                            //    OrderDocumentId = existingDailyOperationWarpingDocument.OrderDocumentId,
                            //    SizingLengthStock = request.WarpingBeamLength
                            //};

                            return existingDailyOperationWarpingDocument;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("MachineStatus", "Can't Produce Beam, latest status is not ONSTART or ONRESUME"));
                        }
                    }
                }
            }
            else
            {
                throw Validator.ErrorValidation(("WarpingBeamProductStatus", "Can't Produce Beam. There isn't ONPROCESS Warping Beam on this Operation"));
            }
        }
    }
}
