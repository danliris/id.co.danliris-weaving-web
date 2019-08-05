using ExtCore.Data.Abstractions;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Domain.Shifts.Queries;
using Manufactures.Domain.Shifts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.Shifts.QueryHandlers
{
    public class ShiftQueryHandler : IShiftQuery<ShiftDto>
    {
        private readonly IStorage _storage;
        private readonly IShiftRepository _shiftRepository;

        public ShiftQueryHandler(IStorage storage)
        {
            _storage = storage;
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();

        }
        public Task<IEnumerable<ShiftDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ShiftDto> GetById(Guid id)
        {
            var query =
               _shiftRepository
                   .Query
                   .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var existingShift =
                _shiftRepository.Find(query)
                    .Where(x => x.Identity.Equals(id))
                    .Select(y => new ShiftDto(y))
                    .FirstOrDefault();

            return existingShift;
        }
    }
}
