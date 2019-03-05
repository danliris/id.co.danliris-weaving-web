using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.Repositories;
using Moonlay;
using System;
using System.Linq;
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
            var exsistingCode = _yarnDocumentRepository.Find(yarn => yarn.Code.Equals(request.Code) && 
                                                                     yarn.Deleted.Equals(false)).Count() >= 1;

            if (yarnDocument == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid ring Id: " + request.Id));
            }
            
            // Check has exsisting code
            if (exsistingCode && !yarnDocument.Code.Equals(request.Code))
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            yarnDocument.SetCode(request.Code);
            yarnDocument.SetName(request.Name);
            yarnDocument.SetTags(request.Tags);
            yarnDocument.SetMaterialTypeId(new MaterialTypeId(Guid.Parse(request.MaterialTypeId)));
            yarnDocument.SetYarnNumberId(new YarnNumberId(Guid.Parse(request.YarnNumberId)));

            await _yarnDocumentRepository.Update(yarnDocument);
            _storage.Save();

            return yarnDocument;
        }
    }
}
