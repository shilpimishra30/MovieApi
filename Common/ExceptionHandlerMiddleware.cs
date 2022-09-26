using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace MovieApi.Common
{
    public static class ExceptionHandlerMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (contextFeature != null)
                    {
                        var ex = contextFeature?.Error;
                        var isDev = env.IsDevelopment();
                        var json = JsonConvert.SerializeObject(
                        new ErrorDetails
                        {

                            Type = ex.GetType().Name,
                            Status = (int)HttpStatusCode.InternalServerError,
                            Instance = contextFeature?.Path,
                            Title = isDev ? $"{ex.Message}" : "An error occurred.",
                            Detail = isDev ? ex.StackTrace : null
                        });
                        await context.Response.WriteAsync(json);
                        logger.LogError(json);
                    }
                });
            });
        }
    }
}
