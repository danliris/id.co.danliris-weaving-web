using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class UpdateStartDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;
        private readonly Mock<IBeamRepository> mockBeamRepo;
        public UpdateStartDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockDailyOperationSizingRepo = mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockDailyOperationSizingRepo.Object);

            mockBeamRepo = mockRepository.Create<IBeamRepository>();
            mockStorage.Setup(x => x.GetRepository<IBeamRepository>())
                 .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private UpdateStartDailyOperationSizingCommandHandler UpdateStartDailyOperationSizingCommandHandler()
        {
            return new UpdateStartDailyOperationSizingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Update Start on Daily Operation Sizing
         * **/
        [Fact]
        public async Task UpdateStart_MachineStatusEntry_DataUpdated()
        {
            //---ARRANGE---//
            //Instantiate Properties
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.UpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var operationStatus = OperationStatus.ONPROCESS;

            //Add Existing Data
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, 0, "0", "0", operationStatus);

            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "ROLLED-UP");
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                "ENTRY",
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");

            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));

            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //mockDailyOperationSizingRepo
            //     .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingReadModel, bool>>>()))
            //     .Returns(new List<DailyOperationSizingDocument>() { resultModel });

            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };
            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            request.SetId(sizingDocumentTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            //Instantiate Command Handler
            var result = await unitUnderTest.Handle(request, cancellationToken);

            //---ASSERT---//
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateStart_MachineStatusComplete_DataUpdated()
        {
            //---ARRANGE---//
            //Instantiate Properties
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.UpdateStartDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var operationStatus = OperationStatus.ONPROCESS;

            //Add Existing Data
            var resultModel = new DailyOperationSizingDocument(sizingDocumentTestId, machineDocumentId, orderId, beamsWarping, emptyWeight, yarnStrands, recipeCode, neReal, 0, "0", "0", operationStatus);

            var sizingBeamDocument = new DailyOperationSizingBeamDocument(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                new DailyOperationSizingCounterValueObject(0, 0),
                new DailyOperationSizingWeightValueObject(0, 0, 0),
                0,
                0,
                "ROLLED-UP");
            var sizingDetail = new DailyOperationSizingDetail(
                Guid.NewGuid(), new ShiftId(Guid.NewGuid()),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddDays(-1),
                "COMPLETED",
                "",
                new DailyOperationSizingCauseValueObject("0", "0"),
                "TS122");

            resultModel.AddDailyOperationSizingBeamDocument(sizingBeamDocument);
            resultModel.AddDailyOperationSizingDetail(sizingDetail);

            //Mocking Repository
            mockDailyOperationSizingRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { resultModel });

            mockDailyOperationSizingRepo
                .Setup(x => x.Update(It.IsAny<DailyOperationSizingDocument>()))
                .Returns(Task.FromResult(It.IsAny<DailyOperationSizingDocument>()));

            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<Domain.Beams.BeamDocument>() { new Domain.Beams.BeamDocument(Guid.NewGuid(), "TS122", "Sizing", 122) });

            //Counter Object on Sizing Beam Document Command (Incoming Object)
            var counterBeamDocument = new DailyOperationSizingCounterCommand
            {
                Start = 0,
                Finish = 0
            };
            //Sizing Beam Document Command (Incoming Object)
            var sizingBeamDocumentCommand = new UpdateStartDailyOperationSizingBeamDocumentCommand
            {
                SizingBeamId = new BeamId(Guid.NewGuid()),
                Counter = counterBeamDocument
            };

            //Sizing Detail Command (Incoming Object)
            var sizingDetailCommand = new UpdateStartDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                ShiftId = new ShiftId(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow,
                StartTime = new TimeSpan(7)
            };

            //Assign Beam Document Command & Detail Command (Childs Objects) to Sizing Document (Parent Objects)
            var sizingDocument = new UpdateStartDailyOperationSizingCommand
            {
                Id = sizingDocumentTestId,
                SizingBeamDocuments = sizingBeamDocumentCommand,
                SizingDetails = sizingDetailCommand
            };

            //Update Incoming Object
            UpdateStartDailyOperationSizingCommand request = new UpdateStartDailyOperationSizingCommand
            {
                SizingBeamDocuments = sizingDocument.SizingBeamDocuments,
                SizingDetails = sizingDocument.SizingDetails
            };
            request.SetId(sizingDocumentTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            //Instantiate Command Handler
            var result = await unitUnderTest.Handle(request, cancellationToken);

            //---ASSERT---//
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }
    }
}
