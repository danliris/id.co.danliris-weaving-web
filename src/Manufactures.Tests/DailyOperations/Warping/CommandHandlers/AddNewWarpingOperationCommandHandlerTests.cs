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
    public class AddNewWarpingOperationCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationWarpingRepository> 
            mockWarpingOperationRepo;


        public AddNewWarpingOperationCommandHandlerTests()
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

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateAddNewWarpingOperationCommandHandler();

            var constructionId = new ConstructionId(Guid.NewGuid());
            var materialTypeId = new MaterialTypeId(Guid.NewGuid());
            var operatorId = new OperatorId(Guid.NewGuid());

            PreparationWarpingOperationCommand request = new PreparationWarpingOperationCommand
            {
                ConstructionId = constructionId,
                MaterialTypeId = materialTypeId,
                AmountOfCones = 10,
                ColourOfCone = "Red",
                DateOperation = DateTimeOffset.UtcNow,
                TimeOperation = "01:00",
                OperatorId = operatorId
            };

            CancellationToken cancellationToken = CancellationToken.None;
            // Assert
            var result = await unitUnderTest.Handle(request, cancellationToken);
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
