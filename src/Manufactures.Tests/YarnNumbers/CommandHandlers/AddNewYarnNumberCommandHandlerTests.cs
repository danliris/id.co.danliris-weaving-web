using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.YarnNumbers.CommandHandlers;
using Manufactures.Domain.YarnNumbers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.ReadModels;
using Manufactures.Domain.YarnNumbers.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.YarnNumbers.CommandHandlers
{
    public class AddNewYarnNumberCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnNumberRepository> mockRingRepo;

        public AddNewYarnNumberCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockRingRepo = 
                this.mockRepository.Create<IYarnNumberRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IYarnNumberRepository>())
                .Returns(mockRingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private AddNewYarnNumberCommandHandler CreatePlaceNewRingDocumentCommandHandler()
        {
            return new AddNewYarnNumberCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreatePlaceNewRingDocumentCommandHandler();

            this.mockRingRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<YarnNumberDocumentReadModel, bool>>>()))
                .Returns(new List<YarnNumberDocument>());

            this.mockRingRepo
                .Setup(x => x.Update(It.IsAny<YarnNumberDocument>()))
                .Returns(Task.FromResult(It.IsAny<YarnNumberDocument>()));

            AddNewYarnNumberCommand request = new AddNewYarnNumberCommand
            {
                Code = "unit-test-01",
                Number = 80,
                Description = "unit-test"
            };

            CancellationToken cancellationToken = CancellationToken.None;

            //Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
