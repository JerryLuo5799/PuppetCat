using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppetCat.AspNetCore.Mvc.Middleware;
using PuppetCat.Sample.Core;
using PuppetCat.Sample.Data;

namespace PuppetCat.Sample.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver());

            //context connection
            SampleDbContext.ConStr = Configuration.GetConnectionString("SampleConnection");

            //AppSettings Section
            ConfigCore.SetAppSettings(Configuration.GetSection("AppSettings").Get<AppSettingsModel>());

            //路由分发配置
            DistributeRoute.DistributeRoutePath = ConfigCore.AppSettings.DistributeRoutePath;
            DistributeRoute.DistributeRouteIgnorePath = ConfigCore.AppSettings.DistributeRouteIgnorePath;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAllOrigin");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            //use ErrorHandlingMiddleware in SSW.ChinaWebstie.Common
            app.UseErrorHandling();
            app.UseMvc(routes =>
            {
                //use my Route rules to distribute request
               routes.Routes.Add(new DistributeRoute(app.ApplicationServices));
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
