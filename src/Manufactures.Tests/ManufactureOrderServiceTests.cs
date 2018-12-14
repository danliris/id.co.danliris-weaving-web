using ExtCore.Data.Abstractions;
using FluentAssertions;
using Moonlay.ExtCore.Mvc.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manufactures.Application;
using Manufactures.Domain.Repositories;
using Manufactures.Domain;
using Manufactures.Domain.Entities;
using Manufactures.Domain.ValueObjects;
using Xunit;

namespace Manufactures.Tests
{
    public class ManufactureOrderServiceTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IManufactureOrderRepository> mockOrderRepo;
        private readonly Mock<IWorkContext> mockWorkContext;

        public ManufactureOrderServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockStorage.Setup(x => x.Save());

            this.mockOrderRepo = this.mockRepository.Create<IManufactureOrderRepository>();

            var userId = Guid.NewGuid().ToString();

            this.mockStorage.Setup(x => x.GetRepository<IManufactureOrderRepository>()).Returns(mockOrderRepo.Object);

            this.mockWorkContext = this.mockRepository.Create<IWorkContext>();

            this.mockWorkContext.Setup(x => x.CurrentUser).Returns(userId);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private IManufactureOrderService CreateService()
        {
            return new ManufactureOrderService(mockStorage.Object, mockWorkContext.Object);
        }

        [Fact]
        public async Task PlacedOrderAsync_UnitTest()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            var unitId = new UnitDepartmentId(1);

            var yarnCodes = new YarnCodes(new List<string> { "sdfsdf", "sdfds" });

            var construction = new GoodsComposition(identity: Guid.NewGuid(), materialIds: new MaterialIds(new List<MaterialId>()));

            var blended = new Blended(new List<float> { 10.5f, 20.23f });

            var machineId = new MachineId(1);

            this.mockOrderRepo.Setup(x => x.Insert(It.IsAny<ManufactureOrder>())).Returns(Task.FromResult(It.IsAny<ManufactureOrder>()));

            // Act
            var result = await unitUnderTest.PlacedOrderAsync(date: DateTime.Now, 
                unitId: unitId, 
                yarnCodes: yarnCodes, 
                construction: construction, 
                blended: blended,
                machineId: machineId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
