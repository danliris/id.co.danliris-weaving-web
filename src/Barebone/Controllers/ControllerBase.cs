// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
        protected IStorage Storage { get; private set; }
        protected IWebApiContext WorkContext { get; private set; }

        public ControllerApiBase(IStorage storage, IWebApiContext apiContext)
        {
            this.Storage = storage;
            this.WorkContext = apiContext;
        }

        protected IActionResult Ok<T>(T data, object info = null, string message = null)
        {
            return base.Ok(new {
                apiVersion = this.WorkContext.ApiVersion,
                success = true,
                data,
                info,
                message
            });
        }
    }
}