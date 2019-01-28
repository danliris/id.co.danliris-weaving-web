using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Yarns.CommandHandlers
{
    public class UpdateExistingYarnCommandHandler : ICommandHandler<UpdateExsistingYarnCommand, YarnDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;

        public UpdateExistingYarnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
        }

        public async Task<YarnDocument> Handle(UpdateExsistingYarnCommand request, CancellationToken cancellationToken)
        {
            var exsistingYarn = _yarnDocumentRepository.Find(yarn => yarn.Identity == request.Id).FirstOrDefault();

            if(exsistingYarn == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid ring Id: " + request.Id));
            }

            var exsistingCode = _yarnDocumentRepository.Find(yarn => yarn.Code.Equals(request.Code)).Count() >= 1;

            if (exsistingCode)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            exsistingYarn.SetCode(request.Code);
            exsistingYarn.SetName(request.Name);
            exsistingYarn.SetDescription(request.Description);
            exsistingYarn.SetTags(request.Tags);
            exsistingYarn.SetCurrency(request.CoreCurrency);
            exsistingYarn.SetUom(request.CoreUom);
            exsistingYarn.SetMaterialTypeDocument(request.MaterialTypeDocument);
            exsistingYarn.SetRingDocument(request.RingDocument);
            exsistingYarn.SetSupplierDocument(request.SupplierDocument);
            exsistingYarn.SetPrice(request.Price);

            await _yarnDocumentRepository.Update(exsistingYarn);
            _storage.Save();

            return exsistingYarn;
        }
    }
}
