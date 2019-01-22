using Manufactures.Domain.Construction.ReadModels;
using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.ValueObjects
{
    public class Warp : ValueObject
    {
        public Warp(Guid id,
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
            yield return Id;
            yield return Quantity;
            yield return Information;
            yield return Yarn;
        }
    }
}
