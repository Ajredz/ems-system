using EMS.Security.Core.AuditLog;
using EMS.Security.Core.SystemErrorLog;
using EMS.Security.Core.SystemRole;
using EMS.Security.Core.SystemUser;
using EMS.Security.Core.Workflow;
using EMS.Security.Data._IntegrationModels;
using EMS.Security.Data.AuditLog;
using EMS.Security.Data.SystemErrorLog;
using EMS.Security.Data.SystemRole;
using EMS.Security.Data.SystemUser;
using EMS.Security.Data.Workflow;
using EMS_SecurityService.DBContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace EMS.Security.API
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Start up method
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration variable
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SystemAccessContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<EMS.Security.Data.DBContexts.SystemAccessContext>
                (o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<EMS.Security.Data.DBContexts.PortalGlobalContext>
                (o => o.UseMySql(Configuration.GetConnectionString("PortalGlobalConnection")));

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Security API",
                    Description = "API for handling Front-end Authentication and Parameters",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IErrorLogDBAccess, ErrorLogDBAccess>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IAuditLogDBAccess, AuditLogDBAccess>();
            services.AddScoped<ISystemUserService, SystemUserService>();
            services.AddScoped<ISystemUserDBAccess, SystemUserDBAccess>();
            services.AddScoped<ISystemRoleService, SystemRoleService>();
            services.AddScoped<ISystemRoleDBAccess, SystemRoleDBAccess>();
            services.AddScoped<Itbl_usersDBAccess, tbl_usersDBAccess>();

            /*H2Pay Controllers*/
            services.AddScoped<EMS.H2Pay.Data.SystemUser.IDBAccess, EMS.H2Pay.Data.SystemUser.DBAccess>();

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Security API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Error Logging for the whole API
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                
                // Get API Credentials
                int UserID = Convert.ToInt32(context.Request.Query["UserID"], new CultureInfo("en-US"));
                bool ReturnExceptionError = Convert.ToBoolean(Configuration.GetSection("ReturnExceptionError").Value, new CultureInfo("en-US"));

                var result = ErrorLog.MySQLInsertErrorLog(
                    string.Concat(env.ContentRootPath, Configuration.GetSection("ErrorLogPath").GetSection("DefaultErrorLogPath").Value)
                , context.Request.Path + "", context.Request.QueryString + ""
                , feature.Error.Message + ". " + feature.Error.StackTrace + "", feature.Error.InnerException + ""
                , UserID, ReturnExceptionError);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result).ConfigureAwait(true);
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}