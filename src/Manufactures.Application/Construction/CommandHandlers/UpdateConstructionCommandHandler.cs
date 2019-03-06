using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.Construction.CommandHandlers
{
    public class UpdateConstructionCommandHandler : ICommandHandler<UpdateConstructionCommand, ConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;
        public readonly IMaterialTypeRepository _materialTypeRepository;
        public readonly IYarnNumberRepository _yarnNumberRepository;

        public UpdateConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
            _materialTypeRepository = _storage.GetRepository<IMaterialTypeRepository>();
            _yarnNumberRepository = _storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<ConstructionDocument> Handle(UpdateConstructionCommand request,
                                                       CancellationToken cancellationToken)
        {
            var query = _constructionDocumentRepository.Query;
            var constructionDocuments = _constructionDocumentRepository.Find(query.Include(o => o.ListOfWarp)).Where(Entity => Entity.Identity.Equals(request.Id))
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
            if (exsistingConstructionNumber && !constructionDocuments.Identity.Equals(request.Id))
            {
                throw Validator.ErrorValidation(("ConstructionNumber", "Construction Number " + request.ConstructionNumber + " has Available"));
            }

            constructionDocuments.SetConstructionNumber(request.ConstructionNumber);
            constructionDocuments.SetAmountOfWarp(request.AmountOfWarp);
            constructionDocuments.SetAmountOfWeft(request.AmountOfWeft);
            constructionDocuments.SetWidth(request.Width);
            constructionDocuments.SetWovenType(request.WovenType);
            constructionDocuments.SetWarpType(request.WarpTypeForm);
            constructionDocuments.SetWeftType(request.WeftTypeForm);
            constructionDocuments.SetTotalYarn(request.TotalYarn);
            constructionDocuments.SetMaterialTypeId(new MaterialTypeId(Guid.Parse(request.MaterialTypeId)));

            // Update exsisting & remove if not has inside request & exsisting data
            foreach (var warp in constructionDocuments.ListOfWarp)
            {
                var countIndetailWarp = request.ItemsWarp.Where(o => o.YarnId == warp.YarnId).Count();

                if (countIndetailWarp == 0)
                {
                    constructionDocuments.RemoveWarp(warp);
                }
            }

            foreach (var weft in constructionDocuments.ListOfWeft)
            {
                var countIndetailWeft = request.ItemsWeft.Where(o => o.YarnId == weft.YarnId).Count();

                if (countIndetailWeft == 0)
                {
                    constructionDocuments.RemoveWeft(weft);
                }
            }

            // Update Detail
            foreach (var warp in request.ItemsWarp)
            {
                constructionDocuments.AddWarp(warp);
            }

            foreach (var weft in request.ItemsWeft)
            {
                constructionDocuments.AddWeft(weft);
            }

            await _constructionDocumentRepository.Update(constructionDocuments);
            _storage.Save();

            return constructionDocuments;
        }
    }
}
