using DanLiris.Admin.Web;
using DanLiris.Admin.Web.Utils;
using ExtCore.Mvc.Infrastructure.Actions;
using FluentValidation.AspNetCore;
using Infrastructure.Mvc.Filters;
using Manufactures.Domain.DailyOperations.Warping.Commands;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.FabricConstructions.Commands;
using Manufactures.Domain.Orders.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using static Manufactures.Domain.DailyOperations.Warping.Commands.WarpingBrokenThreadsCausesCommand;

namespace Infrastructure.Mvc
{
    public class ConfigurationMvc : IAddMvcAction
    {
        public int Priority => 1000;

        public void Execute(IMvcBuilder builder, IServiceProvider sp)
        {
            builder.AddMvcOptions(c =>
            {
                c.Filters.Add<TransactionDbFilter>();

                c.Filters.Add<GlobalExceptionFilter>();
                c.Filters.Add(typeof(ValidateModelStateAttribute));
            });

            builder.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<PlaceConstructionCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateConstructionCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<AddOrderCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateOrderCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<AddNewEstimationCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<AddNewEstimatedProductionDetailCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateEstimationProductCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateEstimatedProductionDetailCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<PreparationDailyOperationWarpingCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateStartDailyOperationWarpingCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<ProduceBeamsDailyOperationWarpingCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<CompletedDailyOperationWarpingCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<WarpingBrokenThreadsCausesCommandValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
        }
    }
}