using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class PlaceConstructionCommandHandler : ICommandHandler<PlaceConstructionCommand, ConstructionDocument>
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
                throw Validator.ErrorValidation(("ConstructionNumber", "Construction Number " + request.ConstructionNumber + " has Available"));
            }
            
            List<ConstructionDetail> constructionDetails = new List<ConstructionDetail>();

            foreach(var detail in request.Warps)
            {
                detail.SetDetail(Constants.WARP);

                ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(), 
                                                                               detail.Quantity, 
                                                                               detail.Information, 
                                                                               detail.Yarn.Serialize(), 
                                                                               detail.Detail);

                constructionDetails.Add(constructionDetail);
            }

            foreach(var detail in request.Wefts)
            {
                detail.SetDetail(Constants.WEFT);

                ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(), 
                                                                               detail.Quantity, 
                                                                               detail.Information, 
                                                                               detail.Yarn.Serialize(), 
                                                                               detail.Detail);

                constructionDetails.Add(constructionDetail);
            }

            var constructionDocument = new ConstructionDocument(id: Guid.NewGuid(),
                                                                constructionNumber: request.ConstructionNumber,
                                                                amountOfWarp: request.AmountOfWarp,
                                                                amountOfWeft: request.AmountOfWeft,
                                                                width: request.Width,
                                                                wofenType: request.WovenType,
                                                                warpType: request.WarpType,
                                                                weftType: request.WeftType,
                                                                totalYarn: request.TotalYarn,
                                                                materialType: request.MaterialType,
                                                                constructionDetails: constructionDetails);

            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
