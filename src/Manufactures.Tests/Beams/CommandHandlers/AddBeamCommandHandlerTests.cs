using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Beams.CommandHandlers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Beams.CommandHandlers
{
    public class AddBeamCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public AddBeamCommandHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockBeamRepo = mockRepository.Create<IBeamRepository>();
            mockStorage.Setup(x => x.GetRepository<IBeamRepository>())
                .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private AddBeamCommandHandler CreateAddBeamCommandHandler()
        {
            return new AddBeamCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_ValidationPassedWarping_DataCreated()
        {
            // Arrange
            var addBeamCommandHandler = this.CreateAddBeamCommandHandler();

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { });

            AddBeamCommand request = new AddBeamCommand
            {
                Number = "T122",
                Type = "Warping",
                EmptyWeight = 122
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await addBeamCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_ValidationPassedSizing_DataCreated()
        {
            // Arrange
            var addBeamCommandHandler = this.CreateAddBeamCommandHandler();

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { });

            AddBeamCommand request = new AddBeamCommand
            {
                Number = "S122",
                Type = "Sizing",
                EmptyWeight = 122
            };
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await addBeamCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_ExistingCode_ThrowError()
        {
            // Arrange
            var addBeamCommandHandler = this.CreateAddBeamCommandHandler();

            var beamDocument = new BeamDocument(Guid.NewGuid(),
                                                "S123",
                                                "Sizing",
                                                123);

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            AddBeamCommand request = new AddBeamCommand
            {
                Number = "S122",
                Type = "Sizing",
                EmptyWeight = 122
            };
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await addBeamCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Number: No. Beam Sudah Digunakan\r\n -- Type: No. Beam Dengan Tipe Sizing Sudah Digunakan", messageException.Message);
            }
        }
    }
}
