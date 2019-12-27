using ExtCore.Data.Abstractions;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.DataTransferObjects.Beams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.Beams.QueryHandlers
{
    public class BeamQueryHandler : IBeamQuery<BeamListDto>
    {
        private readonly IStorage _storage;
        private readonly IBeamRepository _beamRepository;

        public BeamQueryHandler(IStorage storage)
        {
            _storage = storage;
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
        }

        public Task<IEnumerable<BeamListDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<BeamListDto> GetById(Guid id)
        {
            var query =
                _beamRepository
                    .Query
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var existingBeam = 
                _beamRepository.Find(query)
                    .Where(x => x.Identity.Equals(id))
                    .Select(y => new BeamListDto(y))
                    .FirstOrDefault();

            return existingBeam;
        }
    }
}
