using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Moonlay;
using System;
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

        public PlaceConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IFabricConstructionRepository>();
        }

        public async Task<FabricConstructionDocument> Handle(AddFabricConstructionCommand request,
                                                       CancellationToken cancellationToken)
        {
            // Check Available construction number if has defined
            var exsistingConstructionNumber = _constructionDocumentRepository
                    .Find(construction => construction.ConstructionNumber.Equals(request.ConstructionNumber) &&
                                          construction.Deleted.Equals(false))
                    .Count() > 1;

            if (exsistingConstructionNumber)
            {
                Validator.ErrorValidation(("constructionNumber", request.ConstructionNumber + " Has available!"));
            }

            var constructionDocument = new FabricConstructionDocument(id: Guid.NewGuid(),
                                                                constructionNumber: request.ConstructionNumber,
                                                                amountOfWarp: request.AmountOfWarp,
                                                                amountOfWeft: request.AmountOfWeft,
                                                                width: request.Width,
                                                                wofenType: request.WovenType,
                                                                warpType: request.WarpTypeForm,
                                                                weftType: request.WeftTypeForm,
                                                                totalYarn: request.TotalYarn,
                                                                materialTypeName: request.MaterialTypeName);
            if (request.ReedSpace != 0)
            {
                constructionDocument.AddReedSpace(request.ReedSpace);
            }

            if (request.TotalEnds != 0)
            {
                constructionDocument.AddTotalEnds(request.TotalEnds);
            }

            if (request.ItemsWarp.Count > 0)
            {
                foreach (var warp in request.ItemsWarp)
                {
                    constructionDocument.AddWarp(warp);
                }
            }

            if (request.ItemsWeft.Count > 0)
            {
                foreach (var weft in request.ItemsWeft)
                {
                    constructionDocument.AddWeft(weft);
                }
            }


            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
