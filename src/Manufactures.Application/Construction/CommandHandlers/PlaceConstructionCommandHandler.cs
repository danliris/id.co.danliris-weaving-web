using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class PlaceConstructionCommandHandler : ICommandHandler<PlaceConstructionCommand, ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;

        public PlaceConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
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
                                                                warpType: request.WarpType,
                                                                weftType: request.WeftType,
                                                                totalYarn: request.TotalYarn,
                                                                materialTypeId: request.MaterialTypeId);

            foreach (var detail in request.Warps)
            {
                detail.SetDetail(Constants.WARP);

                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(detail.Yarn.Id)).FirstOrDefault();
                var yarn = new Yarn(yarnDocument.Identity, yarnDocument.Code, yarnDocument.Name);
                ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                                               detail.Quantity,
                                                                                               detail.Information,
                                                                                               yarn,
                                                                                               detail.Detail);

                constructionDocument.AddConstructionDetail(constructionDetail);
            }

            foreach (var detail in request.Wefts)
            {
                detail.SetDetail(Constants.WEFT);

                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(detail.Yarn.Id)).FirstOrDefault();
                var yarn = new Yarn(yarnDocument.Identity, yarnDocument.Code, yarnDocument.Name);
                ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                               detail.Quantity,
                                                                               detail.Information,
                                                                               yarn,
                                                                               detail.Detail);
                constructionDocument.AddConstructionDetail(constructionDetail);
            }
            
            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
