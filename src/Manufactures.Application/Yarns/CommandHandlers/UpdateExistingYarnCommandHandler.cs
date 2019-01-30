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
            var yarnDocument = _yarnDocumentRepository.Find(yarn => yarn.Identity == request.Id).FirstOrDefault();

            if(yarnDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid ring Id: " + request.Id));
            }

            var exsistingCode = _yarnDocumentRepository.Find(yarn => yarn.Code.Equals(request.Code)).Count() >= 1;

            if (exsistingCode && yarnDocument.Code != request.Code)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            yarnDocument.SetCode(request.Code);
            yarnDocument.SetName(request.Name);
            yarnDocument.SetDescription(request.Description);
            yarnDocument.SetTags(request.Tags);
            yarnDocument.SetCurrency(request.CoreCurrency);
            yarnDocument.SetUom(request.CoreUom);
            yarnDocument.SetMaterialTypeDocument(request.MaterialTypeDocument);
            yarnDocument.SetRingDocument(request.RingDocument);
            yarnDocument.SetPrice(request.Price);

            await _yarnDocumentRepository.Update(yarnDocument);
            _storage.Save();

            return yarnDocument;
        }
    }
}
