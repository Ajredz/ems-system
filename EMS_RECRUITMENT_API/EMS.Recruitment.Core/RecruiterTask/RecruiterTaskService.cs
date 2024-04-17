using EMS.Recruitment.Data.DBContexts;
using EMS.Recruitment.Data.RecruiterTask;
using EMS.Recruitment.Transfer;
using EMS.Recruitment.Transfer.RecruiterTask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Recruitment.Core.RecruiterTask
{
    public interface IRecruiterTaskService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetPendingList(APICredentials credentials, GetPendingListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, int ID);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> SingleUpdate(APICredentials credentials, Form param);

        Task<IActionResult> BatchUpdate(APICredentials credentials, BatchForm param);
    }

    public class RecruiterTaskService : EMS.Recruitment.Core.Shared.Utilities, IRecruiterTaskService
    {
        private readonly IRecruiterTaskDBAccess _dbAccess;

        public RecruiterTaskService(RecruitmentContext dbContext, IConfiguration iconfiguration,
            IRecruiterTaskDBAccess dBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarRecruiterTask> result = await _dbAccess.GetList(input, credentials.UserID, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.RecruiterTask.GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Recruiter = x.Recruiter,
                Applicant = x.Applicant,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            }).ToList());
        }

        public async Task<IActionResult> GetPendingList(APICredentials credentials, GetPendingListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarPendingTask> result = await _dbAccess.GetPendingList(input, credentials.UserID, rowStart);

            return new OkObjectResult(result.Select(x => new Transfer.RecruiterTask.GetPendingListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Applicant = x.Applicant,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate
            }).ToList());
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.RecruiterTask.RecruiterTask result = await _dbAccess.GetByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    RecruiterID = result.RecruiterUserId,
                    ApplicantID = result.ApplicantId,
                    Description = result.Description,
                    Status = result.Status,
                    Remarks = result.Remarks,
                    CreatedBy = result.CreatedBy
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {

            if (param.RecruiterID <= 0)
                ErrorMessages.Add(string.Concat("Recruiter ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("Applicant ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Description = (param.Description ?? "").Trim();
            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add(string.Concat("Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if ((await _dbAccess.GetByDetails(param.RecruiterID, param.ApplicantID, param.Description)).Count() > 0)
            {
                ErrorMessages.Add("Task " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Post(new Data.RecruiterTask.RecruiterTask
                {
                    RecruiterUserId = param.RecruiterID,
                    ApplicantId = param.ApplicantID,
                    Description = param.Description,
                    Status = Enums.TaskStatus.OPEN.ToString(),
                    CreatedBy = credentials.UserID
                });

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            await _dbAccess.Delete(ID);
            return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            if (param.RecruiterID <= 0)
                ErrorMessages.Add(string.Concat("Recruiter ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.ApplicantID <= 0)
                ErrorMessages.Add(string.Concat("Applicant ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            param.Description = (param.Description ?? "").Trim();
            if (string.IsNullOrEmpty(param.Description))
                ErrorMessages.Add(string.Concat("Description ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.Description.Length > 255)
                ErrorMessages.Add(string.Concat("Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            List<Data.RecruiterTask.RecruiterTask> data = (await _dbAccess.GetByDetails(param.RecruiterID, param.ApplicantID, param.Description)).ToList();
            if (data.Where(x => x.ID != param.ID).Count() > 0)
            {
                ErrorMessages.Add("Task " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS); 
            }
            

            if (ErrorMessages.Count == 0)
            {
                Data.RecruiterTask.RecruiterTask form =
                    await _dbAccess.GetByID(param.ID);

                form.RecruiterUserId = param.RecruiterID;
                form.ApplicantId = param.ApplicantID;
                form.Description = param.Description;
                form.ModifiedBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> SingleUpdate(APICredentials credentials, Form param)
        {

            param.Status = (param.Status ?? "").Trim();
            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.Status.Length > 20)
                ErrorMessages.Add(string.Concat("Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            param.Remarks = (param.Remarks ?? "").Trim();
            if (param.Remarks.Length > 255)
                ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));


            if (ErrorMessages.Count == 0)
            {
                Data.RecruiterTask.RecruiterTask form = await _dbAccess.GetByID(param.ID);

                form.Status = param.Status;
                form.Remarks = param.Remarks;
                form.ModifiedBy = credentials.UserID;
                form.ModifiedDate = DateTime.Now;

                await _dbAccess.Put(form);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> BatchUpdate(APICredentials credentials, BatchForm param)
        {

            param.Status = (param.Status ?? "").Trim();
            if (string.IsNullOrEmpty(param.Status))
                ErrorMessages.Add(string.Concat("Status ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
                if (param.Status.Length > 20)
                ErrorMessages.Add(string.Concat("Status", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));

            param.Remarks = (param.Remarks ?? "").Trim();
            if (param.Remarks.Length > 255)
                ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));

            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.RecruiterTask.RecruiterTask> lstRecruiterTask = await _dbAccess.GetByIDs(param.IDs);

                await _dbAccess.BatchUpdate(
                    lstRecruiterTask.Select(x =>
                    {
                        x.Status = param.Status;
                        x.Remarks = param.Remarks;
                        x.ModifiedBy = credentials.UserID;
                        x.ModifiedDate = DateTime.Now;
                        return x;
                    }).ToList()
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

    }
}
