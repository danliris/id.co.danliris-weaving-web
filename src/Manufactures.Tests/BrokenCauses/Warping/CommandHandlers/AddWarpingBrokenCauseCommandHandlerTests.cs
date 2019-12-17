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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.BrokenCauses.Warping.CommandHandlers
{
    public class AddWarpingBrokenCauseCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IWarpingBrokenCauseRepository> mockWarpingBrokenCauseRepo;

        public AddWarpingBrokenCauseCommandHandlerTests()
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

        private AddWarpingBrokenCauseCommandHandler CreateAddWarpingBrokenCauseCommandHandler()
        {
            return new AddWarpingBrokenCauseCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_SameWarpingBrokenCauseName_ThrowError()
        {
            // Arrange
            var addWarpingBrokenCauseCommandHandler = this.CreateAddWarpingBrokenCauseCommandHandler();

            //Add Existing Warping Broken Cause Object
            var existingWarpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(), "Slub", "Test 1", false);

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<WarpingBrokenCauseReadModel, bool>>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { existingWarpingBrokenCause });

            AddWarpingBrokenCauseCommand request = new AddWarpingBrokenCauseCommand
            {
                WarpingBrokenCauseName = "Slub",
                Information = "Test 1",
                WarpingBrokenCauseCategory = "Umum"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await addWarpingBrokenCauseCommandHandler.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- WarpingBrokenCauseName: Nama Penyebab Putus Sudah Ada", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidationPassed_DataCreated()
        {
            // Arrange
            var addWarpingBrokenCauseCommandHandler = this.CreateAddWarpingBrokenCauseCommandHandler();

            //Mocking Setup
            mockWarpingBrokenCauseRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<WarpingBrokenCauseReadModel, bool>>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { });

            AddWarpingBrokenCauseCommand request = new AddWarpingBrokenCauseCommand
            {
                WarpingBrokenCauseName = "Slub",
                Information = "Test 1",
                WarpingBrokenCauseCategory = "Umum"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await addWarpingBrokenCauseCommandHandler.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
