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
            constructionDocument.SetWarps(request.Warps);
            constructionDocument.SetWefts(request.Wefts);

            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
