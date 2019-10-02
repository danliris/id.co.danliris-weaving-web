using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Defects.YarnDefect;
using Manufactures.Domain.Defects.YarnDefect.Commands;
using Manufactures.Domain.Defects.YarnDefect.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Defects.YarnDefect.CommandHandlers
{
    public class RemoveYarnDefectCommandHandler : ICommandHandler<RemoveYarnDefectCommand, YarnDefectDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDefectRepository _yarnDefectRepository;

        public RemoveYarnDefectCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDefectRepository = _storage.GetRepository<IYarnDefectRepository>();
        }

        public async Task<YarnDefectDocument> Handle(RemoveYarnDefectCommand request, CancellationToken cancellationToken)
        {
            var query = _yarnDefectRepository.Query.Where(y => y.Identity.Equals(request.Id));

            var existingYarnDefectById = _yarnDefectRepository
                                        .Find(query)
                                        .FirstOrDefault();

            if (existingYarnDefectById == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Yarn Defect with : " + request.Id));
            }

            existingYarnDefectById.Remove();

            await _yarnDefectRepository.Update(existingYarnDefectById);
            _storage.Save();

            return existingYarnDefectById;
        }
    }
}
