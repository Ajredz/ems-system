﻿using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.KRASubGroup;
using EMS.IPM.Transfer.KRASubGroup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.KRASubGroup
{
    public interface IKRASubGroupService
    {
        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> GetKRASubGroupDropDown(APICredentials credentials, int KRAGroupID, int SelectedID);

        Task<IActionResult> GetAutoComplete(APICredentials credentials, IPM.Transfer.Shared.GetAutoCompleteInput param);
    }

    public class KRASubGroupService : Core.Shared.Utilities, IKRASubGroupService
    {
        private readonly IKRASubGroupDBAccess _dbAccess;

        public KRASubGroupService(IPMContext dbContext, IConfiguration iconfiguration,
            IKRASubGroupDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            param.Name = param.Name.Trim();

            if (string.IsNullOrEmpty(param.Name))
                ErrorMessages.Add("Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);
            else if (param.Name.Length > 255)
                ErrorMessages.Add(string.Concat("Name", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            else
            {
                if ((await _dbAccess.GetKRASubGroup(param.Name)).Count() > 0)
                {
                    ErrorMessages.Add(param.Name + " " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
                }

                //if (!Regex.IsMatch(param.Description, RegexUtilities.REGEX_CODE))
                //{
                //    ErrorMessages.Add(MessageUtilities.ERRMSG_REGEX_CODE);
                //}
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.KRASubGroup.KRASubGroup
                {
                    Name = param.Name,
                    KRAGroup = param.KRAGroup,
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetKRASubGroupDropDown(APICredentials credentials, int KRAGroupID, int SelectedID)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAll())
                                                                         .Where(x => x.KRAGroup == KRAGroupID)
                                                                         .OrderBy(x => x.ID).ToList(), "ID", "Name", null, SelectedID));
        }

        public async Task<IActionResult> GetAutoComplete(APICredentials credentials, IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return new OkObjectResult((await _dbAccess.GetAutoComplete(param))
                .Select(x => new IPM.Transfer.Shared.GetAutoCompleteOutput { ID = x.ID,Description = x.Description})
            );
        }
    }
}