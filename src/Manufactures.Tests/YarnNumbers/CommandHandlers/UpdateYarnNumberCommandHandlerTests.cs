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
    public class UpdateYarnNumberCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnNumberRepository> mockRingRepo;

        public UpdateYarnNumberCommandHandlerTests()
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

        private UpdateYarnNumberCommandHandler CreateUpdateYarnNumberCommandHandler()
        {
            return new UpdateYarnNumberCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateYarnNumberCommandHandler();

            this.mockRingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<YarnNumberDocumentReadModel, bool>>>()))
                 .Returns(new List<YarnNumberDocument>() { new YarnNumberDocument(testId, "0000", "90", "test-type", "this is test") });

            this.mockRingRepo
                .Setup(x => x.Update(It.IsAny<YarnNumberDocument>()))
                .Returns(Task.FromResult(It.IsAny<YarnNumberDocument>()));

            UpdateYarnNumberCommand request = new UpdateYarnNumberCommand()
            {
                Code = "0001",
                Number = "100",
                Description = "Change value of code and Number"
            };
            request.SetId(testId);

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Code.Should().Equals("0001");
            result.Number.Should().Equals(100);
        }
    }
}
