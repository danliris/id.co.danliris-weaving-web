using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Weft : ValueObject
    {
        public Weft(Guid id,
                    double quantity,
                    string information,
                    Yarn yarn)
        {
            this.Id = id;
            this.Quantity = quantity;
            this.Information = information;
            this.Yarn = yarn;
        }
        public Guid Id { get; private set; }
        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public Yarn Yarn { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
