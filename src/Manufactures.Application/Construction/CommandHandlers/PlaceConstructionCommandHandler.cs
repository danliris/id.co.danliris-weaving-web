using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Data.EntityFrameworkCore.Construction.Repositories;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<ConstructionDocument> Handle(PlaceConstructionCommand request, CancellationToken cancellationToken)
        {
            var constructionDocument = new ConstructionDocument(id: Guid.NewGuid(),
                                                                constructionNumber: request.ConstructionNumber,
                                                                amountOfWarp: request.AmountOfWarp,
                                                                amountOfWeft: request.AmountOfWeft,
                                                                width: request.Width,
                                                                wofenType: request.WovenType,
                                                                warpType: request.WarpType,
                                                                weftType: request.WeftType,
                                                                totalYarn: request.TotalYarn,
                                                                materialType: request.MaterialType);

            constructionDocument.SetWarps(request.Warps);
            constructionDocument.SetWefts(request.Wefts);
            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
