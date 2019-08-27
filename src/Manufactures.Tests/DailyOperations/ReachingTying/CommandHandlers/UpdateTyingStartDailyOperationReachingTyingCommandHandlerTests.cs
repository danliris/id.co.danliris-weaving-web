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
    public class UpdateTyingStartDailyOperationReachingTyingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingTyingRepository>
            mockDailyOperationReachingTyingRepo;

        public UpdateTyingStartDailyOperationReachingTyingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockDailyOperationReachingTyingRepo = mockRepository.Create<IDailyOperationReachingTyingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingTyingRepository>())
                .Returns(mockDailyOperationReachingTyingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateTyingStartDailyOperationReachingTyingCommandHandler CreateUpdateTyingStartDailyOperationReachingTyingCommandHandler()
        {
            return new UpdateTyingStartDailyOperationReachingTyingCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            //Instantiate Properties
            //Add Existing Data
            var reachingTyingTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateTyingStartDailyOperationReachingTyingCommandHandler();
            var machineId = new MachineId(Guid.NewGuid());
            var weavingUnitId = new UnitId(11);
            var orderId = new OrderId(Guid.NewGuid());
            var beamId = new BeamId(Guid.NewGuid());
            var pisPieces = 12;
            var operationStatus = OperationStatus.ONPROCESS;
            var reachingValueObjects = new DailyOperationReachingValueObject("Plain", "Lurus", 127);

            //Assign Property to DailyOperationReachingTyingDocument
            var resultModel = new DailyOperationReachingTyingDocument(reachingTyingTestId, machineId, weavingUnitId, orderId, beamId, pisPieces, reachingValueObjects, operationStatus);

            var reachingTyingDetailTestId = Guid.NewGuid();
            var operatorDocumentId = new OperatorId(Guid.NewGuid());
            var yarnStrandsProcessed = 100;
            var dateTimeMachine = DateTimeOffset.UtcNow.AddDays(-1);
            var shiftId = new ShiftId(Guid.NewGuid());
            var machineStatus = MachineStatus.ONFINISHREACHING;

            //Assign Property to DailyOperationReachingTyingDetail
            var resultDetailModel = new DailyOperationReachingTyingDetail(reachingTyingDetailTestId, operatorDocumentId, yarnStrandsProcessed, dateTimeMachine, shiftId, machineStatus);
            resultModel.AddDailyOperationReachingTyingDetail(resultDetailModel);

            //Mocking Repository
            mockDailyOperationReachingTyingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>()))
                .Returns(new List<DailyOperationReachingTyingDocument>() { resultModel });

            //Instantiate Incoming Object Update
            var updateTyingStartCommand = new UpdateTyingStartDailyOperationReachingTyingCommand
            {
                Id = reachingTyingDetailTestId,
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                TyingStartDate = DateTimeOffset.UtcNow,
                TyingStartTime = new TimeSpan(7),
                TyingEdgeStitching = 16,
                TyingNumber = 90,
                ShiftDocumentId = new ShiftId(Guid.NewGuid())
            };

            //Update Incoming Object
            UpdateTyingStartDailyOperationReachingTyingCommand request = updateTyingStartCommand;
            request.SetId(reachingTyingTestId);

            //Set Cancellation Token
            CancellationToken cancellationToken = default(CancellationToken);

            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }
    }
}
