using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Reaching.QueryHandlers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.ReachingTying.QueryHandlers
{
    public class DailyOperationReachingTyingQueryHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        public DailyOperationReachingTyingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationReachingTyingQueryHandler CreateDailyOperationReachingTyingQueryHandler()
        {
            return new DailyOperationReachingTyingQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetAll();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();
            Guid id = default(global::System.Guid);

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetById(
                id);

            // Assert
            Assert.True(false);
        }
    }
}
