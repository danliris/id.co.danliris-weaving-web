using ExtCore.Data.Abstractions;
using Manufactures.Application.YarnNumbers.CommandHandlers;
using Manufactures.Domain.YarnNumbers.Commands;
using Manufactures.Domain.YarnNumbers.Repositories;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Rings.CommandHandlers
{
    public class PlaceNewRingDocumentCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private Mock<IYarnNumberRepository> mockRingRepo;

        public PlaceNewRingDocumentCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockRingRepo = this.mockRepository.Create<IYarnNumberRepository>();

            //Setup
            this.mockStorage.Setup(x => x.Save());
            this.mockStorage.Setup(x => x.GetRepository<IYarnNumberRepository>()).Returns(mockRingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private AddNewYarnNumberCommandHandler CreatePlaceNewRingDocumentCommandHandler()
        {
            return new AddNewYarnNumberCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreatePlaceNewRingDocumentCommandHandler();
            AddNewYarnNumberCommand request = new AddNewYarnNumberCommand
            {
                Code = "unit-test-01",
                Number = 80,
                Description = "unit-test"
            };

            // Act
            //var result = await unitUnderTest.Handle(
            //    request,
            //    cancellationToken);

            // Assert
            await Task.Yield();
            Assert.True(false);
        }
    }
}
