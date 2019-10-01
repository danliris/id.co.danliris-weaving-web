using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
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
    public class AddYarnDefectCommandHandler : ICommandHandler<AddYarnDefectCommand, YarnDefectDocument>
    {
        private readonly IStorage _storage;
        private readonly IYarnDefectRepository _yarnDefectRepository;

        public AddYarnDefectCommandHandler(IStorage storage)
        {
            _storage = storage;
            _yarnDefectRepository = _storage.GetRepository<IYarnDefectRepository>();
        }

        public async Task<YarnDefectDocument> Handle(AddYarnDefectCommand request, CancellationToken cancellationToken)
        {
            var existingYarnDefectByCode = _yarnDefectRepository
                                        .Find(y => y.DefectCode.Equals(request.DefectCode))
                                        .FirstOrDefault();

            if (existingYarnDefectByCode != null)
            {
                throw Validator.ErrorValidation(("DefectCode", "Kode Cacat Sudah Ada"));
            }

            string defectCategory;
            switch (request.DefectCategory)
            {
                case "Dominan":
                    defectCategory = YarnDefectCategory.DOMINANT;
                    break;
                case "Potong":
                    defectCategory = YarnDefectCategory.CUT;
                    break;
                default:
                    throw Validator.ErrorValidation(("DefectCategory", "Jenis Cacat Kain Harus Diisi"));
            }

            var newYarnDefect = new YarnDefectDocument(Guid.NewGuid(),
                                                       request.DefectCode,
                                                       request.DefectType,
                                                       defectCategory);

            await _yarnDefectRepository.Update(newYarnDefect);
            _storage.Save();

            return newYarnDefect;
        }
    }
}
