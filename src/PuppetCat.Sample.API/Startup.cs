using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using PuppetCat.AspNetCore.Mvc.Middleware;
using PuppetCat.Sample.Core;
using PuppetCat.Sample.Data;
using Swashbuckle.AspNetCore.Swagger;
using System;

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

            services.AddMvc(
                options =>
                {
                    options.Filters.Add<ModelValidationActionFilter>();
                }
               ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "PuppetCat.Sample.API"
                });

                //Determine base path for the application.  
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //Set the comments path for the swagger json and ui.  
                //var xmlPath = Path.Combine(basePath, "PuppetCat.Sample.API.xml");
                //options.IncludeXmlComments(xmlPath);
            });


            //context connection
            SampleDbContext.ConStr = Configuration.GetConnectionString("SampleConnection");

            //AppSettings Section
            ConfigCore.SetAppSettings(Configuration.GetSection("AppSettings").Get<AppSettingsModel>());

            //路由分发配置
            DistributeRoute.DistributeRoutePath = ConfigCore.AppSettings.DistributeRoutePath;
            DistributeRoute.DistributeRouteIgnorePath = ConfigCore.AppSettings.DistributeRouteIgnorePath;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddNLog();//添加NLog
            //LogManager.LoadConfiguration("nlog.config");
            loggerFactory.ConfigureNLog("nlog.config");

            //use Middleware
            app.UseLogHandling();
            app.UseErrorHandling();

            app.UseMvc(routes =>
            {
                //use my Route rules to distribute request
                routes.Routes.Add(new DistributeRoute(app.ApplicationServices));
            });
           
            //app.UseMvcWithDefaultRoute();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"PuppetCat.Sample.API - {DateTime.Now.ToString("yyMMddHHmmss")}");
            });
        }
    }
}
