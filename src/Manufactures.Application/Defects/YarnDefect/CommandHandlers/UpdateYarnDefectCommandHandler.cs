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
    public class UpdateYarnDefectCommandHandler : ICommandHandler<UpdateYarnDefectCommand, YarnDefectDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDefectRepository _yarnDefectRepository;

        public UpdateYarnDefectCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDefectRepository = _storage.GetRepository<IYarnDefectRepository>();
        }

        public async Task<YarnDefectDocument> Handle(UpdateYarnDefectCommand request, CancellationToken cancellationToken)
        {
            var query = _yarnDefectRepository.Query.Where(y => y.Identity.Equals(request.Id));

            var existingYarnDefectById = _yarnDefectRepository
                                        .Find(query)
                                        .FirstOrDefault();

            if (existingYarnDefectById == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Yarn Defect with : " + request.Id));
            }

            existingYarnDefectById.SetDefectCode(request.DefectCode);
            existingYarnDefectById.SetDefectType(request.DefectType);
            existingYarnDefectById.SetDefectCategory(request.DefectCategory);

            await _yarnDefectRepository.Update(existingYarnDefectById);
            _storage.Save();

            return existingYarnDefectById;
        }
    }
}
