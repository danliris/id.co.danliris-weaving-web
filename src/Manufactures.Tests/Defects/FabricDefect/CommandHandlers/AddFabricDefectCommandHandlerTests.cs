using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Defects.FabricDefect.CommandHandlers;
using Manufactures.Domain.Defects.FabricDefect;
using Manufactures.Domain.Defects.FabricDefect.Commands;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Defects.FabricDefect.CommandHandlers
{
    public class AddFabricDefectCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IFabricDefectRepository> mockYarnDefectRepo;

        public AddFabricDefectCommandHandlerTests()
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

        private AddFabricDefectCommandHandler CreateAddYarnDefectCommandHandler()
        {
            return new AddFabricDefectCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_CodeExist_ThrowError()
        {
            // Arrange
            var addYarnDefectCommandHandler = this.CreateAddYarnDefectCommandHandler();

            //Add Existing Object
            var existingYarnDefect = new FabricDefectDocument(Guid.NewGuid(), "AM", "AMBROL", "Potong");

            //Mocking Setup
            mockYarnDefectRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<FabricDefectReadModel, bool>>>()))
                .Returns(new List<FabricDefectDocument>() { existingYarnDefect });

            //Add Yarn Defect Command Object & Instantiate It
            AddFabricDefectCommand request = new AddFabricDefectCommand
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
                .Setup(x => x.Find(It.IsAny<Expression<Func<FabricDefectReadModel, bool>>>()))
                .Returns(new List<FabricDefectDocument>() { });
            mockStorage.Setup(x => x.Save());

            //Add Yarn Defect Command Object & Instantiate It
            AddFabricDefectCommand request = new AddFabricDefectCommand
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
