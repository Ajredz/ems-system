using Utilities.API;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Plantilla.Transfer.EmployeeMovement;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Crypto;
using EMS.IPM.Data.DataDuplication.EmployeeMovement;
//for movement checker 10-11, 93-109
namespace EMS.FrontEnd.SharedClasses.Common_Plantilla
{
    public class Common_EmployeeMovement : Utilities
    {
        public Common_EmployeeMovement(IConfiguration iconfiguration, GlobalCurrentUser globalCurrentUser, IWebHostEnvironment env) : base(iconfiguration, env)
        {
            _globalCurrentUser = globalCurrentUser;
        }

        public async Task<List<GetAutoCompleteByMovementTypeOutput>> GetAutoCompleteByMovementType(GetAutoCompleteByMovementTypeInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetAutoComplete").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "Term=", param.Term, "&",
                     "TopResults=", param.TopResults, "&",
                     "MovementType=", param.MovementType, "&",
                     "OrgGroup=",param.OrgGroup);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAutoCompleteByMovementTypeOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<List<GetAutoPopulateByMovementTypeOutput>> GetAutoPopulateByMovementType(GetAutoPopulateByMovementTypeInput param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                     _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetAutoPopulate").Value, "?",
                     "userid=", _globalCurrentUser.UserID, "&",
                     "MovementType=", param.MovementType);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetAutoPopulateByMovementTypeOutput>(), URL);
            _resultView.IsSuccess = IsSuccess;
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<Form> GetEmployeeMovement(long ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetByID").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);
            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new Form(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(List<Form>, bool, string)> GetEmployeeMovementByIDs(List<long> ID)
        {

            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetByIDs").Value, "?",
                  "userid=", _globalCurrentUser.UserID, "&",
                  "id=", ID);
            return await SharedUtilities.PostFromAPI(new List<Form>(), ID, URL);
        }

        public async Task<List<GetMovementTypeOutput>> GetMovementType(MovementTypeForm param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetMovementType").Value, "?",
                "userid=", _globalCurrentUser.UserID, "&",
                "MovementType=", param.MovementType);

            var (Result, IsSuccess, ErrorMessage) = await SharedUtilities.GetFromAPI(new List<GetMovementTypeOutput>(), URL);
            if (IsSuccess)
                return Result;
            else
                throw new Exception(ErrorMessage);
        }

        public async Task<(bool, string)> PostChangeStatus(ChangeStatus param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                  _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("ChangeStatus").Value, "?",
                  "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }

        public async Task<(bool, string)> UpdateDateTo(Form param)
        {
            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("UpdateDateTo").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(param, URL);
        }
        public async Task<(List<TableVarEmployeeMovementByEmployeeIDsOutput>,bool, string)> GetEmployeeMovementByEmployeeIDs(List<int> EmployeeIDs)
        {
            var URL = string.Concat(_plantillaBaseURL,
                               _iconfiguration.GetSection("PlantillaService_API_URL").GetSection("EmployeeMovement").GetSection("GetEmployeeMovementByEmployeeIDs").Value, "?",
                               "userid=", _globalCurrentUser.UserID);

            return await SharedUtilities.PostFromAPI(new List<TableVarEmployeeMovementByEmployeeIDsOutput>(), EmployeeIDs, URL);
        }
    }
}
