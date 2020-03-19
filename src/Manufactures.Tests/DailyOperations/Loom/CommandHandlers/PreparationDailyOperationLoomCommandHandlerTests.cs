using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Loom.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.CommandHandlers
{
    public class PreparationDailyOperationLoomCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationLoomRepository>
            mockLoomOperationRepo;
        private readonly Mock<IDailyOperationLoomBeamProductRepository>
           mockLoomOperationProductRepo;
        private readonly Mock<IDailyOperationLoomHistoryRepository>
            mockLoomOperationHistoryRepo;

        public PreparationDailyOperationLoomCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockLoomOperationRepo =
                this.mockRepository.Create<IDailyOperationLoomRepository>();
            mockLoomOperationHistoryRepo = mockRepository.Create<IDailyOperationLoomHistoryRepository>();
            mockLoomOperationProductRepo = mockRepository.Create<IDailyOperationLoomBeamProductRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomRepository>())
                .Returns(mockLoomOperationRepo.Object);
            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomHistoryRepository>())
                .Returns(mockLoomOperationHistoryRepo.Object);

            mockStorage
                .Setup(x => x.GetRepository<IDailyOperationLoomBeamProductRepository>())
                .Returns(mockLoomOperationProductRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PreparationDailyOperationLoomCommandHandler CreatePreparationDailyOperationLoomCommandHandler()
        {
            return new PreparationDailyOperationLoomCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            // Set Preparation Command Handler Object
            var preparationDailyOperationLoomCommandHandler = this.CreatePreparationDailyOperationLoomCommandHandler();

            //Mocking Preparation Beam Product Object
            List<PreparationDailyOperationLoomBeamUsedCommand> loomBeamProducts = new List<PreparationDailyOperationLoomBeamUsedCommand>();
            PreparationDailyOperationLoomBeamUsedCommand loomBeamProduct = new PreparationDailyOperationLoomBeamUsedCommand
            {
                BeamOrigin = "Reaching",
                BeamDocumentId = new BeamId(Guid.NewGuid()),
                CombNumber = 123,
                MachineDocumentId = new MachineId(Guid.NewGuid()),
                DateBeamProduct = DateTimeOffset.UtcNow,
                TimeBeamProduct = TimeSpan.Parse("07:00"),
                LoomProcess = BeamStatus.ONPROCESS
            };
            loomBeamProducts.Add(loomBeamProduct);

            //Mocking Preparation History Object
            List<PreparationDailyOperationLoomHistoryCommand> loomBeamHistories = new List<PreparationDailyOperationLoomHistoryCommand>();
            PreparationDailyOperationLoomHistoryCommand loomBeamHistory = new PreparationDailyOperationLoomHistoryCommand
            {
                BeamNumber = "S11",
                MachineNumber = "111",
                OperatorDocumentId = new OperatorId(Guid.NewGuid()),
                DateMachine = DateTimeOffset.UtcNow,
                TimeMachine = TimeSpan.Parse("07:00"),
                ShiftDocumentId = new ShiftId(Guid.NewGuid()),
                Information = "-"
            };
            loomBeamHistories.Add(loomBeamHistory);

            //Instantiate Object for New Preparation Object (Commands)
            PreparationDailyOperationLoomCommand request = new PreparationDailyOperationLoomCommand
            {
                OrderDocumentId = new OrderId(Guid.NewGuid()),
                LoomBeamProducts = loomBeamProducts,
                LoomBeamHistories = loomBeamHistories
            };

            //Setup Mock Object for Loom Repository
            //mockLoomOperationRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationLoomReadModel, bool>>>()))
            //    .Returns(new List<DailyOperationLoomDocument>());
            this.mockStorage.Setup(x => x.Save());

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await preparationDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            //Check if object not null
            result.Should().NotBeNull();

            //Check if has identity
            result.Identity.Should().NotBeEmpty();
        }
    }
}
