using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UILayer.Miscellaneous
{
    public static class ApplicationBuilderExtension
    {
        public static void HandleApplicationException(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature?.Error != null)
                    {
                        var exception = contextFeature?.Error;
                        string strError = "";

                        strError = string.Concat(strError, exception.Message, Environment.NewLine);
                        strError = string.Concat(strError, exception.StackTrace, Environment.NewLine);
                        strError = string.Concat(strError, exception.InnerException?.Message, Environment.NewLine);
                        strError = string.Concat(strError, context.TraceIdentifier, Environment.NewLine);

                        await File.AppendAllTextAsync("C:\\websites\\onlineShopping\\logApp\\"+DateTime.Now.ToString("yyyyMMdd") +".txt", strError);
                        //var traceIdentifier = context.TraceIdentifier;
                        //  context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        //  await context.Response.WriteAsync(traceIdentifier);
                    }

                });
            });
        }
    }
}
