using Infrastructure.Domain;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping
{
    public class WarpingBrokenCauseDocument : AggregateRoot<WarpingBrokenCauseDocument, WarpingBrokenCauseReadModel>
    {
        public string WarpingBrokenCauseName { get; private set; }
        public string Information { get; private set; }
        public bool IsOthers { get; private set; }

        public WarpingBrokenCauseDocument(Guid identity,
                                          string warpingBrokenCauseName,
                                          string information,
                                          bool isOthers) : base(identity)
        {
            Identity = identity;
            WarpingBrokenCauseName = warpingBrokenCauseName;
            Information = information;
            IsOthers = isOthers;

            ReadModel = new WarpingBrokenCauseReadModel(Identity)
            {
                WarpingBrokenCauseName = WarpingBrokenCauseName,
                Information = Information,
                IsOthers = IsOthers
            };

            MarkTransient();
        }

        public WarpingBrokenCauseDocument(WarpingBrokenCauseReadModel readModel) : base(readModel)
        {
            this.WarpingBrokenCauseName = readModel.WarpingBrokenCauseName;
            this.Information = readModel.Information;
            this.IsOthers = readModel.IsOthers;
        }

        public void SetWarpingBrokenCauseName(string value)
        {
            if (WarpingBrokenCauseName != value)
            {
                WarpingBrokenCauseName = value;
                ReadModel.WarpingBrokenCauseName = WarpingBrokenCauseName;

                MarkModified();
            }
        }

        public void SetInformation(string value)
        {
            if (Information != value)
            {
                Information = value;
                ReadModel.Information = Information;

                MarkModified();
            }
        }

        public void SetIsOthers(bool value)
        {
            if (IsOthers != value)
            {
                IsOthers = value;
                ReadModel.IsOthers = IsOthers;

                MarkModified();
            }
        }

        protected override WarpingBrokenCauseDocument GetEntity()
        {
            return this;
        }
    }
}
