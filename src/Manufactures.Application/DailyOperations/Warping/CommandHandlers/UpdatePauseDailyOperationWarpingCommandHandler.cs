using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;

namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
{
    public class UpdatePauseDailyOperationWarpingCommandHandler :
        ICommandHandler<UpdatePauseDailyOperationWarpingCommand, DailyOperationWarpingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;

        public UpdatePauseDailyOperationWarpingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
        }

        public async Task<DailyOperationWarpingDocument> Handle(UpdatePauseDailyOperationWarpingCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Warping
            var warpingQuery =
                _dailyOperationWarpingRepository
                    .Query
                    .Include(x => x.WarpingHistories)
                    .Include(x => x.WarpingBeamProducts);
            var existingDailyOperationWarpingDocument =
                _dailyOperationWarpingRepository
                    .Find(warpingQuery)
                    .Where(doc => doc.Identity.Equals(request.Id))
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

            //Validation for Operation Status
            var operationStatus = existingDailyOperationWarpingDocument.OperationStatus;
            if (operationStatus.Equals(OperationStatus.ONFINISH))
            {
                throw Validator.ErrorValidation(("OperationStatus", "Can't Start. This operation's status already FINISHED"));
            }

            //Reformat DateTime
            var year = request.PauseDate.Year;
            var month = request.PauseDate.Month;
            var day = request.PauseDate.Day;
            var hour = request.PauseTime.Hours;
            var minutes = request.PauseTime.Minutes;
            var seconds = request.PauseTime.Seconds;
            var warpingDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastWarpingDateLogUtc = new DateTimeOffset(lastWarpingHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var warpingPauseDateLogUtc = new DateTimeOffset(request.PauseDate.Date, new TimeSpan(+7, 0, 0));

            if (warpingPauseDateLogUtc < lastWarpingDateLogUtc)
            {
                throw Validator.ErrorValidation(("PauseDate", "Pause date cannot less than latest date log"));
            }
            else
            {
                if (warpingDateTime <= lastWarpingHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("PauseTime", "Pause time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastWarpingHistory.MachineStatus == MachineStatus.ONSTART || lastWarpingHistory.MachineStatus == MachineStatus.ONRESUME)
                    {
                        if (!request.BrokenThreadsCause.Equals(0) && !request.LooseThreadsAmount.Equals(0))
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.PauseShift.Value),
                                                                              new OperatorId(request.PauseOperator.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTOP,
                                                                              request.Information,
                                                                              lastWarpingHistory.WarpingBeamNumber);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                            //                                                          request.BrokenThreadsCause,
                            //                                                          request.ConeDeficient);
                            lastWarpingBeamProduct.SetWarpingBeamId(lastWarpingBeamProduct.WarpingBeamId);

                            lastWarpingBeamProduct.SetBrokenThreadsCause(request.BrokenThreadsCause);
                            lastWarpingBeamProduct.SetConeDeficient(request.ConeDeficient);
                            lastWarpingBeamProduct.SetLooseThreadsAmount(request.LooseThreadsAmount);
                            lastWarpingBeamProduct.SetRightLooseCreel(request.RightLooseCreel);
                            lastWarpingBeamProduct.SetLeftLooseCreel(request.LeftLooseCreel);

                            lastWarpingBeamProduct.SetBeamStatus(lastWarpingBeamProduct.BeamStatus);
                            lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else if (request.BrokenThreadsCause.Equals(0) && !request.LooseThreadsAmount.Equals(0))
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.PauseShift.Value),
                                                                              new OperatorId(request.PauseOperator.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTOP,
                                                                              request.Information,
                                                                              lastWarpingHistory.WarpingBeamNumber);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                            //                                                          request.BrokenThreadsCause,
                            //                                                          request.ConeDeficient);
                            lastWarpingBeamProduct.SetWarpingBeamId(lastWarpingBeamProduct.WarpingBeamId);

                            lastWarpingBeamProduct.SetBrokenThreadsCause(lastWarpingBeamProduct.BrokenThreadsCause ?? 0);
                            lastWarpingBeamProduct.SetConeDeficient(lastWarpingBeamProduct.ConeDeficient ?? 0);
                            lastWarpingBeamProduct.SetLooseThreadsAmount(request.LooseThreadsAmount);
                            lastWarpingBeamProduct.SetRightLooseCreel(request.RightLooseCreel);
                            lastWarpingBeamProduct.SetLeftLooseCreel(request.LeftLooseCreel);

                            lastWarpingBeamProduct.SetBeamStatus(lastWarpingBeamProduct.BeamStatus);
                            lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else if (!request.BrokenThreadsCause.Equals(0) && request.LooseThreadsAmount.Equals(0))
                        {
                            existingDailyOperationWarpingDocument.SetDateTimeOperation(warpingDateTime);

                            //Assign Value to Warping History and Add to Warping Document
                            var newHistory = new DailyOperationWarpingHistory(Guid.NewGuid(),
                                                                              new ShiftId(request.PauseShift.Value),
                                                                              new OperatorId(request.PauseOperator.Value),
                                                                              warpingDateTime,
                                                                              MachineStatus.ONSTOP,
                                                                              request.Information,
                                                                              lastWarpingHistory.WarpingBeamNumber);
                            existingDailyOperationWarpingDocument.AddDailyOperationWarpingHistory(newHistory);

                            //var newBeamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                            //                                                          request.BrokenThreadsCause,
                            //                                                          request.ConeDeficient);
                            lastWarpingBeamProduct.SetWarpingBeamId(lastWarpingBeamProduct.WarpingBeamId);

                            lastWarpingBeamProduct.SetBrokenThreadsCause(request.BrokenThreadsCause);
                            lastWarpingBeamProduct.SetConeDeficient(request.ConeDeficient);
                            lastWarpingBeamProduct.SetLooseThreadsAmount(lastWarpingBeamProduct.LooseThreadsAmount ?? 0);
                            lastWarpingBeamProduct.SetRightLooseCreel(lastWarpingBeamProduct.RightLooseCreel ?? 0);
                            lastWarpingBeamProduct.SetLeftLooseCreel(lastWarpingBeamProduct.LeftLooseCreel ?? 0);

                            lastWarpingBeamProduct.SetBeamStatus(lastWarpingBeamProduct.BeamStatus);
                            lastWarpingBeamProduct.SetLatestDateTimeBeamProduct(warpingDateTime);

                            await _dailyOperationWarpingRepository.Update(existingDailyOperationWarpingDocument);
                            _storage.Save();

                            return existingDailyOperationWarpingDocument;
                        }
                        else
                        {
                            throw Validator.ErrorValidation(("BrokenThreadsCause", "Penyebab Putus Benang harus Diisi"));
                            throw Validator.ErrorValidation(("LooseThreadsAmount", "Jumlah Benang Lolos harus Diisi"));
                        }
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't stop, latest status is not on START or on RESUME"));
                    }
                }
            }

            ////Check if has existing daily operation
            //var warpingQuery =
            //    _warpingOperationRepository
            //        .Query
            //        .Include(x => x.WarpingHistories)
            //        .Include(x => x.WarpingBeamProducts);
            //var existingDailyOperation =
            //    _warpingOperationRepository
            //        .Find(warpingQuery)
            //        .Where(x => x.Identity.Equals(request.Id))
            //        .FirstOrDefault();

            ////Check if has existing daily operation
            //if (existingDailyOperation == null)
            //{
            //    //Throw an error doesn't have any operation
            //    throw Validator
            //            .ErrorValidation(("Id",
            //                              "Unavailable exsisting daily operation warping Document with Id " + request.Id));
            //}

            ////Set date time when user operate
            //var year = request.WarpingPauseDate.Year;
            //var month = request.WarpingPauseDate.Month;
            //var day = request.WarpingPauseDate.Day;
            //var hour = request.WarpingPauseTime.Hours;
            //var minutes = request.WarpingPauseTime.Minutes;
            //var seconds = request.WarpingPauseTime.Seconds;
            //var dateTimeOperation =
            //    new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            ////Add daily operation history
            //var history = new DailyOperationWarpingHistory(Guid.NewGuid(),
            //                                               request.ShiftId,
            //                                               request.OperatorId,
            //                                               dateTimeOperation,
            //                                               MachineStatus.ONSTOP);
            //existingDailyOperation.AddDailyOperationWarpingHistory(history);

            ////Update existing daily operation
            //await _warpingOperationRepository.Update(existingDailyOperation);
            //_storage.Save();

            ////return existing operation
            //return existingDailyOperation;
        }
    }
}
