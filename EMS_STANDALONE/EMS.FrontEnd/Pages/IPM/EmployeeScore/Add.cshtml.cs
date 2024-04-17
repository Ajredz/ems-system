using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS.FrontEnd.SharedClasses.Common_IPM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using EMS.FrontEnd.SharedClasses.Common_Security;
using System.Text;
using EMS.IPM.Transfer.EmployeeScore;
using EMS.FrontEnd.SharedClasses.Common_Workflow;
using EMS.Workflow.Transfer.Workflow;
using EMS.IPM.Data.EmployeeScore;
using System.Threading;

namespace EMS.FrontEnd.Pages.IPM.EmployeeScore
{
    public class AddModel : SharedClasses.Utilities
    {
        [BindProperty]
        public EMS.IPM.Transfer.EmployeeScore.RunScoreForm RunScoreForm { get; set; }

        public AddModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        { }

        public async Task OnGetAsync()
        {
            if (_systemURL != null)
            {
                ViewData["HasDeleteFeature"] = _systemURL.Where(x => x.URL.ToUpper().Equals("/IPM/EMPLOYEESCORE/DELETE")).Count() > 0 ? "true" : "false";
            }

            ViewData["OrgGroupSelectList"] =
                    await new Common_OrgGroup(_iconfiguration, _globalCurrentUser, _env).GetOrgGroupDropDown();

            ViewData["PositionSelectList"] =
                    await new Common_Position(_iconfiguration, _globalCurrentUser, _env).GetPositionDropdown();

            //ViewData["RegionSelectList"] =
            //        await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetPSGCRegionDropDown();

            //ViewData["CitySelectList"] =
            //        await new Common_PSGC(_iconfiguration, _globalCurrentUser, _env).GetPSGCCityDropDown();
        }

        public async Task<JsonResult> OnPostAsync()
        {

            string dates = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";

            int maxDegreeOfParallelism = Convert.ToInt32(string.Concat(_iconfiguration.GetSection("MultiThread").GetSection("MaxDegreeOfParallelism").Value));
            int byBatch = Convert.ToInt32(string.Concat(_iconfiguration.GetSection("MultiThread").GetSection("ByBatch").Value));
            List<Task> tasks = new List<Task>();

            //// INITIALIZE
            var URLCreateInitialSummary = string.Concat(_ipmBaseURL,
           _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("CreateTransScoreSummary").Value, "?",
           "userid=", _globalCurrentUser.UserID);

            var (Resulttest, IsSuccesstest, Messagetest) = await SharedUtilities.PostFromAPI(new TransOutputForTreading(), RunScoreForm, URLCreateInitialSummary);
            _resultView.IsSuccess = IsSuccesstest;
            _resultView.Result = Resulttest;


            if (IsSuccesstest)
            {

                StringBuilder Remarks = new StringBuilder();

                Remarks.Append(string.Concat("Employee Score Transactions Initial Run. Filtered by ", RunScoreForm.Filter, " from ", RunScoreForm.DateFrom, " to ", RunScoreForm.DateTo, ". "));

                if (RunScoreForm.UseCurrent == true)
                {
                    Remarks.Append(string.Concat("Used recent KPI Position. "));
                }

                if (RunScoreForm.IncludeAllLevelsBelow == true)
                {
                    Remarks.Append(string.Concat("Included all levels below. "));
                }

                if (RunScoreForm.Override == true)
                {
                    Remarks.Append(string.Concat("Override existing scores with the same period. "));
                }

                /*Add AuditLog*/
                new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                   .AddAuditLog(new Security.Transfer.AuditLog.Form
                   {
                       EventType = Common_AuditLog.EventType.ADD.ToString(),
                       TableName = "Employee Score",
                       TableID = 0,  // Result.TransSummaryID, // New Record, no ID yet
                       Remarks = Remarks.ToString(), //dates.ToString() + 
                       IsSuccess = true,
                       CreatedBy = _globalCurrentUser.UserID
                   }).Wait();
            }



            try
            {

                var TaskResult = Task.Run(() => ExecuteTask());

                void ExecuteTask()
                {


                    StringBuilder Remarks = new StringBuilder();

                    RunScoreForm.TransSummaryID = Resulttest.TransSummaryID;

                    var URL = string.Concat(_ipmBaseURL,
                    _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresInitialize").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

                    var roles = (new Common_Maintenance(_iconfiguration, _globalCurrentUser, _env)
                       .GetRolesByWorkflowStepCode(new GetRolesByWorkflowStepCodeInput
                       {
                           WorkflowCode = "IPM",
                           StepCode = "NEW" // CurrentStep
                       }).Result).ToList();
                    RunScoreForm.RoleIDs = string.Join(",", roles.Select(x => x.RoleID).ToArray());

                    var (Result, IsSuccess, Message) = SharedUtilities.PostFromAPI(new TransOutputForTreading(), RunScoreForm, URL).Result;
                    _resultView.IsSuccess = IsSuccess;
                    _resultView.Result = Result;

                    RunScoreForm.TransSummaryID = Result.TransSummaryID;
                    RunScoreForm.Override = false;

                    ////// PROCESS BY BATCH
                    var URL2 = string.Concat(_ipmBaseURL, _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresProcessByBatch").Value, "?", "userid=", _globalCurrentUser.UserID);
                    List<int> employees = Result.EmployeeID;
                    int loops = Convert.ToInt32(Math.Ceiling(employees.Count / Convert.ToDouble(byBatch)));

                    ParallelOptions po = new ParallelOptions();
                    po.MaxDegreeOfParallelism = maxDegreeOfParallelism;
                    Parallel.For(0, loops, po, (count, state) =>
                    {
                        RunScoreForm RunScoreByBatchForm = new RunScoreForm();


                        List<int> employeesByBatch = employees.GetRange(byBatch <= employees.Count ? count * byBatch : 0
                            , byBatch <= employees.Count ? employees.Count - ((count * byBatch)) > byBatch ? byBatch : employees.Count - ((count * byBatch))
                                                                : employees.Count);

                        string PK = count + "-";
                        RunScoreByBatchForm.DateFrom = RunScoreForm.DateFrom;
                        RunScoreByBatchForm.DateTo = RunScoreForm.DateTo;
                        RunScoreByBatchForm.TransSummaryID = RunScoreForm.TransSummaryID;
                        RunScoreByBatchForm.Pk = PK;
                        RunScoreByBatchForm.strEmployeeIDList = string.Join(",", employeesByBatch.ToArray());
                        tasks.Add(SharedUtilities.PostFromAPI(new TransOutput(), RunScoreByBatchForm, URL2));

                    });

                    if (tasks.Any())
                    {
                        Task.WhenAll(tasks).Wait();
                        tasks.Clear();
                        ////FINALIZE
                        var URLIsDone = string.Concat(_ipmBaseURL, _iconfiguration.GetSection("IPMService_API_URL").GetSection("EmployeeScore").GetSection("RunTransScoresFinalize").Value, "?", "userid=", _globalCurrentUser.UserID);
                        SharedUtilities.PostFromAPI(new TransOutputForTreading(), RunScoreForm, URLIsDone).Wait();

                    }

                    //if (IsSuccess)
                    //{
                    //    var workflowURL = string.Concat(_workflowBaseURL,
                    //        _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("AddByBatch").Value, "?",
                    //        "userid=", _globalCurrentUser.UserID);

                    //    //var (IsSuccess3, Message3) =
                    //    //    await SharedUtilities.PostFromAPI(Result.TransSummaryID.Select(x =>
                    //    //    new EMS.Workflow.Transfer.EmployeeScore.Form
                    //    //    {
                    //    //        TID = x,
                    //    //        Status = RunScoreForm.Override ? "--" : "NEW",
                    //    //        Remarks = RunScoreForm.Override ? "OVERRIDE" : "INITIAL RUN"
                    //    //    }
                    //    //    ), workflowURL);
                    //}

                    if (IsSuccess)
                    {
                        var workflowURL = string.Concat(_workflowBaseURL,
                            _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("EmployeeScore").GetSection("AddByBatch").Value, "?",
                            "userid=", _globalCurrentUser.UserID);

                        List<EMS.Workflow.Transfer.EmployeeScore.Form> res = new List<EMS.Workflow.Transfer.EmployeeScore.Form>();

                        res.Add(new EMS.Workflow.Transfer.EmployeeScore.Form
                        {
                            TID = Result.TransSummaryID,
                            Status = RunScoreForm.Override ? "--" : "NEW",
                            Remarks = RunScoreForm.Override ? "OVERRIDE" : "INITIAL RUN"
                        });

                        var (resIsSuccess, resIsMessages) = SharedUtilities.PostFromAPI(res, workflowURL).Result;
                    }


                    if (IsSuccess)
                    {
                        // StringBuilder Remarks = new StringBuilder();

                        Remarks.Append(string.Concat("Employee Score Transactions Run completed. ", "Total of Employees ( " + employees.Count() + " ) ", "Filtered by ", RunScoreForm.Filter, " from ", RunScoreForm.DateFrom, " to ", RunScoreForm.DateTo, ". "));


                        if (RunScoreForm.UseCurrent == true)
                        {
                            Remarks.Append(string.Concat("Used recent KPI Position. "));
                        }

                        if (RunScoreForm.IncludeAllLevelsBelow == true)
                        {
                            Remarks.Append(string.Concat("Included all levels below. "));
                        }

                        if (RunScoreForm.Override == true)
                        {
                            Remarks.Append(string.Concat("Overrode existing scores with the same period. "));
                        }

                        /*Add AuditLog*/
                        new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                           .AddAuditLog(new Security.Transfer.AuditLog.Form
                           {
                               EventType = Common_AuditLog.EventType.ADD.ToString(),
                               TableName = "Employee Score",
                               TableID = Result.TransSummaryID, // 0, // New Record, no ID yet
                               Remarks = Remarks.ToString(), // dates.ToString() + 
                               IsSuccess = true,
                               CreatedBy = _globalCurrentUser.UserID
                           }).Wait();

                    }


                }
            }
            catch (Exception ex)
            {


            }


            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnPostProcessTransNewRun(RunScoreForm RunScoreForm, string url)
        {
            _resultView.Result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
              .GetTransProgress();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);
        }

        public async Task<JsonResult> OnGetTransProgress()
        {
            _resultView.Result = await new Common_EmployeeScore(_iconfiguration, _globalCurrentUser, _env)
                .GetTransProgress();
            _resultView.IsSuccess = true;
            return new JsonResult(_resultView);


            if (_resultView.IsSuccess)
            {

                StringBuilder Remarks = new StringBuilder();

                Remarks.Append(string.Concat("Employee Score Initial Re-run. Filtered by ", RunScoreForm.Filter, " from ", RunScoreForm.DateFrom, " to ", RunScoreForm.DateTo, ". "));

                if (RunScoreForm.UseCurrent == true)
                {
                    Remarks.Append(string.Concat("Used recent KPI Position. "));
                }

                if (RunScoreForm.IncludeAllLevelsBelow == true)
                {
                    Remarks.Append(string.Concat("Included all levels below. "));
                }

                if (RunScoreForm.Override == true)
                {
                    Remarks.Append(string.Concat("Override existing scores with the same period. "));
                }

                /*Add AuditLog*/
                new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                   .AddAuditLog(new Security.Transfer.AuditLog.Form
                   {
                       EventType = Common_AuditLog.EventType.ADD.ToString(),
                       TableName = "Employee Score",
                       TableID = 0,  // Result.TransSummaryID, // New Record, no ID yet
                       Remarks = Remarks.ToString(), //dates.ToString() + 
                       IsSuccess = true,
                       CreatedBy = _globalCurrentUser.UserID
                   }).Wait();
            }

        }
    }
}