using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Loom.CommandHandlers;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.CommandHandlers
{
    public class PreparationDailyOperationLoomCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        public PreparationDailyOperationLoomCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
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
            var preparationDailyOperationLoomCommandHandler = this.CreatePreparationDailyOperationLoomCommandHandler();
            PreparationDailyOperationLoomCommand request = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await preparationDailyOperationLoomCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
