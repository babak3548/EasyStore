using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Utility;
using UILayer.Miscellaneous;
using Microsoft.AspNetCore.Diagnostics;
using ServiceLayer;
using System.Runtime.InteropServices;
using DataLayer.EFLog;
using Microsoft.AspNetCore.Rewrite;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace UILayer
{
    public class Startup
    {



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSetting.Configuration = configuration;
            ConstValues.Configuration = configuration;


        }



        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration.GetConnectionString("ConnectionStringShoppingCenter");
            var ConnectionStringLog = Configuration.GetConnectionString("ConnectionStringEasyStoreLog");
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(240);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddDbContext<OnlineShopping>(options => options.UseLazyLoadingProxies().UseSqlServer(ConnectionString));
            services.AddDbContext<EasyStoreLog>(options => options.UseLazyLoadingProxies().UseSqlServer(ConnectionStringLog));

            // services.AddResponseCompression();
            #region MyRegion
            //  services.AddDbContext<OnlineShopping>(options => options.UseLazyLoadingProxies().UseSqlServer(ConnectionStringLog));


            // var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));

            //using (var scope = services.CreateScope())
            //{
            //    var provider = scope.ServiceProvider;
            //    using (var dbContext = provider.GetRequiredService<ApplicationDbContext>())
            //    {
            //        options.AppTenants = dbContext.AppTenants.ToList();
            //    }
            //}
            //        .AddDbContext<BloggingContext>(
            //b => b.UseLazyLoadingProxies()
            //      .UseSqlServer(myConnectionString));
            #endregion

            services.AddControllersWithViews();
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddResponseCompression();

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<CustomCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });

            //services.AddSingleton<HtmlEncoder>(
            //   HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
            //                                UnicodeRanges.Arabic }));

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"fa\" dir=\"rtl\">" +
                            "<head>  <meta charset = \"utf-8\" > <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\"> </head>" +
                            "<body>\r\n");
                        await context.Response.WriteAsync("متاسفانه خطایی پیش آمده است!<br><br>\r\n");

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature?.Error != null)
                        {
                            var exception = contextFeature?.Error;

                            // LogService _LogService;
                            using (var db = new EasyStoreLog())
                            {
                                LogService _LogService = new LogService(db);
                                await _LogService.SaveException(context, exception, context.TraceIdentifier);
                            }

                            if (exception is ExceptionForDisplay)
                            {
                                await context.Response.WriteAsync("<b>" + exception.Message + "</b>\r\n");
                            }
                        }
                        else
                        {
                            using (var db = new EasyStoreLog())
                            {
                                LogService _LogService = new LogService(db);
                                await _LogService.SaveException(context, new Exception("خطایی بدون مقدار اتفاق افتاده است "), context.TraceIdentifier);
                            }
                        }
                        //if (contextFeature?.Error is FileNotFoundException)
                        //{
                        //    await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                        //}

                        await context.Response.WriteAsync("<a href=\"/\">برگشت به صفحه اصلی</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                    });
                });
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // This will add "Libs" as another valid static content location
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(), @"Content")),
                RequestPath = new PathString("/Content")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), @"assets")),
                RequestPath = new PathString("/assets")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Scripts")),
                RequestPath = new PathString("/Scripts")
            });

            #region redirectToNonWww
            var options = new RewriteOptions();
            options.Rules.Add(new NonWwwRule());
            app.UseRewriter(options);
            //app.UseStaticFiles();
            #endregion




            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                RequestPath = new PathString("/Images")
            });


            // app.HandleApplicationException(env);
            //// کنترول خطاها و ذخیره در فایل

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseResponseCompression();
        }
    }
    public class CustomCompressionProvider : ICompressionProvider
    {
        public string EncodingName => "mycustomcompression";
        public bool SupportsFlush => true;

        public Stream CreateStream(Stream outputStream)
        {
            // Create a custom compression stream wrapper here
            return outputStream;
        }
    }

    public class NonWwwRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var req = context.HttpContext.Request;
            var currentHost = req.Host;
            if (currentHost.Host.StartsWith("www."))
            {
                var newHost = new HostString(currentHost.Host.Substring(4), currentHost.Port ?? 80);
                var newUrl = new StringBuilder().Append("http://").Append(newHost).Append(req.PathBase).Append(req.Path).Append(req.QueryString);
                context.HttpContext.Response.Redirect(newUrl.ToString());
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}
