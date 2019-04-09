using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class DailyOperationalMachinesValueObject : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; private set; }

        [JsonProperty(PropertyName = "MachineId")]
        public string MachineId { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public int UnitId { get; private set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public List<Origin> WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public List<Origin> WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "Shift")]
        public string Shift { get; private set; }

        [JsonProperty(PropertyName = "DOMTime")]
        public DOMTimeValueObject DOMTime { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "BeamOperator")]
        public string BeamOperator { get; private set; }

        [JsonProperty(PropertyName = "LoomGroup")]
        public string LoomGroup { get; private set; }

        [JsonProperty(PropertyName = "SizingNumber")]
        public string SizingNumber { get; private set; }

        [JsonProperty(PropertyName = "SizingOperator")]
        public string SizingOperator { get; private set; }

        [JsonProperty(PropertyName = "SizingGroup")]
        public string SizingGroup { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        public DailyOperationalMachinesValueObject(DateTimeOffset dateOperated, string machineId, int unitId, string orderNumber, List<Origin> warpOrigin, List<Origin> weftOrigin, string constructionNumber, string shift, DOMTimeValueObject domTime, string beamNumber, string beamOperator, string loomGroup, string sizingNumber, string sizingOperator, string sizingGroup, string information)
        {
            DateOperated = dateOperated;
            MachineId = machineId;
            UnitId = unitId;
            OrderNumber = orderNumber;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
            ConstructionNumber = constructionNumber;
            Shift = shift;
            DOMTime = domTime;
            BeamNumber = beamNumber;
            BeamOperator = beamOperator;
            LoomGroup = loomGroup;
            SizingNumber = sizingNumber;
            SizingOperator = sizingOperator;
            SizingGroup = sizingGroup;
            Information = information;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DateOperated;
            yield return MachineId;
            yield return UnitId;
            yield return OrderNumber;
            yield return WarpOrigin;
            yield return WeftOrigin;
            yield return ConstructionNumber;
            yield return Shift;
            yield return DOMTime;
            yield return BeamNumber;
            yield return BeamOperator;
            yield return LoomGroup;
            yield return SizingNumber;
            yield return SizingOperator;
            yield return SizingGroup;
            yield return Information;
        }
    }
}
