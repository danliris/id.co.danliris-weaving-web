using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace Infrastructure.Mvc.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            StringValues contentTypes = new StringValues();
            context.HttpContext.Request.Headers.TryGetValue("Content-Type", out contentTypes);

            // FOR API
            if(!contentTypes.Any(o=>o == "application/x-www-form-urlencoded"))
            {
                HttpStatusCode status = HttpStatusCode.InternalServerError;
                string message = string.Empty;

                // FOR API
                if (context.Exception is ArgumentNullException)
                {
                    var ex = context.Exception as ArgumentNullException;
                    message = $"{ex.ParamName} is null";
                    status = HttpStatusCode.InternalServerError;
                }

                context.ExceptionHandled = true;

                HttpResponse response = context.HttpContext.Response;
                response.StatusCode = (int)status;
                response.ContentType = "application/json";

                response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    message,
                    success = false,
                    trace = context.Exception.StackTrace
                }));
            }

            // FOR MVC
        }
    }
}
