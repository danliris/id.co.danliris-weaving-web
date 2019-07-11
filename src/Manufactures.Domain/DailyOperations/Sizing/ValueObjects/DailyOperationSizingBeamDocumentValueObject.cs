using Manufactures.Domain.Shared.ValueObjects;
using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingBeamDocumentValueObject : ValueObject
    {
        public BeamId SizingBeamId { get; set; }

        public double Start { get; set; }

        public double Finish { get; set; }

        public double Netto { get; set; }

        public double Bruto { get; set; }

        public double Theoritical { get; set; }

        public double PISMeter { get; set; }

        public double SPU { get; set; }

        public string SizingBeamStatus { get; set; }

        public DailyOperationSizingBeamDocumentValueObject()
        {
        }

        public DailyOperationSizingBeamDocumentValueObject(BeamId sizingBeamId, double start, double finish, double netto, double bruto, double theoritical, double pisMeter, double spu, string sizingBeamStatus)
        {
            SizingBeamId = sizingBeamId;
            Start = start;
            Finish = finish;
            Netto = netto;
            Bruto = bruto;
            Theoritical = theoritical;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SizingBeamId;
            yield return Start;
            yield return Finish;
            yield return Netto;
            yield return Bruto;
            yield return Theoritical;
            yield return PISMeter;
            yield return SPU;
            yield return SizingBeamStatus;
        }
    }
}
