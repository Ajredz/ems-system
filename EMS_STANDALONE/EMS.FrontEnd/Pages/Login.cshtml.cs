using Utilities.API;
using EMS.FrontEnd.SharedClasses;
using EMS_SecurityServiceModel.SystemPage;
using EMS_SecurityServiceModel.SystemUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EMS.FrontEnd.SharedClasses.Common_Security;
using EMS.FrontEnd.SharedClasses.Common_Plantilla;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace EMS.FrontEnd.Pages
{
    public class LoginModel : SharedClasses.Utilities
    {
        public LoginModel(IConfiguration iconfiguration, IWebHostEnvironment env) : base(iconfiguration, env)
        {

        }

        [BindProperty]
        public Login Login { get; set; }

        public async Task OnGet(string returnUrl = "/")
        {
            HttpContext.Session.SetString("EMS_ReturnURL", returnUrl);
            
            ViewData["UserLookupURL"] = _iconfiguration.GetSection("UserLookupURL").Value;

            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session.GetString("EMS_UserCredentials") != null)
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    GlobalCurrentUser currentUsers = new GlobalCurrentUser();

                    HttpContext.Session.Remove("EMS_SystemMenu");
                    HttpContext.Session.Remove("EMS_SystemURL");
                    HttpContext.Session.Remove("EMS_UserCredentials");
                    HttpContext.Session.Remove("EMS_ModuleStatus");

                    var userDetailsURL = string.Concat(_securityBaseURL,
                        _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetUserDetailsByUsername").Value, "?",
                        "username=", HttpContext.User.Identity.Name, "&",
                        "userid=", 0);
                    var userDetailsTuple = await SharedUtilities.GetFromAPI(currentUsers, userDetailsURL);

                    if (userDetailsTuple.IsSuccess)
                    {
                        if (userDetailsTuple.APIResult != null)
                        {
                            HttpContext.Session.SetString("EMS_UserCredentials", JsonConvert.SerializeObject(userDetailsTuple.APIResult));

                            var systemMenuURL = string.Concat(_securityBaseURL,
                                _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetMenu").Value, "?",
                                "userid=", userDetailsTuple.APIResult.UserID);
                            List<SystemPage> systemPage = new List<SystemPage>();
                            var systemMenuTuple = await SharedUtilities.GetFromAPI(systemPage, systemMenuURL);

                            if (systemMenuTuple.IsSuccess)
                            {
                                HttpContext.Session.SetString("EMS_SystemMenu", JsonConvert.SerializeObject(systemMenuTuple.APIResult));
                            }
                            else
                            {
                                TempData["ErrorMessage"] = systemMenuTuple.ErrorMessage;
                                Response.Redirect("/ErrorPage");
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = MessageUtilities.ERRMSG_INCORRECT_LOGIN;
                            Response.Redirect("/ErrorPage");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = userDetailsTuple.ErrorMessage;
                        Response.Redirect("/ErrorPage");
                    }
                }
            }
        }

        public async Task<JsonResult> OnPost()
        {
            GlobalCurrentUser currentUsers = new GlobalCurrentUser();
                if (!string.IsNullOrEmpty(Login.Username) && !string.IsNullOrEmpty(Login.Password))
                {
                    HttpContext.Session.Remove("EMS_SystemMenu");
                    HttpContext.Session.Remove("EMS_SystemURL");
                    HttpContext.Session.Remove("EMS_UserCredentials");
                    HttpContext.Session.Remove("EMS_ModuleStatus");

                // mild duration
                    var userDetailsURL = string.Concat(_securityBaseURL,
                        _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetUserDetails").Value, "?",
                        "username=", Login.Username, "&",
                        "password=", Login.Password, "&",
                        "userid=", Login.UserId);
                    var userDetailsTuple = await SharedUtilities.GetFromAPI(currentUsers, userDetailsURL);

                    if (userDetailsTuple.IsSuccess)
                    {
                        if (userDetailsTuple.APIResult != null)
                        {
                            //var (GetEmployeeDetails, _) = await TestConnectionPlantilla(userDetailsTuple.APIResult.UserID);

                            //if (GetEmployeeDetails)
                            //{
                                //EMS.Plantilla.Transfer.Employee.GetByIDOutput employeeDetails =
                                //                    await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                //                    .GetEmployeeByUserID(userDetailsTuple.APIResult.UserID);

                                //long duration
                                EMS.Plantilla.Transfer.Employee.GetEmployeeByUsernameOutput employeeDetails = await new Common_Employee(_iconfiguration, new GlobalCurrentUser 
                                                    { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                                    .GetEmployeeByUsername(userDetailsTuple.APIResult.Username);

                                if (employeeDetails != null)
                                {

                                if (employeeDetails.ID != 1)
                                {
                                    var GetEmployeeDetails = await new Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                                            .GetEmployee(employeeDetails.ID);
                                    userDetailsTuple.APIResult.EmploymentStatus = GetEmployeeDetails.EmploymentStatus;
                                }

                                userDetailsTuple.APIResult.Branch =
                                    string.IsNullOrEmpty(employeeDetails.OrgGroupConcatenated) ?
                                    "[Blank]" : employeeDetails.OrgGroupConcatenated;
                                userDetailsTuple.APIResult.Company = employeeDetails.Company;
                                userDetailsTuple.APIResult.Position =
                                string.IsNullOrEmpty(employeeDetails.PositionTitle) ?
                                "[Blank]" : employeeDetails.PositionTitle;

                                userDetailsTuple.APIResult.PositionID = employeeDetails.PositionID;
                                userDetailsTuple.APIResult.OrgGroupID = employeeDetails.OrgGroupID;
                                userDetailsTuple.APIResult.EmployeeID = employeeDetails.ID;
                                

                            //Assign OrgGroupIDs under the logged user's responsibility
                            //long duration - gets data to be displayed for ? but not limited to Plantilla Count
                            userDetailsTuple.APIResult.OrgGroupDescendants =
                                (await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                .GetOrgGroupDescendants(employeeDetails.OrgGroupID)).Distinct().ToList();

                                // Test Replace
                                //Assign OrgGroupIDs under the logged user's responsibility
                                //if (employeeDetails.OrgGroupID > 0)
                                //{
                                //    userDetailsTuple.APIResult.OrgGroupDescendants =
                                //        await new SharedClasses.Common_Plantilla.Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                //        .GetOrgGroupDescendants(employeeDetails.OrgGroupID);
                                //}
                                //else
                                //{
                                //    //Displays Position and Org Group
                                //    userDetailsTuple.APIResult.OrgGroupDescendants = new List<int> { employeeDetails.OrgGroupID };
                                //}

                                // Assign details for Roving employees
                                var rovingDetails = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                                .GetRovingByEmployeeID(employeeDetails.ID));
                                // Test Removal of .ToList() Reduce Duration
                                //var rovingDetails = (await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                //                .GetRovingByEmployeeID(employeeDetails.ID)).ToList();
                                

                                    // Assign Secondary Org Groups
                                    if (rovingDetails != null)
                                        if (rovingDetails.Count > 0)
                                        {
                                            userDetailsTuple.APIResult.Roving =
                                                rovingDetails
                                            .Select(x => new GlobalCurrentUserRoving { 
                                                    OrgGroupID = x.OrgGroupID,
                                                    PositionID = x.PositionID
                                                }).ToList();

                                            // Add his/her secondary org group's descendants
                                        List<int> rovingDescendants = new List<int>();
                                        // Hide causes a long query
                                        //if (userDetailsTuple.APIResult.Roving != null)
                                        //{
                                        if (userDetailsTuple.APIResult.Roving.Count > 0)
                                        {

                                            foreach (var item in userDetailsTuple.APIResult.Roving)
                                            {
                                                rovingDescendants.AddRange(await new SharedClasses.Common_Plantilla
                                                    .Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                                                .GetOrgGroupDescendants(item.OrgGroupID));
                                            }

                                            //System.Threading.Tasks.Parallel.For(0, userDetailsTuple.APIResult.Roving.Count, async index => {
                                            //    string temp = await new SharedClasses.Common_Plantilla
                                            //        .Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                            //                    .GetOrgGroupDescendants()
                                            //});

                                            //rovingDescendants.AddRange(await new SharedClasses.Common_Plantilla
                                            //    .Common_OrgGroup(_iconfiguration, new GlobalCurrentUser { UserID = userDetailsTuple.APIResult.UserID }, _env)
                                            //                .GetOrgGroupDescendants(userDetailsTuple.APIResult.OrgGroupID));

                                        }
                                        userDetailsTuple.APIResult.OrgGroupRovingDescendants = rovingDescendants.Distinct().ToList();
                                        }
                            //}

                            if (userDetailsTuple.APIResult.UserID != 1)
                            {
                                userDetailsTuple.APIResult.EmployeeUnderAccess = string.Join(",", ((await new SharedClasses.Common_Plantilla.Common_Employee(_iconfiguration, _globalCurrentUser, _env)
                                    .GetEmployeeIDDescendant(userDetailsTuple.APIResult.EmployeeID)).Where(y=>y.IsActive).Select(x => x.ID).ToList()));
                            }
                            
                        }

                            //}

                            // Assign IP Address
                            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                                remoteIpAddress = Request.Headers["X-Forwarded-For"];
                            else if (Request.Headers.ContainsKey("X-Forwarded-Proto"))
                                remoteIpAddress = Request.Headers["X-Forwarded-Proto"];

                            userDetailsTuple.APIResult.IPAddress = remoteIpAddress;
                            // Not working on Ubuntu environment
                            //userDetailsTuple.APIResult.ComputerName = GetComputerNameByIP(remoteIpAddress);

                            HttpContext.Session.SetString("EMS_UserCredentials", JsonConvert.SerializeObject(userDetailsTuple.APIResult));

                            #region Authentication 
                            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userDetailsTuple.APIResult.Username)
                        };
                            var claimIdentities = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimPrincipal = new ClaimsPrincipal(claimIdentities);

                            await Request.HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                claimPrincipal,
                                new AuthenticationProperties() { IsPersistent = true });
                            #endregion

                            var systemMenuURL = string.Concat(_securityBaseURL,
                                _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetMenu").Value, "?",
                                "userid=", userDetailsTuple.APIResult.UserID);
                            List<SystemPage> systemPage = new List<SystemPage>();
                            var systemMenuTuple = await SharedUtilities.GetFromAPI(systemPage, systemMenuURL);

                            //IF DRAFT STATUS
                            if (userDetailsTuple.APIResult.EmploymentStatus == "DRAFT")
                            {
                                var GetEmployeeDetailsPageWithoutNull = systemMenuTuple.APIResult.Where(x => !string.IsNullOrEmpty(x.URL)).ToList();
                                var GetEmployeeDetailsPage = GetEmployeeDetailsPageWithoutNull.Where(x => x.URL.Equals("/employeeprofile/employeedetails")).ToList();
                                systemMenuTuple.APIResult = systemMenuTuple.APIResult.Where(x => x.ID.Equals(GetEmployeeDetailsPage.Select(x=>x.ID).FirstOrDefault()) || x.ID.Equals(GetEmployeeDetailsPage.Select(x=>x.ParentPageID).FirstOrDefault())).ToList();
                            }

                            if (systemMenuTuple.IsSuccess)
                            {
                                string redirectTo = HttpContext.Session.GetString("EMS_ReturnURL") ?? "/";

                                // TO DIRECT EDIT EMPLOYEE DETAILS WHEN DRAFT
                                if (userDetailsTuple.APIResult.EmploymentStatus == "DRAFT")
                                    redirectTo = "/employeeprofile/employeedetails";

                                HttpContext.Session.SetString("EMS_SystemMenu", JsonConvert.SerializeObject(systemMenuTuple.APIResult));
                                _resultView.IsSuccess = true;
                                _resultView.Result = redirectTo;

                                /*Add AuditLog*/
                            await new Common_AuditLog(_iconfiguration, new GlobalCurrentUser { 
                                    UserID = userDetailsTuple.APIResult.UserID,
                                    IPAddress = userDetailsTuple.APIResult.IPAddress,
                                    ComputerName = userDetailsTuple.APIResult.ComputerName
                                }, _env)
                                .AddAuditLog(new Security.Transfer.AuditLog.Form { 
                                        EventType = Common_AuditLog.EventType.LOGIN.ToString(),
                                        TableName = "SystemUser",
                                        TableID = userDetailsTuple.APIResult.UserID,
                                        Remarks = string.Concat(userDetailsTuple.APIResult.Username, " logged in"),
                                        IsSuccess = true,
                                        CreatedBy = userDetailsTuple.APIResult.UserID
                                    });

                                /*Check API availability*/
                                var (PlantillaIsSuccess, PlantillaMessage) = await TestConnectionPlantilla(userDetailsTuple.APIResult.UserID);
                                var (ManpowerIsSuccess, ManpowerMessage) = await TestConnectionManpower(userDetailsTuple.APIResult.UserID);
                                var (RecruitmentIsSuccess, RecruitmentMessage) = await TestConnectionRecruitment(userDetailsTuple.APIResult.UserID);
                                var (WorkflowIsSuccess, WorkflowMessage) = await TestConnectionWorkflow(userDetailsTuple.APIResult.UserID);
                                var (IPMIsSuccess, IPMMessage) = await TestConnectionIPM(userDetailsTuple.APIResult.UserID);

                                HttpContext.Session.SetString("EMS_ModuleStatus",
                                        JsonConvert.SerializeObject(new List<ModuleStatus> {
                                        new ModuleStatus{
                                            ModuleCode = "PLANTILLA",
                                            IsRunning = PlantillaIsSuccess,
                                            Message = PlantillaMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "MANPOWER",
                                            IsRunning = ManpowerIsSuccess,
                                            Message = ManpowerMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "RECRUITMENT",
                                            IsRunning = RecruitmentIsSuccess,
                                            Message = RecruitmentMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "WORKFLOW",
                                            IsRunning = WorkflowIsSuccess,
                                            Message = WorkflowMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "IPM",
                                            IsRunning = IPMIsSuccess,
                                            Message = IPMMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "LOGACTIVITY",
                                            IsRunning = WorkflowIsSuccess,
                                            Message = WorkflowMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "TRAINING",
                                            IsRunning = WorkflowIsSuccess,
                                            Message = WorkflowMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "ONBOARDING",
                                            IsRunning = WorkflowIsSuccess,
                                            Message =  WorkflowMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "ACCOUNTABILITY",
                                            IsRunning = WorkflowIsSuccess,
                                            Message = WorkflowMessage,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "ADMINISTRATOR",
                                            IsRunning = true,
                                            Message = MessageUtilities.SCSSMSG_SECURITY_API_STATUS,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "MY_ACCOUNT",
                                            IsRunning = true,
                                            Message = MessageUtilities.SCSSMSG_SECURITY_API_STATUS,
                                        },
                                        new ModuleStatus{
                                            ModuleCode = "EMPLOYEE_PROFILE",
                                            IsRunning = PlantillaIsSuccess,
                                            Message = PlantillaMessage,
                                        }
                                        })
                                   );


                                //HttpContext.Session.SetString("EMS_Manpower_IsRunning",
                                //    (await TestConnectionManpower(userDetailsTuple.APIResult.UserID)).ToString());
                                //HttpContext.Session.SetString("EMS_Recruitment_IsRunning",
                                //    (await TestConnectionRecruitment(userDetailsTuple.APIResult.UserID)).ToString());

                                //if (!userDetailsTuple.APIResult.IsPasswordChanged)
                                //{
                                //    _resultView.IsSuccess = true;
                                //    _resultView.Result = "/FORCECHANGEPASSWORD";
                                //    return new JsonResult(_resultView);
                                //}
                            }
                            else
                            {
                                HttpContext.Session.Remove("EMS_UserCredentials");
                                HttpContext.Session.Remove("EMS_SystemMenu");
                                HttpContext.Session.Remove("EMS_SystemURL");
                                HttpContext.Session.Remove("EMS_ModuleStatus");
                                await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                                _resultView.IsSuccess = false;
                                _resultView.Result = MessageUtilities.ERRMSG_NOACCESS;
                            }

                        }
                        else
                        {
                            _resultView.IsSuccess = false;
                            _resultView.Result = MessageUtilities.ERRMSG_INCORRECT_LOGIN;
                        }
                    }
                    else
                    {
                        _resultView.IsSuccess = false;
                        _resultView.Result = userDetailsTuple.ErrorMessage;
                    }
                }
                else
                {
                    _resultView.IsSuccess = false;
                    _resultView.Result = MessageUtilities.ERRMSG_INCORRECT_LOGIN;
                }
            return new JsonResult(_resultView);

        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            if (HttpContext.Session.GetString("EMS_UserCredentials") != null)
            {
                _globalCurrentUser = JsonConvert.DeserializeObject<GlobalCurrentUser>(HttpContext.Session.GetString("EMS_UserCredentials"));
                
                ///*Add AuditLog*/
                //await new Common_AuditLog(_iconfiguration, _globalCurrentUser, _env)
                //    .AddAuditLog(new Security.Transfer.AuditLog.Form
                //    {
                //        EventType = Common_AuditLog.EventType.LOGOUT.ToString(),
                //        TableName = "SystemUser",
                //        TableID = _globalCurrentUser.UserID,
                //        Remarks = string.Concat(_globalCurrentUser.Username, " logged out"),
                //        IsSuccess = true,
                //        CreatedBy = _globalCurrentUser.UserID
                //    });
            }

            HttpContext.Session.Remove("EMS_UserCredentials");
            HttpContext.Session.Remove("EMS_SystemMenu");
            HttpContext.Session.Remove("EMS_SystemURL");
            HttpContext.Session.Remove("EMS_ModuleStatus");

            await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Login");
            //Response.Redirect("/Login");
        }

    }
}