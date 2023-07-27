using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Production.QueryHandlers;
using Manufactures.Application.DailyOperations.Spu.QueryHandlers;
using Manufactures.Application.DailyOperations.Warping.QueryHandlers;
using Manufactures.Application.Estimations.Productions.QueryHandlers;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.DailyOperations.Spu.ReadModels;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Spu.QueryHandlers
{
    public class WeavingDailyOperationSpuQueryHandlerTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;
        private readonly Mock<IDailyOperationMachineSizingDocumentRepository> _mockEstimationRepo;
        private readonly Mock<IWeavingDailyOperationMachineSizingRepository> _mockWeavingRepo;
        //_storage.GetRepository<IWeavingDailyOperationMachineSizingRepository>();
        public WeavingDailyOperationSpuQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this._mockEstimationRepo = this.mockRepository.Create<IDailyOperationMachineSizingDocumentRepository>();
            this._mockWeavingRepo = this.mockRepository.Create<IWeavingDailyOperationMachineSizingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationMachineSizingDocumentRepository>()).Returns(_mockEstimationRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IWeavingDailyOperationMachineSizingRepository>()).Returns(_mockWeavingRepo.Object);

        }
        private WeavingDailyOperationSpuMachineQueryHandler CreateWeavingDailyOperationSpuQueryHandler()
        {
            //WeavingDailyOperationSpuMachineQueryHandler
            return new WeavingDailyOperationSpuMachineQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationSpuQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            _mockWeavingRepo
               .Setup(s => s.Query)
                //WeavingDailyOperationSpuMachineReadModel
                .Returns(new List<WeavingDailyOperationMachineSizingReadModel>
                {
                    new WeavingDailyOperationMachineSizings(DateTime.Now,DateTime.Now,newGuid,3,"Maret",
                    "2023",20,"3","SZ 1","1",
                    "1","1","warpType","wefttype1","wefttype2",
                    "al","ap1","ap2","thread","cons",
                    "buyer","1","cons","warpxweft","1",
                    "1","1","1","1","1",
                    "1","1","1","1","1",
                    "1","1","1","1","1",
                    "1","1","1","1","1",
                    "1","1","1","1","1",
                    "1","1","1","1","1",
                    "1","1"

                    ).GetReadModel()
                }.AsQueryable());

            //DateTime.Now.AddYears(-5), DateTime.Now, "1", "SZ 1", "A", "{}", "{}"
            var result = queryHandler.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now, null, null, null, null, null);

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }
        //[Fact]
        //public async Task GetById_StateUnderTest_ExpectedBehavior()
        //{
        //    var queryHandler = this.CreateWeavingEstimatedProductionQueryHandler();
        //    Guid newGuid = new Guid();
        //    DateTime _date = new DateTime();
        //    _mockWeavingRepo
        //      .Setup(s => s.Query)
        //       .Returns(new List<WeavingEstimatedProductionReadModel>
        //       {
        //            new WeavingEstimatedProduction(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"SPNO",
        //            "Plait",1,1,1,"warpType","wefttype1","wefttype2","al","ap1","ap2","thread","cons","buyer",1,"cons","warpxweft",1,1,1,1,1,1,1,1
        //            ).GetReadModel()
        //       }.AsQueryable());

        //    var result = await queryHandler.GetById(newGuid);

        //    // Assert
        //    result.Should().NotBeNull();
        //}
        //[Fact]
        //public async Task GetDataByFilter_StateUnderTest_ExpectedBehavior()
        //{
        //    var queryHandler = this.CreateWeavingEstimatedProductionQueryHandler();
        //    Guid newGuid = new Guid();
        //    DateTime _date = new DateTime();
        //    _mockWeavingRepo
        //      .Setup(s => s.Query)
        //       .Returns(new List<WeavingEstimatedProductionReadModel>
        //       {
        //            new WeavingEstimatedProduction(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"SPNO",
        //            "Plait",1,1,1,"warpType","wefttype1","wefttype2","al","ap1","ap2","thread","cons","buyer",1,"cons","warpxweft",1,1,1,1,1,1,1,1
        //            ).GetReadModel()
        //       }.AsQueryable());

        //    var result =  queryHandler.GetDataByFilter("month",_date.Year.ToString());

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Delete_StateUnderTest_ExpectedBehavior()
        //{
        //    var queryHandler = this.CreateWeavingEstimatedProductionQueryHandler();
        //    Guid newGuid = new Guid();
        //    DateTime _date = new DateTime();
        //    _mockWeavingRepo
        //       .Setup(s => s.Query)
        //        .Returns(new List<WeavingEstimatedProductionReadModel>
        //        {
        //             new WeavingEstimatedProduction(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"SPNO",
        //            "Plait",1,1,1,"warpType","wefttype1","wefttype2","al","ap1","ap2","thread","cons","buyer",1,"cons","warpxweft",1,1,1,1,1,1,1,1
        //            ).GetReadModel()
        //        }.AsQueryable());

        //    var result = queryHandler.Delete(_date.Month.ToString(), _date.Year.ToString());

        //    // Assert
        //    result.Should().NotBeNull();
        //}
    }
}

