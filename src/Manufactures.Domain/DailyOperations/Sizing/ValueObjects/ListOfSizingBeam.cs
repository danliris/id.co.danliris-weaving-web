using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class ListOfSizingBeam : ValueObject
    {

        public ListOfSizingBeam()
        {
            SizingBeamDocumentList = new List<DailyOperationSizingBeamDocumentValueObject>();
        }

        public List<DailyOperationSizingBeamDocumentValueObject> SizingBeamDocumentList { get; set; }

        public void AddSizingBeam(DailyOperationSizingBeamDocumentValueObject sizingBeamDocument)
        {
            SizingBeamDocumentList.Add(sizingBeamDocument);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SizingBeamDocumentList;
        }
    }
}
