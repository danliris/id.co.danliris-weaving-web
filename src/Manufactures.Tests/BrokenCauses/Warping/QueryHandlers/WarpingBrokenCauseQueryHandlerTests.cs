using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.BrokenCauses.Warping.QueryHandlers;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.BrokenCauses.Warping.QueryHandlers
{
    public class WarpingBrokenCauseQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IWarpingBrokenCauseRepository> mockWarpingBrokenCauseRepo;

        public WarpingBrokenCauseQueryHandlerTests()
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

        private WarpingBrokenCauseQueryHandler CreateWarpingBrokenCauseQueryHandler()
        {
            return new WarpingBrokenCauseQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var warpingBrokenCauseQueryHandler = this.CreateWarpingBrokenCauseQueryHandler();

            //Instantiate Existing Object
            //Add Existing Warping Broken Cause Object
            var firstWarpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(), "Benang Tipis", "Test 1", false);
            var secondWarpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(), "Untwist", "Test 2", true);

            mockWarpingBrokenCauseRepo.Setup(x => x.Query).Returns(new List<WarpingBrokenCauseReadModel>()
                .AsQueryable());
            mockWarpingBrokenCauseRepo.Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { firstWarpingBrokenCause, secondWarpingBrokenCause });

            // Act
            var result = await warpingBrokenCauseQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var warpingBrokenCauseQueryHandler = this.CreateWarpingBrokenCauseQueryHandler();

            //Instantiate Existing Object
            //Add Existing Warping Broken Cause Object
            var warpingBrokenCause = new WarpingBrokenCauseDocument(Guid.NewGuid(), "Benang Tipis", "Test 1", false);

            mockWarpingBrokenCauseRepo.Setup(x => x.Query).Returns(new List<WarpingBrokenCauseReadModel>()
                .AsQueryable());
            mockWarpingBrokenCauseRepo.Setup(x => x.Find(It.IsAny<IQueryable<WarpingBrokenCauseReadModel>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { warpingBrokenCause });

            // Act
            var result = await warpingBrokenCauseQueryHandler.GetById(warpingBrokenCause.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
