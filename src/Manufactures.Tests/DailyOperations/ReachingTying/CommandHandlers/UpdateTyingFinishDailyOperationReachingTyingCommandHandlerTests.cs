using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.ReachingTying;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Manufactures.Domain.DailyOperations.ReachingTying.Entities;
using Manufactures.Domain.DailyOperations.ReachingTying.ReadModels;
using Manufactures.Domain.DailyOperations.ReachingTying.Repositories;
using Manufactures.Domain.DailyOperations.ReachingTying.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.ReachingTying.CommandHandlers
{
    public class UpdateTyingFinishDailyOperationReachingTyingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingTyingRepository>
            mockDailyOperationReachingTyingRepo;

        public UpdateTyingFinishDailyOperationReachingTyingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationReachingTyingRepo = mockRepository.Create<IDailyOperationReachingTyingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingTyingRepository>())
                .Returns(mockDailyOperationReachingTyingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateTyingFinishDailyOperationReachingTyingCommandHandler CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler()
        {
            return new UpdateTyingFinishDailyOperationReachingTyingCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_OperationStatusOnFinish_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONFINISH;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);
            var tyingValueObjects = new DailyOperationTyingValueObject(16, 90);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderDocumentId, beamId, reachingValueObjects, tyingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTTYING;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingFinishCommand = new UpdateTyingFinishDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                TyingFinishDate = DateTimeOffset.UtcNow,
                TyingFinishTime = new TimeSpan(7),
                TyingWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingFinishDailyOperationReachingTyingCommand request = updateTyingFinishCommand;
            request.SetId(reachingTyingTestId);

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
        public async Task Handle_TyingFinishDateLessThanLatestDate_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);
            var tyingValueObjects = new DailyOperationTyingValueObject(16, 90);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderDocumentId, beamId, reachingValueObjects, tyingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTTYING;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingFinishCommand = new UpdateTyingFinishDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                TyingFinishDate = DateTimeOffset.UtcNow.AddDays(-1),
                TyingFinishTime = new TimeSpan(7),
                TyingWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingFinishDailyOperationReachingTyingCommand request = updateTyingFinishCommand;
            request.SetId(reachingTyingTestId);

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
                Assert.Equal("Validation failed: \r\n -- TyingFinishDate: Finish date cannot less than latest date log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_TyingFinishTimeLessThanLatestTime_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);
            var tyingValueObjects = new DailyOperationTyingValueObject(16, 90);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderDocumentId, beamId, reachingValueObjects, tyingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow;
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTTYING;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingFinishCommand = new UpdateTyingFinishDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                TyingFinishDate = DateTimeOffset.UtcNow.AddHours(-1),
                TyingFinishTime = new TimeSpan(7),
                TyingWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingFinishDailyOperationReachingTyingCommand request = updateTyingFinishCommand;
            request.SetId(reachingTyingTestId);

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
                Assert.Equal("Validation failed: \r\n -- TyingFinishTime: Finish time cannot less than latest time log", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_MachineStatusOnStartTying_ExpectedBehavior()
        {
            // Arrange
            this.mockStorage.Setup(x => x.Save());
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);
            var tyingValueObjects = new DailyOperationTyingValueObject(16, 90);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderDocumentId, beamId, reachingValueObjects, tyingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONSTARTTYING;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingFinishCommand = new UpdateTyingFinishDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                TyingFinishDate = DateTimeOffset.UtcNow,
                TyingFinishTime = new TimeSpan(7),
                TyingWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingFinishDailyOperationReachingTyingCommand request = updateTyingFinishCommand;
            request.SetId(reachingTyingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MachineStatusNotOnStartTying_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingFinishDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderDocumentId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);
            var tyingValueObjects = new DailyOperationTyingValueObject(16, 90);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderDocumentId, beamId, reachingValueObjects, tyingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 14;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONENTRY;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingFinishCommand = new UpdateTyingFinishDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                YarnStrandsProcessed = 16,
                TyingFinishDate = DateTimeOffset.UtcNow,
                TyingFinishTime = new TimeSpan(7),
                TyingWidth = 84,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingFinishDailyOperationReachingTyingCommand request = updateTyingFinishCommand;
            request.SetId(reachingTyingTestId);

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
                Assert.Equal("Validation failed: \r\n -- OperationStatus: Can't Finish. This operation's status not ONSTARTTYING", messageException.Message);
            }
        }
    }
}
