using EMS.Recruitment.Core.Applicant;
using EMS.Recruitment.Core.ApplicantDashboard;
using EMS.Recruitment.Core.ApplicantTagging;
using EMS.Recruitment.Core.DataDuplication;
using EMS.Recruitment.Core.PSGC;
using EMS.Recruitment.Core.RecruiterTask;
using EMS.Recruitment.Core.Reference;
using EMS.Recruitment.Core.SystemErrorLog;
using EMS.Recruitment.Core.Workflow;
using EMS.Recruitment.Data.Applicant;
using EMS.Recruitment.Data.ApplicantDashboard;
using EMS.Recruitment.Data.ApplicantTagging;
using EMS.Recruitment.Data.DataDuplication.OrgGroup;
using EMS.Recruitment.Data.DataDuplication.Position;
using EMS.Recruitment.Data.DataDuplication.PositionLevel;
using EMS.Recruitment.Data.DataDuplication.SystemUser;
using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Data.PSGC;
using EMS.Recruitment.Data.RecruiterTask;
using EMS.Recruitment.Data.Reference;
using EMS.Recruitment.Data.SystemErrorLog;
using EMS.Recruitment.Data.Workflow;
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

namespace RecruitmentRequisition
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
            services.AddDbContext<RecruitmentContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Recruitment API",
                    Description = "API for handling Recruitment Services",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddScoped<IOrgGroupDBAccess, OrgGroupDBAccess>();
            services.AddScoped<IOrgGroupService, OrgGroupService>();
            services.AddScoped<IPositionDBAccess, PositionDBAccess>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IPositionLevelDBAccess, PositionLevelDBAccess>();
            services.AddScoped<IPositionLevelService, PositionLevelService>();
            services.AddScoped<ISystemUserDBAccess, SystemUserDBAccess>();
            services.AddScoped<ISystemUserService, SystemUserService>();
            
            services.AddScoped<IReferenceDBAccess, ReferenceDBAccess>();
            services.AddScoped<IReferenceService, ReferenceService>();

            services.AddScoped<IWorkflowDBAccess, WorkflowDBAccess>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IApplicantDBAccess, ApplicantDBAccess>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IRecruiterTaskDBAccess, RecruiterTaskDBAccess>();
            services.AddScoped<IRecruiterTaskService, RecruiterTaskService>();
            services.AddScoped<IApplicantTaggingDBAccess, ApplicantTaggingDBAccess>();
            services.AddScoped<IApplicantTaggingService, ApplicantTaggingService>();
            services.AddScoped<IApplicantDashboardDBAccess, ApplicantDashboardDBAccess>();
            services.AddScoped<IApplicantDashboardService, ApplicantDashboardService>();

            services.AddScoped<IPSGCDBAccess, PSGCDBAccess>();
            services.AddScoped<IPSGCService, PSGCService>();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment API v1");
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