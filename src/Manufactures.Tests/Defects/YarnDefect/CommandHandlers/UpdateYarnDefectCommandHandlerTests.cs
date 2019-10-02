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
    public class UpdateYarnDefectCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnDefectRepository> mockYarnDefectRepo;

        public UpdateYarnDefectCommandHandlerTests()
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

        private UpdateYarnDefectCommandHandler CreateUpdateYarnDefectCommandHandler()
        {
            return new UpdateYarnDefectCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowError()
        {
            // Arrange
            var updateYarnDefectCommandHandler = this.CreateUpdateYarnDefectCommandHandler();

            //Mocking Setup
            mockYarnDefectRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<YarnDefectReadModel>>()))
                 .Returns(new List<YarnDefectDocument>());

            //Add Yarn Defect Command Object & Instantiate It
            UpdateYarnDefectCommand request = new UpdateYarnDefectCommand
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
                Assert.Equal("Validation failed: \r\n -- Id: Invalid Yarn Defect with : " + request.Id, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ValidId_DataUpdated()
        {
            // Arrange
            var updateYarnDefectCommandHandler = this.CreateUpdateYarnDefectCommandHandler();

            //Add Existing Object
            var existingYarnDefect = new YarnDefectDocument(new Guid("1e201daf-4877-49cd-8866-42315c34f704"), "AM", "AMBROL", "Potong");

            //Mocking Setup
            mockYarnDefectRepo
                 .Setup(x => x.Find(It.IsAny<IQueryable<YarnDefectReadModel>>()))
                 .Returns(new List<YarnDefectDocument>() { existingYarnDefect });
            mockStorage.Setup(x => x.Save());

            //Add Yarn Defect Command Object & Instantiate It
            UpdateYarnDefectCommand request = new UpdateYarnDefectCommand
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
