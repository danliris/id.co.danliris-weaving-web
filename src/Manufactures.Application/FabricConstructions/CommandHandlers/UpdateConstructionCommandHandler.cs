using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;

namespace Manufactures.Application.FabricConstructions.CommandHandlers
{
    public class UpdateConstructionCommandHandler : ICommandHandler<UpdateFabricConstructionCommand, FabricConstructionDocument>
    {
        private readonly IStorage 
            _storage;
        private readonly IFabricConstructionRepository 
            _constructionDocumentRepository;
        private readonly IConstructionYarnDetailRepository
            _constructionYarnDetailRepository;

        public UpdateConstructionCommandHandler(IStorage storage)
        {
            _storage = storage;
            _constructionDocumentRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _constructionYarnDetailRepository =
                _storage.GetRepository<IConstructionYarnDetailRepository>();
        }

        public async Task<FabricConstructionDocument> Handle(UpdateFabricConstructionCommand request, CancellationToken cancellationToken)
        {
            //Get Construction Document
            var constructionDocument = 
                _constructionDocumentRepository
                    .Find(o => o.Identity == request.Id)
                    .FirstOrDefault();

            //Get Constructions Yarn Detail w/ Same Construction Document
            var constructionYarnDetails =
                _constructionYarnDetailRepository
                    .Find(o => o.FabricConstructionDocumentId == constructionDocument.Identity && o.Deleted.Equals(false));

            //await Task.Yield();
            //List<ConstructionYarnDetailCommand> existingYarnDetails = new List<ConstructionYarnDetailCommand>();
            //foreach (var existingYarnDetail in constructionYarnDetails)
            //{
            //    var existingYarnDetailCommand = new ConstructionYarnDetailCommand
            //    {
            //        Id = existingYarnDetail.Identity,
            //        YarnId = existingYarnDetail.YarnId.Value,
            //        Quantity = existingYarnDetail.Quantity,
            //        Information = existingYarnDetail.Information,
            //        Type = existingYarnDetail.Type,
            //        FabricConstructionDocumentId = constructionDocument.Identity
            //    };

            //    existingYarnDetails.Add(existingYarnDetailCommand);
            //}

            await Task.Yield();
            //Get Same Construction Number
            var existingConstructionNumber = 
                _constructionDocumentRepository
                    .Find(o => o.ConstructionNumber == request.ConstructionNumber &&
                               o.Deleted.Equals(false))
                    .Count() > 1;

            // Check Available Construction Document
            if (constructionDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Konstruksi Tidak Ditemukan: " + request.Id));
            }

            // Check Available If Construction Number Exist
            if (existingConstructionNumber == true)
            {
                throw Validator.ErrorValidation(("ConstructionNumber", "No. Konstruksi " + request.ConstructionNumber + " Sudah Pernah Dibuat"));
            }

            constructionDocument.SetConstructionNumber(request.ConstructionNumber);
            constructionDocument.SetMaterialType(request.MaterialType);
            constructionDocument.SetWovenType(request.WovenType);
            constructionDocument.SetAmountOfWarp(request.AmountOfWarp);
            constructionDocument.SetAmountOfWeft(request.AmountOfWeft);
            constructionDocument.SetWidth(request.Width);
            constructionDocument.SetWarpType(request.WarpType);
            constructionDocument.SetWeftType(request.WeftType);
            constructionDocument.SetReedSpace(request.ReedSpace);
            constructionDocument.SetYarnStrandsAmount(request.YarnStrandsAmount);
            constructionDocument.SetTotalYarn(request.TotalYarn);

            var mergedYarnDetails = request.ConstructionWarpsDetail.Concat(request.ConstructionWeftsDetail).ToList();

            //Exist in both UI and Db, Updated
            var updatedDetails = mergedYarnDetails.Where(o => constructionYarnDetails.Any(d => d.Identity == o.Id));
            foreach(var updatedDetail in updatedDetails)
            {
                var dbDetail = constructionYarnDetails.Find(o => o.Identity == updatedDetail.Id);

                await _constructionYarnDetailRepository.Update(dbDetail);
            }

            //Exist in UI but not in Db, Added
            var addedDetails = mergedYarnDetails.Where(o => !constructionYarnDetails.Any(d => d.Identity == o.Id));
            addedDetails
                .Select(o => new ConstructionYarnDetail(Guid.NewGuid(), 
                                                        new YarnId(o.YarnId), 
                                                        o.Quantity, 
                                                        o.Information, 
                                                        o.Type, 
                                                        o.FabricConstructionDocumentId))
                .ToList()
                .ForEach(async o => await _constructionYarnDetailRepository.Update(o));

            //Exist in Db but not from UI, Deleted
            var deletedDetails = constructionYarnDetails.Where(o => !mergedYarnDetails.Any(d => d.Id == o.Identity));
            foreach(var deletedDetail in deletedDetails)
            {
                deletedDetail.SetDeleted();
                await _constructionYarnDetailRepository.Update(deletedDetail);
            }

            constructionDocument.SetModified();

            await _constructionDocumentRepository.Update(constructionDocument);

            _storage.Save();

            return constructionDocument;
        }
    }
}
