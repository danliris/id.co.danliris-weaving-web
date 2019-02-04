using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class UpdateConstructionCommandHandler : ICommandHandler<UpdateConstructionCommand, ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;

        public UpdateConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
        }

        public async Task<ConstructionDocument> Handle(UpdateConstructionCommand request,
                                                       CancellationToken cancellationToken)
        {
            var constructionDocuments = _constructionDocumentRepository.Find(Entity => Entity.Identity.Equals(request.Id))
                                                                       .FirstOrDefault();

            var exsistingConstructionNumber = _constructionDocumentRepository
                    .Find(construction => construction.ConstructionNumber.Equals(request.ConstructionNumber) &&
                                          construction.Deleted.Equals(false))
                    .Count() > 1;

            // Check Available construction document
            if (constructionDocuments == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Construction Document: " + request.Id));
            }

            // Check Available construction number if has defined
            if (exsistingConstructionNumber && !constructionDocuments.ConstructionNumber.Equals(request.ConstructionNumber))
            {
                throw Validator.ErrorValidation(("ConstructionNumber", "Construction Number " + request.ConstructionNumber + " has Available"));
            }

            constructionDocuments.SetConstructionNumber(request.ConstructionNumber);
            constructionDocuments.SetAmountOfWarp(request.AmountOfWarp);
            constructionDocuments.SetAmountOfWeft(request.AmountOfWeft);
            constructionDocuments.SetWidth(request.Width);
            constructionDocuments.SetWovenType(request.WovenType);
            constructionDocuments.SetWarpType(request.WarpType);
            constructionDocuments.SetWeftType(request.WeftType);
            constructionDocuments.SetTotalYarn(request.TotalYarn);
            constructionDocuments.SetMaterialType(request.MaterialType);

            var updateConstructionDetail = new List<ConstructionDetail>();

            // Update Detail
            foreach (var warp in request.Warps)
            {
                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(warp.Yarn.Id)).FirstOrDefault();
                var detail = constructionDocuments.ConstructionDetails.ToList()
                                .Where(o => o.Detail.Equals(Constants.WARP) && o.Yarn.Identity == yarnDocument.Identity).FirstOrDefault();

                if (detail != null)
                {
                    detail.SetQuantity(warp.Quantity);
                    detail.SetInformation(warp.Information);
                    updateConstructionDetail.Add(detail);
                }
                else
                {
                    if (yarnDocument != null)
                    {
                        ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                                       warp.Quantity,
                                                                                       warp.Information,
                                                                                       yarnDocument,
                                                                                       Constants.WARP);
                        constructionDocuments.AddConstructionDetail(constructionDetail);

                        updateConstructionDetail.Add(constructionDetail);
                    }
                }
            }

            foreach (var weft in request.Wefts)
            {
                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(weft.Yarn.Id)).FirstOrDefault();
                var detail = constructionDocuments.ConstructionDetails.ToList()
                                .Where(o => o.Detail.Equals(Constants.WEFT) && o.Yarn.Identity == yarnDocument.Identity).FirstOrDefault();

                if (detail != null)
                {
                    detail.SetQuantity(weft.Quantity);
                    detail.SetInformation(weft.Information);
                    updateConstructionDetail.Add(detail);
                }
                else
                {
                    if (yarnDocument != null)
                    {
                        ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                                       weft.Quantity,
                                                                                       weft.Information,
                                                                                       yarnDocument,
                                                                                       Constants.WEFT);
                        constructionDocuments.AddConstructionDetail(constructionDetail);
                        updateConstructionDetail.Add(constructionDetail);
                    }
                }
            }

            if(updateConstructionDetail.Count > 1)
            {
                constructionDocuments.UpdateConstructionDetail(updateConstructionDetail);
            }
            
            await _constructionDocumentRepository.Update(constructionDocuments);
            _storage.Save();

            return constructionDocuments;
        }
    }
}
