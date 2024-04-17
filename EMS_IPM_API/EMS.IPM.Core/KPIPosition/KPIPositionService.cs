using EMS.IPM.Data.DataDuplication.Position;
using EMS.IPM.Data.DBContexts;
using EMS.IPM.Data.KPI;
using EMS.IPM.Data.KPIPosition;
using EMS.IPM.Data.Reference;
using EMS.IPM.Transfer.KPIPosition;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EMS.IPM.Core.KPIPosition
{
    public interface IKPIPositionService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetByPositionID(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> Put(APICredentials credentials, Form param);

        Task<IActionResult> Delete(APICredentials credentials, GetByIDInput input);

        Task<IActionResult> GetAll(APICredentials credentials);

        Task<IActionResult> GetAllDetails(APICredentials credentials);

        Task<bool> ValidateUploadInsert(APICredentials credentials, List<UploadFileEntity> param);

        Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFileEntity> param);

        Task<IActionResult> GetExportList(APICredentials credentials, GetListInput input);
        Task<IActionResult> CopyKpiPosition(APICredentials credentials, CopyKpiPositionInput param);
        Task<IActionResult> GetCopyPosition(APICredentials credentials, IPM.Transfer.Shared.GetAutoCompleteInput param);
    }

    public class KPIPositionService : Core.Shared.Utilities, IKPIPositionService
    {
        private readonly IKPIPositionDBAccess _dbAccess;
        private readonly IKPIDBAccess _kpiDBAccess;
        private readonly IPositionDBAccess _positionDBAccess;

        public KPIPositionService(IPMContext dbContext, IConfiguration iconfiguration,
            IKPIPositionDBAccess dbAccess, IKPIDBAccess kpiDBAccess, IPositionDBAccess positionAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _kpiDBAccess = kpiDBAccess;
            _positionDBAccess = positionAccess;
        }

        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarKPIPosition> result = await _dbAccess.GetList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Position = x.Position,
                Weight = Math.Round(x.Weight, 2).ToString(),
                EffectiveDate = x.EffectiveDate,
            }).ToList());
        }

        public async Task<IActionResult> GetByPositionID(APICredentials credentials, GetByIDInput input)
        {
            IEnumerable<TableVarKPIPositionDetails> result = await _dbAccess.GetDetailsByPositionID(input.ID, input.EffectiveDate);
            List<KPIPositionForm> kpiPositionForm = new List<KPIPositionForm>();

            // Get Position details for Autocomplete pre-load
            var position = (await _positionDBAccess.GetBySyncIDs(new List<int> { input.ID })).FirstOrDefault();
            string positionDetails = position != null ? position.Code + " - " + position.Title : String.Empty;

            foreach (var e in result)
            {
                kpiPositionForm.Add(new KPIPositionForm
                {
                    ID = e.ID,
                    KRAGroup = e.KRAGroup,
                    KRASubGroup = e.KRASubGroup,
                    KPICode = e.KPICode,
                    KPIName = e.KPIName,
                    KPIDescription = e.KPIDescription,
                    KPI = e.KPI,
                    KPIID = e.KPIID,
                    Weight = e.Weight,
                    WeightNoServiceBay = e.WeightNoServiceBay
                });
            }

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    Position = positionDetails,
                    PositionID = input.ID,
                    EffectiveDate = input.EffectiveDate,
                    KPIPositionList = kpiPositionForm.OrderBy(x => x.KPICode).ToList()
                });
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {

            if (param.PositionID == 0)
                ErrorMessages.Add("Position " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (param.KPIPositionList.Count() == 0)
                ErrorMessages.Add("KPI for Position " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                // Parse Form to List of KPI Positions
                List<Data.KPIPosition.KPIPosition> lstKPIPosition = new List<Data.KPIPosition.KPIPosition>();

                foreach (var e in param.KPIPositionList)
                {
                    // Use KPI Position List (contains KPI & Weight)
                    // to generate KPI Position data (Position ID is not included in the list)
                    
                    DateTime.TryParseExact(param.EffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                    Data.KPIPosition.KPIPosition tempForm = new Data.KPIPosition.KPIPosition
                    {
                        Position = param.PositionID,
                        TDate = convertedDate,
                        KPI = e.KPIID,
                        Weight = e.Weight,
                        WeightNoServiceBay = e.WeightNoServiceBay,
                        IsLatest = true,
                        ModifiedBy = param.ModifiedBy,
                        ModifiedDate = DateTime.Now
                    };

                    lstKPIPosition.Add(tempForm);
                }

                // Get All KPI Setup under selected Position
                var oldKPIPosition = (await _dbAccess.GetByPositionID(param.PositionID)).ToList();

                if (oldKPIPosition.Count > 0)
                {
                    oldKPIPosition.ForEach(x => x.IsLatest = false);
                    await _dbAccess.Put(null, oldKPIPosition, null);
                }

                await _dbAccess.Post(lstKPIPosition);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Put(APICredentials credentials, Form param)
        {

            if (param.PositionID == 0)
                ErrorMessages.Add("Position " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (param.KPIPositionList.Count() == 0)
                ErrorMessages.Add("KPI for Position " + MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED);

            if (ErrorMessages.Count == 0)
            {
                DateTime.TryParseExact(param.EffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                List<Data.KPIPosition.KPIPosition> OldKPIPosition = (await _dbAccess.GetByPositionID(param.PositionID, convertedDate)).ToList();

                List<Data.KPIPosition.KPIPosition> KPIPositionToAdd = GetKPIPositionToAdd(OldKPIPosition,
                    param.KPIPositionList == null ? new List<Data.KPIPosition.KPIPosition>() :
                    param.KPIPositionList.Select(x => new Data.KPIPosition.KPIPosition
                    {
                        Position = param.PositionID,
                        TDate = convertedDate,
                        KPI = x.KPIID,
                        Weight = x.Weight,
                        WeightNoServiceBay = x.WeightNoServiceBay,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                List<Data.KPIPosition.KPIPosition> KPIPositionToUpdate = GetKPIPositionToUpdate(OldKPIPosition,
                    param.KPIPositionList == null ? new List<Data.KPIPosition.KPIPosition>() :
                    param.KPIPositionList.Select(x => new Data.KPIPosition.KPIPosition
                    {
                        Position = param.PositionID,
                        TDate = convertedDate,
                        KPI = x.KPIID,
                        Weight = x.Weight,
                        WeightNoServiceBay = x.WeightNoServiceBay,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    }).ToList()).ToList();

                List<Data.KPIPosition.KPIPosition> KPIPositionToDelete = GetKPIPositionToDelete(OldKPIPosition,
                    param.KPIPositionList == null ? new List<Data.KPIPosition.KPIPosition>() :
                    param.KPIPositionList.Select(x => new Data.KPIPosition.KPIPosition
                    {
                        Position = param.PositionID,
                        KPI = x.KPIID
                    }).ToList()).ToList();

                await _dbAccess.Put(KPIPositionToAdd, KPIPositionToUpdate, KPIPositionToDelete);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, GetByIDInput input)
        {
            if (ErrorMessages.Count == 0)
            {
                DateTime.TryParseExact(input.EffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                var kpiPosition = (await _dbAccess.GetByPositionID(input.ID, convertedDate)).ToList();

                await _dbAccess.Delete(kpiPosition);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetAll(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAll()).ToList());
        }

        public async Task<IActionResult> GetAllDetails(APICredentials credentials)
        {
            return new OkObjectResult((await _dbAccess.GetAllDetails()).ToList());
        }

        private IEnumerable<Data.KPIPosition.KPIPosition> GetKPIPositionToAdd(List<Data.KPIPosition.KPIPosition> left, List<Data.KPIPosition.KPIPosition> right)
        {
            return right.GroupJoin(
                left,
                     x => new { x.Position, x.KPI },
                     y => new { y.Position, y.KPI },
                (x, y) => new { newSet = x, oldSet = y })
                .SelectMany(x => x.oldSet.DefaultIfEmpty(),
                (x, y) => new { newSet = x, oldSet = y })
                .Where(x => x.oldSet == null)
                .Select(x =>
                    new Data.KPIPosition.KPIPosition
                    {
                        Position = x.newSet.newSet.Position,
                        TDate = x.newSet.newSet.TDate,
                        KPI = x.newSet.newSet.KPI,
                        Weight = x.newSet.newSet.Weight,
                        WeightNoServiceBay = x.newSet.newSet.WeightNoServiceBay,
                        ModifiedBy = x.newSet.newSet.ModifiedBy,
                        ModifiedDate = x.newSet.newSet.ModifiedDate
                    }).ToList();
        }

        private IEnumerable<Data.KPIPosition.KPIPosition> GetKPIPositionToUpdate(List<Data.KPIPosition.KPIPosition> left, List<Data.KPIPosition.KPIPosition> right)
        {
            return left.Join(
                right,
                     x => new { x.Position, x.KPI },
                     y => new { y.Position, y.KPI },
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => !x.oldSet.Weight.Equals(x.newSet.Weight) || !x.oldSet.WeightNoServiceBay.Equals(x.newSet.WeightNoServiceBay))
                .Select(y =>
                    new Data.KPIPosition.KPIPosition
                    {
                        ID = y.oldSet.ID,
                        Position = y.newSet.Position,
                        TDate = y.newSet.TDate,
                        KPI = y.newSet.KPI,
                        Weight = y.newSet.Weight,
                        WeightNoServiceBay = y.newSet.WeightNoServiceBay,
                        ModifiedBy = y.newSet.ModifiedBy,
                        ModifiedDate = y.newSet.ModifiedDate
                    }).ToList();
        }

        private IEnumerable<Data.KPIPosition.KPIPosition> GetKPIPositionToDelete(List<Data.KPIPosition.KPIPosition> left, List<Data.KPIPosition.KPIPosition> right)
        {
            return left.GroupJoin(
                right,
                     x => new { x.Position, x.KPI },
                     y => new { y.Position, y.KPI },
                (x, y) => new { oldSet = x, newSet = y })
                .SelectMany(x => x.newSet.DefaultIfEmpty(),
                (x, y) => new { oldSet = x, newSet = y })
                .Where(x => x.newSet == null)
                .Select(x =>
                    new Data.KPIPosition.KPIPosition
                    {
                        ID = x.oldSet.oldSet.ID,
                        Position = x.oldSet.oldSet.Position,
                        KPI = x.oldSet.oldSet.KPI
                    }).ToList();
        }

        public async Task<bool> ValidateUploadInsert(APICredentials credentials, List<UploadFileEntity> param)
        {
            //Check if effective dates are less than the data in the file
            bool hasLessEffectiveDate = false;

            foreach (var obj in param.Select(x => x.PositionCode).Distinct())
            {
                List<Position> positionList = (await _positionDBAccess.GetAll()).ToList();
                List<Data.KPIPosition.KPIPosition> kpiPositionList = (await _dbAccess.GetAll()).ToList();

                var positionID = positionList.FirstOrDefault(x => x.Code == obj).SyncID;
                var kpiPosition = kpiPositionList.Where(x => x.Position == positionID).ToList();

                if (kpiPosition.Count > 0)
                {
                    //Check each effective date in the uploaded file
                    foreach (var uploadEffectiveDate in param.Where(x => x.PositionCode == obj).Select(x => x.EffectiveDate).Distinct())
                    {
                        DateTime.TryParseExact(uploadEffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                        //Get the current active date of the kpi position
                        var currentEffectiveDate = kpiPosition.First().TDate;
                        var uploadConvertedEffectiveDate = convertedDate;

                        if (uploadConvertedEffectiveDate < currentEffectiveDate)
                        {
                            hasLessEffectiveDate = true;
                            break;
                        }
                    }
                }
            }

            return !hasLessEffectiveDate;
        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, List<UploadFileEntity> param)
        {
            //Checking if file is empty
            if (param.Count == 0)
            {
                ErrorMessages.Add("File is empty.");
            }

            /*Checking of required and invalid fields*/
            foreach (UploadFileEntity obj in param)
            {
                /*Effective Date*/
                if (string.IsNullOrEmpty(obj.EffectiveDate))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Effective Date ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }
                else
                {
                    obj.EffectiveDate = obj.EffectiveDate.Trim();
                    if (!DateTime.TryParseExact(obj.EffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Effective Date", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        obj.EffectiveDateConverted = convertedDate;
                    }
                }

                /*KPI Code*/
                if (string.IsNullOrEmpty(obj.KPICode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }

                /*Position Code*/
                if (string.IsNullOrEmpty(obj.PositionCode))
                {
                    ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                }


                /*Weight*/
                if (string.IsNullOrEmpty(obj.Weight))
                {
                    if (!decimal.TryParse(obj.Weight, out decimal result))
                    {
                        ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Weight " + MessageUtilities.SUFF_ERRMSG_INVALID, " Value: " + obj.Weight));
                    }
                }
            }    


            /*Checking if Code was existing on database*/
            if (ErrorMessages.Count == 0)
            {
                List<string> kpiList = (await _kpiDBAccess.GetByCodes(param.Select(x => x.KPICode).ToList())).Select(x => x.Code).Distinct().ToList();
                List<string> positionList = (await _positionDBAccess.GetByCodes(param.Select(x => x.PositionCode).ToList())).Select(x => x.Code).Distinct().ToList();

                foreach (UploadFileEntity obj in param)
                {
                    /*KPI Code*/
                    if (!string.IsNullOrEmpty(obj.KPICode))
                    {
                        if (!kpiList.Contains(obj.KPICode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "KPI Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.KPICode));
                        }
                    }

                    /*Position Code*/
                    if (!string.IsNullOrEmpty(obj.PositionCode))
                    {
                        if (!positionList.Contains(obj.PositionCode))
                        {
                            ErrorMessages.Add(string.Concat(("Row [" + obj.RowNum + "] : "), "Position Code " + MessageUtilities.SUFF_ERRMSG_REC_NOT_EXISTS, " Value: " + obj.PositionCode));
                        }
                    }
                }
            }

            /*Checking if there is an existing kpi position based on the effective dates*/
            if (ErrorMessages.Count == 0)
            {
                List<Position> positionList = (await _positionDBAccess.GetAll()).ToList();
                List<Data.KPIPosition.KPIPosition> kpiPositionList = (await _dbAccess.GetAll()).ToList();

                foreach (var obj in param.Select(x => x.PositionCode).Distinct())
                {
                    var positionID = positionList.FirstOrDefault(x => x.Code == obj).SyncID;
                    var kpiPosition = kpiPositionList.Where(x => x.Position == positionID).ToList();

                    foreach (var uploadEffectiveDate in param.Where(x => x.PositionCode == obj).Select(x => x.EffectiveDate).Distinct())
                    {
                        foreach (var effectiveDate in kpiPosition.Select(x => x.TDate).Distinct())
                        {
                            string position = positionList != null ?
                                positionList.Count > 0 ?
                                positionList.Where(x => x.SyncID == positionID).First().Title : "" : "";

                            DateTime.TryParseExact(uploadEffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                            var uploadConvertedEffectiveDate = convertedDate;

                            if (uploadConvertedEffectiveDate == effectiveDate)
                            {
                                ErrorMessages.Add(string.Concat("KPI for [ " + position + " ] with Effective Date " + uploadConvertedEffectiveDate.ToString("MM/dd/yyyy") + " already exists."));
                            }
                        }
                    }
                }
            }

            //Check if Total Weight is 100%
            if (ErrorMessages.Count == 0)
            {
                foreach (var obj in param.Select(x => x.PositionCode).Distinct())
                {
                    foreach (var uploadEffectiveDate in param.Where(x => x.PositionCode == obj).Select(x => x.EffectiveDate).Distinct())
                    {
                        //calculate the total weight per effective date
                        decimal weight = param.Where(x => x.PositionCode == obj)
                            .Where(x => x.EffectiveDate == uploadEffectiveDate).Sum(x => Convert.ToDecimal(x.Weight));

                        if (weight != 100)
                        {
                            List<Position> positionList = (await _positionDBAccess.GetByCodes(new List<string> { obj })).ToList();
                            string position = positionList != null ? 
                                positionList.Count > 0 ? 
                                positionList.Where(x => x.Code.Equals(obj)).First().Title : "" : "";

                            ErrorMessages.Add(string.Concat("Total Weight for [ " + position + " ] is not 100%. Value: " + weight.ToString("###.####") + "%"));
                        }
                    }
                }
            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                List<Data.KPI.KPI> kpiList = (await _kpiDBAccess.GetAll()).ToList();
                List<Position> positionList = (await _positionDBAccess.GetAll()).ToList();

                var kpiPositionList = (await _dbAccess.GetAll()).ToList();

                var uploadKPIPosition = param.Join(kpiList,
                                                 upload => upload.KPICode,
                                                 kpi => kpi.Code,
                                                 (upload, kpi) => new { upload, kpi }
                                                 )
                                             .Join(positionList,
                                                  uploadKPI => uploadKPI.upload.PositionCode,
                                                  position => position.Code,
                                                  (uploadKPI, position) => new { uploadKPI, position }
                                              )
                                              .Select(x => new Data.KPIPosition.KPIPosition
                                              {
                                                  KPI = x.uploadKPI.kpi.ID,
                                                  Position = x.position.SyncID,
                                                  Weight = Convert.ToDecimal(x.uploadKPI.upload.Weight),
                                                  WeightNoServiceBay = Convert.ToDecimal(x.uploadKPI.upload.Weight),
                                                  TDate = Convert.ToDateTime(x.uploadKPI.upload.EffectiveDateConverted),
                                                  IsLatest = (kpiPositionList.FirstOrDefault(y => y.Position == x.position.SyncID
                                                                                      && y.TDate > Convert.ToDateTime(x.uploadKPI.upload.EffectiveDateConverted))) != null
                                                                                     ? false : true,
                                                  ModifiedBy = credentials.UserID,
                                                  ModifiedDate = DateTime.Now,
                                              }).ToList();

                var latestUpload = uploadKPIPosition.Where(x => x.IsLatest)
                                                    .Select(x => x.Position)
                                                    .Distinct()
                                                    .ToList();

                // Update KPI Setups to Not Latest where upload is most recent
                if (latestUpload.Count() > 0)
                {
                    var toUpdate = new List<Data.KPIPosition.KPIPosition>();

                    foreach (var e in latestUpload)
                    {

                        var tempToUpdate = (await _dbAccess.GetByPositionID(e)).ToList();
                        toUpdate.AddRange(tempToUpdate);
                    }

                    toUpdate.ForEach(x => x.IsLatest = false);

                    await _dbAccess.Put(null, toUpdate, null);
                }

                await _dbAccess.UploadInsert(uploadKPIPosition);

                _resultView.IsSuccess = true;
            }

            if (ErrorMessages.Count != 0)
            {
                ErrorMessages.Insert(0, "Upload was not successful. Error(s) below was found :");
                ErrorMessages.Insert(1, "");
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_FILE_UPLOAD);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> GetExportList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarKPIPositionExport> result = await _dbAccess.GetExportList(input, rowStart);

            return new OkObjectResult(result.Select(x => new GetExportListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Position = x.Position,
                KPI = x.KPI,
                Weight = Math.Round(x.Weight, 2).ToString(),
                EffectiveDate = x.EffectiveDate,
            }).ToList());
        }
        public async Task<IActionResult> CopyKpiPosition(APICredentials credentials, CopyKpiPositionInput param)
        {
            if (param.OldPosition.Equals(0))
                ErrorMessages.Add("Old Position is Required!");
            if (param.NewPosition.Equals(0))
                ErrorMessages.Add("New Position is Required!");

            var CheckIfExist = (await _dbAccess.GetAll())
                .Where(x => x.Position.Equals(param.NewPosition)
                && x.TDate.Equals(Convert.ToDateTime(param.NewEffectiveDate))
                && x.IsLatest).ToList();

            if (CheckIfExist.Count > 0)
                ErrorMessages.Add("KPI Position "+MessageUtilities.SUFF_ERRMSG_REC_EXISTS);

            if (ErrorMessages.Count == 0)
            {
                //GET COPY KPI POSITION
                List<Data.KPIPosition.KPIPosition> getCopyPosition = (await _dbAccess.GetAll())
                    .Where(x => x.Position.Equals(param.OldPosition)
                    && x.TDate.Equals(Convert.ToDateTime(param.OldEffectiveDate))
                    && x.IsLatest).ToList();

                List<Data.KPIPosition.KPIPosition> lstKPIPosition = new List<Data.KPIPosition.KPIPosition>();

                foreach (var e in getCopyPosition)
                {
                    // Use KPI Position List (contains KPI & Weight)
                    // to generate KPI Position data (Position ID is not included in the list)

                    DateTime.TryParseExact(param.NewEffectiveDate, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime convertedDate);

                    Data.KPIPosition.KPIPosition tempForm = new Data.KPIPosition.KPIPosition
                    {
                        Position = param.NewPosition,
                        TDate = convertedDate,
                        KPI = e.KPI,
                        Weight = e.Weight,
                        WeightNoServiceBay = e.WeightNoServiceBay,
                        IsLatest = true,
                        ModifiedBy = credentials.UserID,
                        ModifiedDate = DateTime.Now
                    };

                    lstKPIPosition.Add(tempForm);
                }

                // Get All KPI Setup under selected Position
                var oldKPIPosition = (await _dbAccess.GetByPositionID(param.NewPosition)).ToList();

                if (oldKPIPosition.Count > 0)
                {
                    oldKPIPosition.ForEach(x => x.IsLatest = false);
                    await _dbAccess.Put(null, oldKPIPosition, null);
                }

                await _dbAccess.Post(lstKPIPosition);
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }
        public async Task<IActionResult> GetCopyPosition(APICredentials credentials, IPM.Transfer.Shared.GetAutoCompleteInput param)
        {
            return new OkObjectResult((await _dbAccess.GetCopyPosition(param))
                .Select(x => new IPM.Transfer.Shared.GetAutoCompleteOutput { ID = x.ID, Description = x.Description })
            );
        }

    }
}