using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Machines.ValueObjects
{
    public class CoreAccountValueObject : ValueObject
    {
        public string _id { get; }
        public string Name { get; }

        public CoreAccountValueObject(string _id, string name)
        {
            this._id = _id;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return _id;
            yield return Name;
        }
    }
}
