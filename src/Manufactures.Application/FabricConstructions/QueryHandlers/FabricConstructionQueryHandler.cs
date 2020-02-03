using ExtCore.Data.Abstractions;
using Manufactures.Application.FabricConstructions.DataTransferObjects;
using Manufactures.Domain.FabricConstructions.Queries;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.FabricConstructions.QueryHandlers
{
    public class FabricConstructionQueryHandler : IFabricConstructionQuery<FabricConstructionListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;
        private readonly IYarnDocumentRepository
            _yarnRepository; 

        public FabricConstructionQueryHandler(IStorage storage)
        {
            _storage = storage;
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _constructionYarnDetailRepository =
                _storage.GetRepository<IConstructionYarnDetailRepository>();
            _yarnRepository =
                _storage.GetRepository<IYarnDocumentRepository>();
        }

        public async Task<IEnumerable<FabricConstructionListDto>> GetAll()
        {
            var resultListDto = new List<FabricConstructionListDto>();

            var constructionQuery =
                _fabricConstructionRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var constructionDocument =
                _fabricConstructionRepository
                    .Find(constructionQuery);

            foreach (var document in constructionDocument)
            {
                var resultDto = new FabricConstructionListDto(document);

                resultListDto.Add(resultDto);
            }

            return resultListDto;
        }

        public async Task<FabricConstructionListDto> GetById(Guid id)
        {
            var constructionDocument =
                _fabricConstructionRepository
                    .Find(o => o.Identity == id)
                    .FirstOrDefault();

            await Task.Yield();
            var resultDto = new FabricConstructionByIdDto(constructionDocument);

            var constructionYarnDetails =
                _constructionYarnDetailRepository
                    .Find(o => o.FabricConstructionDocumentId == constructionDocument.Identity);

            foreach (var constructionYarnDetail in constructionYarnDetails)
            {
                var yarnDocument =
                    _yarnRepository
                        .Find(o => o.Identity == constructionYarnDetail.YarnId.Value)
                        .FirstOrDefault();

                var resultDetailDto = new ConstructionYarnDetailDto(constructionDocument, constructionYarnDetail, yarnDocument.Code, yarnDocument.Name);

                await Task.Yield();
                if (resultDetailDto.Type == "Warp")
                {
                    resultDto.AddConstructionWarpsDetail(resultDetailDto);
                }
                else if (resultDetailDto.Type == "Weft")
                {
                    resultDto.AddConstructionWeftsDetail(resultDetailDto);
                }
            }

            return resultDto;
        }
    }
}
