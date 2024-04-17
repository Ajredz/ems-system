using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Data.Employee;
using EMS.Plantilla.Data.EmployeeMovement;
using EMS.Plantilla.Data.OrgGroup;
using EMS.Plantilla.Data.Position;
using EMS.Plantilla.Data.PSGC;
using EMS.Plantilla.Data.Reference;
using EMS.Plantilla.Transfer;
using EMS.Plantilla.Transfer.EmployeeMovement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.Core.EmployeeMovement
{
    public interface IEmployeeMovementService
    {
        Task<IActionResult> GetList(APICredentials credentials, GetListInput input);

        Task<IActionResult> GetAutoComplete(APICredentials credentials, GetAutoCompleteByMovementTypeInput param);

        Task<IActionResult> Post(APICredentials credentials, Form param);

        Task<IActionResult> UploadInsert(APICredentials credentials, UploadFile param);

        Task<IActionResult> GetPrint(APICredentials credentials, GetPrintInput param);

        Task<IActionResult> GetAutoPopulate(APICredentials credentials, GetAutoPopulateByMovementTypeInput param);

        Task<IActionResult> GetByID(APICredentials credentials, long ID);
        // For movement checker 41
        Task<IActionResult> GetByIDs(APICredentials credentials, List<long> ID);

        Task<IActionResult> SyncMovement(APICredentials credentials);

        Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value);

        Task<IActionResult> GetEmployeeFieldList(APICredentials credentials, string type);

        Task<IActionResult> GetMovementType(APICredentials credentials, string MovementType);

        Task<IActionResult> AddMovementEmployeeField(APICredentials credentials, EmployeeFieldForm param);

        Task<IActionResult> BulkRemove(APICredentials credentials, BulkRemoveForm param);

        Task<IActionResult> Delete(APICredentials credentials, BulkRemoveForm ID);

        Task<IActionResult> Update(APICredentials credentials, Form param);
        // For movement checker 59-60
        Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatus param);
        Task<IActionResult> UpdateDateTo(APICredentials credentials, Form param);
        Task<IActionResult> GetEmployeeMovementByEmployeeIDs(APICredentials credentials, List<int> EmployeeIDs);
    }

    public class EmployeeMovementService : Core.Shared.Utilities, IEmployeeMovementService
    {
        private readonly IEmployeeMovementDBAccess _dbAccess;
        private readonly IEmployeeDBAccess _employeeDBAccess;
        private readonly IReferenceDBAccess _referenceDBAccess;
        private readonly IPositionDBAccess _positionDBAccess;
        private readonly IOrgGroupDBAccess _orgGroupDBAccess;

        public EmployeeMovementService(PlantillaContext dbContext, IConfiguration iconfiguration,
            IEmployeeMovementDBAccess dbAccess, IEmployeeDBAccess employeeDBAccess, IReferenceDBAccess referenceDBAccess,
            IPositionDBAccess positionDBAccess, IOrgGroupDBAccess orgGroupDBAccess) : base(dbContext, iconfiguration)
        {
            _dbAccess = dbAccess;
            _referenceDBAccess = referenceDBAccess;
            _employeeDBAccess = employeeDBAccess;
            _positionDBAccess = positionDBAccess;
            _orgGroupDBAccess = orgGroupDBAccess;
        }
        //For movement checker 94
        public async Task<IActionResult> GetList(APICredentials credentials, GetListInput input)
        {
            int rowStart = 1;
            rowStart = input.pageNumber > 1 ? input.pageNumber * input.rows - input.rows + 1 : rowStart;

            IEnumerable<TableVarEmployeeMovement> result = await _dbAccess.GetList(input, rowStart);
            return new OkObjectResult(result.Select(x => new GetListOutput
            {
                TotalNoOfRecord = result.First().TotalNum,
                NoOfPages = (int)Math.Ceiling((float)result.First().TotalNum / (float)input.rows),
                ID = x.ID,
                Status = x.Status,
                EmployeeName = x.EmployeeName,
                OldEmployeeID = x.OldEmployeeID,
                OrgGroup = x.OrgGroup,
                EmployeeField = x.EmployeeField,
                MovementType = x.MovementType,
                From = x.From,
                To = x.To,
                DateEffectiveFrom = x.DateEffectiveFrom,
                DateEffectiveTo = x.DateEffectiveTo,
                CreatedDate = x.CreatedDate,
                CreatedBy = x.CreatedBy,
                CreatedByName = x.CreatedByName,
                Reason = x.Reason,
                HRDComments = x.HRDComments,
                Name = x.Name,
                NewEmployeeID = x.NewEmployeeID,
                DeptSection = x.DeptSection,
                DateHired = x.DateHired,
                Age = x.Age,
                Gender = x.Gender,
                EmploymentStatus = x.EmploymentStatus,
                Region = x.Region
            }).ToList());
        }


        public async Task<IActionResult> GetAutoComplete(APICredentials credentials, GetAutoCompleteByMovementTypeInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoComplete(param))
                .Select(x => new GetAutoCompleteByMovementTypeOutput
                {
                    Value = x.Value,
                    Description = x.Description
                })
            );
        }

        public async Task<IActionResult> Post(APICredentials credentials, Form param)
        {
            //if (param.MovementType.Equals("BRANCH_TRANSFER") /*&& string.IsNullOrEmpty(param.EffectiveDateTo)*/)
            //{
            //    if (string.IsNullOrEmpty(param.EffectiveDateTo))
            //    {
            //        List<Data.Employee.Employee> EmployeeDetails = (await _employeeDBAccess.GetList();

            //        foreach (Data.Employee.Employee item in EmployeeDetails)
            //        {
            //          { 
            //                param.FromValueID = (item.OrgGroupID).ToString();
            //          }
            //        }
            //     }
            //        // Get old org_group_id of employee
            //        //Int32.Parse(_dbContext.Employee.AsNoTracking()
            //        //.Where(x => x.OrgGroupID.Equals(param.EmployeeID/*, StringComparison.CurrentCultureIgnoreCase*/)));
            //        //.ToListAsync();

            //        //_employeeDBAccess.GetByOldEmployeeID((param.EmployeeID)).ToList();
            //        //_dbContext.Employee(Select );
            //        //param.FromValueID.Select(param => param.).FirstOrDefault().ToString();
            //        // '@ SELECT org_group FROM employee WHERE code = param_employee_id ';
            //        //foreach (var item in param.EmployeeFieldList)
            //        //{

            //        //    if (item.EmployeeField != null)
            //        //    {
            //        //        List<Data.EmployeeMovement.EmployeeMovement> latest =
            //        //                            (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(param.EmployeeID, item.EmployeeField)).ToList();

            //        //        if (latest != null)
            //        //        {
            //        //            if (latest.Count > 0)
            //        //            {

            //        //            }
            //        //        }
            //        //    }
            // }
                
            if (param.EmployeeID <= 0)
                ErrorMessages.Add(string.Concat("Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));

            if (string.IsNullOrEmpty(param.MovementType))
                ErrorMessages.Add(string.Concat("Movement Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            else
            {
                param.MovementType = param.MovementType.Trim();
                if (param.MovementType.Length > 50)
                    ErrorMessages.Add(string.Concat("Movement Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (param.EmployeeField != null && !param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString()))
            {
                if (string.IsNullOrEmpty(param.NewValue))
                    ErrorMessages.Add(string.Concat("New Value ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.NewValue = param.NewValue.Trim();
                    if (param.NewValue.Length > 255)
                        ErrorMessages.Add(string.Concat("New Value", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                } 
            }

            //if (string.IsNullOrEmpty(param.NewValueID))
            //    ErrorMessages.Add(string.Concat("New Value ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            //else
            //{
            //    param.NewValueID = param.NewValueID.Trim();
            //    if (param.NewValueID.Length > 255)
            //        ErrorMessages.Add(string.Concat("New Value ID", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
            //}

            if (!param.IsEdit)
            {
                if (string.IsNullOrEmpty(param.EffectiveDateFrom))
                    ErrorMessages.Add(string.Concat("Effective Date From", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.EffectiveDateFrom = param.EffectiveDateFrom.Trim();
                    if (!DateTime.TryParseExact(param.EffectiveDateFrom, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied))
                    {
                        ErrorMessages.Add(string.Concat("Effective Date From ", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        if (param.EmployeeFieldList != null)
                        {
                            foreach (var item in param.EmployeeFieldList)
                            {

                                if (item.EmployeeField != null)
                                {
                                    List<Data.EmployeeMovement.EmployeeMovement> latest =
                                                        (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(param.EmployeeID, item.EmployeeField)).ToList();

                                    if (latest != null)
                                    {
                                        if (latest.Count > 0)
                                        {
                                            //if (dateApplied < latest.First().DateEffectiveFrom)
                                            //    ErrorMessages.Add(
                                            //        string.Concat("The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; ", item.EmployeeField, " (", latest.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ").")
                                            //        );
                                        }
                                    }
                                }
                            } 
                        }

                    }
                }

                if (!param.UseCurrent)
                {
                    if (string.IsNullOrEmpty(param.EffectiveDateTo))
                        ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        param.EffectiveDateTo = param.EffectiveDateTo.Trim();
                        if (!DateTime.TryParseExact(param.EffectiveDateTo, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied))
                        {
                            ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        else
                        {

                            if (
                                    param.EmployeeField != null
                                    && !param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString())
                                    && param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.SECONDARY_DESIG.ToString())
                                )
                            {
                                List<Data.EmployeeMovement.EmployeeMovement> latest =
                                                    (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(param.EmployeeID, param.EmployeeField)).ToList();

                                if (latest != null)
                                {
                                    if (latest.Count > 0)
                                    {
                                        if (dateApplied < latest.First().DateEffectiveTo)
                                            ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.COMPARE_GREATER_EQUAL, "the date effective to of the active movement."));
                                    }
                                }
                            }

                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(param.Reason))
            {
                param.Reason = param.Reason.Trim();
                if (param.Reason.Length > 1000)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }
            
            if (!string.IsNullOrEmpty(param.HRDComments))
            {
                param.HRDComments = param.HRDComments.Trim();
                if (param.HRDComments.Length > 1000)
                    ErrorMessages.Add(string.Concat("Additional Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }

            //Validate Date Range of Adding movement if OverLapping to List of Movement
            DateTime effectiveDateFrom = DateTime.ParseExact(param.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string dateto = string.IsNullOrEmpty(param.EffectiveDateTo) ? null : param.EffectiveDateTo;
            DateTime effectiveDateTo = DateTime.ParseExact(dateto ?? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (!String.IsNullOrEmpty(param.EmployeeField) && param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString()))
                param.EmployeeField = param.EmployeeField;
            else
                param.EmployeeField = param.EmployeeFieldList.Select(s => s.EmployeeField).FirstOrDefault().ToString();
            
            List<Data.EmployeeMovement.EmployeeMovement> movementlist = (await _dbAccess.GetEmployeeMovementList(param.EmployeeID, param.EmployeeField)).ToList();
           
            //TRY2
            if (movementlist != null && !param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString()))
            {
                if (movementlist.Count > 0)
                {
                    if (string.IsNullOrEmpty(param.EffectiveDateTo))
                    {
                        foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                        {
                            foreach (var obj in param.EmployeeFieldList.Select(x => x.EmployeeField))
                            {
                                if (param.EmployeeFieldList.Select(s => s.NewValue).FirstOrDefault().ToString().Equals(item.To) 
                                /* Removed as it causes duplication if it causes to allow if employee is NEW_AREA_ASSIG, ADDITIONAL or TEMP_BRANCH_ASSIG */
                                /* && item.MovementType == param.MovementType*/
                                 && item.EmployeeField == param.EmployeeField)
                                {
                                    if (param.EmployeeField == "EMPLOYMENT_STATUS")
                                    {
                                        if (effectiveDateFrom < movementlist.First().DateEffectiveFrom)
                                        {
                                            //ErrorMessages.Add(string.Concat("The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; "
                                            //        , param.EmployeeField, " (", movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ")."));
                                            ErrorMessages.Add(string.Concat("The Movement is invalid as it will be duplicated by the currently active movement. Dated as "
                                                    , movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), " on ", param.EmployeeField, "."
                                        ));
                                        }
                                    }
                                    else {
                                        if (effectiveDateFrom <= movementlist.First().DateEffectiveFrom)
                                        {
                                            //ErrorMessages.Add(string.Concat("The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; "
                                            //        , param.EmployeeField, " (", movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ")."));
                                            ErrorMessages.Add(string.Concat("The Movement is invalid as it will be duplicated by the currently active movement. Dated as "
                                                    , movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), " on ", param.EmployeeField, "."
                                        ));
                                        } 
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                        {
                            if (item.MovementType == param.MovementType && item.EmployeeField == param.EmployeeField)
                            {
                                foreach (var obj in param.EmployeeFieldList.Select(x => x.EmployeeField))
                                {
                                    //param.NewValue = param.EmployeeFieldList.Select(s => s.NewValue).FirstOrDefault().ToString();
                                    //if (item.DateEffectiveTo == null)
                                    //{
                                    //    item.DateEffectiveTo = DateTime.Now;
                                    //}

                                    if ((param.EmployeeFieldList.Select(s => s.NewValue).FirstOrDefault().ToString()) == item.To &&
                                        effectiveDateFrom < item.DateEffectiveTo && 
                                        item.DateEffectiveFrom < effectiveDateTo)
                                    {
                                        ErrorMessages.Add(string.Concat(("The Movement is overlapping on existing movement(s); "
                                        , param.EmployeeField, " (Effective Date :", item.DateEffectiveFrom.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), " - ",
                                        item.DateEffectiveTo?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) ?? DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), ").")));

                                    }
                                }
                            }

                        }
                    }
                }
            }


            if (ErrorMessages.Count == 0)
            {
                if (param.IsEdit)
                {
                    Data.EmployeeMovement.EmployeeMovement result = await _dbAccess.GetByID(param.ID);
                    result.Reason = param.Reason;
                    result.HRDComments = param.HRDComments;
                    await _dbAccess.Put(result);
                }
                else
                { 
                    if (param.EmployeeFieldList == null)
                    {
                        await _dbAccess.Post(param);
                    }
                    else
                    {
                        foreach (var obj in param.EmployeeFieldList.Select(x => x.EmployeeField))
                        {
                            param.EmployeeField = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().EmployeeField;
                            param.NewValue = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().NewValue;
                            param.NewValueID = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().NewValueID;
                            // test start 
                            param.FromValue = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().OldValue;
                            param.FromValueID = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().OldValueID;                       
                            // test end 


                            await _dbAccess.Post(param);

                            
                        }
                    }
                }

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));

        }

        public async Task<IActionResult> UploadInsert(APICredentials credentials, UploadFile param)
        {
            var BackdatedList = new List<string>();
            var movementType = (await _referenceDBAccess.GetByRefCodes(
                new List<string> { Enums.ReferenceCodes.MOVEMENT_TYPE.ToString() })).Select(x => x.Value).ToList();
            
            if (param.UploadFileMovement != null)
            {

                var employeeField = (await _referenceDBAccess.GetByRefCodes(
                new List<string> { Enums.ReferenceCodes.MOVEMENT_EMP_FIELD.ToString() })).Select(x => x.Value).ToList();

                /*Checking of required and invalid fields*/
                foreach (UploadFileMovement obj in param.UploadFileMovement)
                {
                    if (string.IsNullOrEmpty(obj.FromValueID))
                    {
                        if(obj.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.ORG_GROUP.ToString()))
                            obj.FromValueID = (await _orgGroupDBAccess.GetByCode(obj.FromValue)).FirstOrDefault().ID.ToString();
                    }

                    /*OldEmployeeID*/
                    if (string.IsNullOrEmpty(obj.OldEmployeeID))
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.OldEmployeeID = obj.OldEmployeeID.Trim();
                        if (obj.OldEmployeeID.Length > 7)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "7 characters."));
                        }
                        else
                        {
                            var oldEmployeeID = (await _employeeDBAccess.GetByOldEmployeeID(obj.OldEmployeeID)).ToList();

                            if (oldEmployeeID == null)
                            {
                                ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                            }
                            else
                            {
                                if (oldEmployeeID.Count == 0)
                                {
                                    ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                                }
                                else
                                {
                                    obj.EmployeeID = oldEmployeeID.First().ID;
                                }
                            }
                        }
                    }

                    if (obj.MovementType.Equals("BRANCH_TRANSFER") && (obj.EffectiveDateTo == "") && obj.FromValueID == null)
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [ " + obj.RowNum + " ]: "), ""
                            + "Please enter the employee's current branch, in old value ,before the " + obj.MovementType + " to avoid missing track of records."));

                        /*(("Movement cannot proceed with uploading. Please fill up correctly for the following Movement: " +
                             "Branch Transfer, Promotion and Employment Status requires old value."))); */
                    }

                    if (obj.MovementType.Equals("PROMOTION") && ( obj.EffectiveDateTo == "" ) && obj.FromValueID == null)
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [ " + obj.RowNum + " ]: ") , "" 
                            + "Please enter the employee's current position, in old value ,before the " + obj.MovementType + " to avoid missing track of records."));

                       /*(("Movement cannot proceed with uploading. Please fill up correctly for the following Movement: " +
                            "Branch Transfer, Promotion and Employment Status requires old value."))); */
                    }

                    /*MovementType*/
                    if (string.IsNullOrEmpty(obj.MovementType))
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.MovementType = obj.MovementType.Trim();
                        if (obj.MovementType.Length > 20)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Movement Type", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                        }

                        if (!Regex.IsMatch(obj.MovementType, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (movementType.Where(x => obj.MovementType.Equals(x)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.COMPARE_INVALID));
                        }
                    }


                    /*EmployeeField*/
                    if (string.IsNullOrEmpty(obj.EmployeeField))
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Employee Field ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.EmployeeField = obj.EmployeeField.Trim();
                        if (obj.EmployeeField.Length > 20)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Employee Field", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                        }

                        if (!Regex.IsMatch(obj.EmployeeField, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Employee Field ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (employeeField.Where(x => obj.EmployeeField.Equals(x)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Employee Field ", MessageUtilities.COMPARE_INVALID));
                        }
                    }

                    /*NewValue*/
                    if (string.IsNullOrEmpty(obj.NewValue))
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "New Value ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.NewValue = obj.NewValue.Trim();
                        if (obj.NewValue.Length > 50)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "New Value", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                        }
                        else
                        {
                            var newValue = new List<TableVarEmployeeMovementAutoComplete>();
                            try
                            {
                                newValue = (await _dbAccess.ValidateMovementNewValue(obj.EmployeeField, obj.MovementType, obj.NewValue)).ToList();
                            }
                            catch (Exception ex)
                            {

                            }
                            //var newValue = (await _dbAccess.ValidateMovementNewValue(obj.EmployeeField, obj.MovementType, obj.NewValue)).ToList();

                            if (newValue == null)
                            {
                                ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "New Value ", MessageUtilities.COMPARE_INVALID));
                            }
                            else
                            {
                                if (newValue.Count == 0)
                                {
                                    ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "New Value ", MessageUtilities.COMPARE_INVALID));
                                }
                                else
                                {
                                    if(string.IsNullOrEmpty(newValue.First().Value))
                                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "New Value ", MessageUtilities.COMPARE_INVALID));
                                    else 
                                        obj.NewValueID = newValue.First().Value;
                                }
                            }
                        }
                    }

                    /*OldValue*/
                    if (!string.IsNullOrEmpty(obj.FromValue))
                    {
                        obj.FromValue = obj.FromValue.Trim();
                        if (obj.FromValue.Length > 50)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Value", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                        }
                        else
                        {
                            
                            var oldValue = (await _dbAccess.ValidateMovementNewValue(obj.EmployeeField, obj.MovementType, obj.FromValue)).ToList();

                            if (oldValue == null)
                            {
                                ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Value ", MessageUtilities.COMPARE_INVALID));
                            }
                            else
                            {
                                if (oldValue.Count == 0)
                                {
                                    ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Value ", MessageUtilities.COMPARE_INVALID));
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(oldValue.First().Value))
                                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Old Value ", MessageUtilities.COMPARE_INVALID));
                                    else
                                        obj.FromValueID = oldValue.First().Value;
                                }
                            }
                        }
                    }

                    /*EffectiveDateFrom*/
                    if (string.IsNullOrEmpty(obj.EffectiveDateFrom))
                    {
                        ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Effective Date From ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.EffectiveDateFrom = obj.EffectiveDateFrom.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Effective Date", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                       
                    }

                    /*EffectiveDateTo*/
                    if (!string.IsNullOrEmpty(obj.EffectiveDateTo))
                    {
                        obj.EffectiveDateTo = obj.EffectiveDateTo.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateTo, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Effective Date To", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        
                    }

                    /*Reason*/
                    if (!string.IsNullOrEmpty(obj.Reason))
                    {
                        if (obj.Reason.Length > 1000)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                        }
                    }

                    /*HRDComments*/
                    if (!string.IsNullOrEmpty(obj.HRDComments))
                    {
                        if (obj.HRDComments.Length > 1000)
                        {
                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "Additional Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                        }
                    }

                    //Validate Date Range of Adding movement if OverLapping to List of Movement
                    List<Data.EmployeeMovement.EmployeeMovement> movementlist = (await _dbAccess.GetEmployeeMovementList(obj.EmployeeID, obj.EmployeeField)).ToList();

                   //Try2
                    if (movementlist != null)
                    {
                        if (movementlist.Count > 0)
                        {
                            DateTime effectiveDateFrom = DateTime.ParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            string dateto = string.IsNullOrEmpty(obj.EffectiveDateTo) ? null : obj.EffectiveDateTo;
                            DateTime effectiveDateTo = DateTime.ParseExact(dateto ?? DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                            if (string.IsNullOrEmpty(obj.EffectiveDateTo))
                            {
                                foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                                {
                                    if (item.MovementType == obj.MovementType && item.EmployeeField == obj.EmployeeField)
                                    {
                                        if (effectiveDateFrom <= movementlist.First().DateEffectiveFrom)
                                        {
                                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; "
                                                    , obj.EmployeeField, " (", movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ")."));
                                        }
                                    }
                                }
                                
                            }
                            else 
                            {
                                foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                                {
                                    if (item.MovementType == obj.MovementType && item.EmployeeField == obj.EmployeeField)
                                    {
                                        if (item.DateEffectiveTo == null)
                                        {
                                            item.DateEffectiveTo = DateTime.Now;
                                        }
                                        // Validate of Adding Date Range is Over Lapping to Exisiting Movement or not
                                        // --------tStartA----------tEndB               --------tstartB -----------tEndA--------
                                        if (effectiveDateFrom < item.DateEffectiveTo && item.DateEffectiveFrom < effectiveDateTo)
                                        {
                                            //strDateEffectiveTo = 
                                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "The Movement is overlapping on existing movement(s); "
                                                , obj.EmployeeField, " (Effective Date :", item.DateEffectiveFrom.ToString("MM/dd/yyyy"), " - ",
                                                item.DateEffectiveTo?.ToString("MM/dd/yyyy") ?? DateTime.Now.ToString("MM/dd/yyyy"), ")."));

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (param.UploadFileSecondary != null)
            {
                var orgGroupList = (await _orgGroupDBAccess.GetAll()).ToList();
                var positionList = (await _positionDBAccess.GetAll()).ToList();
                
                //TRY1


                /*Checking of required and invalid fields*/
                foreach (UploadFileSecondary obj in param.UploadFileSecondary)
                {

                    
                    //DateTime effectiveDateFrom = DateTime.ParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //string dateto = string.IsNullOrEmpty(obj.EffectiveDateTo) ? null : obj.EffectiveDateTo;
                    //DateTime effectiveDateTo = DateTime.ParseExact(dateto ?? DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    //foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                    //{
                    //    foreach (var obj1 in param.UploadFileSecondary.Select(x => "SECONDARY_DESIG"))
                    //    {

                    //        if (effectiveDateFrom < item.DateEffectiveTo && item.DateEffectiveFrom < effectiveDateTo)
                    //        {
                    //            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "The Movement is overlapping on existing movement(s); "
                    //                , " (Effective Date :", item.DateEffectiveFrom.ToString("MM/dd/yyyy"), " - ",
                    //                item.DateEffectiveTo?.ToString("MM/dd/yyyy") ?? DateTime.Now.ToString("MM/dd/yyyy"), ")."));

                    //        }
                    //    }
                    //}



                    /*OldEmployeeID*/
                    if (string.IsNullOrEmpty(obj.OldEmployeeID))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.OldEmployeeID = obj.OldEmployeeID.Trim();
                        if (obj.OldEmployeeID.Length > 7)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "7 characters."));
                        }
                        else
                        {
                            var oldEmployeeID = (await _employeeDBAccess.GetByOldEmployeeID(obj.OldEmployeeID)).ToList();

                            if (oldEmployeeID == null)
                            {
                                ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                            }
                            else
                            {
                                if (oldEmployeeID.Count == 0)
                                {
                                    ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                                }
                                else
                                {
                                    obj.EmployeeID = oldEmployeeID.First().ID;
                                }
                            }
                        }
                    }

                    /*MovementType*/
                    if (string.IsNullOrEmpty(obj.MovementType))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.MovementType = obj.MovementType.Trim();
                        if (obj.MovementType.Length > 20)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Movement Type", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                        }

                        if (!Regex.IsMatch(obj.MovementType, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (movementType.Where(x => obj.MovementType.Equals(x)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.COMPARE_INVALID));
                        }
                    }

                    /*OrgGroup*/
                    if (string.IsNullOrEmpty(obj.OrgGroupCode))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.OrgGroupCode = obj.OrgGroupCode.Trim();
                        if (obj.OrgGroupCode.Length > 50)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Org Group Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                        }

                        if (!Regex.IsMatch(obj.OrgGroupCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }
                        var orgGroup = orgGroupList.Where(x => obj.OrgGroupCode.Equals(x.Code));
                        if (orgGroup.Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Org Group Code ", MessageUtilities.COMPARE_INVALID));
                        }
                        else
                        {
                            obj.OrgGroupID = orgGroup.First().ID;
                        }
                    }

                    /*Position*/
                    if (string.IsNullOrEmpty(obj.PositionCode))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.PositionCode = obj.PositionCode.Trim();
                        if (obj.PositionCode.Length > 50)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Position Code", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                        }

                        if (!Regex.IsMatch(obj.PositionCode, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }
                        var position = positionList.Where(x => obj.PositionCode.Equals(x.Code));
                        if (position.Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Position Code ", MessageUtilities.COMPARE_INVALID));
                        }
                        else
                        {
                            obj.PositionID = position.First().ID;
                        }
                    }

                    /*AddRemove*/
                    if (string.IsNullOrEmpty(obj.AddRemove))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Add/Remove ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.AddRemove = obj.AddRemove.Trim();
                        if (obj.AddRemove.Length > 50)
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Add/Remove", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
                        }

                        if (!Regex.IsMatch(obj.AddRemove, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Add/Remove ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }
                        if (!obj.AddRemove.Equals("ADD") & !obj.AddRemove.Equals("REMOVE"))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Add/Remove ", MessageUtilities.COMPARE_INVALID));
                        }
                    }

                    /*EffectiveDateFrom*/
                    if (string.IsNullOrEmpty(obj.EffectiveDateFrom))
                    {
                        ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Effective Date From", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.EffectiveDateFrom = obj.EffectiveDateFrom.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Effective Date From", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                    }

                    /*EffectiveDateTo*/
                    if (!string.IsNullOrEmpty(obj.EffectiveDateTo))
                    {
                        obj.EffectiveDateTo = obj.EffectiveDateTo.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateTo, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Effective Date To", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                    }

                    /*Reason*/
                    if (!string.IsNullOrEmpty(obj.Reason))
                        if (obj.Reason.Length > 1000)
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));

                    /*HRDComments*/
                    if (!string.IsNullOrEmpty(obj.HRDComments))
                        if (obj.HRDComments.Length > 1000)
                            ErrorMessages.Add(string.Concat(("Secondary Designation - Row [" + obj.RowNum + "] : "), "Additional Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));


                    //ADDED VALIDATION FOR UPLOAD
                    List<Data.EmployeeMovement.EmployeeMovement> movementlist = (await _dbAccess.GetEmployeeMovementList(obj.EmployeeID, "SECONDARY_DESIG")).ToList();

                    DateTime effectiveDateFrom = DateTime.ParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string dateto = string.IsNullOrEmpty(obj.EffectiveDateTo) ? null : obj.EffectiveDateTo;
                    DateTime effectiveDateTo = DateTime.ParseExact(dateto ?? DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    if (movementlist != null)
                    {
                        if (movementlist.Count > 0)
                        {

                            if (string.IsNullOrEmpty(obj.EffectiveDateTo) /*&& obj.OrgGroupID.ToString() != ""*/)
                            {
                                foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                                {
                                    if(item.To.Length > 0)
                                    {
                                        if (item.EmployeeField == "SECONDARY_DESIG" &&
                                            obj.AddRemove == "ADD" &&
                                            Int32.Parse(item.To.Substring(0, item.To.IndexOf(','))) == obj.OrgGroupID &&
                                            Int32.Parse((item.To.Substring(item.To.IndexOf(",") + 1)).Substring(0, item.To.Substring(item.To.IndexOf(",") + 1).IndexOf(","))) == obj.PositionID)

                                        {
                                            if (effectiveDateFrom <= movementlist.First().DateEffectiveFrom)
                                            {
                                                ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; "
                                                        , " (", movementlist.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ")."));
                                            }
                                        }
                                    }
                                }

                                }
                            else
                            {
                                foreach (Data.EmployeeMovement.EmployeeMovement item in movementlist)
                                {
                                    if(item.To.Length > 0) 
                                    { 
                                    if ( obj.AddRemove == "ADD" &&
                                    item.EmployeeField == "SECONDARY_DESIG" &&
                                    //obj.OrgGroupID.ToString() != "" &&
                                    Int32.Parse(item.To.Substring(0, item.To.IndexOf(','))) == obj.OrgGroupID &&
                                    Int32.Parse((item.To.Substring(item.To.IndexOf(",") + 1)).Substring(0, item.To.Substring(item.To.IndexOf(",") + 1).IndexOf(","))) == obj.PositionID)
                                      {
                                        if (item.DateEffectiveTo == null)
                                        {
                                            item.DateEffectiveTo = DateTime.Now;
                                        }
                                        // Validate of Adding Date Range is Over Lapping to Exisiting Movement or not
                                        // --------tStartA----------tEndB               --------tstartB -----------tEndA--------
                                        if (effectiveDateFrom < item.DateEffectiveTo && item.DateEffectiveFrom < effectiveDateTo)
                                        {
                                            //strDateEffectiveTo = 
                                            ErrorMessages.Add(string.Concat(("Movement - Row [" + obj.RowNum + "] : "), "The Movement is overlapping on existing movement(s); "
                                                , " (Effective Date :", item.DateEffectiveFrom.ToString("MM/dd/yyyy"), " - ",
                                                item.DateEffectiveTo?.ToString("MM/dd/yyyy") ?? DateTime.Now.ToString("MM/dd/yyyy"), ")."));
                                        }
                                      }
                                    }
                                }
                            }

                        }

                    }

                }

            }

            if (param.UploadFileOthers != null)
            {
                /*Checking of required and invalid fields*/
                foreach (UploadFileOthers obj in param.UploadFileOthers)
                {

                    /*OldEmployeeID*/
                    if (string.IsNullOrEmpty(obj.OldEmployeeID))
                    {
                        ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.OldEmployeeID = obj.OldEmployeeID.Trim();
                        if (obj.OldEmployeeID.Length > 7)
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Old Employee ID", MessageUtilities.COMPARE_NOT_EXCEED, "7 characters."));
                        }
                        else
                        {
                            var oldEmployeeID = (await _employeeDBAccess.GetByOldEmployeeID(obj.OldEmployeeID)).ToList();

                            if (oldEmployeeID == null)
                            {
                                ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                            }
                            else
                            {
                                if (oldEmployeeID.Count == 0)
                                {
                                    ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Old Employee ID ", MessageUtilities.COMPARE_INVALID));
                                }
                                else
                                {
                                    obj.EmployeeID = oldEmployeeID.First().ID;
                                }
                            }
                        }
                    }

                    //* 
                    
                    // 
                    /*MovementType*/
                    if (string.IsNullOrEmpty(obj.MovementType))
                    {
                        ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.MovementType = obj.MovementType.Trim();
                        if (obj.MovementType.Length > 20)
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Movement Type", MessageUtilities.COMPARE_NOT_EXCEED, "20 characters."));
                        }

                        if (!Regex.IsMatch(obj.MovementType, RegexUtilities.REGEX_CODE))
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.ERRMSG_REGEX_CODE));
                        }

                        if (movementType.Where(x => obj.MovementType.Equals(x)).Count() == 0)
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Movement Type ", MessageUtilities.COMPARE_INVALID));
                        }
                    }

                    /*EffectiveDateFrom*/
                    if (string.IsNullOrEmpty(obj.EffectiveDateFrom))
                    {
                        ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Effective Date From ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        obj.EffectiveDateFrom = obj.EffectiveDateFrom.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateFrom, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Effective Date From", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                    }

                    /*EffectiveDateTo*/
                    if (!string.IsNullOrEmpty(obj.EffectiveDateTo))
                    {
                        obj.EffectiveDateTo = obj.EffectiveDateTo.Trim();
                        if (!DateTime.TryParseExact(obj.EffectiveDateTo, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveDate))
                        {
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Effective Date To", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                    }

                    /*Reason*/
                    if (string.IsNullOrEmpty(obj.Reason))
                    {
                        ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        if (obj.Reason.Length > 1000)
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
                    }
                    /*HRDComments*/
                    if (string.IsNullOrEmpty(obj.HRDComments))
                    {
                        ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Additional Remarks ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    }
                    else
                    {
                        if (obj.HRDComments.Length > 255)
                            ErrorMessages.Add(string.Concat(("Others - Row [" + obj.RowNum + "] : "), "Additional Remarks", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                }

            }


            List<string> Duplicates = new List<string>();

            /*Remove Duplicates*/
            if (ErrorMessages.Count == 0)
            {
                if (param.UploadFileMovement != null) {
                    var tempParam = param.UploadFileMovement.ToList();
                    foreach (var obj in tempParam.ToList())
                    {
                        /* Remove duplicates within file */
                        var duplicateWithinFile = param.UploadFileMovement.Where(x =>
                        obj.OldEmployeeID.Equals(x.OldEmployeeID) &
                        obj.EmployeeField.Equals(x.EmployeeField) &
                        obj.MovementType.Equals(x.MovementType) &
                        obj.NewValue.Equals(x.NewValue) &
                        obj.EffectiveDateFrom.Equals(x.EffectiveDateFrom) &
                        obj.EffectiveDateTo.Equals(x.EffectiveDateTo) &
                        obj.RowNum != x.RowNum).FirstOrDefault();

                        if (duplicateWithinFile != null)
                        {
                            param.UploadFileMovement.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("MovementRow [" + obj.RowNum + "]");
                        }
                    }
                }

                if (param.UploadFileSecondary != null)
                {
                    //TRY1
                    var tempParam = param.UploadFileSecondary.ToList();
                    
                    foreach (var obj in tempParam.ToList())
                    {
                        /* Remove duplicates within file */
                        var duplicateWithinFile = param.UploadFileSecondary.Where(x =>
                        obj.OldEmployeeID.Equals(x.OldEmployeeID) &
                        obj.MovementType.Equals(x.MovementType) &
                        obj.OrgGroupCode.Equals(x.OrgGroupCode) &
                        obj.PositionCode.Equals(x.PositionCode) &
                        obj.EffectiveDateFrom.Equals(x.EffectiveDateFrom) &
                        obj.EffectiveDateTo.Equals(x.EffectiveDateTo) &
                        obj.RowNum != x.RowNum).FirstOrDefault();

                        if (duplicateWithinFile != null)
                        {
                            param.UploadFileSecondary.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        }
                    }
                }

                if (param.UploadFileOthers != null)
                {
                    var tempParam = param.UploadFileOthers.ToList();
                    foreach (var obj in tempParam.ToList())
                    {
                        /* Remove duplicates within file */
                        var duplicateWithinFile = param.UploadFileOthers.Where(x =>
                        obj.OldEmployeeID.Equals(x.OldEmployeeID) &
                        obj.MovementType.Equals(x.MovementType) &
                        obj.Reason.Equals(x.Reason) &
                        obj.HRDComments.Equals(x.HRDComments) &
                        obj.EffectiveDateFrom.Equals(x.EffectiveDateFrom) &
                        obj.EffectiveDateTo.Equals(x.EffectiveDateTo) &
                        obj.RowNum != x.RowNum).FirstOrDefault();

                        if (duplicateWithinFile != null)
                        {
                            param.UploadFileOthers.Remove(tempParam.Where(x => x.RowNum == obj.RowNum).FirstOrDefault());
                            Duplicates.Add("Row [" + obj.RowNum + "]");
                        }
                    }
                }

            }

            /*Validated data*/
            if (ErrorMessages.Count == 0)
            {
                if (param != null)
                {
                    List<Form> forms = new List<Form>();

                    if (param.UploadFileMovement != null)
                    {
                        foreach (var obj in param.UploadFileMovement)
                        {
                            forms.Add(new Form
                            {
                                //For movement checker 1216, 1239, 1260
                                Status = "PENDING",
                                EmployeeID = obj.EmployeeID,
                                EmployeeField = obj.EmployeeField,
                                MovementType = obj.MovementType,
                                NewValue = obj.NewValue,
                                NewValueID = obj.NewValueID,
                                EffectiveDateFrom = obj.EffectiveDateFrom,
                                EffectiveDateTo = obj.EffectiveDateTo,
                                Reason = obj.Reason,
                                HRDComments = obj.HRDComments,
                                CreatedBy = credentials.UserID,
                                FromValue = obj.FromValue,
                                FromValueID = obj.FromValueID,
                            });
                        }
                    }

                    //Try2
                    if (param.UploadFileSecondary != null) {
                        foreach (var obj in param.UploadFileSecondary)
                        {
                            forms.Add(new Form
                            {
                                Status = "PENDING",
                                EmployeeID = obj.EmployeeID,
                                EmployeeField = Enums.MOVEMENT_EMP_FIELD.SECONDARY_DESIG.ToString(),
                                MovementType = obj.MovementType,
                                NewValue = string.Concat(obj.OrgGroupID.ToString(), ",", obj.PositionID,",", obj.AddRemove?.ToLower()),
                                NewValueID = "",
                                EffectiveDateFrom = obj.EffectiveDateFrom,
                                EffectiveDateTo = obj.EffectiveDateTo,
                                Reason = obj.Reason,
                                HRDComments = obj.HRDComments,
                                CreatedBy = credentials.UserID,
                            });
                        }
                    }

                    if (param.UploadFileOthers != null)
                    {
                        foreach (var obj in param.UploadFileOthers)
                        {
                            forms.Add(new Form
                            {
                                Status = "PENDING",
                                EmployeeID = obj.EmployeeID,
                                EmployeeField = Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString(),
                                MovementType = obj.MovementType,
                                NewValue = "",
                                NewValueID = "",
                                EffectiveDateFrom = obj.EffectiveDateFrom,
                                EffectiveDateTo = obj.EffectiveDateTo,
                                Reason = obj.Reason,
                                HRDComments = obj.HRDComments,
                                CreatedBy = credentials.UserID,
                            });
                        }
                    }

                    await _dbAccess.Post(forms);


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
                int count =
                    (param.UploadFileMovement == null ? 0 : param.UploadFileMovement.Count)
                    + (param.UploadFileSecondary == null ? 0 : param.UploadFileSecondary.Count)
                    + (param.UploadFileOthers == null ? 0 : param.UploadFileOthers.Count);

                if (Duplicates.Count > 0)
                {
                    return new OkObjectResult(
                        string.Concat(count, " Record(s) ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD, "<br>",
                            MessageUtilities.ERRMSG_DUPLICATE_EMPLOYEE, "<br>",
                            string.Join("<br>", Duplicates.Distinct().ToArray()))
                        );
                }
                else
                {
                    return new OkObjectResult(string.Concat(count, " Records ", MessageUtilities.SCSSMSG_REC_FILE_UPLOAD));
                }
            }
            else
            //return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
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

        public async Task<IActionResult> GetPrint(APICredentials credentials, GetPrintInput param)
        {
            List<Data.EmployeeMovement.TableVarEmployeeMovementGetPrint> result = (await _dbAccess.GetPrint(param)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
            {
                return new OkObjectResult(
                  new GetPrintOutput
                  {
                      ID = result.Count == 0 ? 0 : result.First().ID,
                      CarNumber = result.Count == 0 ? "" : result.First().CarNumber,
                      IDNumber = result.Count == 0 ? "" : result.First().IDNumber,
                      NewIDNumber = result.Count == 0 ? "" : result.First().NewIDNumber,
                      CompanyName = result.Count == 0 ? "" : result.First().CompanyName,
                      CompanyCode = result.Count == 0 ? "" : result.First().CompanyCode,
                      CompanyPresident = result.Count == 0 ? "" : result.First().CompanyPresident,
                      HRDManager = result.Count == 0 ? "" : result.First().HRDManager,
                      RegionalManager = result.Count == 0 ? "" : result.First().RegionalManager,
                      EmployeeName = result.Count == 0 ? "" : result.First().EmployeeName,
                      DateHired = result.Count == 0 ? "" : result.First().DateHired,
                      OrgGroup = result.Count == 0 ? "" : result.First().OrgGroup,
                      Position = result.Count == 0 ? "" : result.First().Position,
                      EmploymentStatus = result.Count == 0 ? "" : result.First().EmploymentStatus,
                      //DateEffectiveFrom = result.Count == 0 ? "" : result.First().DateEffectiveFrom,
                      //DateEffectiveTo = result.Count == 0 ? "" : result.First().DateEffectiveTo,
                      DateEffective = result.Count == 0 ? "" : string.Join("\n", result.Select(x => x.DateEffective).Distinct().ToArray()),
                      PrintDetailsList = result.Select(x => new PrintDetails {
                        DetailsLabel = x.DetailsLabel,
                        From = x.From,
                        To = x.To
                      }).ToList(),
                      Reason = result.Count == 0 ? "" : string.Join("\n ", result.Select(x => x.Reason).Distinct().ToArray()),
                      HRDComments = result.Count == 0 ? "" : string.Join("\n ", result.Select(x => x.HRDComments).Distinct().ToArray()),
                      MovementType = result.Count == 0 ? "" : string.Join("\n", result.Select(x => x.MovementType).Distinct().ToArray()),
                      DateGenerated = result.Count == 0 ? "" : result.First().DateGenerated,
                      Details = result.Count == 0 ? "" : result.First().Details,
                      SpecialCases = result.Count == 0 ? "" : result.First().SpecialCases,
                  });
            }
        }

        public async Task<IActionResult> GetAutoPopulate(APICredentials credentials, GetAutoPopulateByMovementTypeInput param)
        {
            return new OkObjectResult(
                (await _dbAccess.GetAutoPopulate(param))
                .Select(x => new GetAutoPopulateByMovementTypeOutput
                {
                    Value = x.Value,
                    Description = x.Description
                })
            );
        }
        // For movement checker 1392
        public async Task<IActionResult> GetByID(APICredentials credentials, long ID)
        {
            Data.EmployeeMovement.EmployeeMovement result = await _dbAccess.GetByID(ID);
            var oldValue = (await _dbAccess.GetDescription(result.EmployeeField, result.FromID)).ToList();

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(
                new Form
                {
                    ID = result.ID,
                    Status = result.Status,
                    EmployeeField = result.EmployeeField,
                    MovementType = result.MovementType,
                    //OldValue = oldValue != null ? oldValue.Count > 0 ? oldValue.First().Description : "" : "",
                    OldValue = result.From,
                    OldValueID = result.FromID,
                    //FromValue = oldValue != null ? oldValue.Count > 0 ? oldValue.First().Description : "" : "",
                    NewValue = result.To,
                    NewValueID = result.ToID,
                    HRDComments = result.HRDComments,
                    Details = result.Details,
                    //Bookmark
                    EffectiveDateFrom = result.DateEffectiveFrom.ToString("MM/dd/yyyy"),
                    EffectiveDateTo = result.DateEffectiveTo.HasValue ? 
                    result.DateEffectiveTo.Value.ToString("MM/dd/yyyy") : "",
                    EmployeeID = result.EmployeeID,
                    Reason = result.Reason,
                    CreatedBy = result.CreatedBy,
                    UseCurrent = !result.DateEffectiveTo.HasValue
                });
        }
        // For movement checker 1414-1422
        public async Task<IActionResult> GetByIDs(APICredentials credentials, List<long> ID)
        {
            List<Data.EmployeeMovement.EmployeeMovement> result = await _dbAccess.GetByIDs(ID);

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(result);
        }

        public async Task<IActionResult> SyncMovement(APICredentials credentials)
        {
            List<Data.EmployeeMovement.EmployeeMovement> result = (await _dbAccess.SyncMovement()).ToList();
            

            if (result == null)
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_NOT_EXIST);
            else
                return new OkObjectResult(result);
        }

        public async Task<IActionResult> GetLastModified(SharedUtilities.UNIT_OF_TIME unit, int value)
        {
            var (From, To) = SharedUtilities.GetDateRange(unit, value);
            return new OkObjectResult(await _dbAccess.GetLastModified(From, To));
        }

        public async Task<IActionResult> GetMovementType(APICredentials credentials, string MovementType)
        {
            IEnumerable<EmployeeMovementMapping> result = await _dbAccess.GetMovementType(MovementType);
            return new OkObjectResult(result.Select(x => new GetMovementTypeOutput
            {
                EmployeeField = x.EmployeeFieldCode,
                AllowMultiple = x.AllowMultiple
            }).ToList());
        }

        public async Task<IActionResult> GetEmployeeFieldList(APICredentials credentials, string type)
        {
            IEnumerable<EmployeeMovementMapping> result = await _dbAccess.GetEmployeeFieldList(type);
            return new OkObjectResult(result.Select(x => new GetEmployeeFieldListOutput
            {
                ID = x.ID,
                MovementType = x.MovementType,
                EmployeeField = x.EmployeeField,
                EmployeeFieldCode = x.EmployeeFieldCode,
                OldValue = "",
                NewValue = ""
            }).ToList());
        }

        public async Task<IActionResult> AddMovementEmployeeField(APICredentials credentials, EmployeeFieldForm param)
        {
            if ((await _dbAccess.GetEmployeeMovementField(param)).Count() > 0)
            {
                ErrorMessages.Add(param.EmployeeField + " " + MessageUtilities.SUFF_ERRMSG_REC_EXISTS);
            }

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.AddMovementEmployeeField(new Data.EmployeeMovement.EmployeeMovementMapping
                {

                    MovementType = param.MovementType,
                    EmployeeField = param.EmployeeField,
                    EmployeeFieldCode = param.EmployeeFieldCode,
                    AllowMultiple = true
                });
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> BulkRemove(APICredentials credentials, BulkRemoveForm param)
        {
            if (param.IDs.Count() == 0)
                ErrorMessages.Add(MessageUtilities.ERRMSG_NO_RECORDS);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.BulkRemoveEmployeeField(param.IDs);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.PRE_SCSSMSG_REC_SAVE);
            else
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
        }

        public async Task<IActionResult> Delete(APICredentials credentials, BulkRemoveForm ID)
        {
            List<long> newIDList = new List<long>();

            foreach (var i in ID.IDs) 
            {
                newIDList.Add(i);
            }
                

            if (ID.IDs.Count() == 0)
                ErrorMessages.Add(MessageUtilities.ERRMSG_NO_RECORDS);

            if (ErrorMessages.Count == 0)
            {
                await _dbAccess.Delete(credentials.UserID, newIDList);

                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess)
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            else
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_DELETE);

        }

        public async Task<IActionResult> Update(APICredentials credentials, Form param)
        {
            if (param.EmployeeID <= 0){
                ErrorMessages.Add(string.Concat("Employee ID ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }

            //if (param.NewValue.Count() >= 1) 
            //{
            
            //}

            if (string.IsNullOrEmpty(param.MovementType)){
                ErrorMessages.Add(string.Concat("Movement Type ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
            }
            else{
                param.MovementType = param.MovementType.Trim();
                if (param.MovementType.Length > 50)
                    ErrorMessages.Add(string.Concat("Movement Type", MessageUtilities.COMPARE_NOT_EXCEED, "50 characters."));
            }

            if (param.EmployeeField != null && !param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString())){
                if (!string.IsNullOrEmpty(param.NewValue)) {
                    param.NewValue = param.NewValue.Trim();
                    if (param.NewValue.Length > 255){
                        ErrorMessages.Add(string.Concat("New Value", MessageUtilities.COMPARE_NOT_EXCEED, "255 characters."));
                    }
                }
            }

            if (!string.IsNullOrEmpty(param.Reason)){
                param.Reason = param.Reason.Trim();
                if (param.Reason.Length > 1000)
                    ErrorMessages.Add(string.Concat("Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }

            if (!string.IsNullOrEmpty(param.HRDComments)){
                param.HRDComments = param.HRDComments.Trim();
                if (param.HRDComments.Length > 1000)
                    ErrorMessages.Add(string.Concat("Additional Remarks", MessageUtilities.COMPARE_NOT_EXCEED, "1000 characters."));
            }
            if (!param.IsEdit)
            {
                if (string.IsNullOrEmpty(param.EffectiveDateFrom))
                    ErrorMessages.Add(string.Concat("Effective Date From", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                else
                {
                    param.EffectiveDateFrom = param.EffectiveDateFrom.Trim();
                    if (!DateTime.TryParseExact(param.EffectiveDateFrom, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied))
                    {
                        ErrorMessages.Add(string.Concat("Effective Date From ", MessageUtilities.COMPARE_INVALID_DATE));
                    }
                    else
                    {
                        if (param.EmployeeFieldList != null)
                        {
                            foreach (var item in param.EmployeeFieldList)
                            {

                                if (item.EmployeeField != null)
                                {
                                    List<Data.EmployeeMovement.EmployeeMovement> latest =
                                                        (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(param.EmployeeID, item.EmployeeField)).ToList();

                                    if (latest != null)
                                    {
                                        if (latest.Count > 0)
                                        {
                                            //if (dateApplied < latest.First().DateEffectiveFrom)
                                            //    ErrorMessages.Add(
                                            //        string.Concat("The Movement is invalid as the 'Effective Date From' will be overridden by the currently active movement; ", item.EmployeeField, " (", latest.First().DateEffectiveFrom.ToString("MM/dd/yyyy"), ").")
                                            //        );
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (!param.UseCurrent)
                {
                    if (string.IsNullOrEmpty(param.EffectiveDateTo))
                        ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.SUFF_ERRMSG_INPUT_REQUIRED));
                    else
                    {
                        param.EffectiveDateTo = param.EffectiveDateTo.Trim();
                        if (!DateTime.TryParseExact(param.EffectiveDateTo, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateApplied))
                        {
                            ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.COMPARE_INVALID_DATE));
                        }
                        else
                        {

                            if (
                                    param.EmployeeField != null
                                    && !param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString())
                                    && param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.SECONDARY_DESIG.ToString())
                                )
                            {
                                List<Data.EmployeeMovement.EmployeeMovement> latest =
                                                    (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(param.EmployeeID, param.EmployeeField)).ToList();

                                if (latest != null)
                                {
                                    if (latest.Count > 0)
                                    {
                                        if (dateApplied < latest.First().DateEffectiveTo)
                                            ErrorMessages.Add(string.Concat("Effective Date To ", MessageUtilities.COMPARE_GREATER_EQUAL, "the date effective to of the active movement."));
                                    }
                                }
                            }

                        }
                    }
                }
            }


            if (ErrorMessages.Count == 0)
            {
                if (!String.IsNullOrEmpty(param.EmployeeField) && param.EmployeeField.Equals(Enums.MOVEMENT_EMP_FIELD.OTHERS.ToString()))
                {
                    await _dbAccess.Update(param);
                }
                else
                {
                    //added
                    foreach (var obj in param.EmployeeFieldList.Select(x => x.EmployeeField))
                    {
                        param.NewValue = param.EmployeeFieldList.Select(s => s.NewValue).FirstOrDefault().ToString();
                        param.NewValueID = param.EmployeeFieldList.Select(s => s.NewValueID).FirstOrDefault().ToString();
                        param.FromValue = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().OldValue;
                        param.FromValueID = param.EmployeeFieldList.Where(x => x.EmployeeField == obj).First().OldValueID;
                        param.EmployeeField = param.EmployeeFieldList.Select(s => s.EmployeeField).FirstOrDefault();
                        
                        //param.FromValue = param.EmployeeFieldList.Select(s => s.OldValue).FirstOrDefault().ToString();
                        //param.FromValueID = param.EmployeeFieldList.Select(s => s.NewValueID).FirstOrDefault().ToString();
                        
                        await _dbAccess.Update(param);
                    }
                }
                _resultView.IsSuccess = true;
            }

            if (_resultView.IsSuccess) {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_COMPLETE);
            }  
            else{
                return new BadRequestObjectResult(string.Join("<br>", ErrorMessages.ToArray()));
            }


        }
        //For movement checker, ChangeStatus(), AppliedMovement(), UpdateDateTo(),
        public async Task<IActionResult> ChangeStatus(APICredentials credentials, ChangeStatus param)
        {
            List<EmployeeMovementStatusHistory> employeeMovementStatusHistories = param.ID
                .Select(x => new EmployeeMovementStatusHistory()
                {
                    EmployeeMovementID = x,
                    Status = param.Status,
                    Remarks = param.Remarks,
                    IsActive = true,
                    CreatedBy = credentials.UserID,
                    CreatedDate = DateTime.Now
                }).ToList();

            var Result = (await _dbAccess.PostEmployeeMovementStatusHistory(employeeMovementStatusHistories));

            if (Result)
            {
                var GetEditMovement = (await _dbAccess.GetByIDs(param.ID));

                if (param.Status == "APPROVED")
                {
                    await AppliedMovement(credentials,GetEditMovement);
                }

                var UpdateMovement = GetEditMovement.Select(x=> new EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement() 
                                    { 
                                         ID = x.ID,
                                         EmployeeID = x.EmployeeID,
                                         Status = param.Status,
                                         EmployeeField = x.EmployeeField,
                                         MovementType = x.MovementType,
                                         Reason = x.Reason,
                                         From = x.From,
                                         FromID = x.FromID,
                                         To = x.To,
                                         ToID = x.ToID,
                                         TableSource = x.TableSource,
                                         DateEffectiveFrom = x.DateEffectiveFrom,
                                         DateEffectiveTo = x.DateEffectiveTo,
                                         HRDComments = x.HRDComments,
                                         IsLatest = x.IsLatest,
                                         IsApplied = (param.Status == "APPROVED" ? true : x.IsApplied),
                                         IsActive = x.IsActive,
                                         CreatedBy = x.CreatedBy,
                                         CreatedDate = x.CreatedDate,
                                         ModifiedBy = x.ModifiedBy,
                                         ModifiedDate = x.ModifiedDate
                                     }).ToList();
                await _dbAccess.PutEmployeeMovement(UpdateMovement);
            }
            if (Result)
            {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
            }
        }

        public async Task<bool> AppliedMovement(APICredentials credentials, List<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement> param)
        {
            //GET MOVEMENT FROM PAST UP TO TODAY DATE
            var ApplicableMovementDetails = param.Where(x => x.DateEffectiveFrom <= DateTime.Now
            && (x.DateEffectiveTo.Equals(null) || x.DateEffectiveTo.Equals("") || x.DateEffectiveTo > DateTime.Now)).ToList();
            var GetEmployeeDetails = (await _employeeDBAccess.GetByIDs(ApplicableMovementDetails.Select(x => x.EmployeeID).ToList()));

            List<EMS.Plantilla.Data.Employee.Employee> employees = new List<EMS.Plantilla.Data.Employee.Employee>();
            List<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement> updateLastEmployeeMovements = new List<Data.EmployeeMovement.EmployeeMovement>();
            List<EmployeeRoving> addEmployeeRoving = new List<EmployeeRoving>();

            foreach (var item in ApplicableMovementDetails)
            {
                var GetEmployee = GetEmployeeDetails.Where(x => x.ID.Equals(item.EmployeeID)).FirstOrDefault();

                if (item.EmployeeField == "POSITION")
                    GetEmployee.PositionID = Convert.ToInt32(item.ToID);

                if (item.EmployeeField == "ORG_GROUP")
                {
                    GetEmployee.OrgGroupID = Convert.ToInt32(item.ToID);
                    var OrgCode = (await _orgGroupDBAccess.GetByID(Convert.ToInt32(item.ToID)));
                    var Company = (await _orgGroupDBAccess.GetTagsByOrgGroupID(Convert.ToInt32(item.ToID))).Where(y => y.TagRefCode.Equals("COMPANY_TAG"));

                    if (Company.Count() > 0 && OrgCode.OrgType == "BRN")
                        GetEmployee.CorporateEmail = String.Concat((Company.Select(x => x.TagValue).FirstOrDefault()).ToLower(), OrgCode.Code, "@motortrade.com.ph");
                }

                if (item.EmployeeField == "EMPLOYMENT_STATUS")
                    GetEmployee.EmploymentStatus = item.ToID;

                if (item.EmployeeField == "COMPANY")
                    GetEmployee.CompanyTag = item.ToID;

                if (item.EmployeeField == "CIVIL_STATUS")
                    GetEmployee.CivilStatusCode = item.ToID;

                if (item.EmployeeField == "EXEMPTION_STATUS")
                    GetEmployee.ExemptionStatusCode = item.ToID;

                employees.Add(GetEmployee);

                if (item.EmployeeField != "SECONDARY_DESIG" || item.EmployeeField != "OTHERS")
                {
                    var GetLatestMovement = (await _dbAccess.GetLatestByEmployeeIDAndEmployeeField(item.EmployeeID, item.EmployeeField)).Where(x => x.Status.Equals("APPROVED")).ToList();

                    foreach (var Movement in GetLatestMovement)
                    {
                        Movement.IsLatest = false;
                        Movement.DateEffectiveTo = item.DateEffectiveFrom;
                        Movement.ModifiedBy = credentials.UserID;
                        Movement.ModifiedDate = DateTime.Now;

                        updateLastEmployeeMovements.Add(Movement);
                    }
                }
                
                if (item.EmployeeField == "SECONDARY_DESIG")
                {
                    var InsertEmployeeRoving = new EmployeeRoving()
                    {
                        EmployeeID = item.EmployeeID,
                        OrgGroupID = Int32.Parse(item.To.Substring(0, item.To.IndexOf(','))),
                        PositionID = Int32.Parse((item.To.Substring(item.To.IndexOf(",") + 1)).Substring(0, item.To.Substring(item.To.IndexOf(",") + 1).IndexOf(","))),
                        IsActive = true,
                        CreatedBy = credentials.UserID,
                        CreatedDate = DateTime.Now
                    };

                    addEmployeeRoving.Add(InsertEmployeeRoving);
                }
            }

            if (employees.Count() > 0)
                await _employeeDBAccess.PutEmployees(employees.Distinct().ToList());
            if (updateLastEmployeeMovements.Count() > 0)
                await _dbAccess.PutEmployeeMovement(updateLastEmployeeMovements);
            if (addEmployeeRoving.Count() > 0)
                await _dbAccess.PostEmployeeRoving(addEmployeeRoving);

            return true;
        }

        public async Task<IActionResult> UpdateDateTo(APICredentials credentials, Form param)
        {
            List<EMS.Plantilla.Data.EmployeeMovement.EmployeeMovement> employeeMovements = new List<Data.EmployeeMovement.EmployeeMovement>();
            var GetMovement = (await _dbAccess.GetByID(param.ID));

            if (param.EffectiveDateTo != "" || param.EffectiveDateTo != null)
            {
                var DateEffectiveTo = DateTime.ParseExact(param.EffectiveDateTo ?? DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                GetMovement.DateEffectiveTo = DateEffectiveTo;

                if (GetMovement.EmployeeField == "SECONDARY_DESIG" && DateEffectiveTo <= DateTime.Now)
                {
                    var GetRoving = (await _dbAccess.GetEmployeeRoving(
                        GetMovement.EmployeeID, 
                        Int32.Parse(GetMovement.To.Substring(0, GetMovement.To.IndexOf(','))), 
                        Int32.Parse((GetMovement.To.Substring(GetMovement.To.IndexOf(",") + 1)).Substring(0, GetMovement.To.Substring(GetMovement.To.IndexOf(",") + 1).IndexOf(",")))));

                    if (GetRoving.Count() > 0)
                    {
                        List<EmployeeRoving> addEmployeeRoving = new List<EmployeeRoving>();
                        var InsertEmployeeRoving = GetRoving.Select(x => new EmployeeRoving() {
                            ID = x.ID,
                            EmployeeID = x.EmployeeID,
                            OrgGroupID = x.OrgGroupID,
                            PositionID = x.PositionID,
                            IsActive = false,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            ModifiedBy = credentials.UserID,
                            ModifiedDate = DateTime.Now
                        });

                        addEmployeeRoving.AddRange(InsertEmployeeRoving);
                        await _dbAccess.PutEmployeeRoving(addEmployeeRoving);
                    }
                }
            }

            employeeMovements.Add(GetMovement);

            var Result = await _dbAccess.PutEmployeeMovement(employeeMovements);

            if (Result)
            {
                return new OkObjectResult(MessageUtilities.SCSSMSG_REC_UPDATE);
            }
            else
            {
                return new BadRequestObjectResult(MessageUtilities.ERRMSG_REC_UPDATE);
            }
        }

        public async Task<IActionResult> GetEmployeeMovementByEmployeeIDs(APICredentials credentials, List<int> EmployeeIDs)
        {
            return new OkObjectResult(await _dbAccess.GetEmployeeMovementByEmployeeIDs(EmployeeIDs));
        }
    }
}
