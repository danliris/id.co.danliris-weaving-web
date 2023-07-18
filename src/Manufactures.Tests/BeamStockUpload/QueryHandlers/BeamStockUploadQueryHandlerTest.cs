using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.BeamStockUpload.QueryHandlers;
using Manufactures.Domain.BeamStockUpload.Entities;
using Manufactures.Domain.BeamStockUpload.ReadModels;
using Manufactures.Domain.BeamStockUpload.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.BeamStockUpload.QueryHandlers
{
    public class BeamStockUploadQueryHandlerTest
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;

        private readonly Mock<IBeamStockRepository> mockReachingRepository;

        public BeamStockUploadQueryHandlerTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this.mockReachingRepository = this.mockRepository.Create<IBeamStockRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IBeamStockRepository>()).Returns(mockReachingRepository.Object);


        }

        private BeamStockUploadQueryHandler CreateWeavingBeamStockUploadQueryHandler()
        {
            return new BeamStockUploadQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingBeamStockUploadQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<BeamStockReadModel>
                {
                    new BeamStock(newGuid,1,_date.Year.ToString(),"month",_date.Month,"I","","name","","","","").GetReadModel()
                }.AsQueryable());

            var result = await queryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingBeamStockUploadQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<BeamStockReadModel>
                {
                    new BeamStock(newGuid,1,_date.Year.ToString(),"month",_date.Month,"I","","name","","","","").GetReadModel()
                }.AsQueryable());


            var result = await queryHandler.GetById(newGuid);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByMonthYear_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingBeamStockUploadQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<BeamStockReadModel>
                {
                    new BeamStock(newGuid,1,_date.Year.ToString(),"month",_date.Month,"I","","name","","","","").GetReadModel()
                }.AsQueryable());


            var result = queryHandler.GetByMonthYear(_date.Month, _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingBeamStockUploadQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<BeamStockReadModel>
                {
                    new BeamStock(newGuid,1,_date.Year.ToString(),"month",_date.Month,"I","","name","","","","").GetReadModel()
                }.AsQueryable());


            var result = queryHandler.Delete(_date.Month.ToString(), _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }
    }
}
