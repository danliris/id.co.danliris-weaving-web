using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.Commands;
using Manufactures.Domain.Operators.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Operators.CommandHandlers
{
    public class DeleteOperatorCommandHandler 
        : ICommandHandler<RemoveOperatorCommand, OperatorDocument>
    {
        private readonly IStorage _storage;
        private readonly IOperatorRepository _operatorRepository;

        public DeleteOperatorCommandHandler(IStorage storage)
        {
            _storage = storage;
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
        }

        public async Task<OperatorDocument> Handle(RemoveOperatorCommand request, CancellationToken cancellationToken)
        {
            var existingOperator =
                _operatorRepository.Find(o => o.Identity.Equals(request.Id))
                                   .FirstOrDefault();

            if (existingOperator == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Operator Id: " + request.Id));
            }

            existingOperator.Remove();

            await _operatorRepository.Update(existingOperator);

            _storage.Save();

            return existingOperator;
        }
    }
}
