// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Barebone.Controllers
{
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        protected IStorage Storage { get; private set; }

        public ControllerBase(IStorage storage)
        {
            this.Storage = storage;
        }
    }

    [ApiController]
    public abstract class ControllerApiBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected IStorage Storage { get; }
        protected IWebApiContext WorkContext { get; }
        protected IMediator Mediator { get; }

        public ControllerApiBase(IServiceProvider serviceProvider)
        {
            this.Storage = serviceProvider.GetService<IStorage>();
            this.WorkContext = serviceProvider.GetService<IWebApiContext>();
            this.Mediator = serviceProvider.GetService<IMediator>();
        }

        protected IActionResult Ok<T>(T data, object info = null, string message = null)
        {
            return base.Ok(new
            {
                apiVersion = this.WorkContext.ApiVersion,
                success = true,
                data,
                info,
                message
            });
        }
    }
}