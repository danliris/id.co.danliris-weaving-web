using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class UpdatePauseDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public UpdatePauseDailyOperationSizingCommandHandlerTests()
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
            this.mockRepository.VerifyAll();
        }

        private UpdatePauseDailyOperationSizingCommandHandler CreateUpdatePauseDailyOperationSizingCommandHandler()
        {
            return new UpdatePauseDailyOperationSizingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            //---ARRANGE---//
            //Instantiate Properties
            var sizingDocumentTestId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdatePauseDailyOperationSizingCommandHandler();
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderId = new OrderId(Guid.NewGuid());
            List<BeamId> beamsWarping = new List<BeamId> { new BeamId(Guid.NewGuid()) };
            var emptyWeight = 63;
            var yarnStrands = 63;
            var recipeCode = "PCA 133R";
            var neReal = 2;
            var operationStatus = OperationStatus.ONPROCESS;

            var updatePauseDailyOperationSizingCommandHandler = this.CreateUpdatePauseDailyOperationSizingCommandHandler();
            UpdatePauseDailyOperationSizingCommand request = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await updatePauseDailyOperationSizingCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
