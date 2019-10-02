using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Defects.YarnDefect.CommandHandlers;
using Manufactures.Domain.Defects.YarnDefect;
using Manufactures.Domain.Defects.YarnDefect.Commands;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Defects.YarnDefect.CommandHandlers
{
    public class AddYarnDefectCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnDefectRepository> mockYarnDefectRepo;

        public AddYarnDefectCommandHandlerTests()
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

        private AddYarnDefectCommandHandler CreateAddYarnDefectCommandHandler()
        {
            return new AddYarnDefectCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_CodeExist_ThrowError()
        {
            // Arrange
            var addYarnDefectCommandHandler = this.CreateAddYarnDefectCommandHandler();

            //Add Existing Object
            var existingYarnDefect = new YarnDefectDocument(Guid.NewGuid(), "AM", "AMBROL", "Potong");

            //Mocking Setup
            mockYarnDefectRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<YarnDefectReadModel, bool>>>()))
                .Returns(new List<YarnDefectDocument>() { existingYarnDefect });

            //Add Yarn Defect Command Object & Instantiate It
            AddYarnDefectCommand request = new AddYarnDefectCommand
            {
                DefectCode = "AM",
                DefectType = "AMBROL",
                DefectCategory = "Potong"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await addYarnDefectCommandHandler.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- DefectCode: Kode Cacat Sudah Ada", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidationPassed_DataCreated()
        {
            // Arrange
            var addYarnDefectCommandHandler = this.CreateAddYarnDefectCommandHandler();

            //Mocking Setup
            mockYarnDefectRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<YarnDefectReadModel, bool>>>()))
                .Returns(new List<YarnDefectDocument>() { });
            mockStorage.Setup(x => x.Save());

            //Add Yarn Defect Command Object & Instantiate It
            AddYarnDefectCommand request = new AddYarnDefectCommand
            {
                DefectCode = "AM",
                DefectType = "AMBROL",
                DefectCategory = "Potong"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await addYarnDefectCommandHandler.Handle(
                request,
                cancellationToken);

            //---ASSERT---//
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
