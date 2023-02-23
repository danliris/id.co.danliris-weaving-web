using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class AddFabricDefectCommandHandler : ICommandHandler<AddFabricDefectCommand, FabricDefectDocument>
    {
        private readonly IStorage _storage;
        private readonly IFabricDefectRepository _fabricDefectRepository;

        public AddFabricDefectCommandHandler(IStorage storage)
        {
            _storage = storage;
            _fabricDefectRepository = _storage.GetRepository<IFabricDefectRepository>();
        }

        public async Task<FabricDefectDocument> Handle(AddFabricDefectCommand request, CancellationToken cancellationToken)
        {
            var existingFabricDefectByCode = 
                _fabricDefectRepository
                    .Find(y => y.DefectCode.Equals(request.DefectCode))
                    .FirstOrDefault();

            if (existingFabricDefectByCode != null)
            {
                throw Validator.ErrorValidation(("DefectCode", "Kode Cacat Sudah Ada"));
            }

            string defectCategory;
            switch (request.DefectCategory)
            {
                case "Dominan":
                    defectCategory = FabricDefectCategory.DOMINANT;
                    break;
                case "Potong":
                    defectCategory = FabricDefectCategory.CUT;
                    break;
                default:
                    throw Validator.ErrorValidation(("DefectCategory", "Jenis Cacat Kain Harus Diisi"));
            }

            var newFabricDefect = new FabricDefectDocument(Guid.NewGuid(),
                                                           request.DefectCode,
                                                           request.DefectType,
                                                           defectCategory);

            await _fabricDefectRepository.Update(newFabricDefect);
            _storage.Save();

            return newFabricDefect;
        }
    }
}
