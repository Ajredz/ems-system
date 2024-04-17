using System;
using System.Collections.Generic;
using Utilities.API;

namespace EMS.Plantilla.Transfer.EmployeeMovement
{

    public class Form
    {
        public long ID { get; set; }
        public string Status { get; set; }
        public string EmployeeField { get; set; }
        public string MovementType { get; set; }
        public string Reason { get; set; }
        public int EmployeeID { get; set; }
        public string OldValue { get; set; }
        public string OldValueID { get; set; }
        public string NewValue { get; set; }
        public string NewValueID { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public bool UseCurrent { get; set; }
        public string HRDComments { get; set; }
        public string Details { get; set; }
        public int CreatedBy { get; set; }
        public bool IsEdit { get; set; }
        public string FromValue { get; set; }
        public string FromValueID { get; set; }
        public List<EmployeeMovementMappingForm> EmployeeFieldList { get; set; }
        public string To { get; set; }
        public DateTime? DateEffectiveTo { get; set; }
        public DateTime? DateEffectiveFrom { get; set; }
    }

    public class GetListInput : JQGridFilter
    {
        public string EmployeeName { get; set; }
        public string OldEmployeeID { get; set; }
        public string OrgGroupDelimited { get; set; }
        public string EmployeeFieldDelimited { get; set; }
        public string MovementTypeDelimited { get; set; }
        public string StatusDelimited { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string DateEffectiveFromFrom { get; set; }
        public string DateEffectiveFromTo { get; set; }
        public string DateEffectiveToFrom { get; set; }
        public string DateEffectiveToTo { get; set; }
        public string Reason { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public string CreatedByDelimited { get; set; }
        public string HRDComments { get; set; }
        public bool IsShowActiveOnly { get; set; }
        public bool IsExport { get; set; }
        public bool HasConfidentialView { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetAutoCompleteByMovementTypeInput
    {
        public string Term { get; set; }
        public int TopResults { get; set; }
        public string MovementType { get; set; }
        public string OrgGroup { get; set; }
    }

    
    public class UploadFileMovement
    {
        public string RowNum { get; set; }
        public string EmployeeField { get; set; }
        public string MovementType { get; set; }
        public string OldEmployeeID { get; set; }
        public int EmployeeID { get; set; }
        public string NewValue { get; set; }
        public string NewValueID { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public string Reason { get; set; }
        public string HRDComments { get; set; }
        public int CreatedBy { get; set; }
        public string FromValue { get; set; }
        public string FromValueID{ get; set; }
    }

    public class UploadFile
    {
        public List<UploadFileMovement> UploadFileMovement { get; set; }
        public List<UploadFileSecondary> UploadFileSecondary { get; set; }
        public List<UploadFileOthers> UploadFileOthers { get; set; }
    }

    public class UploadFileSecondary
    {
        public string RowNum { get; set; }
        public string MovementType { get; set; }
        public string OldEmployeeID { get; set; }
        public int EmployeeID { get; set; }
        public string OrgGroupCode { get; set; }
        public int OrgGroupID { get; set; }
        public string PositionCode { get; set; }
        public int PositionID { get; set; }
        public string AddRemove { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public string Reason { get; set; }
        public string HRDComments { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UploadFileOthers
    {
        public string RowNum { get; set; }
        public string MovementType { get; set; }
        public string OldEmployeeID { get; set; }
        public int EmployeeID { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public string Reason { get; set; }
        public string HRDComments { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GetAutoPopulateByMovementTypeInput
    {
        public string MovementType { get; set; }
    }

    public class GetPrintInput
    {
        public string IDDelimited { get; set; }
        public bool HasConfidentialView { get; set; }
    }

    public class EmployeeFieldForm
    {
        public string MovementType { get; set; }

        public string EmployeeField { get; set; }

        public string EmployeeFieldCode { get; set; }
    }

    public class EmployeeMovementMappingForm
    {
        public string EmployeeField { get; set; }
        public string OldValue { get; set; }
        public string OldValueID { get; set; }
        public string NewValue { get; set; }
        public string NewValueID { get; set; }
    }

    public class MovementTypeForm
    {
        public string MovementType { get; set; }
    }

    public class BulkRemoveForm
    {
        public List<int> IDs { get; set; }
    }

}