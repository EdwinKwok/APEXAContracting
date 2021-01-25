using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using APEXAContracting.WebAPI.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

namespace APEXAContracting.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        ///  Wrapper for IConfiguration.
        /// </summary>
        public Common.Interfaces.IConfigSettings ConfigSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.ConfigSettings = new Common.ConfigSettings(configuration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //
            // About web api methods enumeration/document/test
            // Reference: https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            //
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            // Reference: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-2.1
            // Register HttpContextAccessor, thus, business layer service can dependency inject HttpContext.
            // Work for getting userContext which generated in "RequestUserContextMiddleware".
            //
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            //
            // Dependency inject Configuration. 
            // Reference: https://blogs.technet.microsoft.com/dariuszporowski/tip-of-the-week-how-to-access-configuration-from-controller-in-asp-net-core-2-0/
            // Using built-in support for Dependency Injection, you can inject configuration data to Controller. 
            // Use AddSingleton method to add a singleton service in Startup.cs file.
            //
            services.AddSingleton<IConfiguration>(this.Configuration);
            //
            // Singletone register a custom config settting entity which is warpper of IConfiguration.
            //
            services.AddSingleton<Common.Interfaces.IConfigSettings>(this.ConfigSettings);

            //
            // Dependency Injection register DataAccess Layer DbContexts, UnitOfWorks in Web application's Startup.cs.
            //
            services.RegisterDataAccessLayer(this.ConfigSettings);

            //
            // Dependency Injection register Business Layer Services in Web Application's Startup.cs. 
            //
            services.RegisterBusinessLayer(this.ConfigSettings);

            // Register AutoMapper.
            // http://docs.automapper.org/en/stable/Dependency-injection.html
            services.RegisterAutoMapperProfiles();

            //
            // Define File Logger settings should get from appsetting.json file section "Logging:File".
            // Corporate with FileLoggerProvider.cs
            // Note: Settings in Logging:File section in appsettings.json file should be same as properties in class called "BatchingLoggerOptions" and "FileLoggerOptions".
            services.Configure<Common.Logging.FileLoggerOptions>(Configuration.GetSection("Logging:File"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("api");
            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
