using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
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

        public async Task<ConstructionDocument> Handle(UpdateConstructionCommand request, CancellationToken cancellationToken)
        {
            var constructionDocument = _constructionDocumentRepository.Find(Entity => Entity.Identity == request.Id).FirstOrDefault();

            if (constructionDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Construction Document: " + request.Id));
            }
            
            constructionDocument.SetConstructionNumber(request.ConstructionNumber);
            constructionDocument.SetAmountOfWarp(request.AmountOfWarp);
            constructionDocument.SetAmountOfWeft(request.AmountOfWeft);
            constructionDocument.SetWidth(request.Width);
            constructionDocument.SetWovenType(request.WovenType);
            constructionDocument.SetWarpType(request.WarpType);
            constructionDocument.SetWeftType(request.WeftType);
            constructionDocument.SetTotalYarn(request.TotalYarn);
            constructionDocument.SetMaterialType(request.MaterialType);

            var constructionDetailsObj = new List<ConstructionDetail>();

            // Update Detail
            foreach (var detail in request.Warps)
            {
                foreach (var constructionDetail in constructionDocument.ConstructionDetails)
                {
                    if (detail.Id == constructionDetail.Identity)
                    {
                        constructionDetail.SetQuantity(detail.Quantity);
                        constructionDetail.SetInformation(detail.Information);
                        constructionDetail.SetYarn(detail.Yarn.Serialize());
                        constructionDetail.SetDetail(detail.Detail);

                        constructionDetailsObj.Add(constructionDetail);
                    }
                }
            }

            foreach (var detail in request.Wefts)
            {
                foreach (var constructionDetail in constructionDocument.ConstructionDetails)
                {
                    if (detail.Id == constructionDetail.Identity)
                    {
                        constructionDetail.SetQuantity(detail.Quantity);
                        constructionDetail.SetInformation(detail.Information);
                        constructionDetail.SetYarn(detail.Yarn.Serialize());
                        constructionDetail.SetDetail(detail.Detail);

                        constructionDetailsObj.Add(constructionDetail);
                    }
                }
            }

            constructionDocument.SetConstructionDetail(constructionDocument.ConstructionDetails);

            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
