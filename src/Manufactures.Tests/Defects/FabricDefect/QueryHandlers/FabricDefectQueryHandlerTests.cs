using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Defects.FabricDefect.QueryHandlers;
using Manufactures.Domain.Defects.FabricDefect;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Defects.FabricDefect.QueryHandlers
{
    public class FabricDefectQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IFabricDefectRepository> mockYarnDefectRepo;

        public FabricDefectQueryHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockYarnDefectRepo = mockRepository.Create<IFabricDefectRepository>();
            mockStorage.Setup(x => x.GetRepository<IFabricDefectRepository>())
                .Returns(mockYarnDefectRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FabricDefectQueryHandler CreateYarnDefectQueryHandler()
        {
            return new FabricDefectQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var yarnDefectQueryHandler = this.CreateYarnDefectQueryHandler();

            //Instantiate Existing Object
            var firstYarnDefect = new FabricDefectDocument(Guid.NewGuid(), "AM", "Ambrol", "POTONG");
            var secondYarnDefect = new FabricDefectDocument(Guid.NewGuid(), "PKD", "Pakan Double", "POTONG");

            mockYarnDefectRepo.Setup(x => x.Query).Returns(new List<FabricDefectReadModel>().AsQueryable());
            mockYarnDefectRepo.Setup(x => x.Find(It.IsAny<IQueryable<FabricDefectReadModel>>())).Returns(
                new List<FabricDefectDocument>() { firstYarnDefect, secondYarnDefect });

            // Act
            var result = await yarnDefectQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var yarnDefectQueryHandler = this.CreateYarnDefectQueryHandler();

            //Instantiate Existing Object
            var firstYarnDefect = new FabricDefectDocument(Guid.NewGuid(), "AM", "Ambrol", "POTONG");
            var secondYarnDefect = new FabricDefectDocument(Guid.NewGuid(), "PKD", "Pakan Double", "POTONG");

            mockYarnDefectRepo.Setup(x => x.Query).Returns(new List<FabricDefectReadModel>().AsQueryable());
            mockYarnDefectRepo.Setup(x => x.Find(It.IsAny<IQueryable<FabricDefectReadModel>>())).Returns(
                new List<FabricDefectDocument>() { firstYarnDefect, secondYarnDefect });

            // Act
            var result = await yarnDefectQueryHandler.GetById(firstYarnDefect.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
