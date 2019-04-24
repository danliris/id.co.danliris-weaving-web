using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.Operators.Repositories;

namespace Manufactures.Data.EntityFrameworkCore.Operators.Repositories
{
    class OperatorRepository : AggregateRepostory<OperatorDocument, OperatorReadModel>, IOperatorRepository
    {
        protected override OperatorDocument Map(OperatorReadModel readModel)
        {
            return new OperatorDocument(readModel);
        }
    }
}
