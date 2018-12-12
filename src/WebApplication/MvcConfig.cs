using ExtCore.Data.Abstractions;
using ExtCore.Mvc.Infrastructure.Actions;
using FluentValidation.AspNetCore;
using Infrastructure.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Mvc
{
    public class MvcConfig : IAddMvcAction
    {
        public int Priority => 100;

        public void Execute(IMvcBuilder builder, IServiceProvider sp)
        {
            builder.AddMvcOptions(c =>
            {
                c.Filters.Add(new TransactionDbFilter(sp.GetRequiredService<IStorage>()));
                c.Filters.Add(new GlobalExceptionFilter());
            });

            builder.AddFluentValidation();
        }
    }
}
