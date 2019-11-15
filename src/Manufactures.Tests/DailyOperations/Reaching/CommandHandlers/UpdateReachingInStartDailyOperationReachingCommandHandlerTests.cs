using ExtCore.Data.Abstractions;
using FluentAssertions;
using FluentValidation;
using Manufactures.Application.DailyOperations.Reaching.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Reaching.CommandHandlers
{
    public class UpdateReachingInStartDailyOperationReachingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingRepository>
            mockDailyOperationReachingRepo;

        public UpdateReachingInStartDailyOperationReachingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationReachingRepo = mockRepository.Create<IDailyOperationReachingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingRepository>())
                .Returns(mockDailyOperationReachingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateReachingInStartDailyOperationReachingCommandHandler CreateUpdateReachingInStartDailyOperationReachingCommandHandler()
        {
            return new UpdateReachingInStartDailyOperationReachingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Update Reaching Start on Daily Operation Reaching
         * **/
        [Fact]
        public async Task Handle_OperationStatusOnFinish_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateReachingInStartDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONFINISH;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId, machineId, orderDocumentId, beamId, operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 0;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateReachingInStartCommand = new UpdateReachingInStartDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 10,
                ReachingInStartDate = DateTimeOffset.UtcNow,
                ReachingInStartTime = new TimeSpan(7),
                ReachingInTypeInput = "Plain",
                ReachingInTypeOutput = "Lurus",
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateReachingInStartDailyOperationReachingCommand request = updateReachingInStartCommand;
            //request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Start. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ReachingInStartDateLessThanLatestDate_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateReachingInStartDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId, machineId, orderDocumentId, beamId, operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 0;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateReachingInStartCommand = new UpdateReachingInStartDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 10,
                ReachingInStartDate = DateTimeOffset.UtcNow.AddDays(-1),
                ReachingInStartTime = new TimeSpan(7),
                ReachingInTypeInput = "Plain",
                ReachingInTypeOutput = "Lurus",
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateReachingInStartDailyOperationReachingCommand request = updateReachingInStartCommand;
            //request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- ReachingInStartDate: Start date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ReachingInStartTimeLessThanLatestTime_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateReachingInStartDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId, machineId, orderDocumentId, beamId, operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 0;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateReachingInStartCommand = new UpdateReachingInStartDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 10,
                ReachingInStartDate = DateTimeOffset.UtcNow.AddHours(-1),
                ReachingInStartTime = new TimeSpan(7),
                ReachingInTypeInput = "Plain",
                ReachingInTypeOutput = "Lurus",
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateReachingInStartDailyOperationReachingCommand request = updateReachingInStartCommand;
            //request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- ReachingInStartTime: Start time cannot less than or equal latest time log", messageException.Message);
            }
        }
        [Fact]
        public async Task Handle_MachineStatusOnEntry_ExpectedBehavior()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateReachingInStartDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId, machineId, orderDocumentId, beamId, operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 0;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateReachingInStartCommand = new UpdateReachingInStartDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 10,
                ReachingInStartDate = DateTimeOffset.UtcNow,
                ReachingInStartTime = new TimeSpan(7),
                ReachingInTypeInput = "Plain",
                ReachingInTypeOutput = "Lurus",
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateReachingInStartDailyOperationReachingCommand request = updateReachingInStartCommand;
            request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(System.Threading.CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnEntry_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateReachingInStartDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId, machineId, orderDocumentId, beamId, operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 0;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateReachingInStartCommand = new UpdateReachingInStartDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 10,
                ReachingInStartDate = DateTimeOffset.UtcNow,
                ReachingInStartTime = new TimeSpan(7),
                ReachingInTypeInput = "Plain",
                ReachingInTypeOutput = "Lurus",
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateReachingInStartDailyOperationReachingCommand request = updateReachingInStartCommand;
            //request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Start. This operation's status not ONENTRY", messageException.Message);
            }
        }
    }
}
