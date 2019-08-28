using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.ReachingTying.CommandHandlers;
using Manufactures.Domain.DailyOperations.ReachingTying.Command;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.ReachingTying.CommandHandlers
{
    public class ChangeOperatorReachingDailyOperationReachingTyingCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        public ChangeOperatorReachingDailyOperationReachingTyingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ChangeOperatorReachingDailyOperationReachingTyingCommandHandler CreateChangeOperatorReachingDailyOperationReachingTyingCommandHandler()
        {
            return new ChangeOperatorReachingDailyOperationReachingTyingCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var changeOperatorReachingDailyOperationReachingTyingCommandHandler = this.CreateChangeOperatorReachingDailyOperationReachingTyingCommandHandler();
            ChangeOperatorReachingDailyOperationReachingTyingCommand request = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await changeOperatorReachingDailyOperationReachingTyingCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
