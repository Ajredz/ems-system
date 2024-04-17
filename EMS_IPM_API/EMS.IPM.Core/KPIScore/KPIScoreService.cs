using EMS.IPM.Data.DataDuplication.Employee;
using EMS.IPM.Data.DataDuplication.OrgGroup;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.KPI;
using EMS.IPM.Data.KPIPosition;
using EMS.IPM.Data.KPIScore;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer.KPIScore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.KPIScore
{
    public interface IKPIScoreService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> ValidateUploadScores(APICredentials credentials, List<UploadScoresFile> param);

        Task<IActionResult> UploadScores(APICredentials credentials, List<UploadScoresFile> param);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        //Task<IActionResult> Post(APICredentials credentials, Form param);

        //Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> GetAll(APICredentials credentials);
    }

    public class KPIScoreService : Core.Shared.Utilities, IKPIScoreService
    {
        private readonly IKPIScoreDBAccess _dbAccess;
        private readonly IKPIDBAccess _kpiDBAccess;
        private readonly IOrgGroupDBAccess _orgGroupDBAccess;
        private readonly IEmployeeDBAccess _employeeDBAccess;

        public KPIScoreService(IPMContext dbContext, IConfiguration iconfiguration,
            IKPIScoreDBAccess dbAccess, IKPIDBAccess kpiDBAccess, IOrgGroupDBAccess orgGroupDBAccess, IEmployeeDBAccess employeeDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _kpiDBAccess = kpiDBAccess;
            _orgGroupDBAccess = orgGroupDBAccess;
            _employeeDBAccess = employeeDBAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarKPIScore> result = null;
                
            if(input.KPIType.Equals("QUANTITATIVE"))
                result = await _dbAccess.GetList(input, rowStart);
            else /*QUALITATIVE*/
                result = await _dbAccess.GetPerEmployeeList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                KPI = x.KPI,
                Employee = x.Employee,
                ParentOrgGroup = x.ParentOrgGroup,
                OrgGroup = x.OrgGroup,
                Target = Math.Round(x.Target, 2).ToString(),
                Actual = Math.Round(x.Actual, 2).ToString(),
                Rate = Math.Round(x.Rate, 2).ToString(),
                Period = x.Period
            }).ToList());
        }

        public async Task<IActionResult> ValidateUploadScores(APICredentials credentials, List<UploadScoresFile> param)
        {
            List<Data.KPIScore.KPIScore> scores = new List<Data.KPIScore.KPIScore>();

            var kpiList = await _kpiDBAccess.GetAll();
            var kpiCodes = kpiList.Select(x => x.Code).ToList();

            var orgGroupList = await _orgGroupDBAccess.GetAll();
            var orgGroups = orgGroupList.Select(x => x.Code).ToList();

            var employeeList = await _employeeDBAccess.GetAll();
            var employees = employeeList.Select(x => x.Code).ToList();

            bool isUploadExist = false;

            #region Excel File Validation
            /*Checking of required and invalid fields*/
            foreach (UploadScoresFile obj in param)
            {
                /* KPI Code */
                if (string.IsNullOrEmpty(obj.KPICode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.KPICode = obj.KPICode.Trim();
                    if (!Regex.IsMatch(obj.KPICode, RegexUtilities.REGEX_CODE))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                    }

                    if (kpiCodes.Where(x => obj.KPICode.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Code ", MessageUtilities.COMPARE_INVALID));
                    }
                    else
                    {
                        if (kpiList.Where(x => x.Code.Equals(obj.KPICode)).ToList().Count() > 0)
                        {
                            obj.KPIID = kpiList.Where(x => x.Code.Equals(obj.KPICode)).FirstOrDefault().ID;
                        }
                    }
                }

                if (obj.KPIType.Equals("QUANTITATIVE"))
                {
                    /* Org. Group Code */
                    if (string.IsNullOrEmpty(obj.OrgGroupCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org. Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.OrgGroupCode = obj.OrgGroupCode.Trim();
                        if (!Regex.IsMatch(obj.OrgGroupCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org. Group Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (orgGroups.Where(x => obj.OrgGroupCode.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Org. Group Code ", MessageUtilities.COMPARE_INVALID));
                        }
                        else
                        {
                            if (orgGroupList.Where(x => x.Code.Equals(obj.OrgGroupCode)).ToList().Count() > 0)
                            {
                                obj.OrgGroupID = orgGroupList.Where(x => x.Code.Equals(obj.OrgGroupCode)).FirstOrDefault().SyncID;
                            }
                        }
                    }  
                }
                else /*QUALITATIVE*/
                {
                    /* Employee Code */
                    if (string.IsNullOrEmpty(obj.EmployeeCode))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.EmployeeCode = obj.EmployeeCode.Trim();
                        if (!Regex.IsMatch(obj.EmployeeCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (employees.Where(x => obj.EmployeeCode.Equals(x, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Employee Code ", MessageUtilities.COMPARE_INVALID));
                        }
                        else
                        {
                            if (employeeList.Where(x => x.Code.Equals(obj.EmployeeCode)).ToList().Count() > 0)
                            {
                                obj.OrgGroupID = employeeList.Where(x => x.Code.Equals(obj.EmployeeCode)).FirstOrDefault().SyncID;
                            }
                        }
                    } 
                }

                /* Target */
                if (string.IsNullOrEmpty(obj.Target))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Target ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Target = obj.Target.Trim();
                    if (!decimal.TryParse(obj.Target, out _))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Target ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                /* Actual */
                if (string.IsNullOrEmpty(obj.Actual))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Actual ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Actual = obj.Actual.Trim();
                    if (!decimal.TryParse(obj.Actual, out _))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Actual ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                /* Rate */
                if (string.IsNullOrEmpty(obj.Rate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Rate ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Rate = obj.Rate.Trim();
                    if (!decimal.TryParse(obj.Rate, out _))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Rate ", MessageUtilities.SUFF_ERRMSG_INVALID));
                    }
                }

                /*Period*/
                if (string.IsNullOrEmpty(obj.Period))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Period ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.Period = obj.Period.Trim();
                    if (!DateTime.TryParseExact(obj.Period, "M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Period", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.PeriodDateConverted = convertedDate;
                    }
                }
            }

            List<string> Duplicates = new List<string>();
            var tempParam = param.ToList();

            /*Remove Duplicates*/
            if (ErrorMessages.Count == 0)
            {
                foreach (var obj in tempParam.ToList())
                {
                    if (obj.KPIType.Equals("QUANTITATIVE"))
                    {
                        /* Remove duplicates within file */
                        var duplicateWithinFile = param.Where(x =>
                        obj.KPICode.Equals(x.KPICode) &
                        obj.OrgGroupCode.Equals(x.OrgGroupCode) &&
                        obj.PeriodDateConverted == x.PeriodDateConverted &&
                        obj.RowNum != x.RowNum).FirstOrDefault();

                        if (duplicateWithinFile != null)
                        {
                            param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        }
                    }
                    else /*QUALITATIVE*/
                    {
                        /* Remove duplicates within file */
                        var duplicateWithinFile = param.Where(x =>
                        obj.KPICode.Equals(x.KPICode) &
                        obj.EmployeeCode.Equals(x.EmployeeCode) &&
                        obj.PeriodDateConverted == x.PeriodDateConverted &&
                        obj.RowNum != x.RowNum).FirstOrDefault();

                        if (duplicateWithinFile != null)
                        {
                            param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        }
                    }
                }

                _resultView.IsSuccess = true;
            }
            #endregion

            #region Check if Org. Groups already have scores

            //var (ValueToAdd, ValueToUpdate) = await GetToAddUpdate(credentials, param);

            //if (ValueToUpdate.Count > 0)
            //    isUploadExist = true;

            //foreach (var obj in tempParam.ToList())
            //{
            //    var orgGroup = orgGroupList.FirstOrDefault(x => x.Code == obj.OrgGroupCode)?.SyncID;
            //    var kpi = kpiList.FirstOrDefault(x => x.Code == obj.KPICode)?.ID;

            //    var duplicateFromDatabase = (await _dbAccess.GetByKPIOrg(orgGroup ?? 0, kpi ?? 0)).ToList();
            //    if (duplicateFromDatabase != null)
            //    {
            //        if (duplicateFromDatabase.Count() > 0)
            //        {
            //            isUploadExist = true;
            //            break;
            //        }
            //    }
            //}
            #endregion

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
            {
                //if (Duplicates.Count > 0)
                //{
                //    return new OkObjectResult(
                //        new UploadScoresOutput
                //        {
                //            Message = string.Concat(scores?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                //            MessageUtilities.ERRMSG_KPI_SCORES, "<br>",
                //            string.Join("<br>", Duplicates.Distinct().ToArray()))
                //        }
                //    );
                //}
                //else
                //{
                    if (isUploadExist)
                    {
                        return new OkObjectResult(
                            new UploadScoresOutput
                            {
                                Message = "Override" // String for validation only; not a message to show the User
                            });
                    }
                    else
                    {
                        return new OkObjectResult(
                            new UploadScoresOutput
                            {
                                Message = "Valid data" // String for validation only; not a message to show the User
                            });
                    }

                //}
            }
            else
            {

                if (ErrorMessages.Count > 52)
                {
                    string ErrorMessage = string.Join("<br>", ErrorMessages.Take(52).ToArray());
                    ErrorMessage += string.Concat("<br><br> ", ErrorMessages.Count - 52, " other errors found.");
                    return new BadRequestObjectResult(ErrorMessage);
                }
                else
                {
                    return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
                }
            }
        }

        private async Task<(List<Data.KPIScore.KPIScore>, List<Data.KPIScore.KPIScore>)> GetToAddUpdate(APICredentials credentials, List<UploadScoresFile> param)
        {
            static List<Data.KPIScore.KPIScore> GetToAdd(List<Data.KPIScore.KPIScore> left, List<Data.KPIScore.KPIScore> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { x.KPI, x.OrgGroup, x.Period },
                    y => new { y.KPI, y.OrgGroup, y.Period },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.KPIScore.KPIScore
                    {
                        OrgGroup = x.newSet.newSet.OrgGroup,
                        KPI = x.newSet.newSet.KPI,
                        Target = x.newSet.newSet.Target,
                        Actual = x.newSet.newSet.Actual,
                        Rate = x.newSet.newSet.Rate,
                        Period = x.newSet.newSet.Period,
                        Formula = x.newSet.newSet.Formula,
                        ModifiedBy = x.newSet.newSet.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                    })
                .ToList();
            }

            static List<Data.KPIScore.KPIScore> GetToUpdate(List<Data.KPIScore.KPIScore> left, List<Data.KPIScore.KPIScore> right)
            {
                return left.Join(
                right,
                x => new { x.KPI, x.OrgGroup, x.Period },
                y => new { y.KPI, y.OrgGroup, y.Period },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.Target != x.newSet.Target
                    || x.oldSet.Actual != x.newSet.Actual
                    || x.oldSet.Rate != x.newSet.Rate
                    || !(x.oldSet.Formula ?? "").Equals((x.newSet.Formula ?? ""))
                )
                .Select(y => new Data.KPIScore.KPIScore
                {
                    ID = y.oldSet.ID,
                    OrgGroup = y.newSet.OrgGroup,
                    KPI = y.newSet.KPI,
                    Target = y.newSet.Target,
                    Actual = y.newSet.Actual,
                    Rate = y.newSet.Rate,
                    Period = y.newSet.Period,
                    Formula = y.newSet.Formula,
                    ModifiedBy = y.newSet.ModifiedBy,
                    ModifiedDate = DateTime.Now,

                })
                .ToList();
            }

            List<Data.KPIScore.KPIScore> localCopy = (await _dbAccess.GetByPeriods(param.Select(x => x.PeriodDateConverted).Distinct().ToList())).ToList();
            List<Data.KPIScore.KPIScore> ValueToAdd = GetToAdd(
                param.Select(x => new Data.KPIScore.KPIScore
                {
                    OrgGroup = x.OrgGroupID,
                    KPI = x.KPIID,
                    Target = Math.Round(Convert.ToDecimal(x.Target), 5),
                    Actual = Math.Round(Convert.ToDecimal(x.Actual), 5),
                    Rate = Math.Round(Convert.ToDecimal(x.Rate), 5),
                    ModifiedBy = credentials.UserID,
                    Period = x.PeriodDateConverted,
                }).ToList()
                , localCopy)
            .ToList();

            List<Data.KPIScore.KPIScore> ValueToUpdate = GetToUpdate(localCopy,
                param.Select(x => new Data.KPIScore.KPIScore
                {
                    OrgGroup = x.OrgGroupID,
                    KPI = x.KPIID,
                    Target = Math.Round(Convert.ToDecimal(x.Target), 5),
                    Actual = Math.Round(Convert.ToDecimal(x.Actual), 5),
                    Rate = Math.Round(Convert.ToDecimal(x.Rate), 5),
                    ModifiedBy = credentials.UserID,
                    Period = x.PeriodDateConverted,
                }).ToList())
            .ToList();

            return (ValueToAdd, ValueToUpdate);
        }

        private async Task<(List<Data.KPIScore.KPIScorePerEmployee>, List<Data.KPIScore.KPIScorePerEmployee>)> GetToAddUpdatePerEmployee(APICredentials credentials, List<UploadScoresFile> param)
        {
            static List<Data.KPIScore.KPIScorePerEmployee> GetToAdd(List<Data.KPIScore.KPIScorePerEmployee> left, List<Data.KPIScore.KPIScorePerEmployee> right)
            {
                return left.GroupJoin(
                    right,
                    x => new { x.KPI, x.EmployeeID, x.Period },
                    y => new { y.KPI, y.EmployeeID, y.Period },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.KPIScore.KPIScorePerEmployee
                    {
                        EmployeeID = x.newSet.newSet.EmployeeID,
                        KPI = x.newSet.newSet.KPI,
                        Target = x.newSet.newSet.Target,
                        Actual = x.newSet.newSet.Actual,
                        Rate = x.newSet.newSet.Rate,
                        Period = x.newSet.newSet.Period,
                        Formula = x.newSet.newSet.Formula,
                        ModifiedBy = x.newSet.newSet.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                    })
                .ToList();
            }

            static List<Data.KPIScore.KPIScorePerEmployee> GetToUpdate(List<Data.KPIScore.KPIScorePerEmployee> left, List<Data.KPIScore.KPIScorePerEmployee> right)
            {
                return left.Join(
                right,
                x => new { x.KPI, x.EmployeeID, x.Period },
                y => new { y.KPI, y.EmployeeID, y.Period },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.Target != x.newSet.Target
                    || x.oldSet.Actual != x.newSet.Actual
                    || x.oldSet.Rate != x.newSet.Rate
                    || !(x.oldSet.Formula ?? "").Equals((x.newSet.Formula ?? ""))
                )
                .Select(y => new Data.KPIScore.KPIScorePerEmployee
                {
                    ID = y.oldSet.ID,
                    EmployeeID = y.newSet.EmployeeID,
                    KPI = y.newSet.KPI,
                    Target = y.newSet.Target,
                    Actual = y.newSet.Actual,
                    Rate = y.newSet.Rate,
                    Period = y.newSet.Period,
                    Formula = y.newSet.Formula,
                    ModifiedBy = y.newSet.ModifiedBy,
                    ModifiedDate = DateTime.Now,

                })
                .ToList();
            }

            List<Data.KPIScore.KPIScorePerEmployee> localCopy = (await _dbAccess.GetPerEmployeeByPeriods(param.Select(x => x.PeriodDateConverted).Distinct().ToList())).ToList();
            List<Data.KPIScore.KPIScorePerEmployee> ValueToAdd = GetToAdd(
                param.Select(x => new Data.KPIScore.KPIScorePerEmployee
                {
                    EmployeeID = x.EmployeeID,
                    KPI = x.KPIID,
                    Target = Math.Round(Convert.ToDecimal(x.Target), 5),
                    Actual = Math.Round(Convert.ToDecimal(x.Actual), 5),
                    Rate = Math.Round(Convert.ToDecimal(x.Rate), 5),
                    ModifiedBy = credentials.UserID,
                    Period = x.PeriodDateConverted,
                }).ToList()
                , localCopy)
            .ToList();

            List<Data.KPIScore.KPIScorePerEmployee> ValueToUpdate = GetToUpdate(localCopy,
                param.Select(x => new Data.KPIScore.KPIScorePerEmployee
                {
                    EmployeeID = x.EmployeeID,
                    KPI = x.KPIID,
                    Target = Math.Round(Convert.ToDecimal(x.Target), 5),
                    Actual = Math.Round(Convert.ToDecimal(x.Actual), 5),
                    Rate = Math.Round(Convert.ToDecimal(x.Rate), 5),
                    ModifiedBy = credentials.UserID,
                    Period = x.PeriodDateConverted,
                }).ToList())
            .ToList();

            return (ValueToAdd, ValueToUpdate);
        }

        public async Task<IActionResult> UploadScores(APICredentials credentials, List<UploadScoresFile> param)
        {
            List<Data.KPIScore.KPIScore> ValueToAdd = new List<Data.KPIScore.KPIScore>();
            List<Data.KPIScore.KPIScore> ValueToUpdate = new List<Data.KPIScore.KPIScore>();
            
            List<Data.KPIScore.KPIScorePerEmployee> ValueToAddPerEmployee = new List<Data.KPIScore.KPIScorePerEmployee>();
            List<Data.KPIScore.KPIScorePerEmployee> ValueToUpdatePerEmployee = new List<Data.KPIScore.KPIScorePerEmployee>();

            // Used for reference
            var kpiList = await _kpiDBAccess.GetAll();
            var orgGroupList = await _orgGroupDBAccess.GetAll();
            var employeeList = await _employeeDBAccess.GetAll();

            // Get all Org Group IDs from upload
            var uploadOrgGroups = param.Select(x => x.OrgGroupCode).Distinct().ToList();
            var uploadEmployees = param.Select(x => x.EmployeeCode).Distinct().ToList();
            var OrgGroupIDs = orgGroupList.Where(x => uploadOrgGroups.Contains(x.Code)).ToList();
            var EmployeeIDs = employeeList.Where(x => uploadEmployees.Contains(x.Code)).ToList();

            // Get all KPI IDs from upload
            var uploadKPIs = param.Select(x => x.KPICode).Distinct().ToList();
            var KPIIDs = kpiList.Where(x => uploadKPIs.Contains(x.Code)).ToList();

            // Excel Validation moved to ValidateUploadScores

            var tempParam = param.ToList();

            param = param.Select(x => {
                DateTime.TryParseExact(x.Period, "M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);
                x.PeriodDateConverted = convertedDate;
                return x;
            }).ToList();

            /*Remove Duplicates*/
            foreach (var obj in tempParam.ToList())
            {
                if (obj.KPIType.Equals("QUANTITATIVE"))
                {
                    /* Remove duplicates within file */
                    var duplicateWithinFile = param.Where(x =>
                    obj.KPICode.Equals(x.KPICode) &
                    obj.OrgGroupCode.Equals(x.OrgGroupCode) &&
                    obj.PeriodDateConverted == x.PeriodDateConverted &&
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                    }
                }
                else /*QUALITATIVE*/
                {
                    /* Remove duplicates within file */
                    var duplicateWithinFile = param.Where(x =>
                    obj.KPICode.Equals(x.KPICode) &
                    obj.EmployeeCode.Equals(x.EmployeeCode) &&
                    obj.PeriodDateConverted == x.PeriodDateConverted &&
                    obj.RowNum != x.RowNum).FirstOrDefault();

                    if (duplicateWithinFile != null)
                    {
                        param.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                    }
                }
            }

            if (param != null)
            {
                // Truncate table
                //await _dbAccess.Truncate();

                if (param.FirstOrDefault().KPIType.Equals("QUANTITATIVE"))
                {
                    // Upload joined with KPI & Org. Group
                    param = param.Join(OrgGroupIDs,
                                        upload => upload.OrgGroupCode,
                                        orgGroup => orgGroup.Code,
                                        (upload, orgGroup) => new { upload, orgGroup }
                                      )
                                 .Join(KPIIDs,
                                        uploadOrgGroup => uploadOrgGroup.upload.KPICode,
                                        kpi => kpi.Code,
                                        (uploadOrgGroup, kpi) => new { uploadOrgGroup, kpi }
                                      ).Select(x => new UploadScoresFile
                                      {
                                          //OrgGroup = x.uploadOrgGroup.orgGroup.SyncID,
                                          //KPI = x.kpi.ID,
                                          KPICode = x.uploadOrgGroup.upload.KPICode,
                                          KPIID = x.kpi.ID,
                                          OrgGroupCode = x.uploadOrgGroup.upload.OrgGroupCode,
                                          OrgGroupID = x.uploadOrgGroup.orgGroup.SyncID,
                                          Target = x.uploadOrgGroup.upload.Target,
                                          Actual = x.uploadOrgGroup.upload.Actual,
                                          Rate = x.uploadOrgGroup.upload.Rate,
                                          Period = x.uploadOrgGroup.upload.Period,
                                          PeriodDateConverted = x.uploadOrgGroup.upload.PeriodDateConverted,
                                          KPIType = x.uploadOrgGroup.upload.KPIType,
                                      })
                                  .OrderBy(x => x.PeriodDateConverted)
                                  .ThenBy(x => x.OrgGroupCode)
                                  .ThenBy(x => x.KPICode).ToList();
                }
                else
                {
                    // Upload joined with KPI & Employee
                    param = param.Join(EmployeeIDs,
                                        upload => upload.EmployeeCode,
                                        employee => employee.Code,
                                        (upload, employee) => new { upload, employee }
                                      )
                                 .Join(KPIIDs,
                                        uploadEmployee => uploadEmployee.upload.KPICode,
                                        kpi => kpi.Code,
                                        (uploadEmployee, kpi) => new { uploadEmployee, kpi }
                                      ).Select(x => new UploadScoresFile
                                      {
                                          KPICode = x.uploadEmployee.upload.KPICode,
                                          KPIID = x.kpi.ID,
                                          EmployeeCode = x.uploadEmployee.upload.EmployeeCode,
                                          EmployeeID = x.uploadEmployee.employee.SyncID,
                                          Target = x.uploadEmployee.upload.Target,
                                          Actual = x.uploadEmployee.upload.Actual,
                                          Rate = x.uploadEmployee.upload.Rate,
                                          Period = x.uploadEmployee.upload.Period,
                                          PeriodDateConverted = x.uploadEmployee.upload.PeriodDateConverted,
                                          KPIType = x.uploadEmployee.upload.KPIType,
                                      })
                                  .OrderBy(x => x.PeriodDateConverted)
                                  .ThenBy(x => x.EmployeeCode)
                                  .ThenBy(x => x.KPICode).ToList();
                }

                if (param.FirstOrDefault().KPIType.Equals("QUANTITATIVE"))
                {
                    (ValueToAdd, ValueToUpdate) = await GetToAddUpdate(credentials, param);
                    await _dbAccess.UploadScoresInsert(ValueToAdd, ValueToUpdate);
                }
                else /*QUALITATIVE*/
                {
                    (ValueToAddPerEmployee, ValueToUpdatePerEmployee) = await GetToAddUpdatePerEmployee(credentials, param);
                    await _dbAccess.UploadScoresPerEmployeeInsert(ValueToAddPerEmployee, ValueToUpdatePerEmployee);
                }
            }

            if (param.FirstOrDefault().KPIType.Equals("QUANTITATIVE"))
            {
                return new OkObjectResult(
                  new UploadScoresOutput
                  {
                      Message = ValueToAdd?.Count() > 0 ?
                      (
                          string.Concat(ValueToAdd?.Count, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED,
                          ValueToUpdate.Count() > 0 ?
                          string.Concat("<br>", ValueToUpdate?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPDATE) : "")
                      ) : ValueToUpdate.Count() > 0 ?
                          string.Concat(ValueToUpdate?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPDATE) : string.Concat("0 ", MessageUtilities.PRE_SCSSMSG_REC_ADDED)
                  });
            }
            else /*QUALITATIVE*/
            {
                return new OkObjectResult(
                      new UploadScoresOutput
                      {
                          Message = ValueToAddPerEmployee?.Count() > 0 ?
                          (
                              string.Concat(ValueToAddPerEmployee?.Count, " ", MessageUtilities.PRE_SCSSMSG_REC_ADDED,
                              ValueToUpdatePerEmployee.Count() > 0 ?
                              string.Concat("<br>", ValueToUpdatePerEmployee?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPDATE) : "")
                          ) : ValueToUpdatePerEmployee.Count() > 0 ?
                              string.Concat(ValueToUpdatePerEmployee?.Count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPDATE) : string.Concat("0 ", MessageUtilities.PRE_SCSSMSG_REC_ADDED)
                      });
            }
        }

        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            IEnumerable<TableVarKPIScoreGetByID> result = await _dbAccess.GetByID(input.ID, input.KPIType);
            List<KPIScoreForm> KPIScoreForm = new List<KPIScoreForm>();

            foreach (var e in result)
            {
                KPIScoreForm.Add(new KPIScoreForm
                {
                    ID = e.ID,
                    KRAGroup = e.KRAGroup,
                    KPICode = e.KPICode,
                    KPIName = e.KPIName,
                    KPIDescription = e.KPIDescription,
                    Target = Math.Round(e.Target, 2),
                    Actual = Math.Round(e.Actual, 2),
                    Rate = Math.Round(e.Rate, 2)
                });
            }

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = input.ID,
                    OrgGroup = result.FirstOrDefault().OrgGroup,
                    EmployeeID = result.FirstOrDefault().EmployeeID,
                    KPIScoreList = KPIScoreForm.OrderBy(x => x.ID).ToList(),
                });
        }
        
        //public async Task<IActionResult> Post(APICredentials credentials, Form param)
        //{
        //    IEnumerable<TableVarKPIScoreGetByID> result = await _dbAccess.GetByID(param.OrgGroup);

        //    if (result.Count() > 0)
        //        ErrorMessages.Add("Score(s) for this Position already exists.");

        //    if (ErrorMessages.Count == 0)
        //    {
        //        List<Data.KPIScore.KPIScore> scores = new List<Data.KPIScore.KPIScore>();

        //        foreach (var e in param.KPIScoreList)
        //        {
        //            Data.KPIScore.KPIScore score = new Data.KPIScore.KPIScore
        //            {
        //                OrgGroup = param.OrgGroup,
        //                KPI = e.KPIID,
        //                Target = e.Target,
        //                Actual = e.Actual,
        //                Rate = e.Rate
        //            };

        //            scores.Add(score);
        //        }

        //        await _dbAccess.Post(scores);
        //        _resultView.IsSuccess = true;
        //    }

        //    if (_resultView.IsSuccess)
        //        return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        //    else
        //        return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        //}

        //public async Task<IActionResult> Put(APICredentials credentials, Form param)
        //{

        //    if (ErrorMessages.Count == 0)
        //    {
        //        List<TableVarKPIScoreGetByID> OldKPIScore = (await _dbAccess.GetByID(param.OrgGroup)).ToList();

        //        List<Data.KPIScore.KPIScore> KPIScoreToUpdate = GetKPIScoreToUpdate(OldKPIScore,
        //            param.KPIScoreList == null ? new List<Data.KPIScore.KPIScore>() :
        //            param.KPIScoreList.Select(x => new Data.KPIScore.KPIScore
        //            {
        //                ID = x.ID,
        //                Target = x.Target,
        //                Actual = x.Actual,
        //                Rate = x.Rate,
        //                ModifiedBy = credentials.UserID,
        //                ModifiedDate = DateTime.Now
        //            }).ToList()).ToList();

        //        await _dbAccess.Put(KPIScoreToUpdate);
        //        _resultView.IsSuccess = true;
        //    }

        //    if (_resultView.IsSuccess)
        //        return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
        //    else
        //        return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        //}

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAll()).ToList());
        }

        private IEnumerable<Data.KPIScore.KPIScore> GetKPIScoreToUpdate(List<TableVarKPIScoreGetByID> left, List<Data.KPIScore.KPIScore> right)
        {
            return left.Join(
                right,
                     x => new { x.ID },
                     y => new { y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Target.Equals(x.newSet.Target) ||
                            !x.oldSet.Actual.Equals(x.newSet.Actual) ||
                            !x.oldSet.Rate.Equals(x.newSet.Rate))
                .Select(y =>
                    new Data.KPIScore.KPIScore
                    {
                        ID = y.oldSet.ID,
                        OrgGroup = y.oldSet.OrgGroup,
                        KPI = y.oldSet.KPIID,
                        Target = y.newSet.Target,
                        Actual = y.newSet.Actual,
                        Rate = y.newSet.Rate,
                        ModifiedBy = y.newSet.ModifiedBy,
                        ModifiedDate = y.newSet.ModifiedDate
                    }).ToList();
        }
    }
}