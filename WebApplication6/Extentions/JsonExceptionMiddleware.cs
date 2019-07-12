using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication6.Extentions
{
    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, env, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, IHostingEnvironment env, Exception exception)
        {
            string result;
            var code = HttpStatusCode.InternalServerError;

            if (env.IsDevelopment())
            {
                var errorMessage = new
                {
                    error = exception.Message,
                    stack = exception.StackTrace,
                    innerException = exception.InnerException
                };

                result = JsonConvert.SerializeObject(errorMessage);
            }
            else
            {
                var errorMessage = new
                {
                    error = exception.Message
                };

                result = JsonConvert.SerializeObject(errorMessage);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
