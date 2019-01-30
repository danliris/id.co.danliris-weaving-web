using ExtCore.Data.Abstractions;
using Manufactures.Application.Rings.CommandHandlers;
using Manufactures.Domain.Rings.Commands;
using Manufactures.Domain.Rings.Repositories;
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
        private Mock<IRingRepository> mockRingRepo;

        public PlaceNewRingDocumentCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockRingRepo = this.mockRepository.Create<IRingRepository>();

            //Setup
            this.mockStorage.Setup(x => x.Save());
            this.mockStorage.Setup(x => x.GetRepository<IRingRepository>()).Returns(mockRingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PlaceNewRingDocumentCommandHandler CreatePlaceNewRingDocumentCommandHandler()
        {
            return new PlaceNewRingDocumentCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreatePlaceNewRingDocumentCommandHandler();
            CreateRingDocumentCommand request = new CreateRingDocumentCommand
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
            Assert.True(false);
        }
    }
}
