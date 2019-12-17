using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Materials.CommandHandlers;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Materials.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Materials.CommandHandlers
{
    public class UpdateMaterialTypeCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IMaterialTypeRepository> mockMaterialTypeRepo;

        public UpdateMaterialTypeCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockMaterialTypeRepo =
                this.mockRepository.Create<IMaterialTypeRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IMaterialTypeRepository>())
                .Returns(mockMaterialTypeRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateMaterialTypeCommandHandler CreateUpdateMaterialTypeCommandHandler()
        {
            return new UpdateMaterialTypeCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_ValidationPassed_DataUpdated()
        {
            // Arrange
            // Mocking Object
            var updateMaterialTypeCommandHandler = this.CreateUpdateMaterialTypeCommandHandler();

            MaterialTypeDocument materialTypeDocument = new MaterialTypeDocument(Guid.NewGuid(), "C", "Cotton", "-");

            List<YarnNumberValueObject> yarnNumberValueObjects = new List<YarnNumberValueObject>();
            YarnNumberValueObject yarnNumberValueObject = new YarnNumberValueObject(Guid.NewGuid(),
                                                                                    "PC",
                                                                                    "11",
                                                                                    null,
                                                                                    "0");
            yarnNumberValueObjects.Add(yarnNumberValueObject);

            UpdateMaterialTypeCommand request = new UpdateMaterialTypeCommand
            {
                Code = "PC",
                Name = "PolyCotton",
                RingDocuments = yarnNumberValueObjects,
                Description = "-"
            };
            request.SetId(materialTypeDocument.Identity);

            this.mockMaterialTypeRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<MaterialTypeReadModel, bool>>>()))
                 .Returns(new List<MaterialTypeDocument>() { materialTypeDocument });

            this.mockMaterialTypeRepo
                .Setup(x => x.Update(It.IsAny<MaterialTypeDocument>()))
                .Returns(Task.FromResult(It.IsAny<MaterialTypeDocument>()));

            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await updateMaterialTypeCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            //Check if has identity
            result.Identity.Should().NotBeEmpty();
            //Check result null
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_MaterialTypeNull_ThrowError()
        {
            // Arrange
            // Mocking Object
            var updateMaterialTypeCommandHandler = this.CreateUpdateMaterialTypeCommandHandler();

            MaterialTypeDocument materialTypeDocument = new MaterialTypeDocument(Guid.NewGuid(), "C", "Cotton", "-");
            MaterialTypeDocument materialTypeDocumentNull = null;

            List<YarnNumberValueObject> yarnNumberValueObjects = new List<YarnNumberValueObject>();
            YarnNumberValueObject yarnNumberValueObject = new YarnNumberValueObject(Guid.NewGuid(),
                                                                                    "C",
                                                                                    "11",
                                                                                    null,
                                                                                    "0");
            yarnNumberValueObjects.Add(yarnNumberValueObject);

            UpdateMaterialTypeCommand request = new UpdateMaterialTypeCommand
            {
                Code = "PC",
                Name = "PolyCotton",
                RingDocuments = yarnNumberValueObjects,
                Description = "-"
            };
            request.SetId(materialTypeDocument.Identity);

            this.mockMaterialTypeRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<MaterialTypeReadModel, bool>>>()))
                 .Returns(new List<MaterialTypeDocument>() { materialTypeDocumentNull });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateMaterialTypeCommandHandler.Handle(
                request,
                cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Id: Invalid Order: " + request.Id, messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_SameMaterialCodeExist_ThrowError()
        {
            // Arrange
            // Mocking Object
            var updateMaterialTypeCommandHandler = this.CreateUpdateMaterialTypeCommandHandler();

            MaterialTypeDocument materialTypeDocument = new MaterialTypeDocument(Guid.NewGuid(), "PC", "PolyCotton", "-");

            List<YarnNumberValueObject> yarnNumberValueObjects = new List<YarnNumberValueObject>();
            YarnNumberValueObject yarnNumberValueObject = new YarnNumberValueObject(Guid.NewGuid(),
                                                                                    "PC",
                                                                                    "11",
                                                                                    null,
                                                                                    "0");
            yarnNumberValueObjects.Add(yarnNumberValueObject);

            UpdateMaterialTypeCommand request = new UpdateMaterialTypeCommand
            {
                Code = "PC",
                Name = "PolyCotton",
                RingDocuments = yarnNumberValueObjects,
                Description = "-"
            };
            request.SetId(materialTypeDocument.Identity);

            this.mockMaterialTypeRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<MaterialTypeReadModel, bool>>>()))
                 .Returns(new List<MaterialTypeDocument>() { materialTypeDocument });

            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await updateMaterialTypeCommandHandler.Handle(
                request,
                cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- Code: Code with " + request.Code + " has available", messageException.Message);
            }
        }
    }
}
