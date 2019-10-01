using Infrastructure.Domain;
using Manufactures.Domain.Defects.YarnDefect.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Defects.YarnDefect
{
    public class YarnDefectDocument : AggregateRoot<YarnDefectDocument, YarnDefectReadModel>
    {
        public string DefectCode { get; private set; }
        public string DefectType { get; private set; }
        public string DefectCategory { get; private set; }

        public YarnDefectDocument(Guid identity,
                                  string defectCode,
                                  string defectType,
                                  string defectCategory) : base(identity)
        {
            Identity = identity;
            DefectCode = defectCode;
            DefectType = defectType;
            DefectCategory = defectCategory;

            ReadModel = new YarnDefectReadModel(Identity)
            {
                DefectCode = DefectCode,
                DefectType = DefectType,
                DefectCategory = DefectCategory
            };

            MarkTransient();
        }

        public YarnDefectDocument(YarnDefectReadModel readModel) : base(readModel)
        {
            this.DefectCode = readModel.DefectCode;
            this.DefectType = readModel.DefectType;
            this.DefectCategory = readModel.DefectCategory;
        }

        public void SetDefectCode(string value)
        {
            if (DefectCode != value)
            {
                DefectCode = value;
                ReadModel.DefectCode = DefectCode;

                MarkModified();
            }
        }

        public void SetDefectType(string value)
        {
            if (DefectType != value)
            {
                DefectType = value;
                ReadModel.DefectType = DefectType;

                MarkModified();
            }
        }

        public void SetDefectCategory(string value)
        {
            if (DefectCategory != value)
            {
                DefectCategory = value;
                ReadModel.DefectCategory = DefectCategory;

                MarkModified();
            }
        }

        protected override YarnDefectDocument GetEntity()
        {
            return this;
        }
    }
}
