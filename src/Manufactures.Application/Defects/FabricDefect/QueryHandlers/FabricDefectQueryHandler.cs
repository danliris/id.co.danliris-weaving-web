using ExtCore.Data.Abstractions;
using Manufactures.Application.Defects.FabricDefect.DataTransferObjects;
using Manufactures.Domain.Defects.FabricDefect.Queries;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Defects.FabricDefect.QueryHandlers
{
    public class FabricDefectQueryHandler : IFabricDefectQuery<FabricDefectDto>
    {
        private readonly IStorage _storage;
        private readonly IFabricDefectRepository _fabricDefectRepository;

        public FabricDefectQueryHandler(IStorage storage)
        {
            _storage = storage;
            _fabricDefectRepository = _storage.GetRepository<IFabricDefectRepository>();
        }

        public async Task<IEnumerable<FabricDefectDto>> GetAll()
        {
            var query = _fabricDefectRepository
                            .Query.OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var existingFabricDefect = _fabricDefectRepository.Find(query).Select(y=>new FabricDefectDto(y));

            return existingFabricDefect;
        }

        public async Task<FabricDefectDto> GetById(Guid id)
        {
            var query = _fabricDefectRepository
                            .Query.OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var existingFabricDefect = _fabricDefectRepository
                                        .Find(query)
                                        .Where(y=>y.Identity.Equals(id))
                                        .Select(y => new FabricDefectDto(y))
                                        .FirstOrDefault();

            return existingFabricDefect;
        }
    }
}
