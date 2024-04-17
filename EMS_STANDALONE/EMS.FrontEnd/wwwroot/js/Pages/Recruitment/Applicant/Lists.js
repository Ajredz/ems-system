var objApplicantListJS;
const ApplicantListURL = "/Recruitment/Applicant?handler=List";
const OrgGroupAutoCompleteURL = "/Recruitment/Applicant?handler=OrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Recruitment/Applicant?handler=PositionAutoComplete";
const GetApplicationSourceURL = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=APPLICATION_SOURCE";
const CurrentStepAutoCompleteURL = "/Recruitment/Applicant?handler=CurrentStepAutoComplete&WorkflowCode=RECRUITMENT";
const GetCityDropDownByRegionURL = "/Recruitment/Applicant?handler=CityDropDownByRegion";
const RegionAutoCompleteURL = "/Recruitment/Applicant?handler=RegionAutoComplete";

const UploadInsertURL = "/Recruitment/Applicant?handler=UploadInsert";
const DownloadFormURL = "/Recruitment/Applicant?handler=DownloadApplicantTemplate";

//const WorkflowAutoCompleteURL = "/Recruitment/Applicant?handler=WorkflowAutoComplete";
const AttachmentTypeDropDown = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=ATTACHMENT_TYPE";
const CourseDropDown = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=COURSE";
//const ReferredByAutoComplete = "/Recruitment/Applicant?handler=ReferredBy";
const DownloadFileURL = "/Recruitment/Applicant?handler=DownloadFile";
const CheckFileIfExistsURL = "/Recruitment/Applicant?handler=CheckFileIfExists";
const OrgGroupOrgTypeAutoCompleteURL = "/Recruitment/Applicant?handler=OrgGroupByOrgTypeAutoComplete";
const GetApplicantStatusURL = "/Recruitment/Applicant?handler=ReferenceValueByCodes&RefCodes=WORKFLOW_STATUS";
const GetCheckApplicantExportListURL = "/Recruitment/Applicant?handler=CheckApplicantExportList";
const DownloadApplicantExportListURL = "/Recruitment/Applicant?handler=DownloadApplicantExportList";
const GetApplicantInfoURL = "/Recruitment/Applicant?handler=ApplicantInfo";
const AssignedUserAutoComplete = "/Recruitment/Applicant?handler=AssignedUser";
const GetMRFIDByMRFTransactionIDURL = "/Recruitment/Applicant?handler=MRFIDByMRFTransactionID";

const GetProvinceDropDownByRegionURL = "/Recruitment/Applicant?handler=ProvinceDropDownByRegion";
const GetCityMunicipalityDropDownByProvinceURL = "/Recruitment/Applicant?handler=CityMunicipalityDropDownByProvince";
const GetBarangayDropDownByCityMunicipalityURL = "/Recruitment/Applicant?handler=BarangayDropDownByCityMunicipality";

const ApplicantAddURL = "/Recruitment/Applicant/Add";
const ApplicantViewURL = "/Recruitment/Applicant/View";
const ApplicantEditURL = "/Recruitment/Applicant/Edit";
const ApplicantDeleteURL = "/Recruitment/Applicant/Delete";
const ApplicantAddPostURL = "/Recruitment/Applicant/Add";
const ApplicantEditPostURL = "/Recruitment/Applicant/Edit";

// Log Activity URLs
const AddPreLoadedActivitiesURL = "/Recruitment/Applicant?handler=AddPreloadedActivities";
const AddLogActivityURL = "/Recruitment/Applicant/AddLogActivity";
const AddLogActivityPostURL = "/Recruitment/Applicant/AddLogActivity";

const UpdateLogActivityURL = "/Recruitment/Applicant/UpdateLogActivity";
const UpdateLogActivityPostURL = "/Recruitment/Applicant/UpdateLogActivity";

const GetLogActivitiesURL = "/Recruitment/Applicant?handler=LogActivities";
const LogActivityByPreloadedIDURL = "/Recruitment/Applicant?handler=LogActivityByPreloadedID";
const GetLogActivityStatusHistoryURL = "/Recruitment/Applicant?handler=LogActivityStatusHistory";
const TitleTypeDropDownURL = "/Recruitment/Applicant/AddLogActivity?handler=TitleTypeDropDown";
const SubTypeDropdownChangeURL = "/Recruitment/Applicant/AddLogActivity?handler=SubType";
const LogActivityPreloadedDropDownURL = "/Recruitment/Applicant?handler=LogActivityPreloadedDropDown";
const GetCommentsURL = "/Recruitment/Applicant/UpdateLogActivity?handler=Comments";
const SaveCommentsURL = "/Recruitment/Applicant/UpdateLogActivity?handler=SaveComments";
const GetAttachmentURL = "/Recruitment/Applicant/UpdateLogActivity?handler=Attachment";
const SaveAttachmentURL = "/Recruitment/Applicant/UpdateLogActivity?handler=SaveAttachment";
var logActivityDeletedAttachments = [];
var LogActivityAttachmentTypeDropDownOptions = [];
const ApplicantUpdateAssignedUserURL = "/Recruitment/Applicant/Edit?handler=UpdateAssignedUser";
// END

// Application History URLs
const MRFIDDropdownURL = "/Recruitment/Applicant?handler=MRFIDDropdown";
const WorkflowTransactionURL = "/Recruitment/Applicant?handler=WorkflowTransaction";
// END

// Reference URLs
const AddReferenceURL = "/Recruitment/Applicant/AddReference";
const ReferencePostURL = "/Recruitment/Applicant/AddReference?refcode=";
const ReferenceEditURL = "/Recruitment/Applicant/EditReference";
const ReferenceEditPostURL = "/Recruitment/Applicant/EditReference?refcode=";
const GetReferenceValueListURL = "/Recruitment/Applicant/AddReference?handler=ReferenceValueList";
// END

// Log Activity List URLs
const ApplicantLogActivityURL = "/Recruitment/Applicant/_LogActivity";
const ApplicantLogActivityGetCurrentStatusURL = "/Recruitment/Applicant?handler=WorkflowReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const ApplicantLogActivityGetTypeURL = "/Recruitment/Applicant?handler=WorkflowReferenceValue&RefCode=ACTIVITY_TYPE";
//

//Legal Profile
const ApplicantLegalProfileURL = "/Recruitment/Applicant/_LegalProfile";
const GetLegalProfileQuestionURL = "/Recruitment/Applicant?handler=ApplicantLegalProfile&RefCode=LEGAL_PROFILE";
//

// MRF URLs
const MRFAddApplicantModalURL = "/Manpower/MRF/Admin/AddApplicant";
const MRFAddApplicantModalSaveWorkflowURL = "/Manpower/MRF/Admin/AddApplicant?handler=SaveWorkflow";
const MRFAddApplicantReferenceValueURL = "/Manpower/MRF/Admin/AddApplicant?handler=ReferenceValue";
const MRFAddApplicantWorkflowStepURL = "/Manpower/MRF/Admin/AddApplicant?handler=WorkflowStep";
const MRFAddApplicantWorkflowTransactionURL = "/Manpower/MRF/Admin/AddApplicant?handler=WorkflowTransaction";
const MRFAddApplicantModalListURL = "/Manpower/MRF/Admin/AddApplicant?handler=List";
const GetApplicantCommentsURL = "/Manpower/MRF/Admin/AddApplicant?handler=MRFApplicantComments";
const SaveApplicantCommentsURL = "/Manpower/MRF/Admin/AddApplicant?handler=SaveComments";

const ApplicantPickerURL = "/Manpower/MRF/Admin/ApplicantPicker";
const ApplicantPickerPostURL = "/Manpower/MRF/Admin/ApplicantPicker";
const MRFAddApplicantModalRemoveApplicantURL = "/Manpower/MRF/Admin/RemoveApplicant";
const MRFUpdateStatusURL = "/Manpower/MRF/Admin/UpdateStatus";
const MRFUpdateForHiringURL = "/Manpower/MRF/Admin/UpdateForHiringApplicant";
const CompanyDropDownURL = "/Manpower/MRF/Admin?handler=CompanyDropDown";
const GetWorkflowDropDownURL = "/Manpower/MRF/Admin?handler=WorkflowDropdown";
const RecruitmentApplicantPickerListURL = "/Manpower/MRF/Admin?handler=ApplicantPickerList";
const GetWorkflowStepDropDownURL = "/Manpower/MRF/Admin?handler=WorkflowStepDropdown";
const ConvertApplicantURL = "/Manpower/MRF/Admin/UpdateStatus?handler=ConvertApplicant";
//

/*Add Applicant URLs*/
const RecruitmentOrgGroupAutoCompleteURL = "/Recruitment/Applicant?handler=OrgTypeAutoComplete";
const RecruitmentPositionAutoCompleteURL = "/Recruitment/Applicant?handler=PositionAutoComplete";
const RecruitmentGetApplicationSourceURL = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=APPLICATION_SOURCE";
const RecruitmentGetCourseURL = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=COURSE";
const RecruitmentCurrentStepAutoCompleteURL = "/Recruitment/Applicant?handler=CurrentStepAutoComplete&WorkflowCode=RECRUITMENT";
const RecruitmentReferredByAutoComplete = "/Recruitment/Applicant?handler=ReferredBy";
const RecruitmentApplicantViewURL = "/Recruitment/Applicant/View";
//

// Employee URLs
const EmployeeAddURL = "/Plantilla/Employee/Add";
const EmployeeAddPostURL = "/Plantilla/Employee/Add";
const EmployeeEditURL = "/Plantilla/Employee/Edit";
const EmployeeEditPostURL = "/Plantilla/Employee/Edit";
const EmployeeViewURL = "/Plantilla/Employee/View";
const OrgGroupDropDownURL = "/Plantilla/Employee?handler=OrgGroupDropDown";
const PositionDropDownURL = "/Plantilla/Employee?handler=PositionDropDown";
const SystemUserAutoCompleteURL = "/Plantilla/Employee?handler=UserNameAutoComplete";
const ReferredByAutoComplete = "/Plantilla/Employee?handler=ReferredBy";
const AddSystemUserURL = "/Plantilla/Employee/Add?handler=SystemUser";
//const GetCityDropDownByRegionURL = "/Plantilla/Employee?handler=CityDropDownByRegion";
const RelationshipDropDownURL = "/Plantilla/Employee?handler=RelationshipDropDown";
const GetFamilyBackgroundURL = "/Plantilla/Employee?handler=FamilyByEmployeeID";
const GetWorkingHistoryURL = "/Plantilla/Employee?handler=WorkingHistoryByEmployeeID";
const GetEmployeeRovingURL = "/Plantilla/Employee/View?handler=EmployeeRoving";
const GetSystemUserNameURL = "/Plantilla/Employee/View?handler=SystemUserName";
const GetEmploymentStatusURL = "/Plantilla/Employee?handler=EmploymentStatusByEmployeeID";
const GetRegionByOrgGroupIDURL = "/Plantilla/Employee?handler=RegionByOrgGroupID";
const GetJobClassByPositionIDURL = "/Plantilla/Employee?handler=JobClassByPositionID";
const EmployeeUpdateAssignedUserURL = "/Plantilla/Employee/Edit?handler=UpdateAssignedUser";
// Education 
const SchoolLevelDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_SCHOOL_LEVEL";
const EducationalAttainmentDegreeDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_ED_ATT_DEG";
const EducationalAttainmentStatusDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_ED_ATT_STAT";
const GetEducationURL = "/Plantilla/Employee?handler=EducationByEmployeeID";


var objEmployeeListJS;
// END

// Employee Log Activity URLs
const EmployeeAddPreLoadedActivitiesURL = "/Plantilla/Employee?handler=AddPreloadedActivities";
const EmployeeAddLogActivityURL = "/Plantilla/Employee/AddLogActivity";
const EmployeeAddLogActivityPostURL = "/Plantilla/Employee/AddLogActivity";

const EmployeeUpdateLogActivityURL = "/Plantilla/Employee/UpdateLogActivity";
const EmployeeUpdateLogActivityPostURL = "/Plantilla/Employee/UpdateLogActivity";

const EmployeeGetLogActivitiesURL = "/Plantilla/Employee?handler=LogActivities";
const EmployeeLogActivityByPreloadedIDURL = "/Plantilla/Employee?handler=LogActivityByPreloadedID";
const EmployeeGetLogActivityStatusHistoryURL = "/Plantilla/Employee?handler=LogActivityStatusHistory";
const EmployeeTitleTypeDropDownURL = "/Plantilla/Employee/AddLogActivity?handler=TitleTypeDropDown";
const EmployeeSubTypeDropdownChangeURL = "/Plantilla/Employee/AddLogActivity?handler=SubType";
const EmployeeLogActivityPreloadedDropDownURL = "/Plantilla/Employee?handler=LogActivityPreloadedDropDown";
const EmployeeGetCommentsURL = "/Plantilla/Employee/UpdateLogActivity?handler=Comments";
const EmployeeSaveCommentsURL = "/Plantilla/Employee/UpdateLogActivity?handler=SaveComments";
const EmployeeGetAttachmentURL = "/Plantilla/Employee/UpdateLogActivity?handler=Attachment";
const EmployeeSaveAttachmentURL = "/Plantilla/Employee/UpdateLogActivity?handler=SaveAttachment";
var EmployeelogActivityDeletedAttachments = [];
var EmployeeLogActivityAttachmentTypeDropDownOptions = [];
const EmployeeLogActivityURL = "/Plantilla/Employee/_LogActivity";
const EmployeeLogActivityGetCurrentStatusURL = "/Plantilla/Employee?handler=WorkflowReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const EmployeeLogActivityGetTypeURL = "/Plantilla/Employee?handler=WorkflowReferenceValue&RefCode=ACTIVITY_TYPE";
// END

// Employee Accountability URLs
const AddPreLoadedAccountabilityURL = "/Plantilla/Employee?handler=AddPreLoadedAccountability";
const AddAccountabilityURL = "/Plantilla/Employee/AddAccountability";
const AddAccountabilityPostURL = "/Plantilla/Employee/AddAccountability";

const UpdateAccountabilityURL = "/Plantilla/Employee/UpdateAccountability";
const UpdateAccountabilityPostURL = "/Plantilla/Employee/UpdateAccountability";

const GetAccountabilityURL = "/Plantilla/Employee?handler=Accountability";
const AccountabilityDetailsByIDURL = "/Plantilla/Employee?handler=AccountabilityDetailsByID";
const AccountabilityDropDownURL = "/Plantilla/Employee?handler=AccountabilityDropDown";

const GetAccountabilityStatusHistoryURL = "/Plantilla/Employee?handler=AccountabilityStatusHistory";
const GetAccountabilityCommentsURL = "/Plantilla/Employee/UpdateAccountability?handler=Comments";
const SaveAccountabilityCommentsURL = "/Plantilla/Employee/UpdateAccountability?handler=SaveComments";
const GetAccountabilityAttachmentURL = "/Plantilla/Employee/UpdateAccountability?handler=Attachment";
const SaveAccountabilityAttachmentURL = "/Plantilla/Employee/UpdateAccountability?handler=SaveAttachment";
var AccountabilityDeletedAttachments = [];
var AccountabilityAttachmentTypeDropDownOptions = [];
// END

// Onboarding Workflow URLs
const OnboardingWorkflowTransactionURL = "/Plantilla/Employee?handler=OnboardingWorkflowTransaction";
const SaveOnboardingWorkflowURL = "/Plantilla/Employee/Edit?handler=SaveOnboardingWorkflow";
const GetOnboardingWorkflowStepDropDownURL = "/Plantilla/Employee?handler=WorkflowStepDropdown";
const ReferenceValueURL = "/Plantilla/Employee/Edit?handler=ReferenceValue";
const OnboardingWorkflowStepURL = "/Plantilla/Employee/Edit?handler=WorkflowStep";
// END

var deletedAttachments = [];

// Tab FirstLoad tags, set to True if tab is clicked/loaded
var personalInfoTabFirstLoad = false;
var attachmentTabFirstLoad = false;
var appHistoryTabFirstLoad = false;
var logActivityTabFirstLoad = false;

$(document).ready(function () {
    objApplicantListJS = {

        Initialize: function () {
            $("#divAssignedUserBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            
            s.ElementBinding();
            var param = {
                ID: localStorage["ApplicantListID"],
                ApplicantLastName: localStorage["ApplicantListApplicantLastName"],
                ApplicantFirstName: localStorage["ApplicantListApplicantFirstName"],
                ApplicantMiddleName: localStorage["ApplicantListApplicantMiddleName"],
                ApplicantSuffix: localStorage["ApplicantListApplicantSuffix"],
                ApplicationSourceDelimited: localStorage["ApplicantListApplicationSourceDelimited"],
                MRFTransactionID: localStorage["ApplicantListMRFTransactionID"],
                CurrentStepDelimited: localStorage["ApplicantListCurrentStepDelimited"],
                DateScheduledFrom: localStorage["ApplicantListDateScheduledFrom"],
                DateScheduledTo: localStorage["ApplicantListDateScheduledTo"],
                DateCompletedFrom: localStorage["ApplicantListDateCompletedFrom"],
                DateCompletedTo: localStorage["ApplicantListDateCompletedTo"],
                ApproverRemarks: localStorage["ApplicantListApproverRemarks"],
                PositionRemarks: localStorage["ApplicantListPositionRemarks"],
                Course: localStorage["ApplicantListCourse"],
                CurrentPositionTitle: localStorage["ApplicantListCurrentPositionTitle"],
                ExpectedSalaryFrom: localStorage["ApplicantListExpectedSalaryFrom"],
                ExpectedSalaryTo: localStorage["ApplicantListExpectedSalaryTo"],
                DateAppliedFrom: localStorage["ApplicantListDateAppliedFrom"],
                DateAppliedTo: localStorage["ApplicantListDateAppliedTo"],
                ScopeOrgType: $("#hdnOrgListFilter").val(),
                ScopeOrgGroupDelimited: localStorage["ApplicantListScopeOrgGroupDelimited"],
                WorkflowStatusDelimited: localStorage["ApplicantListWorkflowStatusDelimited"]
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

        },

        ElementBinding: function () {
            var s = this;

            $("#txtFilterDateAppliedFrom, #txtFilterDateAppliedTo").datetimepicker({
                    useCurrent: false,
                    format: 'MM/DD/YYYY'
            });

            $("#txtFilterDateScheduledFrom, #txtFilterDateScheduledTo, \
                #txtFilterDateCompletedFrom, #txtFilterDateCompletedTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY',
            });

            NumberOnly($("#txtFilterApplicantID"));
            NumberOnly($("#txtFilterExpectedSalaryFrom"));
            NumberOnly($("#txtFilterExpectedSalaryTo"));

            $("#btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterApplicantID").val(),
                    LastName: $("#txtFilterApplicantLastName").val(),
                    FirstName: $("#txtFilterApplicantFirstName").val(),
                    MiddleName: $("#txtFilterApplicantMiddleName").val(),
                    Suffix: $("#txtFilterApplicantSuffix").val(),
                    ApplicationSourceDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value,
                    MRFTransactionID: $("#txtFilterMRFTransactionID").val(),
                    CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value,
                    DateScheduledFrom: $("#txtFilterDateScheduledFrom").val(),
                    DateScheduledTo: $("#txtFilterDateScheduledTo").val(),
                    DateCompletedFrom: $("#txtFilterDateCompletedFrom").val(),
                    DateCompletedTo: $("#txtFilterDateCompletedTo").val(),
                    ApproverRemarks: $("#txtFilterApproverRemarks").val(),
                    PositionRemarks: $("#txtFilterPositionRemarks").val(),
                    //PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value,
                    Course: $("#txtFilterCourse").val(),
                    CurrentPositionTitle: $("#txtFilterCurrentPositionTitle").val(),
                    ExpectedSalaryFrom: $("#txtFilterExpectedSalaryFrom").val(),
                    ExpectedSalaryTo: $("#txtFilterExpectedSalaryTo").val(),
                    DateAppliedFrom: $("#txtFilterDateAppliedFrom").val(),
                    DateAppliedTo: $("#txtFilterDateAppliedTo").val(),
                    ScopeOrgType: $("#hdnOrgListFilter").val(),
                    ScopeOrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value,
                    //WorkflowStatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflowStatus").value

                };
                s.SetLocalStorage();
                ResetJQGridState("tblRecruitmentApplicantList");
                s.LoadJQGrid(param);
            });

            $("#btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedCurrentStep").html("");
                $("#multiSelectedApplicationSource").html("");
                $("#multiSelectedApplicationSourceOption label, #multiSelectedApplicationSourceOption input").prop("title", "add");
                //$("#multiSelectedCurrentStep").html("");
                //$("#multiSelectedWorkflow").html("");
                //$("#multiSelectedDesiredOrgGroup").html("");
                //$("#multiSelectedDesiredPosition").html("");
                $("#multiSelectedScopeOrgGroup").html("");
                //$("#multiSelectedWorkflowStatus").html("");
                //$("#multiSelectedWorkflowStatusOption label, #multiSelectedWorkflowStatusOption input").prop("title", "add");
                $("#btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(ApplicantAddURL, "divApplicantBodyModal");
                $("#divApplicantModal").modal("show");
            });

            $("#btnUploadInsert").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertURL, "Upload (Insert)", DownloadFormURL);
                $('#divModalErrorMessage').html('');
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedApplicationSource", GetApplicationSourceURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCurrentStep"
                , CurrentStepAutoCompleteURL, 20, "multiSelectedCurrentStep");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCurrentStep"
            //    , CurrentStepAutoCompleteURL, 20, "multiSelectedCurrentStep");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterWorkflow"
            //    , WorkflowAutoCompleteURL, 20, "multiSelectedWorkflow");
            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredOrgGroup"
            //    , OrgGroupAutoCompleteURL, 20, "multiSelectedDesiredOrgGroup");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterDesiredPosition"
            //    , PositionAutoCompleteURL, 20, "multiSelectedDesiredPosition");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterScopeOrgGroup"
            //    , OrgGroupOrgTypeAutoCompleteURL + "&OrgType=" + $("#hdnOrgListFilter").val(), 20, "multiSelectedScopeOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterScopeOrgGroup"
                , RegionAutoCompleteURL, 20, "multiSelectedScopeOrgGroup");

            //objEMSCommonJS.BindFilterMultiSelectEnumValueDisplay("multiSelectedWorkflowStatus", GetApplicantStatusURL, "Value", "Description");

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objApplicantListJS.ExportFunction()",
                    "function");
            });
        },

        ExportFunction: function () {
            var parameters = "&sidx=" + $("#tblRecruitmentApplicantList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblRecruitmentApplicantList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterApplicantID").val()
                + "&LastName=" + $("#txtFilterApplicantLastName").val()
                + "&FirstName=" + $("#txtFilterApplicantFirstName").val()
                + "&MiddleName=" + $("#txtFilterApplicantMiddleName").val()
                + "&Suffix=" + $("#txtFilterApplicantSuffix").val()
                + "&ApplicationSourceDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value
                + "&MRFTransactionID=" + $("#txtFilterMRFTransactionID").val()
                + "&CurrentStep=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value
                + "&DateScheduledFrom=" + $("#txtFilterDateScheduledFrom").val()
                + "&DateScheduledTo=" + $("#txtFilterDateScheduledTo").val()
                + "&DateCompletedFrom=" + $("#txtFilterDateCompletedFrom").val()
                + "&DateCompletedTo=" + $("#txtFilterDateCompletedTo").val()
                + "&ApproverRemarks=" + $("#txtFilterApproverRemarks").val()
                //+ "&CurrentStepDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value
                //+ "&OrgGroupRemarks=" + $("#txtFilterOrgGroupRemarks").val()
                //+ "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value
                + "&PositionRemarks=" + $("#txtFilterPositionRemarks").val()
                //+ "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value
                + "&Course=" + $("#txtFilterCourse").val()
                + "&CurrentPositionTitle=" + $("#txtFilterCurrentPositionTitle").val()
                + "&ExpectedSalaryFrom=" + $("#txtFilterExpectedSalaryFrom").val()
                + "&ExpectedSalaryTo=" + $("#txtFilterExpectedSalaryTo").val()
                + "&DateAppliedFrom=" + $("#txtFilterDateAppliedFrom").val()
                + "&DateAppliedTo=" + $("#txtFilterDateAppliedTo").val()
                + "&ScopeOrgType=" + $("#hdnOrgListFilter").val()
                + "&ScopeOrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value
                //+ "&WorkflowStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflowStatus").value
                ;

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadApplicantExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckApplicantExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblRecruitmentApplicantList") == "" ? "" : $.parseJSON(localStorage.getItem("tblRecruitmentApplicantList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divApplicantList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divApplicantList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divApplicantList #filterFieldsContainer");
                });

                $("#divApplicantList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divApplicantList div.filterFields").unbind("keyup");
                $("#divApplicantList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblRecruitmentApplicantList").jqGrid("GridUnload");
            $("#tblRecruitmentApplicantList").jqGrid("GridDestroy");
            $("#tblRecruitmentApplicantList").jqGrid({
                url: ApplicantListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "Date Applied", "Applicant Name", "Application Source", "MRF ID", "Applicant Status", "Filter By Region", "Last Name", "First Name", "Middle Name", "Suffix", 
                     
                    "Date Scheduled", "Date Completed", "Approver Remarks",
                    /*"Workflow Status", "Current Step",*/
                    "Desired Position (Remarks)", "Course", "Current Position Title", "Expected Salary"
                ],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { name: "", index: "View", width: 130, align: "center", sortable: true, formatter: objApplicantListJS.AddLink },
                    { name: "DateApplied", index: "DateApplied", align: "left", sortable: true },
                    { name: "ApplicantName", index: "ApplicantName", align: "left", sortable: true },
                    { name: "ApplicationSource", index: "ApplicationSource", align: "left", sortable: true },
                    { name: "MRFTransactionID", index: "MRFTransactionID", align: "left", sortable: true },
                    { name: "CurrentStep", index: "CurrentStep", align: "left", sortable: true },
                    { name: "ScopeOrgGroup", index: "ScopeOrgGroup", editable: true, align: "left", sortable: false },
                    { name: "LastName", index: "LastName", align: "left", sortable: true },
                    { name: "FirstName", index: "FirstName", align: "left", sortable: true },
                    { name: "MiddleName", index: "MiddleName", align: "left", sortable: true },
                    { name: "Suffix", index: "Suffix", align: "left", sortable: true },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: true },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: true },
                    { name: "ApproverRemarks", index: "ApproverRemarks", align: "left", sortable: true },
                    //{ name: "WorkflowStatus", index: "WorkflowStatus", align: "center", sortable: true },
                    //{ name: "CurrentStep", index: "CurrentStep", align: "center", sortable: true },
                    //{ name: "WorkflowDescription", index: "WorkflowDescription", align: "left", sortable: true },
                    //{ name: "OrgGroupRemarks", index: "OrgGroupRemarks", align: "left", sortable: true },
                    //{ name: "DesiredOrgGroup", index: "DesiredOrgGroup", align: "left", sortable: true },
                    { name: "PositionRemarks", index: "PositionRemarks", align: "left", sortable: true },
                    //{ name: "DesiredPosition", index: "DesiredPosition", align: "left", sortable: true },
                    { name: "Course", index: "Course", align: "left", sortable: true },
                    { name: "CurrentPositionTitle", index: "CurrentPositionTitle", align: "left", sortable: true },
                    { name: "ExpectedSalary", index: "ExpectedSalary", align: "right", sortable: true, formatter: objApplicantListJS.FormatAmount },
                ],
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblRecruitmentApplicantList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {

                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblRecruitmentApplicantList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divApplicantList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divApplicantList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#divApplicantList #tblRecruitmentApplicantList .jqgrid-id-link").click(function () {
                            $('#divApplicantModal').modal('show');
                        });

                    }

                    if (localStorage["ApplicantListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["ApplicantListFilterOption"]));
                    }
                    objApplicantListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objApplicantListJS.ShowHideFilter();
                        localStorage["ApplicantListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divApplicantList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    $("table tr.jqgfirstrow td").each(function (index) {
                        var i = index + 1;
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='search']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("input[type='number']").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") input").val() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                        if ($("table .jqgfirstrow td:nth-child(" + i + ")").find("div.multiselect-control").length) {
                            if ($("table .jqgfirstrow td:nth-child(" + i + ") div.multiselect-control").text() != "") {
                                $(".ui-jqgrid-htable tr th:nth-child(" + i + ")").addClass("filtered");
                                if ($(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").find('span.glyphicon-table-header').length == 0) {
                                    $(".ui-jqgrid-htable tr th:nth-child(" + i + ").filtered div").prepend("<span class=\"glyphicon glyphicon-filter glyphicon-table-header\"></span>")
                                }
                            }
                        }
                    });
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblRecruitmentApplicantList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            jQuery(".ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
        },

        SetLocalStorage: function () {
            localStorage["ApplicantListID"] = $("#txtFilterApplicantID").val();
            localStorage["ApplicantListApplicantLastName"] = $("#txtFilterApplicantLastName").val();
            localStorage["ApplicantListApplicantFirstName"] = $("#txtFilterApplicantFirstName").val();
            localStorage["ApplicantListApplicantMiddleName"] = $("#txtFilterApplicantMiddleName").val();
            localStorage["ApplicantListApplicantSuffix"] = $("#txtFilterApplicantSuffix").val();

            localStorage["ApplicantListApplicationSourceDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").value;
            localStorage["ApplicantListApplicationSourceDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedApplicationSource").text;
            localStorage["ApplicantListMRFTransactionID"] = $("#txtFilterMRFTransactionID").val();

            localStorage["ApplicantListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value;
            localStorage["ApplicantListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").text;

            localStorage["ApplicantListScheduledFrom"] = $("#txtFilterScheduledFrom").val();
            localStorage["ApplicantListScheduledTo"] = $("#txtFilterScheduledTo").val();
            localStorage["ApplicantListCompletedFrom"] = $("#txtFilterCompletedFrom").val();
            localStorage["ApplicantListCompletedTo"] = $("#txtFilterCompletedTo").val();
            localStorage["ApplicantListApproverRemarks"] = $("#txtFilterApproverRemarks").val();


            //localStorage["ApplicantListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value;
            //localStorage["ApplicantListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").text;

            //localStorage["ApplicantListWorkflowDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflow").value;
            //localStorage["ApplicantListWorkflowDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflow").text;

            //localStorage["ApplicantListOrgGroupRemarks"] = $("#txtFilterOrgGroupRemarks").val();
            //localStorage["ApplicantListDesiredOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").value;
            //localStorage["ApplicantListDesiredOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredOrgGroup").text;

            localStorage["ApplicantListPositionRemarks"] = $("#txtFilterPositionRemarks").val();
            localStorage["ApplicantListDesiredPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").value;
            localStorage["ApplicantListDesiredPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedDesiredPosition").text;
            
            localStorage["ApplicantListCourse"] = $("#txtFilterCourse").val();
            localStorage["ApplicantListCurrentPositionTitle"] = $("#txtFilterCurrentPositionTitle").val();
            localStorage["ApplicantListExpectedSalaryFrom"] = $("#txtFilterSalaryFrom").val();
            localStorage["ApplicantListExpectedSalaryTo"] = $("#txtFilterSalaryTo").val();
            localStorage["ApplicantListDateAppliedFrom"] = $("#txtFilterDateAppliedFrom").val();
            localStorage["ApplicantListDateAppliedTo"] = $("#txtFilterDateAppliedTo").val();

            localStorage["ApplicantListScopeOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").value;
            localStorage["ApplicantListScopeOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedScopeOrgGroup").text;

            //localStorage["ApplicantListWorkflowStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflowStatus").value;
            //localStorage["ApplicantListWorkflowStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedWorkflowStatus").text;
        },

        GetLocalStorage: function () {
            $("#txtFilterApplicantID").val(localStorage["ApplicantListID"]);
            $("#txtFilterApplicantLastName").val(localStorage["ApplicantListApplicantLastName"]);
            $("#txtFilterApplicantFirstName").val(localStorage["ApplicantListApplicantFirstName"]);
            $("#txtFilterApplicantMiddleName").val(localStorage["ApplicantListApplicantMiddleName"]);
            $("#txtFilterApplicantSuffix").val(localStorage["ApplicantListApplicantSuffix"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedApplicationSource"
                , "ApplicantListApplicationSourceDelimited"
                , "ApplicantListApplicationSourceDelimitedText");
            $("#txtFilterMRFTransactionID").val(localStorage["ApplicantListMRFTransactionID"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
                , "ApplicantListCurrentStepDelimited"
                , "ApplicantListCurrentStepDelimitedText");

            $("#txtFilterScheduledFrom").val(localStorage["ApplicantListScheduledFrom"]);
            $("#txtFilterScheduledTo").val(localStorage["ApplicantListScheduledTo"]);
            $("#txtFilterCompletedFrom").val(localStorage["ApplicantListCompletedFrom"]);
            $("#txtFilterCompletedTo").val(localStorage["ApplicantListCompletedTo"]);
            $("#txtFilterApproverRemarks").val(localStorage["ApplicantListApproverRemarks"]);

            //objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
            //    , "ApplicantListCurrentStepDelimited"
            //    , "ApplicantListCurrentStepDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("multiSelectedWorkflow"
            //    , "ApplicantListWorkflowDelimited"
            //    , "ApplicantListWorkflowDelimitedText");

            //$("#txtFilterOrgGroupRemarks").val(localStorage["ApplicantListOrgGroupRemarks"]);
            //objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredOrgGroup"
            //    , "ApplicantListDesiredOrgGroupDelimited"
            //    , "ApplicantListDesiredOrgGroupDelimitedText");

            $("#txtFilterPositionRemarks").val(localStorage["ApplicantListPositionRemarks"]);
            objEMSCommonJS.SetMultiSelectList("multiSelectedDesiredPosition"
                , "ApplicantListDesiredPositionDelimited"
                , "ApplicantListDesiredPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedScopeOrgGroup"
                , "ApplicantListScopeOrgGroupDelimited"
                , "ApplicantListScopeOrgGroupDelimitedText");

            //objEMSCommonJS.SetMultiSelectList("multiSelectedWorkflowStatus"
            //    , "ApplicantListWorkflowStatusDelimited"
            //    , "ApplicantListWorkflowStatusDelimitedText");

            $("#txtFilterCourse").val(localStorage["ApplicantListCourse"]);
            $("#txtFilterCurrentPositionTitle").val(localStorage["ApplicantListCurrentPositionTitle"]);
            $("#txtFilterSalaryFrom").val(localStorage["ApplicantListExpectedSalaryFrom"]);
            $("#txtFilterSalaryTo").val(localStorage["ApplicantListExpectedSalaryTo"]);
            $("#txtFilterDateAppliedFrom").val(localStorage["ApplicantListDateAppliedFrom"]);
            $("#txtFilterDateAppliedTo").val(localStorage["ApplicantListDateAppliedTo"]);            
        },

        RemoveDynamicFields: function (id, deleteAttachmentFunction) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
            if (deleteAttachmentFunction != null)
                deleteAttachmentFunction();
        },

        AddLink: function (cellvalue, options, rowObject) {

            return rowObject.WorkflowStatus == "Completed" & rowObject.EmployeeID == 0
                ? "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>"
                    + " | " + "<a class=\"btn-link\" id=\"btnEncode\" onclick=\"objApplicantListJS.EncodeEmployee('" + rowObject.ID + "')\">Encode 201</a>"
                : "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";

            // return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">" + rowObject.ID + "</a>";
        },

        UpdateLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='activity-id-link' onclick=\"return LoadPartial('" + ApplicantViewURL + "?ID=" + rowObject.ID + "', 'divApplicantBodyModal');\">Update</a>";
        },

        FormatAmount: function (cellvalue, options, rowObject) {

            return AddZeroes(rowObject.ExpectedSalary+"").withComma();
        },

        EncodeEmployee: function (ID) {
            $("#divEmployeeModal").modal("show");
            var isSuccessFunction = function () {
                var GetSuccessFunction = function (data) {
                    $("#divEmployeeBodyModal #txtLastName").val(data.Result.PersonalInformation.LastName);
                    $("#divEmployeeBodyModal #txtFirstName").val(data.Result.PersonalInformation.FirstName);
                    $("#divEmployeeBodyModal #txtMiddleName").val(data.Result.PersonalInformation.MiddleName);
                    //$("#divEmployeeBodyModal #ddlOrgGroup").val(data.Result.OrgGroupID);
                    //$("#divEmployeeBodyModal #ddlPosition").val(data.Result.PositionID);
                    $('label[for="txtSystemUserName"]').hide();
                    $("#divEmployeeBodyModal #txtSystemUserName").hide();
                    $("#divEmployeeBodyModal #btnSave").click(function () {
                        objEmployeeAddJS.AddSuccessFunction = function () {
                            $("#divEmployeeModal").modal("hide");
                            objEmployeeAddJS.AddSystemUser
                            (
                                data.Result.PersonalInformation.FirstName,
                                data.Result.PersonalInformation.MiddleName,
                                data.Result.PersonalInformation.LastName,
                                data.Result.ID
                            );
                            $("#divApplicantFilter #btnSearch").click();
                        };
                    });
                };

                objEMSCommonJS.GetAjax(GetApplicantInfoURL + "&ID=" + ID, {}, "", GetSuccessFunction);
            };
            LoadPartialSuccessFunction(EmployeeAddURL, "divEmployeeBodyModal", isSuccessFunction);
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

    };

    objApplicantListJS.Initialize();

    objEmployeeListJS = {

        OnboardingWorkflowTransactionJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblWorkflowTransactionList").jqGrid("GridUnload");
            $("#tblWorkflowTransactionList").jqGrid("GridDestroy");
            $("#tblWorkflowTransactionList").jqGrid({
                url: OnboardingWorkflowTransactionURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "Order", "Step", "Status", "Date Scheduled", "Date Completed",
                    "Timestamp", "Remarks"],
                colModel: [
                    { name: "", hidden: true },
                    { name: "Order", index: "Type", align: "left", sortable: false, hidden: true },
                    { name: "Step", index: "Title", align: "left", sortable: false },
                    { name: "Status", index: "Status", align: "left", sortable: false },
                    { name: "DateScheduled", index: "DateScheduled", align: "left", sortable: false },
                    { name: "DateCompleted", index: "DateCompleted", align: "left", sortable: false },
                    { name: "Timestamp", index: "Timestamp", align: "left", sortable: false },
                    { name: "Remarks", index: "Remarks", align: "left", sortable: false }
                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblWorkflowTransactionList", data);

                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        LoadEmploymentStatusJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblEmploymentStatusList").jqGrid("GridUnload");
            $("#tblEmploymentStatusList").jqGrid("GridDestroy");
            $("#tblEmploymentStatusList").jqGrid({
                url: GetEmploymentStatusURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["Status", "Date Effective"],
                colModel: [
                    { name: "EmploymentStatus", index: "EmploymentStatus", align: "left", sortable: false },
                    { name: "DateEffective", index: "DateEffective", align: "left", sortable: false },
                ],
                rowList: SetRowList(),
                loadonce: false,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblEmploymentStatusList", data);
                        $("#tblEmploymentStatusList_subgrid").width(20);
                        $(".jqgfirstrow td:first").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },
    };
});