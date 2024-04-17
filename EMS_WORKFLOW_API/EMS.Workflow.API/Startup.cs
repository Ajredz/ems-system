using EMS.Workflow.Core.Accountability;
using EMS.Workflow.Core.EmailServerCredential;
using EMS.Workflow.Core.LogActivity;
using EMS.Workflow.Core.Reference;
using EMS.Workflow.Core.Training;
using EMS.Workflow.Core.Workflow;
using EMS.Workflow.Core.EmployeeScore;
using EMS.Workflow.Data.Accountability;
using EMS.Workflow.Data.EmployeeScore;
using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.EmailServerCredential;
using EMS.Workflow.Data.LogActivity;
using EMS.Workflow.Data.Reference;
using EMS.Workflow.Data.Training;
using EMS.Workflow.Data.Workflow;
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
using EMS.Workflow.Core.Question;
using EMS.Workflow.Data.Question;
using EMS.Workflow.Data.Case;
using EMS.Workflow.Core.Case;

namespace WorkflowRequisition
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
            services.AddDbContext<WorkflowContext>(o => o.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Workflow API",
                    Description = "API for handling Workflow Services",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddScoped<ILogActivityDBAccess, LogActivityDBAccess>();
            services.AddScoped<ILogActivityService, LogActivityService>(); 
            
            services.AddScoped<IReferenceDBAccess, ReferenceDBAccess>();
            services.AddScoped<IReferenceService, ReferenceService>();

            services.AddScoped<IWorkflowDBAccess, WorkflowDBAccess>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            
            services.AddScoped<IWorkflowDBAccess, WorkflowDBAccess>();
            services.AddScoped<IWorkflowService, WorkflowService>();

            services.AddScoped<IAccountabilityDBAccess, AccountabilityDBAccess>();
            services.AddScoped<IAccountabilityService, AccountabilityService>();

            services.AddScoped<IEmailServerCredentialDBAccess, EmailServerCredentialDBAccess>();
            services.AddScoped<IEmailServerCredentialService, EmailServerCredentialService>();

            services.AddScoped<IEmployeeScoreDBAccess, EmployeeScoreDBAccess>();
            services.AddScoped<IEmployeeScoreService, EmployeeScoreService>();

            services.AddScoped<ITrainingDBAccess, TrainingDBAccess>();
            services.AddScoped<ITrainingService, TrainingService>();

            services.AddScoped<IQuestionDBAccess, QuestionDBAccess>();
            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<ICaseDBAccess, CaseDBAccess>();
            services.AddScoped<ICaseService, CaseService>();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow API v1");
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