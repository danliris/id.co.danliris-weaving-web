using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateResumeDailyOperationLoomCommandHandler : ICommandHandler<UpdateResumeDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationLoomRepository
            _dailyOperationLoomDocumentRepository;
        private readonly IDailyOperationLoomBeamHistoryRepository _dailyOperationLoomHistoryRepository;
        private readonly IDailyOperationLoomBeamProductRepository _dailyOperationLoomProductRepository;

        public UpdateResumeDailyOperationLoomCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationLoomDocumentRepository = _storage.GetRepository<IDailyOperationLoomRepository>();
            _dailyOperationLoomHistoryRepository = _storage.GetRepository<IDailyOperationLoomBeamHistoryRepository>();
            _dailyOperationLoomProductRepository = _storage.GetRepository<IDailyOperationLoomBeamProductRepository>();
        }

        public async Task<DailyOperationLoomDocument> Handle(UpdateResumeDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            //Get Daily Operation Document Loom
            //var loomQuery =
            //    _dailyOperationLoomDocumentRepository
            //            .Query
            //            .Include(o => o.LoomBeamHistories)
            //            .Include(o => o.LoomBeamProducts)
            //            .Where(doc => doc.Identity.Equals(request.Id));
            var existingDailyOperationLoomDocument =
                _dailyOperationLoomDocumentRepository
                        .Find(s => s.Identity == request.Id)
                        .FirstOrDefault();

            if (existingDailyOperationLoomDocument == null)
                throw Validator.ErrorValidation(("Id", "Invalid Daily Operation Loom: " + request.Id));

            var loomHistories = _dailyOperationLoomHistoryRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);
            var loomProducts = _dailyOperationLoomProductRepository.Find(s => s.DailyOperationLoomDocumentId == existingDailyOperationLoomDocument.Identity);


            //Get Daily Operation Loom History
            var existingDailyOperationLoomHistories =
               loomHistories
                        .Where(o => o.BeamNumber.Equals(request.ResumeBeamNumber))
                        .OrderByDescending(o => o.DateTimeMachine);
            var lastHistory = existingDailyOperationLoomHistories.FirstOrDefault();

            //Get Daily Operation Loom Beam Product
            var existingDailyOperationLoomBeamProducts =
                loomProducts
                        .Where(o => o.BeamDocumentId.Value == request.ResumeBeamProductBeamId)
                        .OrderByDescending(o => o.LatestDateTimeBeamProduct);
            var lastBeamProduct = existingDailyOperationLoomBeamProducts.FirstOrDefault();

            //Reformat DateTime
            var year = request.ResumeDateMachine.Year;
            var month = request.ResumeDateMachine.Month;
            var day = request.ResumeDateMachine.Day;
            var hour = request.ResumeTimeMachine.Hours;
            var minutes = request.ResumeTimeMachine.Minutes;
            var seconds = request.ResumeTimeMachine.Seconds;
            var resumeDateTime =
                new DateTimeOffset(year, month, day, hour, minutes, seconds, new TimeSpan(+7, 0, 0));

            //Validation for Start Date
            var lastDateMachineLogUtc = new DateTimeOffset(lastHistory.DateTimeMachine.Date, new TimeSpan(+7, 0, 0));
            var resumeDateMachineLogUtc = new DateTimeOffset(request.ResumeDateMachine.Date, new TimeSpan(+7, 0, 0));

            if (resumeDateMachineLogUtc < lastDateMachineLogUtc)
            {
                throw Validator.ErrorValidation(("ResumeDate", "Resume date cannot less than latest date log"));
            }
            else
            {
                if (resumeDateTime <= lastHistory.DateTimeMachine)
                {
                    throw Validator.ErrorValidation(("ResumeTime", "Resume time cannot less than or equal latest time log"));
                }
                else
                {
                    if (lastHistory.MachineStatus == MachineStatus.ONSTOP)
                    {
                        var newLoomHistory =
                            new DailyOperationLoomBeamHistory(Guid.NewGuid(),
                                                              request.ResumeBeamNumber,
                                                              request.ResumeMachineNumber,
                                                              new OperatorId(request.ResumeOperatorDocumentId.Value),
                                                              resumeDateTime,
                                                              new ShiftId(request.ResumeShiftDocumentId.Value),
                                                              MachineStatus.ONRESUME,
                                                              existingDailyOperationLoomDocument.Identity);

                        newLoomHistory.SetWarpBrokenThreads(lastHistory.WarpBrokenThreads ?? 0);
                        newLoomHistory.SetWeftBrokenThreads(lastHistory.WeftBrokenThreads ?? 0);
                        newLoomHistory.SetLenoBrokenThreads(lastHistory.LenoBrokenThreads ?? 0);

                        //existingDailyOperationLoomDocument.AddDailyOperationLoomHistory(newLoomHistory);
                        await _dailyOperationLoomHistoryRepository.Update(newLoomHistory);

                        lastBeamProduct.SetLatestDateTimeBeamProduct(resumeDateTime);

                        await _dailyOperationLoomDocumentRepository.Update(existingDailyOperationLoomDocument);
                        _storage.Save();

                        return existingDailyOperationLoomDocument;
                    }
                    else
                    {
                        throw Validator.ErrorValidation(("MachineStatus", "Can't resume, latest machine status must ONSTOP"));
                    }
                }
            }
        }
    }
}
