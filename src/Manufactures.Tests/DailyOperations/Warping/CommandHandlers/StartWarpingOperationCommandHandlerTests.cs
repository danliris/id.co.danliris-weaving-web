using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Warping.CommandHandlers;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.CommandHandlers
{
    public class StartWarpingOperationCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockWarpingOperationRepo;

        public StartWarpingOperationCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockWarpingOperationRepo =
                this.mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IDailyOperationWarpingRepository>())
                .Returns(mockWarpingOperationRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PreparationWarpingOperationCommandHandler CreateAddNewWarpingOperationCommandHandler()
        {
            return new PreparationWarpingOperationCommandHandler(this.mockStorage.Object);
        }

        private StartWarpingOperationCommandHandler CreateStartWarpingOperationCommandHandler()
        {
            return new StartWarpingOperationCommandHandler(this.mockStorage.Object);
        }

        /**
        * Test for create new start on daily
        * operation warping
        * **/
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Set preparation command handler object
            var preparationWarpingOperationCommandHandler =
                this.CreateAddNewWarpingOperationCommandHandler();

            //Instantiate new Object
            var constructionId = new ConstructionId(Guid.NewGuid());
            var materialTypeId = new MaterialTypeId(Guid.NewGuid());
            var operatorId = new OperatorId(Guid.NewGuid());

            //Create new preparation object
            PreparationWarpingOperationCommand preparationRequest =
                new PreparationWarpingOperationCommand
                {
                    ConstructionId = constructionId,
                    MaterialTypeId = materialTypeId,
                    AmountOfCones = 10,
                    ColourOfCone = "Red",
                    DateOperation = DateTimeOffset.UtcNow,
                    TimeOperation = "01:00",
                    OperatorId = operatorId
                };

            //Set Cancellation Token
            CancellationToken PreparationcancellationToken = CancellationToken.None;

            // Instantiate command handler
            var preparationResult =
                await preparationWarpingOperationCommandHandler
                    .Handle(preparationRequest, PreparationcancellationToken);

            //Check if object not null
            preparationResult.Should().NotBeNull();

            //Check if has identity
            preparationResult.Identity.Should().NotBeEmpty();

            //check if has history
            preparationResult.DailyOperationWarpingDetailHistory.Should().NotBeEmpty();


            // Set start command handler object
            var startWarpingOperationCommandHandler = this.CreateStartWarpingOperationCommandHandler();

            //Create new start object
            StartWarpingOperationCommand startRequest =
                new StartWarpingOperationCommand
                {
                    Id  = preparationResult.Identity

                };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result =
                await startWarpingOperationCommandHandler
                    .Handle(startRequest, cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
