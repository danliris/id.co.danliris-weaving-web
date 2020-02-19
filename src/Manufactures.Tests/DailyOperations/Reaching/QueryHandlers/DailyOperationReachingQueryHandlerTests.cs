using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.QueryHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.ReadModels;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Domain.Suppliers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Reaching.QueryHandlers
{
    public class DailyOperationReachingQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;
        private readonly Mock<IDailyOperationReachingRepository>
            mockDailyOperationReachingTyingRepo;
        private readonly Mock<IMachineRepository>
            mockMachineRepo;
        private readonly Mock<IOperatorRepository>
            mockOperatorRepo;
        private readonly Mock<IShiftRepository>
            mockShiftRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IOrderRepository>
            mockOrderDocumentRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;
        private readonly Mock<IDailyOperationReachingHistoryRepository>
            mockDailyOperationReachingHistoryRepo;

        public DailyOperationReachingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();

            this.mockMachineRepo = this.mockRepository.Create<IMachineRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockShiftRepo = this.mockRepository.Create<IShiftRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockOrderDocumentRepo = this.mockRepository.Create<IOrderRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockDailyOperationReachingTyingRepo = this.mockRepository.Create<IDailyOperationReachingRepository>();
            mockDailyOperationReachingHistoryRepo = mockRepository.Create<IDailyOperationReachingHistoryRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IMachineRepository>()).Returns(mockMachineRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOperatorRepository>()).Returns(mockOperatorRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IShiftRepository>()).Returns(mockShiftRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IFabricConstructionRepository>()).Returns(mockFabricConstructionRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOrderRepository>()).Returns(mockOrderDocumentRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockBeamRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingRepository>()).Returns(mockDailyOperationReachingTyingRepo.Object);
            mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingHistoryRepository>())
                .Returns(mockDailyOperationReachingHistoryRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationReachingQueryHandler CreateDailyOperationReachingTyingQueryHandler()
        {
            return new DailyOperationReachingQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();

            //Instantiate Object for Machine
            var machine = new MachineDocument(
                new Guid("98B34DC4-16CD-4DF2-8A62-2C59E078D9DB"),
                "32",
                "Timur",
                new MachineTypeId(new Guid("E6655428-E60B-4923-AF39-875B5BA61CFB")),
                new UnitId(11), "Loom", "Utara", 0);
            //mockMachineRepo.Setup(x => x.Query)
            //    .Returns(new List<MachineDocumentReadModel>()
            //    .AsQueryable());
            //mockMachineRepo.Setup(x => x.Find(
            //    It.IsAny<IQueryable<MachineDocumentReadModel>>()))
            //    .Returns(new List<MachineDocument>() { firstMachine, secondMachine });
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machine });

            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Instantiate Object for Beam
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Instantiate Object to Operator
            var operatorDocument = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");

            //Instantiate Object fo Shift
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));

            //Instantiate Object for Daily Operation Reaching-Tying
            var document = new DailyOperationReachingDocument(
                Guid.NewGuid(),
                new MachineId(machine.Identity),
                new OrderId(orderDocument.Identity),
                new BeamId(beam.Identity),
                "Plain", 
                "Lurus", 
                17,
                164, 
                90, 
                127,
                OperationStatus.ONFINISH);
            var detail = new DailyOperationReachingHistory(
                Guid.NewGuid(),
                new OperatorId(operatorDocument.Identity),
                100,
                DateTimeOffset.UtcNow,
                new ShiftId(shift.Identity),
                MachineStatus.ONCOMPLETE,
                document.Identity);
            //document.AddDailyOperationReachingHistory(detail);
            
            mockDailyOperationReachingTyingRepo.Setup(x => x.Query).Returns(new List<DailyOperationReachingReadModel>().AsQueryable());
            mockDailyOperationReachingTyingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingReadModel>>())).Returns(
                new List<DailyOperationReachingDocument>() { document });

            mockDailyOperationReachingHistoryRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { detail });

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();

            //Instantiate Object for Machine
            var machine = new MachineDocument(
                new Guid("98B34DC4-16CD-4DF2-8A62-2C59E078D9DB"),
                "32",
                "Timur",
                new MachineTypeId(new Guid("E6655428-E60B-4923-AF39-875B5BA61CFB")),
                new UnitId(11), "Loom", "Utara", 0);
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machine });

            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Instantiate Object for Beam
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Instantiate Object to Operator
            var operatorDocument = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Instantiate Object fo Shift
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Instantiate Object for Daily Operation Reaching-Tying
            var document = new DailyOperationReachingDocument(
                Guid.NewGuid(),
                new MachineId(machine.Identity),
                new OrderId(orderDocument.Identity),
                new BeamId(beam.Identity),
                "Plain",
                "Lurus",
                17,
                164,
                90,
                127,
                OperationStatus.ONFINISH);
            var detail = new DailyOperationReachingHistory(
                Guid.NewGuid(),
                new OperatorId(operatorDocument.Identity),
                100,
                DateTimeOffset.UtcNow,
                new ShiftId(shift.Identity),
                MachineStatus.ONCOMPLETE,
                document.Identity);
            //document.AddDailyOperationReachingHistory(detail);
            
            mockDailyOperationReachingTyingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingReadModel, bool>>>())).Returns(
                new List<DailyOperationReachingDocument>() { document });
            mockDailyOperationReachingHistoryRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingHistoryReadModel, bool>>>()))
                .Returns(new List<DailyOperationReachingHistory>() { detail });
            //mockDailyOperationReachingTyingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingTyingReadModel, bool>>>())).Returns(
            //    new List<DailyOperationReachingTyingDocument>() { firstDocument, secondDocument });

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetById(document.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
