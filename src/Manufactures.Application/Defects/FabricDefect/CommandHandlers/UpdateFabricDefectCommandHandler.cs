using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Defects.FabricDefect;
using Manufactures.Domain.Defects.FabricDefect.Commands;
using Manufactures.Domain.Defects.FabricDefect.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Defects.FabricDefect.CommandHandlers
{
    public class UpdateFabricDefectCommandHandler : ICommandHandler<UpdateFabricDefectCommand, FabricDefectDocument>
    {
        private readonly IStorage _storage;
        private readonly IFabricDefectRepository _fabricDefectRepository;

        public UpdateFabricDefectCommandHandler(IStorage storage)
        {
            _storage = storage;
            _fabricDefectRepository = _storage.GetRepository<IFabricDefectRepository>();
        }

        public async Task<FabricDefectDocument> Handle(UpdateFabricDefectCommand request, CancellationToken cancellationToken)
        {
            var query = _fabricDefectRepository.Query.Where(y => y.Identity.Equals(request.Id));

            var existingFabricDefectById = _fabricDefectRepository
                                        .Find(query)
                                        .FirstOrDefault();

            if (existingFabricDefectById == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Fabric Defect with : " + request.Id));
            }

            existingFabricDefectById.SetDefectCode(request.DefectCode);
            existingFabricDefectById.SetDefectType(request.DefectType);
            existingFabricDefectById.SetDefectCategory(request.DefectCategory);

            await _fabricDefectRepository.Update(existingFabricDefectById);
            _storage.Save();

            return existingFabricDefectById;
        }
    }
}
