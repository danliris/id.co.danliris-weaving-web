using Manufactures.Application.Helpers;
using Manufactures.Domain.Construction;
using Manufactures.Domain.Construction.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Construction
{
    public class ConstructionByIdDto
    {
        public Guid Id { get; }
        public string ConstructionNumber { get; }
        public MaterialTypeDocument MaterialTypeDocument { get; }
        public string WovenType { get; }
        public int AmountOfWarp { get; }
        public int AmountOfWeft { get; }
        public int Width { get; }
        public string WarpTypeForm { get; }
        public string WeftTypeForm { get; }
        public double TotalYarn { get; }
        public IReadOnlyCollection<Warp> ItemsWarp { get; }
        public IReadOnlyCollection<Weft> ItemsWeft { get; }


        public ConstructionByIdDto(ConstructionDocument document)
        {
            Id = document.Identity;
            ConstructionNumber = document.ConstructionNumber;
            AmountOfWarp = document.AmountOfWarp;
            AmountOfWeft = document.AmountOfWeft;
            Width = document.Width;
            WovenType = document.WovenType;
            WarpTypeForm = document.WarpType;
            WeftTypeForm = document.WeftType;
            TotalYarn = document.TotalYarn;
            MaterialTypeDocument = document.MaterialType;


            var warps = new List<Warp>();
            var wefts = new List<Weft>();

            foreach (var constructionDetail in document.ConstructionDetails)
            {
                if (constructionDetail.Detail.Contains(Constants.WARP))
                {
                    var warpObj = new Warp(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);
                    warps.Add(warpObj);

                }
                else if (constructionDetail.Detail.Contains(Constants.WEFT))
                {
                    var weftObj = new Weft(constructionDetail.Identity,
                                           constructionDetail.Quantity,
                                           constructionDetail.Information,
                                           constructionDetail.Yarn.Deserialize<Yarn>(),
                                           constructionDetail.Detail);

                    wefts.Add(weftObj);
                }
            }

            this.ItemsWarp = warps;
            this.ItemsWeft = wefts;
        }


    }
}
