using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utilities.API;

namespace EMS.Plantilla.Transfer.EmployeeMovement
{

    public class GetListOutput : JQGridResult
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string StatusColor { get; set; }
        public string EmployeeName { get; set; }
        public string OldEmployeeID { get; set; }
        public string OrgGroup { get; set; }
        public string EmployeeField { get; set; }
        public string MovementType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DateEffectiveFrom { get; set; }
        public string DateEffectiveTo { get; set; }
        public string CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Reason { get; set; }
        public string HRDComments { get; set; }
        public string Name { get; set; }
        public string NewEmployeeID { get; set; }
        public string Region { get; set; }
        public string DeptSection { get; set; }
        public string DateHired { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string EmploymentStatus { get; set; }
    }

    public class GetAutoCompleteByMovementTypeOutput
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class GetPrintOutput
    {
        public int ID { get; set; }
        public string CarNumber { get; set; }
        public string IDNumber { get; set; }
        public string NewIDNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyPresident { get; set; }
        public string HRDManager { get; set; }
        public string RegionalManager { get; set; }
        public string EmployeeName { get; set; }
        public string DateHired { get; set; }
        public string OrgGroup { get; set; }
        public string Position { get; set; }
        public string EmploymentStatus { get; set; }
        public string DateGenerated { get; set; }
        public string Reason { get; set; }
        public string HRDComments { get; set; }
        public string MovementType { get; set; }
        public string DateEffective { get; set; }
        public string Details { get; set; }
        public string SpecialCases { get; set; }
        public List<PrintDetails> PrintDetailsList { get; set; }
    }

    public class PrintDetails
    {
        public string DetailsLabel { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    public class GetAutoPopulateByMovementTypeOutput
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class GetEmployeeFieldListOutput : JQGridResult
    {
        public int? ID { get; set; }
        public string MovementType { get; set; }
        public string EmployeeField { get; set; }
        public string EmployeeFieldCode { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public class GetMovementTypeOutput
    {
        public string EmployeeField { get; set; }
        public bool AllowMultiple { get; set; }
    }

    public class TableVarEmployeeMovementByEmployeeIDsOutput
    {
        public long ID { get; set; }
        public int EmployeeID { get; set; }
        public string MovementType { get; set; }
        public string EmployeeField { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DateEffectiveFrom { get; set; }
        public string DateEffectiveTo { get; set; }
    }
}