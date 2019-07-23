using Manufactures.Domain.DailyOperations.Warping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingByIdDto : DailyOperationWarpingListDto
    {
        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document) 
            : base(document)
        {

        }
    }
}
