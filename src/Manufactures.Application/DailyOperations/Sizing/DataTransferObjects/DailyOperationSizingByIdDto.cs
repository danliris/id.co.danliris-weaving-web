using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingByIdDto : DailyOperationSizingListDto
    {
        [JsonProperty(PropertyName = "MachineType")]
        public string MachineType { get; private set; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; private set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; private set; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; private set; }

        [JsonProperty(PropertyName = "BeamProductResult")]
        public int BeamProductResult { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationSizingBeamsWarping")]
        public List<DailyOperationSizingBeamsWarpingDto> DailyOperationSizingBeamsWarping { get; set; }

        [JsonProperty(PropertyName = "DailyOperationSizingBeamProducts")]
        public List<DailyOperationSizingBeamProductDto> DailyOperationSizingBeamProducts { get; set; }

        [JsonProperty(PropertyName = "DailyOperationSizingHistories")]
        public List<DailyOperationSizingHistoryDto> DailyOperationSizingHistories { get; set; }

        public DailyOperationSizingByIdDto(DailyOperationSizingDocument document)
            : base(document)
        {
            DailyOperationSizingBeamsWarping = new List<DailyOperationSizingBeamsWarpingDto>();
            DailyOperationSizingBeamProducts = new List<DailyOperationSizingBeamProductDto>();
            DailyOperationSizingHistories = new List<DailyOperationSizingHistoryDto>();
        }

        public void SetMachineType(string machineType)
        {
            MachineType = machineType;
        }

        public void SetEmptyWeight(double emptyWeight)
        {
            EmptyWeight = emptyWeight;
        }

        public void SetYarnStrands(double yarnStrands)
        {
            YarnStrands = yarnStrands;
        }

        public void SetNeReal(double neReal)
        {
            NeReal = neReal;
        }

        public void SetBeamProductResult(int beamProductResult)
        {
            BeamProductResult = beamProductResult;
        }

        public void AddBeamsWarping(DailyOperationSizingBeamsWarpingDto beamsWarping)
        {
            DailyOperationSizingBeamsWarping.Add(beamsWarping);
        }

        public void AddDailyOperationSizingBeamProducts(DailyOperationSizingBeamProductDto beamProduct)
        {
            if (!DailyOperationSizingBeamProducts.Any(product => product.Id.Equals(beamProduct.Id)))
            {
                DailyOperationSizingBeamProducts.Add(beamProduct);
            }
        }

        public void AddDailyOperationSizingHistories(DailyOperationSizingHistoryDto history)
        {
            DailyOperationSizingHistories.Add(history);
        }
    }
}
