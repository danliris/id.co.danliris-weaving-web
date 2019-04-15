using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.Commands;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Operators.CommandHandlers
{
    public class AddOperatorCommandHandler 
        : ICommandHandler<AddOperatorCommand, OperatorDocument>
    {
        private readonly IStorage _storage;
        private readonly IOperatorRepository _operatorRepository;

        public AddOperatorCommandHandler(IStorage storage)
        {
            _storage = storage;
            _operatorRepository = 
                _storage.GetRepository<IOperatorRepository>();
        }

        public async Task<OperatorDocument> Handle(AddOperatorCommand request, 
                                                   CancellationToken cancellationToken)
        {
            var coreAccount = 
                new CoreAccount(request.CoreAccount.MongoId, 
                                request.CoreAccount.Id.HasValue ? 
                                    request.CoreAccount.Id.Value : 0);
            var newOperator = 
                new OperatorDocument(Guid.NewGuid(), 
                                     coreAccount, 
                                     request.Group, 
                                     request.Assignment,
                                     request.Type);

            await _operatorRepository.Update(newOperator);
            _storage.Save();

            return newOperator;
        }
    }
}
