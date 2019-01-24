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
using MediatR;
using Moonlay;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class UpdateConstructionCommandHandler : ICommandHandler<UpdateConstructionCommand, ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;

        public UpdateConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
        }

        public async Task<ConstructionDocument> Handle(UpdateConstructionCommand request, 
                                                       CancellationToken cancellationToken)
        {
            var constructionDocuments = _constructionDocumentRepository.Find(Entity => Entity.Identity == request.Id)
                                                                       .FirstOrDefault();

            if (constructionDocuments == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Construction Document: " + request.Id));
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

            var constructionDetailsObj = new List<ConstructionDetail>();

            // Update Detail
            foreach (var detail in request.Warps)
            {
                foreach (var constructionDetail in constructionDocuments.ConstructionDetails)
                {
                    if (detail.Id == constructionDetail.Identity)
                    {
                        constructionDetail.SetQuantity(detail.Quantity);
                        constructionDetail.SetInformation(detail.Information);
                        constructionDetail.SetYarn(detail.Yarn.Serialize());

                        if(!detail.Detail.Contains(Constants.WARP))
                        {
                            constructionDetail.SetDetail(Constants.WARP);
                        }

                        constructionDetailsObj.Add(constructionDetail);
                    }
                }
            }

            foreach (var detail in request.Wefts)
            {
                foreach (var constructionDetail in constructionDocuments.ConstructionDetails)
                {
                    if (detail.Id == constructionDetail.Identity)
                    {
                        constructionDetail.SetQuantity(detail.Quantity);
                        constructionDetail.SetInformation(detail.Information);
                        constructionDetail.SetYarn(detail.Yarn.Serialize());

                        if (!detail.Detail.Contains(Constants.WEFT))
                        {
                            constructionDetail.SetDetail(Constants.WEFT);
                        }

                        constructionDetailsObj.Add(constructionDetail);
                    }
                }
            }
            
            constructionDocuments.SetConstructionDetail(constructionDetailsObj);

            await _constructionDocumentRepository.Update(constructionDocuments);
            _storage.Save();

            return constructionDocuments;
        }
    }
}
