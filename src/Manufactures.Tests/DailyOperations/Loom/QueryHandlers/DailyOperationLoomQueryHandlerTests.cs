using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Loom.QueryHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.Repositories;
using Manufactures.Domain.Suppliers;
using Manufactures.Domain.Suppliers.ReadModels;
using Manufactures.Domain.Suppliers.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.QueryHandlers
{
    public class DailyOperationLoomQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationLoomRepository>
            mockDailyOperationLoomRepo;
        private readonly Mock<IWeavingOrderDocumentRepository>
            mockOrderDocumentRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IWeavingSupplierRepository>
            mockSupplierRepo;
        private readonly Mock<IOperatorRepository>
            mockOperatorRepo;
        private readonly Mock<IShiftRepository>
            mockShiftRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public DailyOperationLoomQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationLoomRepo = this.mockRepository.Create<IDailyOperationLoomRepository>();
            this.mockOrderDocumentRepo = this.mockRepository.Create<IWeavingOrderDocumentRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockSupplierRepo = this.mockRepository.Create<IWeavingSupplierRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockShiftRepo = this.mockRepository.Create<IShiftRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationLoomRepository>()).Returns(mockDailyOperationLoomRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IWeavingOrderDocumentRepository>()).Returns(mockOrderDocumentRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IFabricConstructionRepository>()).Returns(mockFabricConstructionRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IWeavingSupplierRepository>()).Returns(mockSupplierRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOperatorRepository>()).Returns(mockOperatorRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IShiftRepository>()).Returns(mockShiftRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationLoomQueryHandler CreateDailyOperationLoomQueryHandler()
        {
            return new DailyOperationLoomQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationLoomQueryHandler = this.CreateDailyOperationLoomQueryHandler();

            //Create Mock Object
            //Fabric Construction Object
            var fabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
                new Guid("03A861FC-4A97-40CC-B478-70357FDF3065"),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "Melintang",
                "PLCTD100",
                "PLCTD100",
                33,
                44,
                55,
                1000,
                "PolyCotton100");
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<Domain.FabricConstructions.FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                new Guid("0121ACFF-A3F6-463B-AC75-51291C920221"),
                "0002/08-2019",
                new ConstructionId(fabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(50, 40, 10),
                new Composition(30, 50, 20),
                "PC",
                "CD",
                4000,
                "PolyCotton",
                new UnitId(11),
                "OPEN-ORDER");
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderDocumentReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Supplier Object
            var firstSupplierDocument =
                new WeavingSupplierDocument(new Guid("A837663E-5FC0-41A3-B417-B2ADD94A5120"),
                                            "KDKI",
                                            "KDK INDONESIA",
                                            "3");
            var secondSupplierDocument =
                 new WeavingSupplierDocument(new Guid("19C4B6C8-ACAD-4336-A4BB-BC1CD043C04C"),
                                             "UAE",
                                             "UTAMA AUVI ELECTRONIC",
                                             "4");
            mockSupplierRepo.Setup(x => x.Find(It.IsAny<Expression<Func<WeavingSupplierDocumentReadModel, bool>>>()))
                .Returns(new List<WeavingSupplierDocument>() { firstSupplierDocument, secondSupplierDocument });

            //Beam Object
            var firstBeam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            var secondBeam = new BeamDocument(
                new Guid("47017EF1-7C7A-4238-920E-FC3DF98BBC92"),
                "TS33",
                "Sizing",
                33);

            //Machine Type Object
            var firstMachineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            var secondMachineTypeDocument = new MachineTypeDocument(
                new Guid("B940085D-772B-42AB-9CFC-F24C6E6A61F8"),
                "Sucker Muller",
                3500,
                "Meter");

            //Machine Object
            var firstMachineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(firstMachineTypeDocument.Identity),
                new UnitId(11));
            var secondMachineDocument = new MachineDocument(
                new Guid("7CC36BC1-EAF5-4BED-BBA1-5ECA177F30F8"),
                "124",
                "Selatan",
                new MachineTypeId(secondMachineTypeDocument.Identity),
                new UnitId(14));

            //Operator Object
            var firstOperator = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            var secondOperator = new OperatorDocument(
                new Guid("B94C2C48-7962-4D7D-A7F5-71D90F828350"),
                new CoreAccount("5AEAA60D14F4850039294B68", 0, "Ana Kwee"),
                new UnitId(11),
                "F",
                "AJL",
                "Operator");

            //Shift Object
            var morningShift = new ShiftDocument(
                new Guid("6C5C1CF3-0742-46E7-A2E0-31E56974F71A"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            var afternoonShift = new ShiftDocument(
                new Guid("34AF0708-A130-4DFC-B0FC-85F50D3780D1"),
                "Siang",
                new TimeSpan(14, 01, 00),
                new TimeSpan(22, 00, 00));
            var nightShift = new ShiftDocument(
                new Guid("2517B0DB-A14F-4FE3-A557-FA3430A55F2F"),
                "Siang",
                new TimeSpan(22, 01, 00),
                new TimeSpan(06, 00, 00));

            //Create Loom Object
            //First Loom Document Object
            var loomDocument = new DailyOperationLoomDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                              new OrderId(orderDocument.Identity),
                                                              OperationStatus.ONPROCESS);
            var loomBeamProduct = new DailyOperationLoomBeamProduct(new Guid("BD127989-0CFF-447E-9B26-59AF5210D619"),
                                                                    new BeamId(firstBeam.Identity),
                                                                    new MachineId(firstMachineDocument.Identity),
                                                                    DateTimeOffset.UtcNow,
                                                                    "Normal",
                                                                    BeamStatus.ONPROCESS);
            var loomBeamHistory = new DailyOperationLoomBeamHistory(new Guid("CFBFECCC-BED5-4DE9-A9C9-9FC1FDA15E48"),
                                                                    "S11",
                                                                    "123",
                                                                    new OperatorId(firstOperator.Identity),
                                                                    DateTimeOffset.UtcNow,
                                                                    new ShiftId(morningShift.Identity),
                                                                    MachineStatus.ONENTRY);
            loomDocument.AddDailyOperationLoomBeamProduct(loomBeamProduct);
            loomDocument.AddDailyOperationLoomHistory(loomBeamHistory);

            mockDailyOperationLoomRepo.Setup(x => x.Query).Returns(new List<DailyOperationLoomReadModel>().AsQueryable());
            mockDailyOperationLoomRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationLoomReadModel>>())).Returns(
                new List<DailyOperationLoomDocument>() { loomDocument });

            // Act
            var result = await dailyOperationLoomQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationLoomQueryHandler = this.CreateDailyOperationLoomQueryHandler();
            Guid id = default(global::System.Guid);

            // Act
            var result = await dailyOperationLoomQueryHandler.GetById(
                id);

            // Assert
            Assert.True(false);
        }
    }
}
