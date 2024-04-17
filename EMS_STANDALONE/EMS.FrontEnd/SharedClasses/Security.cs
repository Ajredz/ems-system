using Utilities.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using System.Threading.Tasks;
using EMS_SecurityServiceModel.SystemURL;
using System.Collections.Generic;

namespace EMS.FrontEnd.SharedClasses
{
    public class Security : PageModel
    {

        public GlobalCurrentUser _globalCurrentUser = null;
        public ResultView _resultView = new ResultView();
        public readonly IConfiguration _iconfiguration;
        public List<SystemURL> _systemURL = null;
        public static string _securityBaseURL, _plantillaBaseURL, _manpowerBaseURL, _recruitmentBaseURL, _workflowBaseURL, _onboardingBaseURL, _ipmBaseURL;



        public Security(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _securityBaseURL = _iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value;
            _plantillaBaseURL = _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value;
            _manpowerBaseURL = _iconfiguration.GetSection("ManPowerService_API_URL").GetSection("Base_URL").Value;
            _recruitmentBaseURL = _iconfiguration.GetSection("RecruitmentService_API_URL").GetSection("Base_URL").Value;
            _workflowBaseURL = _iconfiguration.GetSection("WorkflowService_API_URL").GetSection("Base_URL").Value;
            _onboardingBaseURL = _iconfiguration.GetSection("OnboardingService_API_URL").GetSection("Base_URL").Value;
            _ipmBaseURL = _iconfiguration.GetSection("IPMService_API_URL").GetSection("Base_URL").Value;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var url = (HttpContext.Request.Path.Value ?? "").ToUpper();
            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (url != "/SESSIONEXPIRED" 
                && url != "/LOGIN"
                && url != "/CRONJOB/"
                && url != "/ERRORPAGE"
                )
            {
                if (_globalCurrentUser == null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        if (HttpContext.Session.GetString("EMS_UserCredentials") != null)
                        {
                            _globalCurrentUser = JsonConvert.DeserializeObject<GlobalCurrentUser>(HttpContext.Session.GetString("EMS_UserCredentials"));
                        } 
                    }
                }

                if (_globalCurrentUser == null)
                {

                    if (isAjaxCall)
                    {
                        context.Result = new JsonResult(
                            new ResultView
                            {
                                IsSuccess = false,
                                Result = MessageUtilities.ERRMSG_LOGIN_SESSION
                            });
                    }
                    else if (url.Contains("/CRONJOB/"))
                    {
                        Response.Redirect(HttpContext.Request.Path.Value);
                    }
                    else
                    {
                        Response.Redirect("/Login");
                    }
                }
                else
                {
                    if (!_globalCurrentUser.IsPasswordChanged & url != "/FORCECHANGEPASSWORD")
                    {
                        Response.Redirect("/ForceChangePassword");
                    }

                    if (HttpContext.Session.GetString("EMS_SystemURL") == null)
                    {
                        GetUserAccess().Wait();
                    }

                    if (HttpContext.Session.GetString("EMS_SystemURL") != null)
                    {
                        //List<SystemURL> systemURL = new List<SystemURL>();

                        _systemURL = JsonConvert.DeserializeObject<List<SystemURL>>(HttpContext.Session.GetString("EMS_SystemURL"));

                        string[] urls = {
                                    "/"
                                    , "/INDEX"
                                    , "/LOGIN"
                                    , "/SESSIONEXPIRED"
                                    , "/ERRORPAGE"
                                    , "/MYACCOUNT/MANAGEACCOUNT"
                                    , "/MYACCOUNT/MANAGEACCOUNT/EDIT"
                                    , "/FORCECHANGEPASSWORD"
                                };

                        if (!urls.Contains(url))
                        {
                            if (_systemURL.Where(x => x.URL.ToUpper() == url).Count() == 0)
                            {
                                if (isAjaxCall)
                                {
                                    context.Result = new BadRequestObjectResult(
                                    //new ResultView
                                    //{
                                    //    IsSuccess = false,
                                    //    Result = MessageUtilities.ERRMSG_NOACCESS
                                    //}
                                     MessageUtilities.ERRMSG_NOACCESS
                                    );
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = MessageUtilities.ERRMSG_NOACCESS;
                                    Response.Redirect("/ErrorPage");
                                }
                            } 
                        }
                    }

                    //// Uncomment to load the current partial view upon refresh
                    //}
                    //else
                    //{
                    //    string url = ("" + Request.RawUrl).Trim().ToUpper().Split('?')[0];
                    //    if (url != "/SECURITY/CHANGEPASSWORD" && url != "/SECURITY/CHANGESYSTEMUSER")
                    //        Response.Redirect("~/Security/ChangePassword");
                    //}
                }
            }
            //Set Company
            //if (!isAjaxCall && _globalCurrentUser != null)
            //{
            //    ViewData["User"] = string.Concat(_globalCurrentUser.LastName, 
            //        (_globalCurrentUser.FirstName ?? "") == "" ? "" : ", " + _globalCurrentUser.FirstName, 
            //        (_globalCurrentUser.MiddleName ?? ""));
            //    ViewData["Branch"] = _globalCurrentUser.Branch;
            //    ViewData["Company"] = _globalCurrentUser.Company;
            //    ViewData["Position"] = _globalCurrentUser.Position;
            //    ViewData["LastLoggedIn"] = _globalCurrentUser.LastLoggedIn;
            //    ViewData["LastLoggedOut"] = _globalCurrentUser.LastLoggedOut;
            //    ViewData["LastPasswordChange"] = _globalCurrentUser.LastPasswordChange;
            //}
        }

        private async Task GetUserAccess()
        {
            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            try
            {
                var systemURLLink = string.Concat(_securityBaseURL, 
                    _iconfiguration.GetSection("SecurityService_API_URL").GetSection("GetSystemURL").Value, "?",
                    "userid=", _globalCurrentUser.UserID);

                var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<SystemURL>(), systemURLLink);

                if (IsSuccess)
                    HttpContext.Session.SetString("EMS_SystemURL", JsonConvert.SerializeObject(Result));
                else
                {
                    if (isAjaxCall)
                    {
                        throw new Exception(ErrorMessage);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ErrorMessage;
                        Response.Redirect("/Login");
                    }
                }
            }
            catch (Exception ex)
            {
                if (isAjaxCall)
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message;
                    Response.Redirect("/Login");
                }
            }
        }


    }
}
