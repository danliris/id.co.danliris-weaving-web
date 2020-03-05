using ExtCore.Data.Abstractions;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.CoreMicroservice.HttpClientService;
using Manufactures.Application.DailyOperations.Warping.QueryHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Repositories;
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

namespace Manufactures.Tests.DailyOperations.Warping.QueryHandlers
{
    public class DailyOperationWarpingQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IHttpClientService> mockClientService;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;
        private readonly Mock<IDailyOperationWarpingHistoryRepository>
            mockDailyOperationWarpingHistoryRepo;
        private readonly Mock<IDailyOperationWarpingBeamProductRepository>
            mockDailyOperationWarpingBeamProductRepo;
        private readonly Mock<IDailyOperationWarpingBrokenCauseRepository>
            mockDailyOperationWarpingBrokenCauseRepo;
        private readonly Mock<IOrderRepository>
            mockOrderDocumentRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IMaterialTypeRepository>
            mockMaterialTypeRepo;
        private readonly Mock<IOperatorRepository>
            mockOperatorRepo;
        private readonly Mock<IShiftRepository>
            mockShiftRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;
        private readonly Mock<IWarpingBrokenCauseRepository>
            mockWarpingBrokenCauseRepo;

        public DailyOperationWarpingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockClientService = this.mockRepository.Create<IHttpClientService>();
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();

            this.mockDailyOperationWarpingRepo = this.mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockDailyOperationWarpingHistoryRepo = this.mockRepository.Create<IDailyOperationWarpingHistoryRepository>();
            this.mockDailyOperationWarpingBeamProductRepo = this.mockRepository.Create<IDailyOperationWarpingBeamProductRepository>();
            this.mockDailyOperationWarpingBrokenCauseRepo = this.mockRepository.Create<IDailyOperationWarpingBrokenCauseRepository>();
            this.mockOrderDocumentRepo = this.mockRepository.Create<IOrderRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockMaterialTypeRepo = this.mockRepository.Create<IMaterialTypeRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockShiftRepo = this.mockRepository.Create<IShiftRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockWarpingBrokenCauseRepo = this.mockRepository.Create<IWarpingBrokenCauseRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingRepository>()).Returns(mockDailyOperationWarpingRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingHistoryRepository>()).Returns(mockDailyOperationWarpingHistoryRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBeamProductRepository>()).Returns(mockDailyOperationWarpingBeamProductRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBrokenCauseRepository>()).Returns(mockDailyOperationWarpingBrokenCauseRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOrderRepository>()).Returns(mockOrderDocumentRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IFabricConstructionRepository>()).Returns(mockFabricConstructionRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IMaterialTypeRepository>()).Returns(mockMaterialTypeRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOperatorRepository>()).Returns(mockOperatorRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IShiftRepository>()).Returns(mockShiftRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockBeamRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IWarpingBrokenCauseRepository>()).Returns(mockWarpingBrokenCauseRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationWarpingQueryHandler CreateDailyOperationWarpingQueryHandler()
        {
            return new DailyOperationWarpingQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationWarpingQueryHandler = this.CreateDailyOperationWarpingQueryHandler();

            //Create Mock Object
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
                new SupplierId(supplierDocument.Identity),
                3500,
                new UnitId(14),
                Constants.ONORDER);
            orderDocument.SetWarpCompositionPoly(40);
            orderDocument.SetWarpCompositionCotton(40);
            orderDocument.SetWarpCompositionOthers(20);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(40);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Material Type Object
            var materialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            //mockMaterialTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MaterialTypeReadModel, bool>>>()))
            //    .Returns(new List<MaterialTypeDocument>() { materialType });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                Guid.NewGuid(),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            //mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
            //    .Returns(new List<OperatorDocument>() { operator_ });

            //Shift Object
            var shift = new ShiftDocument(
                Guid.NewGuid(),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            //mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
            //    .Returns(new List<ShiftDocument>() { shift });

            //Beam Object
            var beam = new BeamDocument(
                Guid.NewGuid(),
                "TS56",
                "Sizing",
                6);
            //mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { beam });

            //Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                    new OrderId(orderDocument.Identity),
                                                                    40,
                                                                    1,
                                                                    DateTimeOffset.UtcNow,
                                                                    OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            history.SetOperatorDocumentId(new OperatorId(operatorDocument.Identity));
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                   new BeamId(beam.Identity),
                                                                   new UomId(195),
                                                                   "Meter",
                                                                   DateTimeOffset.UtcNow,
                                                                   BeamStatus.ONPROCESS,
                                                                   warpingDocument.Identity);
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            mockDailyOperationWarpingRepo
                .Setup(x => x.Query)
                .Returns(new List<DailyOperationWarpingDocumentReadModel>()
                .AsQueryable());
            mockDailyOperationWarpingRepo
                .Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>()))
                .Returns(
                new List<DailyOperationWarpingDocument>() { warpingDocument });

            // Act
            var result = await dailyOperationWarpingQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_MachineStatusNotEntryOrFinish_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationWarpingQueryHandler = this.CreateDailyOperationWarpingQueryHandler();

            //Create Mock Object
            //Shift Object
            var shift = new ShiftDocument(
                Guid.NewGuid(),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                Guid.NewGuid(),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Beam Object
            var beam = new BeamDocument(
                Guid.NewGuid(),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Broken Cause Object
            var brokenCause = new WarpingBrokenCauseDocument(
                Guid.NewGuid(),
                "Slub",
                "-",
                false);
            mockWarpingBrokenCauseRepo.Setup(x => x.Find(It.IsAny<Expression<Func<WarpingBrokenCauseReadModel, bool>>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { brokenCause });

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
                new SupplierId(supplierDocument.Identity),
                3500,
                new UnitId(14),
                Constants.ONORDER);
            orderDocument.SetWarpCompositionPoly(40);
            orderDocument.SetWarpCompositionCotton(40);
            orderDocument.SetWarpCompositionOthers(20);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(40);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                    new OrderId(orderDocument.Identity),
                                                                    40,
                                                                    1,
                                                                    DateTimeOffset.UtcNow,
                                                                    OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            history.SetOperatorDocumentId(new OperatorId(operatorDocument.Identity));
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                   new BeamId(beam.Identity),
                                                                   new UomId(195),
                                                                   "Meter",
                                                                   DateTimeOffset.UtcNow,
                                                                   BeamStatus.ONPROCESS,
                                                                   warpingDocument.Identity);
            var warpingBroken = new DailyOperationWarpingBrokenCause(
                Guid.NewGuid(),
                new BrokenCauseId(brokenCause.Identity),
                2,
                beamProduct.Identity);
            beamProduct.BrokenCauses = new List<DailyOperationWarpingBrokenCause>() { warpingBroken };
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { warpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { history });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { beamProduct });

            mockDailyOperationWarpingBrokenCauseRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBrokenCauseReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBrokenCause>() { warpingBroken });

            // Act
            var result = await dailyOperationWarpingQueryHandler.GetById(warpingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusEntry_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationWarpingQueryHandler = this.CreateDailyOperationWarpingQueryHandler();

            //Create Mock Object
            //Shift Object
            var shift = new ShiftDocument(
                Guid.NewGuid(),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                Guid.NewGuid(),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Beam Object
            var beam = new BeamDocument(
                Guid.NewGuid(),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Broken Cause Object
            var brokenCause = new WarpingBrokenCauseDocument(
                Guid.NewGuid(),
                "Slub",
                "-",
                false);
            mockWarpingBrokenCauseRepo.Setup(x => x.Find(It.IsAny<Expression<Func<WarpingBrokenCauseReadModel, bool>>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { brokenCause });

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
                new SupplierId(supplierDocument.Identity),
                3500,
                new UnitId(14),
                Constants.ONORDER);
            orderDocument.SetWarpCompositionPoly(40);
            orderDocument.SetWarpCompositionCotton(40);
            orderDocument.SetWarpCompositionOthers(20);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(40);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                    new OrderId(orderDocument.Identity),
                                                                    40,
                                                                    1,
                                                                    DateTimeOffset.UtcNow,
                                                                    OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            history.SetOperatorDocumentId(new OperatorId(operatorDocument.Identity));
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                   new BeamId(beam.Identity),
                                                                   new UomId(195),
                                                                   "Meter",
                                                                   DateTimeOffset.UtcNow,
                                                                   BeamStatus.ONPROCESS,
                                                                   warpingDocument.Identity);
            var warpingBroken = new DailyOperationWarpingBrokenCause(
                Guid.NewGuid(), 
                new BrokenCauseId(brokenCause.Identity), 
                2,
                beamProduct.Identity);
            beamProduct.BrokenCauses = new List<DailyOperationWarpingBrokenCause>() { warpingBroken };
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { warpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { history });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { beamProduct });

            mockDailyOperationWarpingBrokenCauseRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBrokenCauseReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBrokenCause>() { warpingBroken });

            //mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            //mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
            //    new List<DailyOperationWarpingDocument>() { warpingDocument });

            // Act
            var result = await dailyOperationWarpingQueryHandler.GetById(warpingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusFinish_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationWarpingQueryHandler = this.CreateDailyOperationWarpingQueryHandler();

            //Create Mock Object
            //Shift Object
            var shift = new ShiftDocument(
                Guid.NewGuid(),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                Guid.NewGuid(),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Beam Object
            var beam = new BeamDocument(
                Guid.NewGuid(),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Broken Cause Object
            var brokenCause = new WarpingBrokenCauseDocument(
                Guid.NewGuid(),
                "Slub",
                "-",
                false);
            mockWarpingBrokenCauseRepo.Setup(x => x.Find(It.IsAny<Expression<Func<WarpingBrokenCauseReadModel, bool>>>()))
                .Returns(new List<WarpingBrokenCauseDocument>() { brokenCause });

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
                new SupplierId(supplierDocument.Identity),
                3500,
                new UnitId(14),
                Constants.ONORDER);
            orderDocument.SetWarpCompositionPoly(40);
            orderDocument.SetWarpCompositionCotton(40);
            orderDocument.SetWarpCompositionOthers(20);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(30);
            orderDocument.SetWeftCompositionPoly(40);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(Guid.NewGuid(),
                                                                    new OrderId(orderDocument.Identity),
                                                                    40,
                                                                    1,
                                                                    DateTimeOffset.UtcNow,
                                                                    OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            history.SetOperatorDocumentId(new OperatorId(operatorDocument.Identity));
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(Guid.NewGuid(),
                                                                   new BeamId(beam.Identity),
                                                                   new UomId(195),
                                                                   "Meter",
                                                                   DateTimeOffset.UtcNow,
                                                                   BeamStatus.ONPROCESS,
                                                                   warpingDocument.Identity);
            var warpingBroken = new DailyOperationWarpingBrokenCause(
                Guid.NewGuid(),
                new BrokenCauseId(brokenCause.Identity),
                2,
                beamProduct.Identity);
            beamProduct.BrokenCauses = new List<DailyOperationWarpingBrokenCause>() { warpingBroken };
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            mockDailyOperationWarpingRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingDocument>() { warpingDocument });

            mockDailyOperationWarpingHistoryRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingHistoryReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingHistory>() { history });

            mockDailyOperationWarpingBeamProductRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBeamProductReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBeamProduct>() { beamProduct });

            mockDailyOperationWarpingBrokenCauseRepo
                 .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingBrokenCauseReadModel, bool>>>()))
                 .Returns(new List<DailyOperationWarpingBrokenCause>() { warpingBroken });

            // Act
            var result = await dailyOperationWarpingQueryHandler.GetById(warpingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
