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
    public class UpdateBeamCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public UpdateBeamCommandHandlerTests()
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

        private UpdateBeamCommandHandler CreateUpdateBeamCommandHandler()
        {
            return new UpdateBeamCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_ValidationPassed_DataUpdated()
        {
            // Arrange
            var updateBeamCommandHandler = this.CreateUpdateBeamCommandHandler();

            //Add Beam Document Object
            var beamDocument = new BeamDocument(Guid.NewGuid(),
                                                "S123",
                                                "Sizing",
                                                123);

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            UpdateBeamCommand request = new UpdateBeamCommand
            {
                Number = "S122",
                Type = "Sizing",
                EmptyWeight = 122
            };
            request.SetId(Guid.NewGuid());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateBeamCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().NotBeEmpty();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ExistingBeamNullResult_ThrowError()
        {
            // Arrange
            var updateBeamCommandHandler = this.CreateUpdateBeamCommandHandler();

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { });

            UpdateBeamCommand request = new UpdateBeamCommand
            {
                Number = "S122",
                Type = "Sizing",
                EmptyWeight = 122
            };
            request.SetId(Guid.NewGuid());

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateBeamCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Number: Beam not available with number " + request.Number, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_ExistingBeamCode_ThrowError()
        {
            // Arrange
            var updateBeamCommandHandler = this.CreateUpdateBeamCommandHandler();

            //Add Beam Document Object
            var beamDocument = new BeamDocument(Guid.NewGuid(),
                                                "S122",
                                                "Sizing",
                                                122);

            //Mocking Setup
            mockBeamRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            UpdateBeamCommand request = new UpdateBeamCommand
            {
                Number = "S122",
                Type = "Sizing",
                EmptyWeight = 122
            };
            request.SetId(Guid.NewGuid());

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateBeamCommandHandler.Handle(
                    request,
                    cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Number: Beam Number has available", messageException.Message);
            }
        }
    }
}
