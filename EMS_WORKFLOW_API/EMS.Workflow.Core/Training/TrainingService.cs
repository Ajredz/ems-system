using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.Training;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.Training;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Core.Training
{
    public interface ITrainingService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput param);
        Task<IActionResult> Add(APICredentials credentials, TrainingTempateInput param);
        Task<IActionResult> Edit(APICredentials credentials, TrainingTempateInput param);
        Task<IActionResult> GetTrainingTemplateDropdown(APICredentials credentials);
        Task<IActionResult> GetByID(APICredentials credentials, int ID);
        Task<IActionResult> GetDetailsByTrainingTemplateID(APICredentials credentials, int ID);
        Task<IActionResult> GetEmployeeTrainingList(APICredentials credentials, GetEmployeeTrainingListInput param);
        Task<IActionResult> AddEmployeeTrainingTemplate(APICredentials credentials, AddEmployeeTrainingInput param);
        Task<IActionResult> AddEmployeeTraining(APICredentials credentials, EmployeeTrainingForm param);
        Task<IActionResult> UploadInsert(APICredentials credentials, List<TrainingUploadFile> param);
        Task<IActionResult> EditEmployeeTraining(APICredentials credentials, EmployeeTrainingForm param);
        Task<IActionResult> GetEmployeeTrainingByID(APICredentials credentials, int ID);
        Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatus param);
        Task<IActionResult> GetEmployeeTrainingStatusHistory(APICredentials credentials, int EmployeeTrainingID);
        Task<IActionResult> GetEmployeeTrainingScore(APICredentials credentials, int EmployeeTrainingID);
        Task<IActionResult> GetEmployeeByEmployeeTrainingIDs(APICredentials credentials, List<int> EmployeeTrainingID);

    }

    public class TrainingService : Core.Shared.Utilities, ITrainingService
    {
        private readonly EMS.Workflow.Data.Training.ITrainingDBAccess _dbAccess;
        private readonly EMS.Workflow.Data.Reference.IReferenceDBAccess _dbReferenceService;

        public TrainingService(WorkflowContext dbContext, IConfiguration iconfiguration,
            EMS.Workflow.Data.Training.ITrainingDBAccess dBAccess, Data.Reference.IReferenceDBAccess dbReferenceService) : base(dbContext, iconfiguration)
        {
            _dbAccess = dBAccess;
            _dbReferenceService = dbReferenceService;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput param)
        {
            int rowStart = 1;
            rowStart = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : rowStart;

            IEnumerable<TableVarTableTemplate> result = await _dbAccess.GetList(param, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)param.rows),
                ID = x.ID,
                TemplateName = x.TemplateName,
                CreatedDate = x.CreatedDate
            }).ToList());
        }

        public async Task<IActionResult> Add(APICredentials credentials, TrainingTempateInput param)
        {
            param.TemplateName = param.TemplateName.Trim();
            if (string.IsNullOrEmpty(param.TemplateName))
                ErrorMessages.Add("Template Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                DateTime dateTime = DateTime.Now;
                await _dbAccess.Add(new Data.Training.TrainingTemplate
                {
                    TemplateName = param.TemplateName,
                    IsActive = true,
                    CreatedBy = credentials.UserID,
                    CreatedDate = dateTime
                },
                param.TrainingTempateDetailsInputList?.Select(x => new Data.Training.TrainingTemplateDetails
                {
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    IsActive = true,
                    CreatedBy = credentials.UserID
                }).ToList()
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Edit(APICredentials credentials, TrainingTempateInput param)
        {
            param.TemplateName = param.TemplateName.Trim();
            if (string.IsNullOrEmpty(param.TemplateName))
                ErrorMessages.Add("Template Name " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                DateTime dateTime = DateTime.Now;

                IEnumerable<Data.Training.TrainingTemplateDetails> GetDetailsToAdd(List<Data.Training.TrainingTemplateDetails> left, List<Data.Training.TrainingTemplateDetails> right)
                {
                    return right.GroupJoin(
                        left,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { newSet = x, oldSet = y })
                        .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                        (x, y) => new { newSet = x, oldSet = y })
                        .Where(x => x.oldSet == null)
                        .Select(x =>
                            new Data.Training.TrainingTemplateDetails
                            {
                                TrainingTemplateID = x.newSet.newSet.TrainingTemplateID,
                                Type = x.newSet.newSet.Type,
                                Title = x.newSet.newSet.Title,
                                Description = x.newSet.newSet.Description,
                                IsActive = x.newSet.newSet.IsActive,
                                CreatedBy = x.newSet.newSet.CreatedBy
                            }).ToList();
                }
                IEnumerable<Data.Training.TrainingTemplateDetails> GetDetailsToEdit(List<Data.Training.TrainingTemplateDetails> left, List<Data.Training.TrainingTemplateDetails> right)
                {
                    return left.Join(
                        right,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => !x.oldSet.Type.Equals(x.newSet.Type) ||
                                    !x.oldSet.Title.Equals(x.newSet.Title) ||
                                    (x.oldSet.Description != x.newSet.Description)
                              )
                        .Select(y =>
                            new Data.Training.TrainingTemplateDetails
                            {
                                ID = y.oldSet.ID,
                                TrainingTemplateID = y.newSet.TrainingTemplateID,
                                Type = y.newSet.Type,
                                Title = y.newSet.Title,
                                Description = y.newSet.Description,
                                IsActive = y.oldSet.IsActive,
                                CreatedBy = y.oldSet.CreatedBy,
                                CreatedDate = y.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = dateTime
                            }).ToList();
                }
                IEnumerable<Data.Training.TrainingTemplateDetails> GetDetailsToDelete(List<Data.Training.TrainingTemplateDetails> left, List<Data.Training.TrainingTemplateDetails> right)
                {
                    return left.GroupJoin(
                        right,
                             x => new { x.Type, x.Title },
                             y => new { y.Type, y.Title },
                        (x, y) => new { oldSet = x, newSet = y })
                        .SelectMany(x => x.newSet.DefaultIfEmpty(),
                        (x, y) => new { oldSet = x, newSet = y })
                        .Where(x => x.newSet == null)
                        .Select(x =>
                            new Data.Training.TrainingTemplateDetails
                            {
                                ID = x.oldSet.oldSet.ID,
                                TrainingTemplateID = x.oldSet.oldSet.TrainingTemplateID,
                                Type = x.oldSet.oldSet.Type,
                                Title = x.oldSet.oldSet.Title,
                                Description = x.oldSet.oldSet.Description,
                                IsActive = false,
                                CreatedBy = x.oldSet.oldSet.CreatedBy,
                                CreatedDate = x.oldSet.oldSet.CreatedDate,
                                ModifiedBy = credentials.UserID,
                                ModifiedDate = dateTime
                            }).ToList();
                }

                List<Data.Training.TrainingTemplateDetails> OldTrainingDetails = (await _dbAccess.GetDetailsByTrainingTemplateID(param.ID)).ToList();

                List<Data.Training.TrainingTemplateDetails> DetailsToAdd = GetDetailsToAdd(OldTrainingDetails,
                    param.TrainingTempateDetailsInputList.Select(x => new Data.Training.TrainingTemplateDetails
                    {
                        TrainingTemplateID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        IsActive = true,
                        CreatedBy = credentials.UserID,
                        CreatedDate = dateTime
                    }).ToList()).ToList();
                List<Data.Training.TrainingTemplateDetails> DetailsToEdit = GetDetailsToEdit(OldTrainingDetails,
                    param.TrainingTempateDetailsInputList.Select(x => new Data.Training.TrainingTemplateDetails
                    {
                        TrainingTemplateID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        IsActive = true,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = dateTime
                    }).ToList()).ToList();
                List<Data.Training.TrainingTemplateDetails> DetailsToDelete = GetDetailsToDelete(OldTrainingDetails,
                    param.TrainingTempateDetailsInputList.Select(x => new Data.Training.TrainingTemplateDetails
                    {
                        TrainingTemplateID = param.ID,
                        Type = x.Type,
                        Title = x.Title,
                        Description = x.Description,
                        IsActive = false,
                        CreatedBy = x.CreatedBy,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                Data.Training.TrainingTemplate trainingTemplate = await _dbAccess.GetByID(param.ID);
                trainingTemplate.TemplateName = param.TemplateName;
                trainingTemplate.IsActive = true;
                trainingTemplate.ModifiedBy = credentials.UserID;
                trainingTemplate.ModifiedDate = dateTime;

                await _dbAccess.Edit(
                    trainingTemplate,
                    DetailsToAdd,
                    DetailsToEdit,
                    DetailsToDelete
                );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> GetTrainingTemplateDropdown(APICredentials credentials)
        {
            return new OkObjectResult(SharedUtilities.GetDropdown((await _dbAccess.GetAllTrainingTemplate()).ToList(), "ID", "TemplateName", null));
        }
        public async Task<IActionResult> GetByID(APICredentials credentials, int ID)
        {
            Data.Training.TrainingTemplate result = await _dbAccess.GetByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new TrainingTemplate
                {
                    ID = result.ID,
                    TemplateName = result.TemplateName
                });
        }

        public async Task<IActionResult> GetDetailsByTrainingTemplateID(APICredentials credentials, int ID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetDetailsByTrainingTemplateID(ID))
                .Select(x => new TrainingTempateDetailsInput
                {
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description
                }).ToList()
            );
        }

        public async Task<IActionResult> GetEmployeeTrainingList(APICredentials credentials, GetEmployeeTrainingListInput param)
        {
            int rowStart = 1;
            rowStart = param.pageNumber > 1 ? param.pageNumber * param.rows - param.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeTraining> result = await _dbAccess.GetEmployeeTrainingList(param, rowStart);

            return new OkObjectResult(result.Select(x => new GetEmployeeTrainingListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)param.rows),
                ID = x.ID,
                EmployeeID = x.EmployeeID,
                Status = x.Status,
                StatusUpdateDate = x.StatusUpdateDate,
                DateSchedule = x.DateSchedule,
                Type = x.Type,
                Title = x.Title,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
            }).ToList());
        }

        public async Task<IActionResult> AddEmployeeTrainingTemplate(APICredentials credentials, AddEmployeeTrainingInput param)
        {
            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("EmployeeID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (param.TrainingEmployeeID.Count <= 0)
                ErrorMessages.Add(string.Concat("TrainingTemplateID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (ErrorMessages.Count == 0)
            {
                List<EmployeeTrainingStatusHistory> result =
                        (await _dbAccess.AddEmployeeTrainingTemplate(param, credentials.UserID)).ToList();
                return new OkObjectResult(string.Concat(result.First().ID, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED));
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> AddEmployeeTraining(APICredentials credentials, EmployeeTrainingForm param)
        {
            DateTime dateTime = DateTime.Now;

            EmployeeTraining employeeTraining = new EmployeeTraining();

            employeeTraining.EmployeeID = param.EmployeeID;
            employeeTraining.Status = param.Status;
            employeeTraining.StatusUpdateDate = dateTime;
            employeeTraining.Type = param.Type;
            employeeTraining.Title = param.Title;
            employeeTraining.Description = param.Description;
            employeeTraining.DateSchedule = param.DateSchedule;
            employeeTraining.IsActive = true;
            employeeTraining.CreatedBy = credentials.UserID;
            employeeTraining.CreatedDate = dateTime;

            EmployeeTrainingStatusHistory employeeTrainingStatusHistory = new EmployeeTrainingStatusHistory();

            employeeTrainingStatusHistory.Status = param.Status;
            employeeTrainingStatusHistory.IsActive = true;
            employeeTrainingStatusHistory.CreatedBy = credentials.UserID;
            employeeTrainingStatusHistory.CreatedDate = dateTime;

            var Result = (await _dbAccess.AddEmployeeTraining(employeeTraining, employeeTrainingStatusHistory));
            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<TrainingUploadFile> param)
        {

            var typeList = (await _dbReferenceService.GetByRefCodes(new List<string> { "TRAINING_TYPE" })).Select(x => x.Value).ToList();

            /*Checking of required and invalid fields*/
            foreach (TrainingUploadFile obj in param)
            {

                /*Old Employee ID*/
                if (string.IsNullOrEmpty(obj.EmployeeCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else if (obj.EmployeeID == 0)
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Code ", MessageUtilities.COMPARE_INVALID));
                }
                else
                {
                    obj.EmployeeCode = obj.EmployeeCode.Trim();
                    if (obj.EmployeeCode.Length > 6)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "5 characters."));
                    }
                }


                /*Type*/
                if (string.IsNullOrEmpty(obj.Type))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Type = obj.Type.Trim();
                    if (obj.Type.Length > 50)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                    }

                    if (!Regex.IsMatch(obj.Type, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (typeList.Where(x => obj.Type.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Type ", MessageUtilities.COMPARE_INVALID));
                    }
                }

                /*Title*/
                if (string.IsNullOrEmpty(obj.Title))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Title = obj.Title.Trim();
                    if (obj.Title.Length > 100)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Title", MessageUtilities.COMPARE_NOT_EXCEED, "100 characters."));
                    }
                }

                /*Description*/
                if (!string.IsNullOrEmpty(obj.Description))
                {
                    obj.Description = obj.Description.Trim();
                    if (obj.Description.Length > 255)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Description", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }

                /*DateSchedule*/
                if (string.IsNullOrEmpty(obj.DateSchedule))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Schedule ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.DateSchedule = obj.DateSchedule.Trim();
                    if (!DateTime.TryParseExact(obj.DateSchedule, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateSchedule))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Date Schedule", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.DateScheduleConvert = dateSchedule;
                    }
                }

                /*ClassroomID*/
                if (!string.IsNullOrEmpty(obj.ClassroomIDString))
                {
                    if (int.TryParse(obj.ClassroomIDString, out int value))
                        obj.ClassroomID = Convert.ToInt32(obj.ClassroomIDString);
                    else
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Classroom ID", MessageUtilities.COMPARE_INVALID_NUMBER));

                }

                if((await _dbAccess.GetEmployeeTrainingByEmployeeIDTitle(obj.EmployeeID,obj.Title)).Count()>0)
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Training is ", MessageUtilities.SUFF_ERRMSG_REC_EXISTS));

            }

            List<string> Duplicates = new List<string>();

            /*Remove Duplicates*/
            if (ErrorMessages.Count == 0)
            {
                var tempParam = param.ToList();
                foreach (var obj in tempParam.ToList())
                {
                    /* Remove duplicates within file */
                    var duplicateWithinFile = param.Where(x =>
                    obj.EmployeeCode.Equals(x.EmployeeCode, StringComparison.OrdinalIgnoreCase) &
                    obj.Type.Equals(x.Type, StringComparison.OrdinalIgnoreCase) &
                    obj.Title.Equals(x.Title, StringComparison.OrdinalIgnoreCase) &
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                        Duplicates.Add("Row [" + obj.RowNum + "]");
                    }

                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                List<TrainingUploadFile> trainingUploadFiles = new List<TrainingUploadFile>();

                if (param != null)
                {
                    foreach (var obj in param)
                    {
                        trainingUploadFiles.Add(new TrainingUploadFile
                        {
                            EmployeeID = obj.EmployeeID,
                            Status = obj.Status,
                            Type = obj.Type,
                            Title = obj.Title,
                            Description = obj.Description,
                            DateScheduleConvert = obj.DateScheduleConvert,
                            ClassroomID = obj.ClassroomID,
                            ClassroomName = obj.ClassroomName
                        });
                    }

                    await _dbAccess.UploadInsert(trainingUploadFiles,credentials);
                }

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
            {
                if (Duplicates.Count > 0)
                {
                    return new OkObjectResult(
                        string.Concat(param?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                            MessageUtilities.ERRMSG_DUPLICATE_APPLICANT, "<br>",
                            string.Join("<br>", Duplicates.Distinct().ToArray()))
                        );
                }
                else
                {
                    return new OkObjectResult(string.Concat(param?.Count, " Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD));
                }
            }
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> EditEmployeeTraining(APICredentials credentials, EmployeeTrainingForm param)
        {
            DateTime dateTime = DateTime.Now;

            var employeeTraining = await _dbAccess.GetEmployeeTrainingByID(param.ID);

            employeeTraining.ClassroomID = param.ClassroomID;
            employeeTraining.ClassroomName = param.ClassroomName;
            employeeTraining.Type = param.Type;
            employeeTraining.Title = param.Title;
            employeeTraining.Description = param.Description;
            employeeTraining.DateSchedule = param.DateSchedule;
            employeeTraining.ModifiedBy = credentials.UserID;
            employeeTraining.ModifiedDate = dateTime;

            var Result = (await _dbAccess.EditEmployeeTraining(employeeTraining));
            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }

        public async Task<IActionResult> GetEmployeeTrainingByID(APICredentials credentials, int ID)
        {
            Data.Training.EmployeeTraining result = await _dbAccess.GetEmployeeTrainingByID(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(
                 new EmployeeTrainingForm
                 {
                     ID = result.ID,
                     EmployeeID = result.EmployeeID,
                     Status = result.Status,
                     StatusUpdateDate = result.StatusUpdateDate,
                     ClassroomID = result.ClassroomID,
                     ClassroomName = result.ClassroomName,
                     Type = result.Type,
                     Title = result.Title,
                     Description = result.Description,
                     DateSchedule = result.DateSchedule,
                     IsActive = result.IsActive,
                     CreatedBy = result.CreatedBy,
                     CreatedDate = result.CreatedDate,
                     ModifiedBy = result.ModifiedBy,
                     ModifiedDate = result.ModifiedDate
                 });
            }
        }
        public async Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatus param)
        {
            DateTime dateTime = DateTime.Now;

            var GetEmployeeTraining = (await _dbAccess.GetEmployeeTrainingByIDs(param.ID));
            var UpdateEmployeeTraining = GetEmployeeTraining
                .Select(x => new EmployeeTraining()
                {
                    ID = x.ID,
                    EmployeeID = x.EmployeeID,
                    Status = param.Status,
                    StatusUpdateDate = dateTime,
                    ClassroomID = x.ClassroomID,
                    ClassroomName = x.ClassroomName,
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    DateSchedule = x.DateSchedule,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate
                }).ToList();

            List<EmployeeTrainingStatusHistory> employeeTrainingStatusHistories = param.ID
                .Select(x => new EmployeeTrainingStatusHistory()
                {
                    EmployeeTrainingID = (int)x,
                    Status = param.Status,
                    Remarks = param.Remarks,
                    IsActive = true,
                    CreatedBy = credentials.UserID,
                    CreatedDate = dateTime
                }).ToList();

            var Result = (await _dbAccess.ChangeStatus(UpdateEmployeeTraining, employeeTrainingStatusHistories));
            if (Result)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
        }
        public async Task<IActionResult> GetEmployeeTrainingStatusHistory(APICredentials credentials, int EmployeeTrainingID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeTrainingStatusHistory(EmployeeTrainingID))
                .Select(x => new GetEmployeeTrainingStatusHistoryOutput
                {
                    Status = x.Status,
                    Remarks = x.Remarks ?? "",
                    CreatedName = "",
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate

                }).ToList()
            );
        }
        public async Task<IActionResult> GetEmployeeTrainingScore(APICredentials credentials, int EmployeeTrainingID)
        {
            return new OkObjectResult(
                (await _dbAccess.GetEmployeeTrainingScore(EmployeeTrainingID))
                .Select(x => new GetEmployeeTrainingScoreOutput
                {
                    ID = x.ID,
                    TakeExamID = x.TakeExamID,
                    ExamScore = x.ExamScore,
                    TotalExamScore = x.TotalExamScore,
                    AverageScore = x.AverageScore,
                    TotalExamQuestion = x.TotalExamQuestion,
                    CompletedDate = x.CompletedDate
                }).ToList()
            );
        }
        public async Task<IActionResult> GetEmployeeByEmployeeTrainingIDs(APICredentials credentials, List<int> EmployeeTrainingID)
        {
            return new OkObjectResult(((await _dbAccess.GetEmployeeByEmployeeTrainingIDs(EmployeeTrainingID)).ToList()));
        }
    }
}
