using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using PuppetCat.AspNetCore.Mvc.Middleware;
using PuppetCat.Sample.Core;
using PuppetCat.Sample.Data;
using PuppetCat.Sample.Repository;
using Microsoft.OpenApi.Models;
using System;

namespace PuppetCat.Sample.API
{
    /// <summary>
    /// Startup configuration class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration instance</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.AddControllers(
                options =>
                {
                    options.Filters.Add<ModelValidationActionFilter>();
                }
               ).AddJsonOptions(opt =>
               {
                   opt.JsonSerializerOptions.PropertyNamingPolicy = null;
               });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
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

            //Register DbContext and Repositories for Dependency Injection
            services.AddDbContext<SampleDbContext>(ServiceLifetime.Scoped);
            services.AddScoped<UserRepository>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Web host environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //添加NLog
            LogManager.Setup().LoadConfigurationFromFile("nlog.config");

            //use Middleware
            app.UseLogHandling();
            app.UseErrorHandling();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
