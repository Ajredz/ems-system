using EMS.Manpower.Core.DataDuplication;
using EMS.Manpower.Core.MRF;
using EMS.Manpower.Core.Reference;
using EMS.Manpower.Data.DataDuplication.OrgGroup;
using EMS.Manpower.Data.DataDuplication.Position;
using EMS.Manpower.Data.DataDuplication.PositionLevel;
using EMS.Manpower.Data.DataDuplication.SystemRole;
using EMS.Manpower.Data.DBContexts;
using EMS.Manpower.Data.MRF;
using EMS.Manpower.Data.Reference;
using EMS.Manpower.Core.MRFSignatories;
using EMS.Manpower.Data.MRFSignatories;
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
using System.Reflection;
using EMS.Manpower.Data.DataDuplication.Region;
using EMS.Manpower.Data.Dashboard;
using EMS.Manpower.Core.Dashboard;
using System.IO;
using EMS.Manpower.Data.ApproverSetup;
using EMS.Manpower.Core.ApproverSetup;
using EMS.Manpower.Core.SystemErrorLog;
using EMS.Manpower.Data.SystemErrorLog;

namespace ManpowerRequisition
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
            services.AddDbContext<ManpowerContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Manpower API",
                    Description = "API for handling Manpower Services",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });


            services.AddScoped<IReferenceDBAccess, ReferenceDBAccess>();
            services.AddScoped<IReferenceService, ReferenceService>();

            services.AddScoped<IMRFSignatoriesDBAccess, MRFSignatoriesDBAccess>();
            services.AddScoped<IMRFSignatoriesService, MRFSignatoriesService>();
            services.AddScoped<IMRFDBAccess, MRFDBAccess>();
            services.AddScoped<IMRFService, MRFService>();

            services.AddScoped<IDashboardDBAccess, DashboardDBAccess>();
            services.AddScoped<IDashboardService, DashboardService>();
            
            services.AddScoped<IApproverSetupDBAccess, ApproverSetupDBAccess>();
            services.AddScoped<IApproverSetupService, ApproverSetupService>();
            
            /*=========================== Data Duplicated Tables ==================================*/
            // Plantilla DB
            services.AddScoped<IPositionDBAccess, PositionDBAccess>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPositionLevelDBAccess, PositionLevelDBAccess>();
            services.AddScoped<IPositionLevelService, PositionLevelService>();
            services.AddScoped<IOrgGroupDBAccess, OrgGroupDBAccess>();
            services.AddScoped<IOrgGroupService, OrgGroupService>();
            services.AddScoped<IRegionDBAccess, RegionDBAccess>();
            services.AddScoped<IRegionService, RegionService>();
            
            // System Access DB
            services.AddScoped<ISystemRoleDBAccess, SystemRoleDBAccess>();
            services.AddScoped<ISystemRoleService, SystemRoleService>();
            /*==================================== End =============================================*/

            services.AddScoped<ISynchronizeService, SynchronizeService>();

            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IErrorLogDBAccess, ErrorLogDBAccess>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manpower API v1");
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