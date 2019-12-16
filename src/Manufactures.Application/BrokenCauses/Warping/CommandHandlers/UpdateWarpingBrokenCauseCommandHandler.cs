using Infrastructure.Domain.Commands;
using Manufactures.Domain.BrokenCauses.Warping;
using Manufactures.Domain.BrokenCauses.Warping.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.BrokenCauses.Warping.CommandHandlers
{
    public class UpdateWarpingBrokenCauseCommandHandler : ICommandHandler<UpdateWarpingBrokenCauseCommand, WarpingBrokenCauseDocument>
    {
    }
}
