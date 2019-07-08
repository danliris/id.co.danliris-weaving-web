using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;

namespace Manufactures.Application.FabricConstructions.CommandHandlers
{
    public class UpdateConstructionCommandHandler : ICommandHandler<UpdateFabricConstructionCommand, FabricConstructionDocument>
    {
        private readonly IStorage _storage;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;
        public readonly IMaterialTypeRepository _materialTypeRepository;
        public readonly IYarnNumberRepository _yarnNumberRepository;

        public UpdateConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository = 
                _storage.GetRepository<IFabricConstructionRepository>();
            _yarnDocumentRepository = 
                _storage.GetRepository<IYarnDocumentRepository>();
            _materialTypeRepository = 
                _storage.GetRepository<IMaterialTypeRepository>();
            _yarnNumberRepository = 
                _storage.GetRepository<IYarnNumberRepository>();
        }

        public async Task<FabricConstructionDocument> Handle(UpdateFabricConstructionCommand request,
                                                       CancellationToken cancellationToken)
        {
            var query = _constructionDocumentRepository.Query;
            var constructionDocument = 
                _constructionDocumentRepository
                    .Find(query)
                    .Where(Entity => Entity.Identity.Equals(request.Id))
                    .FirstOrDefault();

            var exsistingConstructionNumber = 
                _constructionDocumentRepository
                    .Find(construction => construction.ConstructionNumber.Equals(request.ConstructionNumber) &&
                                          construction.Deleted.Equals(false))
                    .Count() > 1;

            // Check Available construction document
            if (constructionDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Construction Document: " + request.Id));
            }

            // Check Available construction number if has defined
            if (exsistingConstructionNumber && !constructionDocument.Identity.Equals(request.Id))
            {
                throw Validator.ErrorValidation(("ConstructionNumber", "Construction Number " + request.ConstructionNumber + " has Available"));
            }

            constructionDocument.SetConstructionNumber(request.ConstructionNumber);
            constructionDocument.SetAmountOfWarp(request.AmountOfWarp);
            constructionDocument.SetAmountOfWeft(request.AmountOfWeft);
            constructionDocument.SetWidth(request.Width);
            constructionDocument.SetWovenType(request.WovenType);
            constructionDocument.SetWarpType(request.WarpTypeForm);
            constructionDocument.SetWeftType(request.WeftTypeForm);
            constructionDocument.SetTotalYarn(request.TotalYarn);
            constructionDocument.SetMaterialTypeName(request.MaterialTypeName);

            if (request.ReedSpace != 0)
            {
                constructionDocument.AddReedSpace(request.ReedSpace);
            }

            if (request.TotalEnds != 0)
            {
                constructionDocument.AddTotalEnds(request.TotalEnds);
            }

            // Update exsisting & remove if not has inside request & exsisting data
            foreach (var warp in constructionDocument.ListOfWarp)
            {
                var removedWarp = 
                    request
                        .ItemsWarp
                        .Where(o => o.YarnId == warp.YarnId)
                        .FirstOrDefault();

                if (removedWarp == null)
                {
                    constructionDocument.RemoveWarp(warp);
                }
            }

            foreach (var requestWarp in request.ItemsWarp)
            {

                var existingWarp = 
                    constructionDocument
                        .ListOfWarp
                        .Where(o => o.YarnId == requestWarp.YarnId)
                        .FirstOrDefault();

                if (existingWarp == null)
                {

                    constructionDocument.AddWarp(requestWarp);
                }
                else
                {

                    if (existingWarp.YarnId == requestWarp.YarnId)
                    {
                        constructionDocument.UpdateWarp(requestWarp);
                    }
                }
            }

            foreach (var weft in constructionDocument.ListOfWeft)
            {
                var removedWeft = 
                    request
                        .ItemsWeft
                        .Where(o => o.YarnId == weft.YarnId)
                        .FirstOrDefault();

                if (removedWeft == null)
                {
                    constructionDocument.RemoveWeft(weft);
                }
            }

            foreach (var requestweft in request.ItemsWeft)
            {
                var existingWeft = 
                    constructionDocument
                        .ListOfWeft
                        .Where(o => o.YarnId == requestweft.YarnId)
                        .FirstOrDefault();

                if (existingWeft == null)
                {

                    constructionDocument.AddWeft(requestweft);
                }
                else
                {
                    
                    if (existingWeft.YarnId == requestweft.YarnId)
                    {
                        constructionDocument.UpdateWeft(requestweft);
                    }
                }
            }

            await _constructionDocumentRepository.Update(constructionDocument);
            _storage.Save();

            return constructionDocument;
        }
    }
}
