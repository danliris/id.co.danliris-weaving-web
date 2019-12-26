using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.DataTransferObjects
{
    public class DailyOperationWarpingByIdDto : DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; private set; }

        [JsonProperty(PropertyName = "BeamProductResult")]
        public int BeamProductResult { get; private set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; private set; }

        //[JsonProperty(PropertyName = "TotalWarpingBeamLength")]
        //public double TotalWarpingBeamLength { get; private set; }

        //[JsonProperty(PropertyName = "CountWarpingBeamProducts")]
        //public int CountWarpingBeamProducts { get; private set; }

        //[JsonProperty(PropertyName = "IsFinishFlag")]
        //public bool IsFinishFlag { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingBeamProducts")]
        public List<DailyOperationWarpingBeamProductDto> DailyOperationWarpingBeamProducts { get; set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingHistories")]
        public List<DailyOperationWarpingHistoryDto> DailyOperationWarpingHistories { get; set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            AmountOfCones = document.AmountOfCones;
            DailyOperationWarpingBeamProducts = new List<DailyOperationWarpingBeamProductDto>();
            DailyOperationWarpingHistories = new List<DailyOperationWarpingHistoryDto>();
        }

        public void SetBeamProductResult(int beamProductResult)
        {
            BeamProductResult = beamProductResult;
        }

        public void SetMaterialType(string materialType)
        {
            MaterialType = materialType;
        }

        //public void SetIsFinishFlag (bool isFinishFlag)
        //{
        //    IsFinishFlag = isFinishFlag;
        //}

        //public void SetTotalWarpingBeamLength(double totalWarpingBeamLength)
        //{
        //    TotalWarpingBeamLength = totalWarpingBeamLength;
        //}

        //public void SetCountWarpingBeamProducts(int countWarpingBeamProducts)
        //{
        //    CountWarpingBeamProducts = countWarpingBeamProducts;
        //}

        public void AddDailyOperationWarpingBeamProducts(DailyOperationWarpingBeamProductDto beamProduct)
        {
            if (!DailyOperationWarpingBeamProducts.Any(product => product.Id.Equals(beamProduct.Id)))
            {
                DailyOperationWarpingBeamProducts.Add(beamProduct);
            }
        }

        public void AddDailyOperationWarpingHistories(DailyOperationWarpingHistoryDto history)
        {
            DailyOperationWarpingHistories.Add(history);
        }
    }
}
