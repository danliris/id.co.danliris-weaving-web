using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weaving.Application;
using Weaving.Domain.Entities;
using Weaving.Domain.ValueObjects;
using Xunit;

namespace Weaving.Tests
{
    public class ManufactureOrderServiceTests : IDisposable
    {
        private readonly MockRepository mockRepository;



        public ManufactureOrderServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private IManufactureOrderService CreateService()
        {
            return new ManufactureOrderService();
        }

        [Fact]
        public async Task PlacedOrderAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();

            var unitId = new UnitDepartmentId(1);

            var yarnCodes = new YarnCodes(new List<string> { "sdfsdf", "sdfds" });

            var construction = new GoodsConstruction(identity: Guid.NewGuid(), codes: new List<string>());

            var blended = new Blended(new List<float> { 10.5f, 20.23f });

            var machineId = new MachineId(1);

            // Act
            var result = await unitUnderTest.PlacedOrderAsync(date: DateTime.Now, 
                unitId: unitId, 
                yarnCodes: yarnCodes, 
                construction: construction, 
                blended: blended,
                machineId: machineId);

            // Assert
            Assert.True(false);
        }
    }
}
