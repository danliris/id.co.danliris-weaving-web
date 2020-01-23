using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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

            var newConstructionDocument = new FabricConstructionDocument(Guid.NewGuid(),
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
            await _constructionDocumentRepository.Update(newConstructionDocument);

            var mergedYarnsDetail = request.ConstructionWarpsDetail.Concat(request.ConstructionWeftsDetail).ToList();
            foreach(var yarnDetail in mergedYarnsDetail)
            {
                var newConstructionYarnDetail = new ConstructionYarnDetail(Guid.NewGuid(),
                                                                           new YarnId(yarnDetail.YarnId),
                                                                           yarnDetail.Quantity, 
                                                                           yarnDetail.Information, 
                                                                           yarnDetail.Type, 
                                                                           newConstructionDocument.Identity);

                await _constructionYarnDetailRepository.Update(newConstructionYarnDetail);
            }

            _storage.Save();

            return newConstructionDocument;
        }
    }
}
