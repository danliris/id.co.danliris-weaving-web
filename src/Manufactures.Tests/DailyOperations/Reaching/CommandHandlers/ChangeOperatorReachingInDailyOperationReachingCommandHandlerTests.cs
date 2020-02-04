using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Command;
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
using System.Linq.Expressions;

namespace Manufactures.Tests.DailyOperations.Reaching.CommandHandlers
{
    public class ChangeOperatorReachingInDailyOperationReachingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingRepository>
            mockDailyOperationReachingRepo;
        private readonly Mock<IDailyOperationReachingHistoryRepository>
            mockDailyOperationReachingHistoryRepo;

        public ChangeOperatorReachingInDailyOperationReachingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationReachingRepo = mockRepository.Create<IDailyOperationReachingRepository>();
            mockDailyOperationReachingHistoryRepo = mockRepository.Create<IDailyOperationReachingHistoryRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingRepository>())
                .Returns(mockDailyOperationReachingRepo.Object);
            mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingHistoryRepository>())
                .Returns(mockDailyOperationReachingHistoryRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ChangeOperatorReachingInDailyOperationReachingCommandHandler CreateChangeOperatorReachingInDailyOperationReachingCommandHandler()
        {
            return new ChangeOperatorReachingInDailyOperationReachingCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONFINISH;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });


            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow,
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Change Operator. This operation's status already FINISHED", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ChangeOperatorDateLessThanLatestDate_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });


            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow.AddDays(-1),
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- ReachingChangeOperator: Change Operator date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ChangeOperatorTimeLessThanLatestTime_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });


            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow.AddHours(-1),
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- ReachingChangeOperator: Change Operator time cannot less than or equal latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartReaching_ExpectedBehavior()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });

            mockStorage.Setup(s => s.Save()).Verifiable();

            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow,
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusChangeOperatorReaching_ExpectedBehavior()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.CHANGEOPERATORREACHINGIN;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });

            mockStorage.Setup(s => s.Save()).Verifiable();

            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow,
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnStartReachingOrNotChangeOperatorReaching_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateChangeOperatorReachingInDailyOperationReachingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingInTypeInput = "Plain";
            var reachingInTypeOutput = "Lurus";
            var reachingInWidth = 0;

            //Assign Property to DailyOperationReachingDocument
            var resultModel = new DailyOperationReachingDocument(reachingTestId,
                                                                 machineId,
                                                                 orderDocumentId,
                                                                 beamId,
                                                                 reachingInTypeInput,
                                                                 reachingInTypeOutput,
                                                                 reachingInWidth,
                                                                 operationStatus);

            var reachingHistoryTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 10;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingHistory
            var resultHistoryModel = new DailyOperationReachingHistory(reachingHistoryTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus, resultModel.Identity);
            //resultModel.AddDailyOperationReachingHistory(resultHistoryModel);

            //Mocking Repository
            mockDailyOperationReachingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingDocument>() { resultModel });

            mockDailyOperationReachingHistoryRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { resultHistoryModel });


            var changeOperatorReachingCommand = new ChangeOperatorReachingInDailyOperationReachingCommand
            {
                Id = reachingHistoryTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 12,
                ChangeOperatorReachingInDate = DateTimeOffset.UtcNow,
                ChangeOperatorReachingInTime = new TimeSpan(7),
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            ChangeOperatorReachingInDailyOperationReachingCommand request = changeOperatorReachingCommand;

            //Set Cancellation Token
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            try
            {
                // Act
                var result = await unitUnderTest.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Change Operator. This operation's status not ONSTARTREACHINGIN or CHANGEOPERATORREACHINGIN", messageException.Message);
            }
        }
    }
}
