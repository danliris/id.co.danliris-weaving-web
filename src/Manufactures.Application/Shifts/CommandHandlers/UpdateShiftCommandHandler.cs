using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.Commands;
using Manufactures.Domain.Shifts.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Shifts.CommandHandlers
{
    public class UpdateShiftCommandHandler : ICommandHandler<UpdateShiftCommand, ShiftDocument>
    {
        private readonly IStorage _storage;
        private readonly IShiftRepository _shiftRepository;

        public UpdateShiftCommandHandler(IStorage storage)
        {
            _storage = storage;
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public async Task<ShiftDocument> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            var existingShift = _shiftRepository.Find(o => o.Identity.Equals(request.Id)).FirstOrDefault();
            var startTime = TimeSpan.Parse(request.StartTime);
            var endTime = TimeSpan.Parse(request.EndTime);

            if (existingShift == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid ShiftDocumentId Id: " + request.Id));
            }
            
            existingShift.SetName(request.Name);
            existingShift.SetStartTime(startTime);
            existingShift.SetEndTime(endTime);

            await _shiftRepository.Update(existingShift);
            _storage.Save();

            return existingShift;
        }
    }
}
