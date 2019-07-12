using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentDto
    {
        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        //[JsonProperty(PropertyName = "Counter")]
        //public DailyOperationSizingCounterDto Counter { get; set; }

        //[JsonProperty(PropertyName = "Weight")]
        //public DailyOperationSizingWeightDto Weight { get; private set; }

        [JsonProperty(PropertyName = "Start")]
        public double Start { get; set; }

        [JsonProperty(PropertyName = "Finish")]
        public double Finish { get; set; }

        [JsonProperty(PropertyName = "Netto")]
        public double Netto { get; set; }

        [JsonProperty(PropertyName = "Bruto")]
        public double Bruto { get; set; }

        [JsonProperty(PropertyName = "Theoritical")]
        public double Theoritical { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; private set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; private set; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; private set; }

        public DailyOperationSizingBeamDocumentDto(BeamId sizingBeamId, DailyOperationSizingCounterDto counter, DailyOperationSizingWeightDto weight, double pisMeter, double spu, string sizingBeamStatus)
        {
            SizingBeamId = sizingBeamId;
            Start = counter.Start;
            Finish = counter.Finish;
            Netto = weight.Netto;
            Bruto = weight.Bruto;
            Theoritical = weight.Theoritical;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
    }
}
