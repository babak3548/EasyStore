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
using ServiceLayer;

namespace AdminPanel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSetting.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration.GetConnectionString("ConnectionStringShoppingCenter");
            services.AddDbContext<OnlineShopping>(options => options.UseLazyLoadingProxies().UseSqlServer(ConnectionString));
            services.AddTransient<ProductService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<ContentService>();
            services.AddTransient<InvoiceService>();
            services.AddTransient<BusinessOwnerService>();
            services.AddTransient<UserService>();
            services.AddTransient<ContentService>();
            services.AddTransient<PromotionProductService>();
            services.AddTransient<AccountingService>();
            services.AddTransient<SettingService>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpClient();
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(3600000);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(300);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppSetting appSetting = new AppSetting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(appSetting.ImagePathInServer),//Path.Combine(appSetting.ImagePathInServer, @"Content")Directory.GetCurrentDirectory()@"Content"
                RequestPath = new PathString(appSetting.ImagePathInVirtual)
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(appSetting.ImagePathOtherFileServer),//Path.Combine(appSetting.ImagePathInServer, @"Content")Directory.GetCurrentDirectory()@"Content"
                RequestPath = new PathString(appSetting.ImagePathOtherFileVirtual)
            });
            
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

    
        }
    }
}
