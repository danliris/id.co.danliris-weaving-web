using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstruction;
using Manufactures.Domain.FabricConstruction.Commands;
using Manufactures.Domain.FabricConstruction.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.FabricConstruction.CommandHandlers
{
    public class PlaceConstructionCommandHandler : ICommandHandler<PlaceConstructionCommand,
                                                                   ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;

        public PlaceConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
        }

        public async Task<ConstructionDocument> Handle(PlaceConstructionCommand request,
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

            var constructionDocument = new ConstructionDocument(id: Guid.NewGuid(),
                                                                constructionNumber: request.ConstructionNumber,
                                                                amountOfWarp: request.AmountOfWarp,
                                                                amountOfWeft: request.AmountOfWeft,
                                                                width: request.Width,
                                                                wofenType: request.WovenType,
                                                                warpType: request.WarpTypeForm,
                                                                weftType: request.WeftTypeForm,
                                                                totalYarn: request.TotalYarn,
                                                                materialTypeName: request.MaterialTypeName);


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
