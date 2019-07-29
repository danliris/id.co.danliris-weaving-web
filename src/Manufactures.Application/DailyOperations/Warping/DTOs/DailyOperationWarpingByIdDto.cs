using Manufactures.Domain.DailyOperations.Warping;

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
