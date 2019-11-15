using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Reaching.CommandHandlers
{
    public class UpdateCombFinishDailyOperationReachingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingRepository>
            mockDailyOperationReachingRepo;

        public UpdateCombFinishDailyOperationReachingCommandHandlerTests()
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

        private UpdateCombFinishDailyOperationReachingCommandHandler CreateUpdateCombFinishDailyOperationReachingCommandHandler()
        {
            return new UpdateCombFinishDailyOperationReachingCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateCombFinishDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONFINISH;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 127;
            var combEdgeStitching = 16;
            var combNumber = 90;
            var combWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 combEdgeStitching,
                                                                 combNumber,
                                                                 combWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTCOMB;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateCombFinishCommand = new UpdateCombFinishDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                CombFinishDate = DateTimeOffset.UtcNow,
                CombFinishTime = new TimeSpan(7),
                CombWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateCombFinishDailyOperationReachingCommand request = updateCombFinishCommand;
            request.SetId(reachingTestId);

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Finish. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_CombFinishDateLessThanLatestDate_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateCombFinishDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 127;
            var combEdgeStitching = 16;
            var combNumber = 90;
            var combWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 combEdgeStitching,
                                                                 combNumber,
                                                                 combWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTCOMB;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateCombFinishCommand = new UpdateCombFinishDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                CombFinishDate = DateTimeOffset.UtcNow.AddDays(-1),
                CombFinishTime = new TimeSpan(7),
                CombWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateCombFinishDailyOperationReachingCommand request = updateCombFinishCommand;
            request.SetId(reachingTestId);

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
                Assert.Equal("Validation failed: \r\n -- CombFinishDate: Finish date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_CombFinishTimeLessThanLatestTime_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateCombFinishDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 127;
            var combEdgeStitching = 16;
            var combNumber = 90;
            var combWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 combEdgeStitching,
                                                                 combNumber,
                                                                 combWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTCOMB;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateCombFinishCommand = new UpdateCombFinishDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                CombFinishDate = DateTimeOffset.UtcNow.AddHours(-1),
                CombFinishTime = new TimeSpan(7),
                CombWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateCombFinishDailyOperationReachingCommand request = updateCombFinishCommand;
            request.SetId(reachingTestId);

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
                Assert.Equal("Validation failed: \r\n -- CombFinishTime: Finish time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartComb_ExpectedBehavior()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateCombFinishDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 127;
            var combEdgeStitching = 16;
            var combNumber = 90;
            var combWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 combEdgeStitching,
                                                                 combNumber,
                                                                 combWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTCOMB;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateCombFinishCommand = new UpdateCombFinishDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                CombFinishDate = DateTimeOffset.UtcNow,
                CombFinishTime = new TimeSpan(7),
                CombWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateCombFinishDailyOperationReachingCommand request = updateCombFinishCommand;
            request.SetId(reachingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnStartComb_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateCombFinishDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 127;
            var combEdgeStitching = 16;
            var combNumber = 90;
            var combWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 combEdgeStitching,
                                                                 combNumber,
                                                                 combWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
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
            var updateCombFinishCommand = new UpdateCombFinishDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                CombFinishDate = DateTimeOffset.UtcNow,
                CombFinishTime = new TimeSpan(7),
                CombWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateCombFinishDailyOperationReachingCommand request = updateCombFinishCommand;
            request.SetId(reachingTestId);

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Finish. This operation's status not ONSTARTCOMB", messageException.Message);
            }
        }
    }
}
