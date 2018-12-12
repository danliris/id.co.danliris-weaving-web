using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;

namespace Infrastructure
{
    public class ServicesRegistrar : IConfigureServicesAction
    {
        public int Priority => 100;

        public void Execute(IServiceCollection services, IServiceProvider sp)
        {
            services.AddSingleton<IWorkContext>(c => new WorkContext() { CurrentUser = "System" });
        }
    }
}
