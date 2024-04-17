using EMS.IPM.Core.DataDuplication;
using EMS.IPM.Core.EmployeeScore;
using EMS.IPM.Core.EmployeeScoreDashboard;
using EMS.IPM.Core.KPI;
using EMS.IPM.Core.KPIPosition;
using EMS.IPM.Core.KPIScore;
using EMS.IPM.Core.KRAGroup;
using EMS.IPM.Core.KRASubGroup;
using EMS.IPM.Core.MyKPIScores;
using EMS.IPM.Core.RatingTable;
using EMS.IPM.Core.Reference;
using EMS.IPM.Core.SystemErrorLog;
using EMS.IPM.Data.DataDuplication.Employee;
using EMS.IPM.Data.DataDuplication.EmployeeMovement;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.EmployeeScore;
using EMS.IPM.Data.EmployeeScoreDashboard;
using EMS.IPM.Data.KPI;
using EMS.IPM.Data.KPIPosition;
using EMS.IPM.Data.KPIScore;
using EMS.IPM.Data.KRAGroup;
using EMS.IPM.Data.KRASubGroup;
using EMS.IPM.Data.MyKPIScores;
using EMS.IPM.Data.RatingTable;
using EMS.IPM.Data.Reference;
using EMS.IPM.Data.SystemErrorLog;
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

namespace EMS.IPM.API
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
            services.AddDbContext<IPMContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")
                //, options => options.EnableRetryOnFailure(
                //    maxRetryCount: 10,
                //    maxRetryDelay: TimeSpan.FromSeconds(10),
                //    errorNumbersToAdd: null)
                ));

            var options = services.BuildServiceProvider()
                      .GetRequiredService<DbContextOptions<IPMContext>>();
            Task.Run(() =>
            {
                using (var dbContext = new IPMContext(options))
                {
                    var model = dbContext.Model; //force the model creation
                }
            });

            //services.AddDbContext<SystemAccessContext>(o => o.UseMySql(Configuration.GetConnectionString("SystemAccessConnection")));

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IPM API",
                    Description = "API for handling IPM Services",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddScoped<IKPIService, KPIService>();
            services.AddScoped<IKPIDBAccess, KPIDBAccess>();

            services.AddScoped<IKPIPositionService, KPIPositionService>();
            services.AddScoped<IKPIPositionDBAccess, KPIPositionDBAccess>();

            services.AddScoped<IKPIScoreService, KPIScoreService>();
            services.AddScoped<IKPIScoreDBAccess, KPIScoreDBAccess>();

            services.AddScoped<IEmployeeScoreService, EmployeeScoreService>();
            services.AddScoped<IEmployeeScoreDBAccess, EmployeeScoreDBAccess>();
            
            services.AddScoped<IEmployeeScoreDashboardService, EmployeeScoreDashboardService>();
            services.AddScoped<IEmployeeScoreDashboardDBAccess, EmployeeScoreDashboardDBAccess>();

            services.AddScoped<IMyKPIScoresService, MyKPIScoresService>();
            services.AddScoped<IMyKPIScoresDBAccess, MyKPIScoresDBAccess>();

            services.AddScoped<IKRAGroupService, KRAGroupService>();
            services.AddScoped<IKRAGroupDBAccess, KRAGroupDBAccess>();

            services.AddScoped<IKRASubGroupService, KRASubGroupService>();
            services.AddScoped<IKRASubGroupDBAccess, KRASubGroupDBAccess>();

            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPositionDBAccess, PositionDBAccess>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeDBAccess, EmployeeDBAccess>();

            services.AddScoped<IEmployeeMovementService, EmployeeMovementService>();
            services.AddScoped<IEmployeeMovementDBAccess, EmployeeMovementDBAccess>();

            //services.AddScoped<IPSGCCityService, PSGCCityService>();
            //services.AddScoped<IPSGCCityDBAccess, PSGCCityDBAccess>();

            //services.AddScoped<IPSGCRegionService, PSGCRegionService>();
            //services.AddScoped<IPSGCRegionDBAccess, PSGCRegionDBAccess>();

            services.AddScoped<IOrgGroupService, OrgGroupService>();
            services.AddScoped<IOrgGroupDBAccess, OrgGroupDBAccess>();

            services.AddScoped<IReferenceService, ReferenceService>();
            services.AddScoped<IReferenceDBAccess, ReferenceDBAccess>(); 
            
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IErrorLogDBAccess, ErrorLogDBAccess>(); 
            
            services.AddScoped<IRatingTableService, RatingTableService>();
            services.AddScoped<IRatingTableDBAccess, RatingTableDBAccess>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IPM API v1");
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