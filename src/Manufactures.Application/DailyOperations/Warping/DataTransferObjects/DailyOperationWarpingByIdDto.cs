using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingByIdDto : DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "OrderDocumentId")]
        public Guid OrderDocumentId { get; private set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; private set; }

        [JsonProperty(PropertyName = "ColourOfCones")]
        public string ColourOfCones { get; private set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; private set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; private set; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; private set; }

        [JsonProperty(PropertyName = "TotalWarpingBeamLength")]
        public double TotalWarpingBeamLength { get; private set; }

        [JsonProperty(PropertyName = "CountWarpingBeamProducts")]
        public double CountWarpingBeamProducts { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingBeamProducts")]
        public List<DailyOperationWarpingBeamProductDto> DailyOperationWarpingBeamProducts { get; set; }

        [JsonProperty(PropertyName = "DailyOperationWarpingHistories")]
        public List<DailyOperationWarpingHistoryDto> DailyOperationWarpingHistories { get; set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            OrderDocumentId = document.OrderDocumentId.Value;
            AmountOfCones = document.AmountOfCones;
            ColourOfCones = document.ColourOfCone;
            DailyOperationWarpingBeamProducts = new List<DailyOperationWarpingBeamProductDto>();
            DailyOperationWarpingHistories = new List<DailyOperationWarpingHistoryDto>();
        }

        //public void SetWeavingUnit(UnitId value)
        //{
        //    WeavingUnitId = value;
        //}

        public void SetOrderDocumentId(OrderId value)
        {
            OrderDocumentId = value.Value;
        }

        public void SetOperator(OperatorDocument value)
        {
            OperatorName = value.CoreAccount.Name;
            OperatorGroup = value.Group;
        }

        public void SetMaterialName(string value)
        {
            MaterialType = value;
        }

        public void SetTotalWarpingBeamLength(double value)
        {
            TotalWarpingBeamLength = value;
        }

        public void SetCountWarpingBeamProducts(double value)
        {
            CountWarpingBeamProducts = value;
        }

        public void AddDailyOperationBeamProducts(DailyOperationWarpingBeamProductDto value)
        {
            if (!DailyOperationWarpingBeamProducts.Any(product => product.Id.Equals(value.Id)))
            {
                DailyOperationWarpingBeamProducts.Add(value);
            }
        }

        public void AddDailyOperationWarpingHistories(DailyOperationWarpingHistoryDto value)
        {
            DailyOperationWarpingHistories.Add(value);
        }
    }
}
