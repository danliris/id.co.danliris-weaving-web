using ExtCore.Data.Abstractions;
using Manufactures.Application.Beams.CommandHandlers;
using Manufactures.Domain.Beams.Commands;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Beams.CommandHandlers
{
    public class RemoveBeamCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IStorage> mockStorage;

        public RemoveBeamCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RemoveBeamCommandHandler CreateRemoveBeamCommandHandler()
        {
            return new RemoveBeamCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var removeBeamCommandHandler = this.CreateRemoveBeamCommandHandler();
            RemoveBeamCommand request = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await removeBeamCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
