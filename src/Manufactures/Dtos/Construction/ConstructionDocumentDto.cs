using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using System;

namespace Manufactures.Dtos.Construction
{
    public class ConstructionDocumentDto
    {
        public ConstructionDocumentDto(ConstructionDocument constructionDocument)
        {
            Id = constructionDocument.Identity;
            ConstructionNumber = constructionDocument.ConstructionNumber;
            TotalYarn = constructionDocument.TotalYarn;
            Date = constructionDocument.Date.ToString("MMMM dd, yyyy");

            if (constructionDocument.ConstructionDetails.Count != 0)
            {
                var indexCount = 0;
                foreach (var detail in constructionDocument.ConstructionDetails)
                {
                    if (indexCount == 0)
                    {
                        YarnType = detail.Yarn.Deserialize<Yarn>().Code;
                        indexCount++;
                    } else
                    {
                        YarnType = YarnType + "x" + detail.Yarn.Deserialize<Yarn>().Code;
                        indexCount++;
                    }
                }
            }
        }

        public Guid Id { get; }
        public string ConstructionNumber { get; }
        public string Date { get; }
        public double TotalYarn { get; }
        public string YarnType { get; }
    }
}
