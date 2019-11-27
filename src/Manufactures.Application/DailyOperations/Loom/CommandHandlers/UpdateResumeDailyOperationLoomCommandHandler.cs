using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Loom.CommandHandlers
{
    public class UpdateResumeDailyOperationLoomCommandHandler : ICommandHandler<UpdateResumeDailyOperationLoomCommand, DailyOperationLoomDocument>
    {
        public async Task<DailyOperationLoomDocument> Handle(UpdateResumeDailyOperationLoomCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
