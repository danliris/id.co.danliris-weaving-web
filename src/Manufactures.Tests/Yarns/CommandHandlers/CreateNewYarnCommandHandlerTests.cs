using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Yarns.CommandHandlers;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.ReadModels;
using Manufactures.Domain.Yarns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Yarns.CommandHandlers
{
    public class CreateNewYarnCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnDocumentRepository> mockYarnRepo;

        public CreateNewYarnCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockYarnRepo =
                this.mockRepository.Create<IYarnDocumentRepository>();

            this.mockStorage
                .Setup(x => x.GetRepository<IYarnDocumentRepository>())
                .Returns(mockYarnRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private CreateNewYarnCommandHandler CreateCreateNewYarnCommandHandler()
        {
            return new CreateNewYarnCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            this.mockYarnRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<YarnDocumentReadModel, bool>>>()))
                .Returns(new List<YarnDocument>());

            this.mockYarnRepo
                .Setup(x => x.Update(It.IsAny<YarnDocument>()))
                .Returns(Task.FromResult(It.IsAny<YarnDocument>()));

            //Arrange
            var unitUnderTest = this.CreateCreateNewYarnCommandHandler();
            CreateNewYarnCommand request = new CreateNewYarnCommand()
            {
                Code = "0001",
                Name = "Test Yarn",
                MaterialTypeId = "a3b59953-b4ea-4d9e-b7f7-8265170d6d2d",
                Tags = "Weaving Unit",
                YarnNumberId = "9fea181a-8b5d-421d-9367-540e730b4650"
            };

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
