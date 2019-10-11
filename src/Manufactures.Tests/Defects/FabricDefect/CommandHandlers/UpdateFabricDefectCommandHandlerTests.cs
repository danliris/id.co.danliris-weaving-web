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
    public class UpdateFabricDefectCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IFabricDefectRepository> mockFabricDefectRepo;

        public UpdateFabricDefectCommandHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockFabricDefectRepo = mockRepository.Create<IFabricDefectRepository>();
            mockStorage.Setup(x => x.GetRepository<IFabricDefectRepository>())
                .Returns(mockFabricDefectRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateFabricDefectCommandHandler CreateUpdateYarnDefectCommandHandler()
        {
            return new UpdateFabricDefectCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowError()
        {
            // Arrange
            var updateYarnDefectCommandHandler = this.CreateUpdateYarnDefectCommandHandler();

            //Mocking Setup
            mockFabricDefectRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<FabricDefectReadModel>>()))
                 .Returns(new List<FabricDefectDocument>());

            //Add Yarn Defect Command Object & Instantiate It
            UpdateFabricDefectCommand request = new UpdateFabricDefectCommand
            {
                Id = new Guid("5cef3e62-80b0-4e31-b7f3-f6b9d333ff84"),
                DefectCode = "AM",
                DefectType = "AMBROL",
                DefectCategory = "Potong"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateYarnDefectCommandHandler.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Id: Invalid Fabric Defect with : " + request.Id, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidId_DataUpdated()
        {
            // Arrange
            var updateYarnDefectCommandHandler = this.CreateUpdateYarnDefectCommandHandler();

            //Add Existing Object
            var existingYarnDefect = new FabricDefectDocument(new Guid("1e201daf-4877-49cd-8866-42315c34f704"), "AM", "AMBROL", "Potong");

            //Mocking Setup
            mockFabricDefectRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<FabricDefectReadModel>>()))
                 .Returns(new List<FabricDefectDocument>() { existingYarnDefect });
            mockStorage.Setup(x => x.Save());

            //Add Yarn Defect Command Object & Instantiate It
            UpdateFabricDefectCommand request = new UpdateFabricDefectCommand
            {
                Id = new Guid("1e201daf-4877-49cd-8866-42315c34f704"),
                DefectCode = "AM",
                DefectType = "AMBROL",
                DefectCategory = "Potong"
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateYarnDefectCommandHandler.Handle(request, cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }
    }
}
