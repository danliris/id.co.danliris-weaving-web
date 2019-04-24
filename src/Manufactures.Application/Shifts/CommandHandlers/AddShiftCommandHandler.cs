using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.Commands;
using Manufactures.Domain.Shifts.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Shifts.CommandHandlers
{
    public class AddShiftCommandHandler : ICommandHandler<AddShiftCommand, ShiftDocument>
    {
        private readonly IStorage _storage;
        private readonly IShiftRepository _shiftRepository;

        public AddShiftCommandHandler(IStorage storage)
        {
            _storage = storage;
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public async Task<ShiftDocument> Handle(AddShiftCommand request, CancellationToken cancellationToken)
        {
            var startTime = request.StartTime.TimeOfDay;
            var endTime = request.EndTime.TimeOfDay;
            var newShift = new ShiftDocument(Guid.NewGuid(), 
                                             request.Name, 
                                             startTime.ToString(), 
                                             endTime.ToString());

            await _shiftRepository.Update(newShift);
            _storage.Save();

            return newShift;
        }
    }
}
