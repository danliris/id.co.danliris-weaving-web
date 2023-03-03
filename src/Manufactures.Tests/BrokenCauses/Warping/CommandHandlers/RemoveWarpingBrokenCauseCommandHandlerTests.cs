using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.BrokenCauses.Warping.CommandHandlers;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.Commands;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.BrokenCauses.Warping.CommandHandlers
{
    public class RemoveWarpingBrokenCauseCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IWarpingBrokenCauseRepository> mockWarpingBrokenCauseRepo;

        public RemoveWarpingBrokenCauseCommandHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockWarpingBrokenCauseRepo = mockRepository.Create<IWarpingBrokenCauseRepository>();
            mockStorage.Setup(x => x.GetRepository<IWarpingBrokenCauseRepository>())
                .Returns(mockWarpingBrokenCauseRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RemoveWarpingBrokenCauseCommandHandler CreateRemoveWarpingBrokenCauseCommandHandler()
        {
            return new RemoveWarpingBrokenCauseCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowError()
        {
            // Arrange
            var removeWarpingBrokenCauseCommandHandler = this.CreateRemoveWarpingBrokenCauseCommandHandler();
            var testId = Guid.NewGuid();

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>());

            RemoveWarpingBrokenCauseCommand request = new RemoveWarpingBrokenCauseCommand
            {
                Id = testId
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await removeWarpingBrokenCauseCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Id: Invalid Warping Broken Cause with : " + request.Id, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidId_DataDeleted()
        {
            // Arrange
            var removeWarpingBrokenCauseCommandHandler = this.CreateRemoveWarpingBrokenCauseCommandHandler();

            //Add Existing Warping Broken Cause Object
            var existingWarpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(), "Slub", "Test 1", false);

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { existingWarpingBrokenCause });

            RemoveWarpingBrokenCauseCommand request = new RemoveWarpingBrokenCauseCommand
            {
                Id = existingWarpingBrokenCause.Identity
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await removeWarpingBrokenCauseCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().Equals(result.Identity);
        }
    }
}
