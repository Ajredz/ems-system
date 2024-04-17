using Microsoft.AspNetCore.Mvc;
using EMS.External.API.Shared;
using EMS.External.API.Model;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using EMS.External.API.DBContext;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace EMS.External.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : Controller
    {
        public readonly ExternalContext _externalContext;
        public ResultView _resultView = new ResultView();
        public readonly IConfiguration _iconfiguration;

        public MainController(ExternalContext externalContext, IConfiguration iconfiguration)
        {
            _externalContext = externalContext;
            _iconfiguration = iconfiguration;
        }

        [HttpPost]
        [Route("get-user-details")]
        public async Task<IActionResult> GetUserDetails([FromQuery] APICredentials param, [FromBody]GetUserDetailsInput input)
        {
            var TokenErs = "ERS" + DateTime.Now.ToString("MM/dd/yyyy");
            var TokenResult = Utilities.ComputeSHA256Hash(TokenErs);
            if (!TokenResult.Equals(param.Token))
            {
                _resultView.Message = MessageUtilities.ERRMSG_INVALID_TOKEN;
                return new BadRequestObjectResult(_resultView);
            }
            else
            {
                var URL = string.Concat(_iconfiguration.GetSection("SecurityService_API_URL").GetSection("Base_URL").Value,
                    _iconfiguration.GetSection("SecurityService_API_URL").GetSection("SystemUser").GetSection("GetUserDetails").Value, "?",
                  "username=",input.Username,"&",
                  "password=",input.Password
                  );

                var (Result, IsSuccess, ErrorMessage) = await Utilities.GetFromAPI(new GetUserDetailsOutput(), URL);
                _resultView.IsSuccess = IsSuccess;
                _resultView.Message = ErrorMessage;
                _resultView.Result = Result;
                if (IsSuccess)
                {
                    var GetEmployeeByUsernameURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                        _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Employee").GetSection("GetEmployeeByUsername").Value, "?",
                      "Username=", Result.Username
                      );
                    var (Result1, IsSuccess1, ErrorMessage1) = await Utilities.GetFromAPI(new GetEmployeeByUsernameOutput(), GetEmployeeByUsernameURL);

                    //TO GET ORG TYPE
                    //START
                    var GetOrgTypeURL = string.Concat(_iconfiguration.GetSection("PlantillaService_API_URL").GetSection("Base_URL").Value,
                        _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("OrgGroup").GetSection("GetByID").Value, "?",
                      "ID=", Result1.OrgGroupID
                      );
                    var (Result2, IsSuccess2, ErrorMessage2) = await Utilities.GetFromAPI(new GetEmployeeByUsernameOutput(), GetOrgTypeURL);
                    Result1.OrgType = Result2.OrgType;
                    //END

                    _resultView.IsSuccess = IsSuccess1;
                    _resultView.Message = ErrorMessage1;
                    _resultView.Result = Result1;

                    if (IsSuccess1)
                        return new OkObjectResult(_resultView);
                    else
                        return new BadRequestObjectResult(_resultView);
                }
                else
                {
                    return new BadRequestObjectResult(_resultView);
                }
            }
        }

    }
}
