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
    public class CreateNewYarnCommandHandler : ICommandHandler<CreateNewYarnCommand, YarnDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDocumentRepository _yarnDocumentRepository;

        public CreateNewYarnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDocumentRepository = _storage.GetRepository<IYarnDocumentRepository>();
        }

        public async Task<YarnDocument> Handle(CreateNewYarnCommand request, CancellationToken cancellationToken)
        {
            var exsistingCode = _yarnDocumentRepository.Find(yarn => yarn.Code.Equals(request.Code)).Count() >= 1;

            if(exsistingCode)
            {
                throw Validator.ErrorValidation(("Code", "Code with " + request.Code + " has available"));
            }

            var newYarn = new YarnDocument(Guid.NewGuid(), 
                                           request.Code, 
                                           request.Name, 
                                           request.Description, 
                                           request.Tags,
                                           request.CoreCurrency, 
                                           request.CoreUom, 
                                           request.MaterialTypeDocument, 
                                           request.RingDocument, 
                                           request.SupplierDocument, 
                                           request.Price);

            await _yarnDocumentRepository.Update(newYarn);
            _storage.Save();

            return newYarn;
        }
    }
}
