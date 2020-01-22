using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.FabricConstructions.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.FabricConstructions.CommandHandlers
{
    public class PlaceConstructionCommandHandler : ICommandHandler<AddFabricConstructionCommand,
                                                                   FabricConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IConstructionYarnDetailRepository _constructionYarnDetailRepository;

        public PlaceConstructionCommandHandler(IStorage storage)
        {
            _storage = 
                storage;
            _constructionDocumentRepository = 
                _storage.GetRepository<IFabricConstructionRepository>();
            _constructionYarnDetailRepository = 
                _storage.GetRepository<IConstructionYarnDetailRepository>();
        }

        public async Task<FabricConstructionDocument> Handle(AddFabricConstructionCommand request, CancellationToken cancellationToken)
        {
            // Check Available construction number if has defined
            var existingConstructionNumber = 
                _constructionDocumentRepository
                    .Find(construction => construction.ConstructionNumber.Equals(request.ConstructionNumber) &&
                                          construction.Deleted.Equals(false))
                    .Count() > 1;

            if (existingConstructionNumber)
            {
                Validator.ErrorValidation(("ConstructionNumber", request.ConstructionNumber + " Has Available!"));
            }

            var constructionDocument = new FabricConstructionDocument(Guid.NewGuid(),
                                                                      request.ConstructionNumber,
                                                                      request.MaterialType,
                                                                      request.WovenType,
                                                                      request.AmountOfWarp,
                                                                      request.AmountOfWeft,
                                                                      request.Width,
                                                                      request.WarpType,
                                                                      request.WeftType,
                                                                      request.ReedSpace,
                                                                      request.YarnStrandsAmount,
                                                                      request.TotalYarn);
            await _constructionDocumentRepository.Update(constructionDocument);

            var mergedYarnsDetail = request.ConstructionWarpsDetail.Concat(request.ConstructionWeftsDetail).ToList();
            foreach(var yarnDetail in mergedYarnsDetail)
            {
                await _constructionYarnDetailRepository.Update(yarnDetail);
            }
            //if (request.ReedSpace != 0)
            //{
            //    constructionDocument.AddReedSpace(request.ReedSpace);
            //}

            //if (request.YarnStrandsAmount != 0)
            //{
            //    constructionDocument.AddYarnStrandsAmount(request.YarnStrandsAmount);
            //}

            //if (request.ConstructionWarpsDetail.Count > 0)
            //{
            //    foreach (var warp in request.ConstructionWarpsDetail)
            //    {
            //        constructionDocument.AddWarp(warp);
            //    }
            //}

            //if (request.ConstructionWeftsDetail.Count > 0)
            //{
            //    foreach (var weft in request.ConstructionWeftsDetail)
            //    {
            //        constructionDocument.AddWeft(weft);
            //    }
            //}
            _storage.Save();

            return constructionDocument;
        }
    }
}
