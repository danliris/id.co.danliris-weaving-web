using Infrastructure.Domain.Repositories;
using Manufactures.Domain.Operators.ReadModels;

namespace Manufactures.Domain.Operators.Repositories
{
    public interface IOperatorRepository : IAggregateRepository<OperatorDocument, OperatorReadModel>
    {
    }
}
