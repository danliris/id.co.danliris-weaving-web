using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Infrastructure.Mvc.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
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
                message = message,
                success = false,
                trace = context.Exception.StackTrace
            }));

            // FOR MVC
        }
    }
}
