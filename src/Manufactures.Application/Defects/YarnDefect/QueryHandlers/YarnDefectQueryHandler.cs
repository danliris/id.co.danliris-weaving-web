using ExtCore.Data.Abstractions;
using Manufactures.Application.Defects.YarnDefect.DataTransferObjects;
using Manufactures.Domain.Defects.YarnDefect.Queries;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Defects.YarnDefect.QueryHandlers
{
    public class YarnDefectQueryHandler : IYarnDefectQuery<YarnDefectDto>
    {
        private readonly IStorage _storage;
        private readonly IYarnDefectRepository _yarnDefectRepository;

        public YarnDefectQueryHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDefectRepository = _storage.GetRepository<IYarnDefectRepository>();
        }

        public async Task<IEnumerable<YarnDefectDto>> GetAll()
        {
            var query = _yarnDefectRepository
                            .Query.OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var existingYarnDefect = _yarnDefectRepository.Find(query).Select(y=>new YarnDefectDto(y));

            return existingYarnDefect;
        }

        public async Task<YarnDefectDto> GetById(Guid id)
        {
            var query = _yarnDefectRepository
                            .Query.OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var existingYarnDefect = _yarnDefectRepository
                                        .Find(query)
                                        .Where(y=>y.Identity.Equals(id))
                                        .Select(y => new YarnDefectDto(y))
                                        .FirstOrDefault();

            return existingYarnDefect;
        }
    }
}
