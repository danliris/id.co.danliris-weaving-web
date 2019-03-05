using System;
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
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Materials.Repositories;
using Manufactures.Domain.YarnNumbers.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;

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
            var constructionDocuments = _constructionDocumentRepository.Find(query.Include(o => o.ConstructionDetails)).Where(Entity => Entity.Identity.Equals(request.Id))
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
            constructionDocuments.SetMaterialType(request.MaterialTypeDocument);

            // Update exsisting & remove if not has inside request & exsisting data
            foreach (var detail in constructionDocuments.ConstructionDetails)
            {
                var countIndetailWarp = request.ItemsWarp.Where(o => o.Id == detail.Identity).Count();
                var countIndetailWeft = request.ItemsWeft.Where(o => o.Id == detail.Identity).Count();

                if (countIndetailWarp == 0 && detail.Detail == Constants.WARP)
                {
                    var list = constructionDocuments.ConstructionDetails.ToList();
                    list.Remove(detail);
                    constructionDocuments.UpdateConstructionDetail(list);
                }

                if (countIndetailWeft == 0 && detail.Detail == Constants.WEFT)
                {
                    var list = constructionDocuments.ConstructionDetails.ToList();
                    list.Remove(detail);
                    constructionDocuments.UpdateConstructionDetail(list);
                }
            }

            // Update Detail
            foreach (var warp in request.ItemsWarp)
            {
                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(warp.Yarn.Id)).FirstOrDefault();
                var materialTypeDocument = _materialTypeRepository.Find(o => o.Identity == yarnDocument.MaterialTypeId.Value).FirstOrDefault();
                var yarnNumberDocument = _yarnNumberRepository.Find(o => o.Identity == yarnDocument.YarnNumberId.Value).FirstOrDefault();
                var detail = constructionDocuments.ConstructionDetails.ToList()
                                .Where(o => o.Detail.Equals(Constants.WARP) && 
                                            o.Yarn.Deserialize<YarnValueObject>().Id == yarnDocument.Identity)
                                                  .FirstOrDefault();

                if (detail != null)
                {
                    detail.SetQuantity(warp.Quantity);
                    detail.SetInformation(warp.Information);
                }
                else
                {
                    if (yarnDocument != null)
                    {
                        var yarn = new YarnValueObject(yarnDocument.Identity, yarnDocument.Code, yarnDocument.Name, materialTypeDocument.Code, yarnNumberDocument.Code);
                        ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                                       warp.Quantity,
                                                                                       warp.Information,
                                                                                       yarn,
                                                                                       Constants.WARP);
                        constructionDocuments.AddConstructionDetail(constructionDetail);
                    }
                }
            }

            foreach (var weft in request.ItemsWeft)
            {
                var yarnDocument = _yarnDocumentRepository.Find(o => o.Identity.Equals(weft.Yarn.Id)).FirstOrDefault();
                var materialTypeDocument = _materialTypeRepository.Find(o => o.Identity == yarnDocument.MaterialTypeId.Value).FirstOrDefault();
                var yarnNumberDocument = _yarnNumberRepository.Find(o => o.Identity == yarnDocument.YarnNumberId.Value).FirstOrDefault();
                var detail = constructionDocuments.ConstructionDetails.ToList()
                                .Where(o => o.Detail.Equals(Constants.WEFT) && 
                                            o.Yarn.Deserialize<YarnValueObject>().Id == yarnDocument.Identity)
                                                  .FirstOrDefault();

                if (detail != null)
                {
                    detail.SetQuantity(weft.Quantity);
                    detail.SetInformation(weft.Information);
                }
                else
                {
                    if (yarnDocument != null)
                    {
                        var yarn = new YarnValueObject(yarnDocument.Identity, yarnDocument.Code, yarnDocument.Name, materialTypeDocument.Code, yarnNumberDocument.Code);
                        ConstructionDetail constructionDetail = new ConstructionDetail(Guid.NewGuid(),
                                                                                       weft.Quantity,
                                                                                       weft.Information,
                                                                                       yarn,
                                                                                       Constants.WEFT);
                        constructionDocuments.AddConstructionDetail(constructionDetail);
                    }
                }
            }

            await _constructionDocumentRepository.Update(constructionDocuments);
            _storage.Save();

            return constructionDocuments;
        }
    }
}
