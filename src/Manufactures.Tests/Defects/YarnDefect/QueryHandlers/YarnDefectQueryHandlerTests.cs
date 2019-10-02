using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Defects.YarnDefect.QueryHandlers;
using Manufactures.Domain.Defects.YarnDefect;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Defects.YarnDefect.QueryHandlers
{
    public class YarnDefectQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnDefectRepository> mockYarnDefectRepo;

        public YarnDefectQueryHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockYarnDefectRepo = mockRepository.Create<IYarnDefectRepository>();
            mockStorage.Setup(x => x.GetRepository<IYarnDefectRepository>())
                .Returns(mockYarnDefectRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private YarnDefectQueryHandler CreateYarnDefectQueryHandler()
        {
            return new YarnDefectQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var yarnDefectQueryHandler = this.CreateYarnDefectQueryHandler();

            //Instantiate Existing Object
            var firstYarnDefect = new YarnDefectDocument(Guid.NewGuid(), "AM", "Ambrol", "POTONG");
            var secondYarnDefect = new YarnDefectDocument(Guid.NewGuid(), "PKD", "Pakan Double", "POTONG");

            mockYarnDefectRepo.Setup(x => x.Query).Returns(new List<YarnDefectReadModel>().AsQueryable());
            mockYarnDefectRepo.Setup(x => x.Find(It.IsAny<IQueryable<YarnDefectReadModel>>())).Returns(
                new List<YarnDefectDocument>() { firstYarnDefect, secondYarnDefect });

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
            var firstYarnDefect = new YarnDefectDocument(Guid.NewGuid(), "AM", "Ambrol", "POTONG");
            var secondYarnDefect = new YarnDefectDocument(Guid.NewGuid(), "PKD", "Pakan Double", "POTONG");

            mockYarnDefectRepo.Setup(x => x.Query).Returns(new List<YarnDefectReadModel>().AsQueryable());
            mockYarnDefectRepo.Setup(x => x.Find(It.IsAny<IQueryable<YarnDefectReadModel>>())).Returns(
                new List<YarnDefectDocument>() { firstYarnDefect, secondYarnDefect });

            // Act
            var result = await yarnDefectQueryHandler.GetById(firstYarnDefect.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
