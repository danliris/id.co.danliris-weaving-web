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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.BrokenCauses.Warping.CommandHandlers
{
    public class UpdateWarpingBrokenCauseCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IWarpingBrokenCauseRepository> mockWarpingBrokenCauseRepo;

        public UpdateWarpingBrokenCauseCommandHandlerTests()
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

        private UpdateWarpingBrokenCauseCommandHandler CreateUpdateWarpingBrokenCauseCommandHandler()
        {
            return new UpdateWarpingBrokenCauseCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowError()
        {
            // Arrange
            var updateWarpingBrokenCauseCommandHandler = this.CreateUpdateWarpingBrokenCauseCommandHandler();
            var testId = Guid.NewGuid();

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>());

            UpdateWarpingBrokenCauseCommand request = new UpdateWarpingBrokenCauseCommand
            {
                Id = testId,
                WarpingBrokenCauseName = "Slub",
                Information = "Test 1",
                WarpingBrokenCauseCategory = "Umum"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateWarpingBrokenCauseCommandHandler.Handle(
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
        public async Task Handle_ValidId_DataUpdated()
        {
            // Arrange
            var updateWarpingBrokenCauseCommandHandler = this.CreateUpdateWarpingBrokenCauseCommandHandler();
            var testId = Guid.NewGuid();

            //Add Existing Warping Broken Cause Object
            var existingWarpingBrokenCause = new WarpingBrokenCauseDocument(testId, "Slub", "Test 1", false);

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { existingWarpingBrokenCause });

            UpdateWarpingBrokenCauseCommand request = new UpdateWarpingBrokenCauseCommand
            {
                Id = testId,
                WarpingBrokenCauseName = "Benang Tipis",
                Information = "Test 2",
                WarpingBrokenCauseCategory = "Umum"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateWarpingBrokenCauseCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }
    }
}
