using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.Commands;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Operators.CommandHandlers
{
    public class UpdateOperatorCommandHandler 
        : ICommandHandler<UpdateOperatorCommand, OperatorDocument>
    {
        private readonly IStorage _storage;
        private readonly IOperatorRepository _operatorRepository;

        public UpdateOperatorCommandHandler(IStorage storage)
        {
            _storage = storage;
            _operatorRepository = 
                _storage.GetRepository<IOperatorRepository>();
        }

        public async Task<OperatorDocument> Handle(UpdateOperatorCommand request, 
                                                   CancellationToken cancellationToken)
        {
            var coreAccount =
                new CoreAccount(request.CoreAccount.MongoId,
                                request.CoreAccount.Id.HasValue ?
                                    request.CoreAccount.Id.Value : 0, 
                                request.CoreAccount.Name);

            var existingOperator = 
                _operatorRepository.Find(o => o.Identity.Equals(request.Id))
                                   .FirstOrDefault();

            existingOperator.SetUnitId(new UnitId(request.UnitId.Id));
            existingOperator.SetCoreAccount(coreAccount);
            existingOperator.SetAssignment(request.Assignment);
            existingOperator.SetGroup(request.Group);
            existingOperator.SetType(request.Type);

            await _operatorRepository.Update(existingOperator);
            _storage.Save();

            return existingOperator;
        }
    }
}
