﻿using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories
{
    public class EstimatedProductionDetailRepository : AggregateRepostory<EstimatedProductionDetail, EstimatedProductionDetailReadModel>, IEstimatedProductionDetailRepository
    {
        protected override EstimatedProductionDetail Map(EstimatedProductionDetailReadModel readModel)
        {
            return new EstimatedProductionDetail(readModel);
        }

        //public async Task<string> GetEstimationNumber()
        //{
        //    DateTimeOffset now = DateTimeOffset.Now;
        //    var year = now.Year.ToString();
        //    var month = now.Month.ToString();
        //    var estimationNumber = (this.dbSet.Where(o => o.Period.Deserialize<Period>().Year.Contains(year))).Count() + 1.ToString();
        //    estimationNumber = estimationNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + year;

        //    return await Task.FromResult(estimationNumber);
        //}
    }
}
