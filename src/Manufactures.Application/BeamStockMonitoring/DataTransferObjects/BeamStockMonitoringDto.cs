using Manufactures.Domain.BeamStockMonitoring;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.BeamStockMonitoring.DataTransferObjects
{
    public class BeamStockMonitoringDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "BeamEntryDate")]
        public DateTimeOffset BeamEntryDate { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "StockLength")]
        public double StockLength { get; private set; }

        public void SetBeamStockId(Guid beamStockId)
        {
            Id = beamStockId;
        }

        public void SetBeamNumber(string beamNumber)
        {
            BeamNumber = beamNumber;
        }

        public void SetBeamEntryDate(DateTimeOffset beamEntryDate)
        {
            BeamEntryDate = beamEntryDate;
        }

        public void SetOrderNumber(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public void SetConstructionNumber(string constructionNumber)
        {
            ConstructionNumber = constructionNumber;
        }

        public void SetStockLength(double stockLength)
        {
            StockLength = stockLength;
        }
    }
}
