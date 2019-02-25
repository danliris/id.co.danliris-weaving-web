using DanLiris.Admin.Web;
using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Moonlay.ExtCore.Mvc.Abstractions;
using FluentValidation.AspNetCore;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure
{
    public class ServicesRegistrar : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection services, IServiceProvider sp)
        {
            services.AddSingleton(c => new WorkContext() { CurrentUser = "System" });
            services.AddSingleton<IWebApiContext>(c => c.GetRequiredService<WorkContext>());
            services.AddSingleton<IWorkContext>(c => c.GetRequiredService<WorkContext>());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}