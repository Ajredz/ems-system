using EMS.H2Pay.Data.OrgGroup;
using EMS.Plantilla.Core.Dashboard;
using EMS.Plantilla.Core.Employee;
using EMS.Plantilla.Core.EmployeeMovement;
using EMS.Plantilla.Core.OrgGroup;
using EMS.Plantilla.Core.Position;
using EMS.Plantilla.Core.PositionLevel;
using EMS.Plantilla.Core.PSGC;
using EMS.Plantilla.Core.Reference;
using EMS.Plantilla.Core.Region;
using EMS.Plantilla.Core.SystemErrorLog;
using EMS.Plantilla.Data.Dashboard;
using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.Employee;
using EMS.Plantilla.Data.EmployeeMovement;
using EMS.Plantilla.Data.OrgGroup;
using EMS.Plantilla.Data.Position;
using EMS.Plantilla.Data.PositionLevel;
using EMS.Plantilla.Data.PSGC;
using EMS.Plantilla.Data.Reference;
using EMS.Plantilla.Data.Region;
using EMS.Plantilla.Data.SystemErrorLog;
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
using System.Threading.Tasks;

namespace EMS.Plantilla.API
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
            services.AddDbContext<PlantillaContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")
                //, options => options.EnableRetryOnFailure(
                //    maxRetryCount: 10,
                //    maxRetryDelay: TimeSpan.FromSeconds(10),
                //    errorNumbersToAdd: null)
                ));

            //services.AddTransient<EMS.H2Pay.Data.OrgGroup.IDBAccess, EMS.H2Pay.Data.OrgGroup.DBAccess>();

            //var options = services.BuildServiceProvider()
            //          .GetRequiredService<DbContextOptions<PlantillaContext>>();
            //Task.Run(() =>
            //{
            //    using (var dbContext = new PlantillaContext(options))
            //    {
            //        var model = dbContext.Model; //force the model creation
            //    }
            //});


            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Plantilla API",
                    Description = "API for handling Plantilla Services",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddScoped<IOrgGroupService, OrgGroupService>();
            services.AddScoped<IOrgGroupDBAccess, OrgGroupDBAccess>();

            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPositionDBAccess, PositionDBAccess>();

            services.AddScoped<IPositionLevelService, PositionLevelService>();
            services.AddScoped<IPositionLevelDBAccess, PositionLevelDBAccess>();

            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegionDBAccess, RegionDBAccess>();

            services.AddScoped<IReferenceService, ReferenceService>();
            services.AddScoped<IReferenceDBAccess, ReferenceDBAccess>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeDBAccess, EmployeeDBAccess>();

            services.AddScoped<IPSGCService, PSGCService>();
            services.AddScoped<IPSGCDBAccess, PSGCDBAccess>();

            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardDBAccess, DashboardDBAccess>();

            services.AddScoped<IEmployeeMovementService, EmployeeMovementService>();
            services.AddScoped<IEmployeeMovementDBAccess, EmployeeMovementDBAccess>();

            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IErrorLogDBAccess, ErrorLogDBAccess>();

            /*H2Pay Controllers*/
            services.AddScoped<EMS.H2Pay.Data.OrgGroup.IDBAccess, EMS.H2Pay.Data.OrgGroup.DBAccess>();
            services.AddScoped<EMS.H2Pay.Data.Position.IDBAccess, EMS.H2Pay.Data.Position.DBAccess>();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plantilla API v1");
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