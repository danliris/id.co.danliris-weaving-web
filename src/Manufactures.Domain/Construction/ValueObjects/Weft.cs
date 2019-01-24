using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Weft : ValueObject
    {
        public Weft(Guid id,
                    double quantity,
                    string information,
                    Yarn yarn,
                    string detail)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.Information = information;
            this.Yarn = yarn;
            this.Detail = detail;
        }
        public Guid Id { get; private set; }
        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public Yarn Yarn { get; private set; }
        public string Detail { get; private set; }

        public void SetDetail(string detail)
        {
            this.Detail = detail;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Quantity;
            yield return Information;
            yield return Yarn;
            yield return Detail;
        }
    }
}
