using Manufactures.Domain.FabricConstructions.Entity;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.FabricConstructions.DataTransferObjects
{
    public class ConstructionYarnDetailDto
    {
        public Guid Id { get; }
        public Guid YarnId { get; }
        public double Quantity { get; }
        public string Information { get; }
        public string Type { get; }
        public Guid FabricConstructionDocumentId { get; }
        public ConstructionYarnDetailDto(ConstructionYarnDetail constructionYarnDetail)
        {
            Id = constructionYarnDetail.Identity;
            YarnId = constructionYarnDetail.YarnId.Value;
            Quantity = constructionYarnDetail.Quantity;
            Information = constructionYarnDetail.Information;
            Type = constructionYarnDetail.Type;
            FabricConstructionDocumentId = constructionYarnDetail.FabricConstructionDocumentId;
        }
    }
}
