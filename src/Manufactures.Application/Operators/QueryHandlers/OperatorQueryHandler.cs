using ExtCore.Data.Abstractions;
using Manufactures.Application.Operators.DTOs;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Operators.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.Operators.QueryHandlers
{
    public class OperatorQueryHandler : IOperatorQuery<OperatorListDto>
    {
        private readonly IStorage _storage;
        private readonly IOperatorRepository _operatorRepository;

        public OperatorQueryHandler(IStorage storage)
        {
            _storage = storage;
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
        }

        public Task<IEnumerable<OperatorListDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<OperatorListDto> GetById(Guid id)
        {
            var query =
                _operatorRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var existingOperator =
                _operatorRepository.Find(query)
                    .Where(x => x.Identity.Equals(id))
                    .Select(y => new OperatorListDto(y))
                    .FirstOrDefault();

            return existingOperator;
        }
    }
}
