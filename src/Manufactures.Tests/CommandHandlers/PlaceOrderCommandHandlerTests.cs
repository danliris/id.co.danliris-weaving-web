using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Orders.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers
{
    public class PlaceOrderCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IManufactureOrderRepository> mockOrderRepo;

        public PlaceOrderCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockStorage.Setup(x => x.Save());

            this.mockOrderRepo = this.mockRepository.Create<IManufactureOrderRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IManufactureOrderRepository>()).Returns(mockOrderRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PlaceOrderCommandHandler CreatePlaceOrderCommandHandler()
        {
            return new PlaceOrderCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreatePlaceOrderCommandHandler();

            var unitId = new UnitDepartmentId(1);

            var yarnCodes = new YarnCodes(new List<string> { "sdfsdf", "sdfds" });

            //var construction = new GoodsComposition(identity: Guid.NewGuid(), materialIds: new MaterialIds(new List<MaterialId>()));
            var compositionId = new GoodsCompositionId(Guid.NewGuid().ToString());

            var blended = new Blended(new List<float> { 10.5f, 20.23f });

            var machineId = new MachineIdValueObject(1);

            this.mockOrderRepo.Setup(x => x.Update(It.IsAny<ManufactureOrder>())).Returns(Task.FromResult(It.IsAny<ManufactureOrder>()));

            PlaceOrderCommand request = new PlaceOrderCommand
            {
                OrderDate = DateTime.Now,
                UnitDepartmentId = unitId,
                YarnCodes = yarnCodes,
                GoodsCompositionId = compositionId,
                Blended = blended,
                MachineId = machineId,
                UserId = "Test"
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