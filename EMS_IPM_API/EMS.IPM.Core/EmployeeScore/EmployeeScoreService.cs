using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.EmployeeScore;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer;
using EMS.IPM.Transfer.EmployeeScore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.IPM.Core.EmployeeScore
{
    public interface IEmployeeScoreService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetKeyInApprovalList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, int ID);

        Task<IActionResult> BulkDelete(APICredentials credentials, BulkDeleteForm param);

        //added
        Task<IActionResult> BulkVoid(APICredentials credentials, BulkVoidForm param);

        Task<IActionResult> BulkApproved(APICredentials credentials, BulkApprovedForm param);

        Task<IActionResult> GetScores(APICredentials credentials, RunScoreForm param);

        Task<IActionResult> GetAll(APICredentials credentials);

        Task<IActionResult> GetEmployeeScoreDashboard(APICredentials credentials, GetEmployeeScoreDashboardInput input);

        //Task<IActionResult> EmployeeScoreApproval(APICredentials credentials, EmployeeScoreApprovalResponse param);

        Task<IActionResult> RunTransScores(APICredentials credentials, RunScoreForm param);

        Task<IActionResult> GetTransProgress(APICredentials credentials);

        Task<IActionResult> BatchUpdateEmployeeScore(APICredentials credentials, BatchEmployeeScoreForm param);

        //added
        Task<IActionResult> BatchUpdateEmployeesScore(APICredentials credentials, BatchEmployeesScoreForm param);

        Task<IActionResult> GetTransEmployeeScoreSummary(APICredentials credentials, int TransSummaryID);

        Task<IActionResult> GetSummaryAutoComplete(APICredentials credentials, GetSummaryAutoCompleteInput param);

        Task<IActionResult> UpdateRunDescription(APICredentials credentials, UpdateRunDescriptionInput param);

        Task<IActionResult> EmployeeKPIScoreGetList(APICredentials credentials, EmployeeKPIScoreGetListInput input);

        Task<IActionResult> RerunTransScores(APICredentials credentials, RerunForm param);

        Task<IActionResult> RunTransScoreInitialize(APICredentials credentials, RunScoreForm param);

        Task<IActionResult> CreateTransScoreSummary(APICredentials credentials, RunScoreForm param);

        Task<IActionResult> RunTransScoreFinalize(APICredentials credentials, RunScoreForm param);
        Task<IActionResult> GetIPMRaterByTransID(APICredentials credentials, int ID);
        Task<IActionResult> GetIPMFinalScore(APICredentials credentials, int RunID);
        Task<IActionResult> GetFinalScoreList(APICredentials credentials, GetFinalScoreListInput input);
        Task<IActionResult> GetRunIDDropDown(APICredentials credentials);
    }

    public class EmployeeScoreService : Core.Shared.Utilities, IEmployeeScoreService
    {
        private readonly IEmployeeScoreDBAccess _dbAccess;

        public EmployeeScoreService(IPMContext dbContext, IConfiguration iconfiguration,
            IEmployeeScoreDBAccess dbAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeScoreOnly> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                TransSummaryID = x.TransSummaryID,
                Description = x.Description,
                IsActive = x.IsActive,
                ID = x.ID,
                Employee = x.Employee,
                ParentOrgGroup = x.ParentOrgGroup,
                OrgGroup = x.OrgGroup,
                Position = x.Position,
                Score = x.Score,
                TDateFrom = x.TDateFrom,
                TDateTo = x.TDateTo,
                Status = x.Status,
                PDateFrom = x.PDateFrom,
                PDateTo = x.PDateTo,
                DateEffectiveFrom = x.DateEffectiveFrom,
                DateEffectiveTo = x.DateEffectiveTo,
                QualiPlan = x.QualiPlan,
                QualiActual = x.QualiActual,
                QualiBranchPerformance = x.QualiBranchPerformance,
                QualiProRatePerformance = x.QualiProRatePerformance,
                QualiFinal = x.QualiFinal,
                QualiRemarks = x.QualiRemarks,
                IPMMonths = x.IPMMonths
            }).ToList());
        }


        public async Task<IActionResult> GetKeyInApprovalList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeScore> result = await _dbAccess.GetKeyInApprovalList(credentials, input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput  
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                TransSummaryID = x.TransSummaryID,
                Description = x.Description,
                ID = x.ID,
                Employee = x.Employee,
                ParentOrgGroup = x.ParentOrgGroup,
                OrgGroup = x.OrgGroup,
                Position = x.Position,
                //Changes from 2 to 5
                Score = Math.Round(x.Score, 5),
                TDateFrom = x.TDateFrom,
                TDateTo = x.TDateTo,
                Status = x.Status,
                PDateFrom = x.PDateFrom,
                PDateTo = x.PDateTo
            }).ToList());
        }


        public async Task<IActionResult> GetByID(APICredentials credentials, GetByIDInput input)
        {
            IEnumerable<TableVarEmployeeScoreGetByID> result = (await _dbAccess.GetByID(input, credentials.UserID)).ToList();
            List<EmployeeScoreForm> EmployeeScoreForm = new List<EmployeeScoreForm>();

            //List<Position> positionList = (await _positionDBAccess.GetAll()).ToList();
            //var a = positionList.Where(x => x.ParentPositionID == )

            foreach (var e in result)
            {
                EmployeeScoreForm.Add(new EmployeeScoreForm
                {
                    ID = e.ID,
                    KRAGroup = e.KRAGroup,
                    KPICode = e.KPICode,
                    KPIName = e.KPIName,
                    KPIDescription = e.KPIDescription,
                    KPIGuidelines = e.KPIGuidelines,
                    //Changes from 2 to 5
                    Weight = Math.Round(e.Weight, 5),
                    Target = Math.Round(e.Target ?? 0, 5),
                    Actual = Math.Round(e.Actual ?? 0, 5),
                    Rate = Math.Round(e.Rate ?? 0, 5),
                    IsEditable = e.IsEditable,
                    Requestor = e.Requestor,
                    Grade = e.Grade,
                    SourceType = e.SourceType,
                });
            }

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    TID = input.ID,
                    TransSummaryID = result.First().TransSummaryID,
                    Employee = result.First().Employee,
                    EmployeeCode = result.First().EmployeeCode,
                    Position = result.First().Position,
                    OrgGroup = result.First().OrgGroup,
                    TDateFrom = result.First().TDateFrom,
                    TDateTo = result.First().TDateTo,
                    Status = result.First().Status,
                    Requestor = result.First().Requestor,
                    EmployeeScoreList = EmployeeScoreForm.OrderBy(x => x.ID).ToList(),
                    HasEditAccess = result.First().HasEditAccess
                });
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {
            if (ErrorMessages.Count == 0)
            {
                Data.EmployeeScore.TransEmployeeScore EmployeeScoreToUpdate = await _dbAccess.GetEmployeeScoreByID(param.TID);
                List<Data.EmployeeScore.TransEmployeeScoreDetails> OldEmployeeScoreDetails = (await _dbAccess.GetEmployeeScoreDetailsByID(param.TID)).ToList();

                EmployeeScoreToUpdate.Status = param.Status;
                if (!param.isAdmin)
                    EmployeeScoreToUpdate.ApproverRoleIDs = param.NextApproverRoleIDs;

                if (!param.isApproval)
                {
                    EmployeeScoreToUpdate.RequestorID = credentials.UserID;
                    EmployeeScoreToUpdate.Score = param.TotalScore;
                }
                else
                {
                    if (param.Status.Equals(Enums.EmployeeScore_ApproverStatus.FOR_REVISION.ToString()))
                    {
                        //string approvers = EmployeeScoreToUpdate.ApproverIDs;
                        //approvers = string.Concat("|", approvers ?? "", "|");
                        //approvers = approvers.Replace("|" + credentials.UserID.ToString() + ",", "");
                        //approvers = approvers.Replace("," + credentials.UserID.ToString() + "|", "");
                        //approvers = approvers.Replace("," + credentials.UserID.ToString() + ",", ",");
                        //approvers = approvers.Replace("|", "");
                        EmployeeScoreToUpdate.ApproverIDs = "";
                    }
                    else
                    {
                        EmployeeScoreToUpdate.ApproverIDs = string.IsNullOrEmpty(EmployeeScoreToUpdate.ApproverIDs) ? credentials.UserID.ToString() :
                          string.Concat(EmployeeScoreToUpdate.ApproverIDs, ",", credentials.UserID.ToString());
                    }
                }

                List<Data.EmployeeScore.TransEmployeeScoreDetails> EmployeeScoreDetailsToUpdate = GetEmployeeScoreDetailsToUpdate(OldEmployeeScoreDetails.Where(x => x.IsEditable).ToList(),
                    param.EmployeeScoreList == null ? new List<Data.EmployeeScore.TransEmployeeScoreDetails>() :
                    param.EmployeeScoreList.Select(x => new Data.EmployeeScore.TransEmployeeScoreDetails
                    {
                        ID = x.ID,
                        KPIWeight = x.Weight,
                        KPITarget = x.Target,
                        KPIActual = x.Actual,
                        KPIScore = x.Rate,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                await _dbAccess.UpdateEmployeeScore(EmployeeScoreToUpdate, EmployeeScoreDetailsToUpdate);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, int ID)
        {
            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Delete(ID);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> BulkDelete(APICredentials credentials, BulkDeleteForm param)
        {
            if (param.IDs.Count() == 0)
                ErrorMessages.Add(MessageUtilities.ERRMSG_NO_RECORDS);

            if (ErrorMessages.Count == 0)
            {
                IEnumerable<Data.EmployeeScore.TransEmployeeScore> empScoreList = await _dbAccess.GetEmployeeScoreListByID(param.IDs);

                await _dbAccess.BulkDeleteEmployeeScore(
                        empScoreList.Select(x =>
                        {
                            x.isActive = false;
                            return x;
                        }).ToList()
                    );

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.PRE_SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> BulkVoid(APICredentials credentials, BulkVoidForm param)
        {
            if (param.IDs.Count() == 0)
                ErrorMessages.Add(MessageUtilities.ERRMSG_NO_RECORDS);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.BulkVoidEmployeeScore(param);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.PRE_SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> BulkApproved(APICredentials credentials, BulkApprovedForm param)
        {
            if (param.IDs.Count() == 0)
                ErrorMessages.Add(MessageUtilities.ERRMSG_NO_RECORDS);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.BulkApprovedEmployeeScore(param);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.PRE_SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }



        public async Task<IActionResult> GetScores(APICredentials credentials, RunScoreForm param)
        {
            var runScores = (await _dbAccess.RunScores(param)).ToList();

            var scores = runScores.Select(x => new ScoresOutput
            {
                TDateFrom = x.TDateFrom,
                TDateTo = x.TDateTo,
                Employee = x.Employee,
                OrgGroup = x.OrgGroup,
                KPI = x.KPI,
                Position = x.Position,
                KPIWeight = x.KPIWeight,
                KPIScore = x.KPIScore,
                PDateFrom = x.PDateFrom,
                PDateTo = x.PDateTo,
            }).OrderBy(x => x.Employee).ToList();

            return new OkObjectResult(scores);

        }

        //public async Task<IActionResult> RunScores(APICredentials credentials, RunScoreForm param)
        //{
        //    var uploadEmployees = (await _dbAccess.RunScores(param)).ToList();
        //    var lstEmployeeScores = (await _dbAccess.GetAll()).ToList();
        //    var lstPosition = (await _positionDBAccess.GetAll()).ToList();

        //    if (ErrorMessages.Count == 0)
        //    {
        //        var uniqOrgIDs = uploadEmployees.Select(x => x.OrgGroup).Distinct().ToList();
        //        var delimitedKPIs = String.Join(",", uploadEmployees.OrderBy(x => x.KPI).Select(x => x.KPI).Distinct().ToList());

        //        var averageScores = new List<TableVarAverageScore>();

        //        foreach (var e in uniqOrgIDs)
        //        {
        //            var avg = (await _dbAccess.GetAverageScore(e, delimitedKPIs)).ToList();

        //            if (avg.Count() > 0)
        //                averageScores.AddRange(avg);
        //        }

        //        var employeeScores = uploadEmployees
        //                            .Select(x => new Data.EmployeeScore.EmployeeScore
        //                            {
        //                                TDateFrom = x.TDateFrom,
        //                                TDateTo = x.TDateTo,
        //                                Employee = x.Employee,
        //                                OrgGroup = x.OrgGroup,
        //                                KPI = x.KPI,
        //                                Position = x.Position,
        //                                KPIWeight = x.KPIWeight,
        //                                KPIScore = averageScores.FirstOrDefault(y => y.OrgGroup == x.OrgGroup && y.KPI == x.KPI) == null ? x.KPIScore :
        //                                           averageScores.FirstOrDefault(y => y.OrgGroup == x.OrgGroup && y.KPI == x.KPI).Average

        //                            }).OrderBy(x => x.OrgGroup).ToList();



        //        List<Data.EmployeeScore.EmployeeScore> EmployeeScoreToAdd = GetEmployeeScoreToAdd(lstEmployeeScores,
        //            employeeScores == null ? new List<Data.EmployeeScore.EmployeeScore>() :
        //            employeeScores.Select(x => new Data.EmployeeScore.EmployeeScore
        //            {
        //                TDateFrom = x.TDateFrom,
        //                TDateTo = x.TDateTo,
        //                Employee = x.Employee,
        //                OrgGroup = x.OrgGroup,
        //                KPI = x.KPI,
        //                Position = x.Position,
        //                KPIWeight = x.KPIWeight,
        //                KPIScore = x.KPIScore,
        //            }).ToList()).ToList();

        //        List<Data.EmployeeScore.EmployeeScore> EmployeeScoreToUpdate = GetRunEmployeeScoreToUpdate(lstEmployeeScores,
        //            employeeScores == null ? new List<Data.EmployeeScore.EmployeeScore>() :
        //            employeeScores.Select(x => new Data.EmployeeScore.EmployeeScore
        //            {
        //                ID = x.ID,
        //                TDateFrom = x.TDateFrom,
        //                TDateTo = x.TDateTo,
        //                Employee = x.Employee,
        //                OrgGroup = x.OrgGroup,
        //                KPI = x.KPI,
        //                Position = x.Position,
        //                KPIWeight = x.KPIWeight,
        //                KPIScore = x.KPIScore,
        //            }).ToList()).ToList();

        //        //List<Data.EmployeeScore.EmployeeScore> EmployeeScoreToDelete = GetEmployeeScoreToDelete(lstEmployeeScores,
        //        //    employeeScores == null ? new List<Data.EmployeeScore.EmployeeScore>() :
        //        //    employeeScores.Select(x => new Data.EmployeeScore.EmployeeScore
        //        //    {
        //        //        TDateFrom = x.TDateFrom,
        //        //        TDateTo = x.TDateTo,
        //        //        Employee = x.Employee,
        //        //        OrgGroup = x.OrgGroup,
        //        //        Position = x.Position,
        //        //        KPI = x.KPI,
        //        //    }).ToList()).ToList();

        //        await _dbAccess.Put(EmployeeScoreToAdd, EmployeeScoreToUpdate, null);
        //        _resultView.IsSuccess = true;
        //    }

        //    if (_resultView.IsSuccess)
        //        return new OkObjectResult(MessageUtilities.PRE_SCSSMSG_REC_ADDED);
        //    else
        //        return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        //}

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAll()).ToList());
        }

        private IEnumerable<Data.EmployeeScore.EmployeeScore> GetEmployeeScoreToAdd(List<Data.EmployeeScore.EmployeeScore> left, List<Data.EmployeeScore.EmployeeScore> right)
        {
            return right.GroupJoin(
                left,
                     x => new { x.TDateFrom, x.TDateTo, x.PDateFrom, x.PDateTo, x.Employee, x.KPI, x.Position, x.OrgGroup },
                     y => new { y.TDateFrom, y.TDateTo, y.PDateFrom, y.PDateTo, y.Employee, y.KPI, y.Position, y.OrgGroup },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.EmployeeScore.EmployeeScore
                    {
                        TID = x.newSet.newSet.TID,
                        TDateFrom = x.newSet.newSet.TDateFrom,
                        TDateTo = x.newSet.newSet.TDateTo,
                        Employee = x.newSet.newSet.Employee,
                        OrgGroup = x.newSet.newSet.OrgGroup,
                        KPI = x.newSet.newSet.KPI,
                        Position = x.newSet.newSet.Position,
                        KPIWeight = x.newSet.newSet.KPIWeight,
                        KPIScore = x.newSet.newSet.KPIScore,
                        Status = x.newSet.newSet.Status,
                        PDateFrom = x.newSet.newSet.PDateFrom,
                        PDateTo = x.newSet.newSet.PDateTo,
                        IsEditable = x.newSet.newSet.IsEditable,
                    }).ToList();
        }

        private IEnumerable<Data.EmployeeScore.TransEmployeeScoreDetails> GetEmployeeScoreDetailsToUpdate(List<Data.EmployeeScore.TransEmployeeScoreDetails> left, List<Data.EmployeeScore.TransEmployeeScoreDetails> right)
        {
            return left.Join(
                right,
                     x => new { x.ID },
                     y => new { y.ID },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.KPIScore != x.newSet.KPIScore
                         || x.oldSet.KPIWeight != x.newSet.KPIWeight
                         || x.oldSet.KPITarget != x.newSet.KPITarget
                         || x.oldSet.KPIActual != x.newSet.KPIActual
                         //|| x.oldSet.ModifiedBy != x.newSet.ModifiedBy
                         //|| x.oldSet.ModifiedDate != x.newSet.ModifiedDate
                         )
                .Select(y =>
                    new Data.EmployeeScore.TransEmployeeScoreDetails
                    {
                        ID = y.oldSet.ID,
                        TransID = y.oldSet.TransID,
                        KPIID = y.oldSet.KPIID,
                        KPIWeight = y.newSet.KPIWeight,
                        KPITarget = y.newSet.KPITarget,
                        KPIActual = y.newSet.KPIActual,
                        IsEditable = y.oldSet.IsEditable,
                        KPIScore = y.newSet.KPIScore,
                        ModifiedBy = y.newSet.ModifiedBy,
                        ModifiedDate = y.newSet.ModifiedDate
                    }).ToList();
        }

        private IEnumerable<Data.EmployeeScore.EmployeeScore> GetRunEmployeeScoreToUpdate(List<Data.EmployeeScore.EmployeeScore> left, List<Data.EmployeeScore.EmployeeScore> right)
        {
            return left.Join(
                right,
                     x => new { x.TDateFrom, x.TDateTo, x.PDateFrom, x.PDateTo, x.Employee, x.KPI, x.Position, x.OrgGroup },
                     y => new { y.TDateFrom, y.TDateTo, y.PDateFrom, y.PDateTo, y.Employee, y.KPI, y.Position, y.OrgGroup },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.oldSet.KPIScore != x.newSet.KPIScore
                         || x.oldSet.KPIWeight != x.newSet.KPIWeight)
                .Select(y =>
                    new Data.EmployeeScore.EmployeeScore
                    {
                        ID = y.oldSet.ID,
                        TID = y.oldSet.TID,
                        TDateFrom = y.newSet.TDateFrom,
                        TDateTo = y.newSet.TDateTo,
                        Employee = y.oldSet.Employee,
                        OrgGroup = y.oldSet.OrgGroup,
                        KPI = y.oldSet.KPI,
                        Position = y.oldSet.Position,
                        KPIWeight = y.oldSet.KPIWeight,
                        KPIScore = y.newSet.KPIScore,
                        Status = y.newSet.Status,
                        PDateFrom = y.newSet.PDateFrom,
                        PDateTo = y.newSet.PDateTo,
                        IsEditable = y.oldSet.IsEditable,
                    }).ToList();
        }

        private IEnumerable<Data.EmployeeScore.EmployeeScore> GetEmployeeScoreToDelete(List<Data.EmployeeScore.EmployeeScore> left, List<Data.EmployeeScore.EmployeeScore> right)
        {
            return left.GroupJoin(
                right,
                     x => new { x.TDateFrom, x.TDateTo, x.Employee, x.KPI, x.Position, x.OrgGroup },
                     y => new { y.TDateFrom, y.TDateTo, y.Employee, y.KPI, y.Position, y.OrgGroup },
                (x, y) => new { oldSet = x, newSet = y })
                .SelectMany(x => x.newSet.DefaultIfEmpty(),
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.newSet == null)
                .Select(x =>
                    new Data.EmployeeScore.EmployeeScore
                    {
                        ID = x.oldSet.oldSet.ID,
                        TDateFrom = x.oldSet.oldSet.TDateFrom,
                        TDateTo = x.oldSet.oldSet.TDateTo,
                        Employee = x.oldSet.oldSet.Employee,
                        OrgGroup = x.oldSet.oldSet.OrgGroup,
                        Position = x.oldSet.oldSet.Position,
                    }).ToList();
        }

        public async Task<IActionResult> GetEmployeeScoreDashboard(APICredentials credentials, GetEmployeeScoreDashboardInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            GetEmployeeScoreDashboardOutput output = new GetEmployeeScoreDashboardOutput();

            if (input.DashboardType == EMS.IPM.Transfer.Enums.DashboardType.POSITION.ToString())
            {
                IEnumerable<TableVarEmployeeScoreCountByPosition> report = await _dbAccess.GetEmployeeScoreCountByPosition(input, rowStart);

                output.DashboardType = EMS.IPM.Transfer.Enums.DashboardType.POSITION.ToString();
                output.Dashboard1Output =
                    report.Select(x => new GetEmployeeScoreCountByPositionOutput
                    {
                        Position = x.Position,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.DashboardType == EMS.IPM.Transfer.Enums.DashboardType.BRANCH_WITH_POSITION.ToString())
            {
                IEnumerable<TableVarEmployeeScoreCountByBranch> report = await _dbAccess.GetEmployeeScoreCountByBranch(input, rowStart);

                output.DashboardType = EMS.IPM.Transfer.Enums.DashboardType.BRANCH_WITH_POSITION.ToString();
                output.Dashboard2Output =
                    report.Select(x => new GetEmployeeScoreCountByAreaOutput
                    {
                        Branch = x.Branch,
                        Position = x.Position,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }
            else if (input.DashboardType == EMS.IPM.Transfer.Enums.DashboardType.REGION_WITH_POSITION.ToString())
            {
                IEnumerable<TableVarEmployeeScoreCountByRegion> report = await _dbAccess.GetEmployeeScoreCountByRegion(input, rowStart);

                output.DashboardType = EMS.IPM.Transfer.Enums.DashboardType.REGION_WITH_POSITION.ToString();
                output.Dashboard3Output =
                    report.Select(x => new GetEmployeeScoreCountByRegionOutput
                    {
                        Region = x.Region,
                        Position = x.Position,
                        Count = x.Count
                    }).ToList();
                output.TotalNoOfRecord = report.Count() == 0 ? 0 : report.First().TotalNum;
                output.NoOfPages = report.Count() == 0 ? 0 : (int)Math.Ceiling((float)report.First().TotalNum / (float)input.rows);
            }

            return new OkObjectResult(output);
        }

        //public async Task<IActionResult> EmployeeScoreApproval(APICredentials credentials, EmployeeScoreApprovalResponse param)
        //{
        //    string successMessage = param.Status == Enums.EmployeeScore_ApproverStatus.APPROVED ?
        //        MessageUtilities.SCSSMSG_REC_APPROVE :
        //        param.Status == Enums.EmployeeScore_ApproverStatus.FOR_APPROVAL ?
        //        MessageUtilities.SCSSMSG_REC_REJECT : "";

        //    await _dbAccess.EmployeeScoreApproval(credentials.UserID, param);
        //    return new OkObjectResult(successMessage);
        //}


        public async Task<IActionResult> RunTransScores(APICredentials credentials, RunScoreForm param)
        {

            List<TransEmployeeScore> results = new List<TransEmployeeScore>();

            results = (await _dbAccess.RunTransScores(param, credentials.UserID)).ToList();
            Console.WriteLine(string.Concat(string.Join(',', results.Select(x => x.EmployeeID).Distinct().ToList())
                , Environment.NewLine, param.strEmployeeIDList, Environment.NewLine, Environment.NewLine));

            if (results.Count > 0)
                return new OkObjectResult(
                    new TransOutput
                    {
                        TransSummaryID = results.First().TransSummaryID,
                        TransIDList = results.Select(x => x.ID).ToList(),
                        Message = MessageUtilities.PRE_SCSSMSG_REC_ADDED
                    }
                    );
            else
                return new OkObjectResult(
                    new TransOutput
                    {
                        TransSummaryID = 0,
                        TransIDList = new List<int>(),
                        Message = MessageUtilities.ERRMSG_KPI_RUN_NO_EMP
                    }
                    );

        }

        public async Task<IActionResult> GetByRefCodes(APICredentials credentials, List<string> RefCodes)
        {
            return new OkObjectResult(
                (await _dbAccess.GetRefCodes(RefCodes))
                .OrderBy(y => y.Description)
                .Select(x =>
                new Utilities.API.ReferenceMaintenance.ReferenceValue
                {
                    ID = x.ID,
                    RefCode = x.RefCode,
                    Value = x.Value,
                    Description = x.Description,
                    UserID = x.UserID
                }));
        }

        public async Task<IActionResult> GetTransProgress(APICredentials credentials)
        {
            List<TransEmployeeScoreSummary> result = new List<TransEmployeeScoreSummary>();
            List<int> result2 = new List<int>();
            int totalemploueeprocessed = 0;


            result = (await _dbAccess.GetTransEmployeeScoreProgress(credentials.UserID)).ToList();

            if (result.Count > 0)
            {
                if (result.First().EmployeesWithIPM > 0)
                {
                    result2 = (await _dbAccess.GetEmployeeOnStaging(credentials.UserID)).ToList();
                    totalemploueeprocessed = result2.Count();
                }
                else
                {
                    totalemploueeprocessed = 0;
                }
            }



            if (result == null)
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            }
            else
            {
                if (result.Count == 0)
                {
                    return new OkObjectResult(new GetTransProgressOutput
                    {
                        ProcessedEmployees = 0,
                        EmployeesWithIPM = 0,
                        CreatedBy = 0,
                        IsDone = true,
                        TransSummaryID = 0 

                    });
                }
                else
                {
                    return new OkObjectResult(new GetTransProgressOutput
                    {
                        ProcessedEmployees = totalemploueeprocessed,
                        EmployeesWithIPM = result.First().EmployeesWithIPM,
                        CreatedBy = result.First().CreatedBy,
                        IsDone = false ,
                        TransSummaryID = result.First().ID

                    });
                }

            }
        }

        public async Task<IActionResult> BatchUpdateEmployeeScore(APICredentials credentials, BatchEmployeeScoreForm param)
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

                IEnumerable<TransEmployeeScore> employeeScoreToUpdate = await _dbAccess.GetEmployeeScoreListByID(param.IDs);

                await _dbAccess.BatchUpdateEmployeeScore(
                    employeeScoreToUpdate.Select(x =>
                    {
                        x.Status = param.Status;
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

        //added
        public async Task<IActionResult> BatchUpdateEmployeesScore(APICredentials credentials, BatchEmployeesScoreForm param)
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

                IEnumerable<TransEmployeeScore> employeeScoreToUpdate = await _dbAccess.GetEmployeeScoreListByID(param.IDs);

                await _dbAccess.BatchUpdateEmployeesScore(
                    employeeScoreToUpdate.Select(x =>
                    {
                        x.Status = param.Status;
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

        public async Task<IActionResult> GetTransEmployeeScoreSummary(APICredentials credentials, int TransSummaryID)
        {
            List<TableVarTransEmployeeScoreSummary> result =
                (await _dbAccess.GetTransEmployeeScoreSummary(TransSummaryID)).ToList();
            
            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(new GetTransEmployeeScoreSummaryOutput
                {
                    ID = result.First().ID,
                    Description = result.First().Description,
                    FilterBy = result.First().FilterBy ?? "N/A",
                    FilterOrgGroup = result.First().FilterOrgGroup ?? "N/A",
                    FilterIncludeLevelBelow = result.First().FilterIncludeLevelBelow == true ? "Yes" : "No",
                    FilterPosition = result.First().FilterPosition ?? "N/A",
                    FilterEmployee = result.First().FilterEmployee ?? "N/A",
                    FilterOverride = result.First().FilterOverride == true ? "Yes" : "No",
                    FilterUseCurrent = result.First().FilterUseCurrent == true ? "Yes" : "No",
                    TDateFrom = result.First().TDateFrom,
                    TDateTo = result.First().TDateTo,
                    ProcessedEmployees = result.First().ProcessedEmployees,
                    TotalNumOfEmployees = result.First().TotalNumOfEmployees,
                    EmployeesWithIPM = result.First().EmployeesWithIPM,
                    RatingEEEmployees = result.First().RatingEEEmployees,
                    RatingMEEmployees = result.First().RatingMEEmployees,
                    RatingSBEEmployees = result.First().RatingSBEEmployees,
                    RatingBEEmployees = result.First().RatingBEEmployees,
                    RatingEEMin = result.First().RatingEEMin,
                    RatingEEMax = result.First().RatingEEMax,
                    RatingMEMin = result.First().RatingMEMin,
                    RatingMEMax = result.First().RatingMEMax,
                    RatingSBEMin = result.First().RatingSBEMin,
                    RatingSBEMax = result.First().RatingSBEMax,
                    RatingBEMin = result.First().RatingBEMin,
                    RatingBEMax = result.First().RatingBEMax,
                    IsDone = result.First().IsDone,
                    IsTransActive = result.First().IsTransActive,
                    CreatedDate = result.First().CreatedDate,
                    //TotalEmployeesWithIPM = result.First().TotalEmployeesWithIPM,
                    //EmployeesWithMultiple = result.First().EmployeesWithMultiple,
                    //TotalIPMResult = result.First().TotalIPMResult,
                    //RunStart = result.First().RunStart,
                    //RunEnd = result.First().RunEnd
                });
        }

        public async Task<IActionResult> GetSummaryAutoComplete(APICredentials credentials, GetSummaryAutoCompleteInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetSummaryAutoComplete(param))
                .Select(x => new Transfer.Shared.GetAutoCompleteOutput
                {
                    ID = x.ID,
                    Description = x.Description
                })
            );
        }

        public async Task<IActionResult> UpdateRunDescription(APICredentials credentials, UpdateRunDescriptionInput param)
        {

            var result = (await _dbAccess.GetTransEmployeeScoreSummaryByID(param.RunID));


            if (result != null)
            {
                if (result.Count() > 0)
                {
                    var toUpdate = result.First();
                    toUpdate.Description = param.Description;
                    toUpdate.IsTransActive = param.IsTransActive;
                    await _dbAccess.UpdateRunDescription(toUpdate);
                    _resultView.IsSuccess = true;
                }
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> EmployeeKPIScoreGetList(APICredentials credentials, EmployeeKPIScoreGetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeKPIScore> result = await _dbAccess.EmployeeKPIScoreGetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new EmployeeKPIScoreGetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                KPIID = x.KPIID,
                KRAGroup = x.KRAGroup,
                KPICode = x.KPICode,
                KPIName = x.KPIName,
                KPIDescription = x.KPIDescription,
                KPIGuidelines = x.KPIGuidelines,
                //Changes from 2 to 5
                Weight = Math.Round(x.Weight, 5),
                Target = Math.Round(x.Target, 5),
                Actual = Math.Round(x.Actual, 5),
                Rate = Math.Round(x.Rate, 5),
                Total = Math.Round(x.Total, 5),
                Grade = x.Grade,
                IsEditable = x.IsEditable,
                SourceType = x.SourceType,
            }).ToList());
        }

        public async Task<IActionResult> RerunTransScores(APICredentials credentials, RerunForm param)
        {

            List<TransEmployeeScoreSummary> results = new List<TransEmployeeScoreSummary>();

            results = (await _dbAccess.RerunTransScores(param, credentials.UserID)).ToList();

            if (results.Count > 0)
                return new OkObjectResult(
                    new ReRunParamOutput
                    {
                        Description = results.Select(x => x.Description).FirstOrDefault(),
                        Filter = results.Select(x => x.FilterBy).FirstOrDefault(),
                        Ids = results.Select(x => x.FilterBy.Equals("POS") ? x.FilterPosition : x.FilterOrgGroup).FirstOrDefault(),
                        Employees = results.Select(x => x.FilterEmployee).FirstOrDefault(), // not sure
                        DateFrom = param.DateFrom,
                        DateTo = param.DateTo,
                        UseCurrent = true,
                        RegularOnly = true,
                        IncludeLvlBelow = results.Select(x => x.filterIncludeLvlBelow).FirstOrDefault(),
                        Override = true,
                        TransSummaryID = results.Select(x => x.ID).FirstOrDefault(),
                        IncludeSecDesig = true,
                        RoleID = param.RoleIDs,
                        CreatedBy = results.Select(x => x.CreatedBy).FirstOrDefault(),

                    }
                    );
            else
                return new OkObjectResult(
                    new ReRunParamOutput
                    {
                        Description = results.Select(x => x.Description).FirstOrDefault(),
                        Filter = results.Select(x => x.FilterBy).FirstOrDefault(),
                        Ids = results.Select(x => x.Description).FirstOrDefault(), // not sure
                        Employees = results.Select(x => x.Description).FirstOrDefault(), // not sure
                        DateFrom = results.Select(x => x.TDateFrom?.ToString("MM.DD.YYYY")).FirstOrDefault(),
                        DateTo = results.Select(x => x.TDateTo?.ToString("MM.DD.YYYY")).FirstOrDefault(),
                        UseCurrent = true,
                        RegularOnly = true,
                        IncludeLvlBelow = true,
                        Override = true,
                        TransSummaryID = results.Select(x => x.ID).FirstOrDefault(),
                        IncludeSecDesig = true,
                        RoleID = null,
                        CreatedBy = results.Select(x => x.CreatedBy).FirstOrDefault(),
                    }
                    );
        }

        public async Task<IActionResult> RunTransScoreInitialize(APICredentials credentials, RunScoreForm param)
        {

            List<TransEmployeeScoreStagingTest> results = new List<TransEmployeeScoreStagingTest>();

            results = (await _dbAccess.RunTransScoreInitialize(param, credentials.UserID)).ToList();

            if (results.Count > 0)
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = results.Select(x => x.EmployeeID).ToList(),
                        TransSummaryID = results.Select(x => x.TransSummaryID).FirstOrDefault(),
                        Message = MessageUtilities.PRE_SCSSMSG_REC_ADDED
                    }
                    );
            else
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = new List<int>(),
                        TransSummaryID = 0,
                        Message = MessageUtilities.ERRMSG_KPI_RUN_NO_EMP
                    }
                    ); ;
        }

        public async Task<IActionResult> CreateTransScoreSummary(APICredentials credentials, RunScoreForm param)
        {

            List<TransEmployeeScoreStagingTest> results = new List<TransEmployeeScoreStagingTest>();

            results = (await _dbAccess.CreateTransScoreSummary(param, credentials.UserID)).ToList();

            if (results.Count > 0)
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = results.Select(x => x.EmployeeID).ToList(),
                        TransSummaryID = results.Select(x => x.TransSummaryID).FirstOrDefault(),
                        Message = MessageUtilities.PRE_SCSSMSG_REC_ADDED
                    }
                    );
            else
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = new List<int>(),
                        TransSummaryID = 0,
                        Message = MessageUtilities.ERRMSG_KPI_RUN_NO_EMP
                    }
                    ); ;
        }

        public async Task<IActionResult> RunTransScoreFinalize(APICredentials credentials, RunScoreForm param)
        {

            List<TransEmployeeScoreSummary> results = new List<TransEmployeeScoreSummary>();

            results = (await _dbAccess.RunTransScoreFinalize(param, credentials.UserID)).ToList();

            if (results.Count > 0)
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = new List<int>(),
                        TransSummaryID = param.TransSummaryID,
                        Message = MessageUtilities.PRE_SCSSMSG_REC_ADDED
                    }
                    );
            else
                return new OkObjectResult(
                    new TransOutputForTreading
                    {
                        EmployeeID = new List<int>(),
                        TransSummaryID = 0,
                        Message = MessageUtilities.ERRMSG_KPI_RUN_NO_EMP
                    }
                    ); ;
        }
        public async Task<IActionResult> GetIPMRaterByTransID(APICredentials credentials, int ID)
        {
            return new OkObjectResult((await _dbAccess.GetIPMRaterByTransID(ID)).ToList());
        }

        public async Task<IActionResult> GetIPMFinalScore(APICredentials credentials, int RunID)
        {
            var Result = (await _dbAccess.GetIPMFinalScore(RunID,credentials.UserID)).ToList();
            if(Result.Count > 0)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_PROCESS);
            else
                return new OkObjectResult(MessageUtilities.ERRMSG_REC_PROCESS);
        }
        public async Task<IActionResult> GetFinalScoreList(APICredentials credentials, GetFinalScoreListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeFinalScore> result = await _dbAccess.GetFinalScoreList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetFinalScoreListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),

                ID = x.ID,
                RunID = x.RunID,
                RunTitle = x.RunTitle,
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                IPMCount = x.IPMCount,
                IPMMonths = x.IPMMonths,
                FinalScore = x.FinalScore,
                FinalQuali = x.FinalQuali,
                FinalRemarks = x.FinalRemarks,
                IsOld = x.IsOld,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate
            }).ToList());
        }
        public async Task<IActionResult> GetRunIDDropDown(APICredentials credentials)
        {
            var result = await _dbAccess.GetRunIDDropDown();

            return new OkObjectResult(result.OrderByDescending(y=>y.ID).Select(x=> new Dropdown()
            { 
                Value = x.ID.ToString(),
                Text = String.Concat(x.ID," - ",x.Description)
            }).ToList());
        }
    }
}