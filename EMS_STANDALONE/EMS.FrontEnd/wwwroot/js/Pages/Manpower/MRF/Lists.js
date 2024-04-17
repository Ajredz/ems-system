var objMRFListJS;
const MRFListURL = window.location.pathname + "?handler=List";
const PositionLevelAutoCompleteURL = window.location.pathname + "?handler=PositionLevelAutoComplete";
const OrgGroupAutoCompleteURL = window.location.pathname + "?handler=OrgTypeAutoComplete";
const OrgGroupOrgTypeAutoCompleteURL = window.location.pathname + "?handler=OrgGroupByOrgTypeAutoComplete";
const PositionAutoCompleteURL = window.location.pathname + "?handler=PositionAutoComplete";
const GetNatureOfEmploymentURL = window.location.pathname + "?handler=ReferenceValue&RefCode=NATURE_OF_EMPLOYMENT";
const GetStatusURL = window.location.pathname + "?handler=ReferenceValue&RefCode=MRF_STATUS";
const GetApplicantStatusURL = window.location.pathname + "?handler=ReferenceValueByCodes&RefCodes=WORKFLOW_STATUS";
const GetPositionLevelDropDownByOrgGroupIDURL = window.location.pathname + "/View?handler=PositionLevelDropDownByOrgGroupID";
const GetPositionDropDownURL = window.location.pathname + "/View?handler=PositionDropDown";
const GetPositionDropDownWithCountURL = window.location.pathname + "/Add?handler=PositionDropDownWithCount";
const GetOrgGroupRollupPositionDropdownURL = window.location.pathname + "/Add?handler=OrgGroupRollupPositionDropdown";
const GetWorkflowDropDownURL = window.location.pathname + "?handler=WorkflowDropdown";
const GetWorkflowStepDropDownURL = window.location.pathname + "?handler=WorkflowStepDropdown";
//const GetSignatoriesURL = window.location.pathname + "/View?handler=Signatories";
const GetApprovalHistoryURL = window.location.pathname + "/View?handler=MRFApprovalHistory";
const GetRegionByIDURL = window.location.pathname + "/View?handler=RegionByID";
const ValidateExistingActualURL = window.location.pathname + "?handler=ValidateExistingActual";

const MRFAddURL = window.location.pathname + "/Add";
const MRFViewURL = window.location.pathname + "/View";
const MRFAddApplicantModalURL = window.location.pathname + "/AddApplicant";
const MRFAddApplicantModalSaveWorkflowURL = window.location.pathname + "/AddApplicant?handler=SaveWorkflow";
const MRFAddApplicantReferenceValueURL = window.location.pathname + "/AddApplicant?handler=ReferenceValue";
const MRFAddApplicantWorkflowStepURL = window.location.pathname + "/AddApplicant?handler=WorkflowStep";
const MRFAddApplicantWorkflowTransactionURL = window.location.pathname + "/AddApplicant?handler=WorkflowTransaction";
const MRFAddApplicantModalListURL = window.location.pathname + "/AddApplicant?handler=List";
const ApplicantPickerURL = window.location.pathname + "/ApplicantPicker";
const MRFAddApplicantModalRemoveApplicantURL = window.location.pathname + "/RemoveApplicant";
const MRFUpdateStatusURL = window.location.pathname + "/UpdateStatus";
const EmployeeIfExistURL = window.location.pathname + "/UpdateStatus?handler=EmployeeIfExist";
const MRFUpdateForHiringURL = window.location.pathname + "/UpdateForHiringApplicant";
const ApplicantPickerPostURL = window.location.pathname + "/ApplicantPicker";
const MRFEditURL = window.location.pathname + "/Edit";
const MRFDeleteURL = window.location.pathname + "/Delete";
const MRFCancelURL = window.location.pathname + "/Cancel";
const MRFAddPostURL = window.location.pathname + "/Add?handler=Submit";
const MRFEditPostURL = window.location.pathname + "/Edit?handler=Submit";
const GetCheckMRFExportListURL = window.location.pathname + "?handler=CheckMRFExportList";
const DownloadMRFExportListURL = window.location.pathname + "?handler=DownloadMRFExportList";
const HRCancelMRFRequestURL = window.location.pathname + "/CancelMRF";

const RecruitmentApplicantPickerListURL = window.location.pathname + "?handler=ApplicantPickerList";

const GetMRFCommentsURL = window.location.pathname + "/View?handler=MRFComments";
const SaveMRFCommentsURL = window.location.pathname + "/View?handler=SaveComments";
const GetApplicantCommentsURL = window.location.pathname + "/AddApplicant?handler=MRFApplicantComments";
const SaveApplicantCommentsURL = window.location.pathname + "/AddApplicant?handler=SaveComments";
const GetApplicantInfoURL = window.location.pathname + "?handler=ApplicantInfo";
const AddSystemUserURL = window.location.pathname + "?handler=SystemUser";
const ConvertApplicantURL = window.location.pathname + "/UpdateStatus?handler=ConvertApplicant";
const CompanyDropDownURL = window.location.pathname + "?handler=CompanyDropDown";

/*Add Applicant URLs*/
const RecruitmentOrgGroupAutoCompleteURL = "/Recruitment/Applicant?handler=OrgTypeAutoComplete";
const RecruitmentPositionAutoCompleteURL = "/Recruitment/Applicant?handler=PositionAutoComplete";
const RecruitmentGetApplicationSourceURL = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=APPLICATION_SOURCE";
const RecruitmentGetCourseURL = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=COURSE";
const RecruitmentCurrentStepAutoCompleteURL = "/Recruitment/Applicant?handler=CurrentStepAutoComplete&WorkflowCode=RECRUITMENT";
//const RecruitmentWorkflowAutoCompleteURL = "/Recruitment/Applicant?handler=WorkflowAutoComplete";
const RecruitmentReferredByAutoComplete = "/Recruitment/Applicant?handler=ReferredBy";
const RecruitmentApplicantViewURL = "/Recruitment/Applicant/View";
const CheckFileIfExistsURL = "/Recruitment/Applicant?handler=CheckFileIfExists";
const DownloadFileURL = "/Recruitment/Applicant?handler=DownloadFile";
const ApplicantEditURL = "/Recruitment/Applicant/Edit";
const AttachmentTypeDropDown = "/Recruitment/Applicant?handler=ReferenceValue&RefCode=ATTACHMENT_TYPE";
const ApplicantViewURL = "/Recruitment/Applicant/View";
const ApplicantEditPostURL = "/Recruitment/Applicant/Edit";
const AssignedUserAutoComplete = "/Recruitment/Applicant?handler=AssignedUser";
const ApplicantLogActivityURL = "/Recruitment/Applicant/_LogActivity";
const ApplicantLogActivityGetCurrentStatusURL = "/Recruitment/Applicant?handler=WorkflowReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const ApplicantLogActivityGetTypeURL = "/Recruitment/Applicant?handler=WorkflowReferenceValue&RefCode=ACTIVITY_TYPE";
const ApplicantUpdateAssignedUserURL = "/Recruitment/Applicant/Edit?handler=UpdateAssignedUser";
const GetMRFIDByMRFTransactionIDURL = "/Recruitment/Applicant?handler=MRFIDByMRFTransactionID";
const AddReferenceURL = "/Recruitment/Applicant/AddReference";
const ReferencePostURL = "/Recruitment/Applicant/AddReference?refcode=";
const ReferenceEditURL = "/Recruitment/Applicant/EditReference";
const ReferenceEditPostURL = "/Recruitment/Applicant/EditReference?refcode=";
const GetReferenceValueListURL = "/Recruitment/Applicant/AddReference?handler=ReferenceValueList";

//Address Dropdowns
const GetProvinceDropDownByRegionURL = "/Recruitment/Applicant?handler=ProvinceDropDownByRegion";
const GetCityMunicipalityDropDownByProvinceURL = "/Recruitment/Applicant?handler=CityMunicipalityDropDownByProvince";
const GetBarangayDropDownByCityMunicipalityURL = "/Recruitment/Applicant?handler=BarangayDropDownByCityMunicipality";

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
// END

// Application History URLs
const MRFIDDropdownURL = "/Recruitment/Applicant?handler=MRFIDDropdown";
const WorkflowTransactionURL = "/Recruitment/Applicant?handler=WorkflowTransaction";
// END

//Legal Profile
const ApplicantLegalProfileURL = "/Recruitment/Applicant/_LegalProfile";
const GetLegalProfileQuestionURL = "/Recruitment/Applicant?handler=ApplicantLegalProfile&RefCode=LEGAL_PROFILE";
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
const GetCityDropDownByRegionURL = "/Plantilla/Employee?handler=CityDropDownByRegion";
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

const GetApplicantByMRFIDAndIDURL = window.location.pathname + "/AddApplicant?handler=ApplicantByMRFIDAndID";
const RegionAutoCompleteURL = window.location.pathname + "?handler=RegionAutoComplete";

// Main Attachment Tab
const EmployeeGetMainAttachmentURL = "/Plantilla/Employee?handler=MainAttachment";
const EmployeeSaveMainAttachmentURL = "/Plantilla/Employee?handler=SaveMainAttachment";
var EmployeeDeletedMainAttachments = [];
// END

const SendEmailURL = "/Manpower/MRF/SendEmail";

const GetOnlineJobPositionURL = "/Manpower/MRF?handler=OnlineJobPosition";

// Employee Skills
const EmployeeSkillsURL = "/Plantilla/Employee/_Skills";
const EmployeeGetSkillsListURL = "/Plantilla/Employee?handler=SkillsList";
const EmployeeAddSkillsURL = "/Plantilla/Employee/AddSkills";
const EmployeeUpdateSkillsURL = "/Plantilla/Employee/UpdateSkills";
const AddSkillsPostURL = "/Plantilla/Employee/AddSkills";
const AddAddSkillsReferenceURL = "/Plantilla/Employee/AddSkillsReference";
const GetSkillsReferenceValueListURL = "/Plantilla/Employee/AddSkillsReference?handler=ReferenceValueList";
const SkillsReferencePostURL = "/Plantilla/Employee/AddSkillsReference?refcode=";
const GetSkillsRereferenceNewURL = "/Plantilla/Employee/AddSkills?handler=ReferenceValue&RefCode=SKILLS";
const SkillsReferenceEditURL = "/Plantilla/Employee/EditSkillsReference";
const UpdateEmployeeSkillsURL = "/Plantilla/Employee/UpdateSkills?handler=UpdateSkills";
const SkillsReferenceEditPostURL = "/Plantilla/Employee/EditSkillsReference?refcode=";

const NewEmployeeDetailsURL = "/Manpower/MRF/Admin/EditNewEmployeeDetails";

const GetMRFKickoutQuestionURL = "/Manpower/MRF?handler=MRFKickoutQuestionByMRFID";
const GetKickoutQuestionAutoCompleteURL = "/Manpower/MRF?handler=KickoutQuestionAutoComplete";
const GetKickoutQuestionByIDURL = "/Manpower/MRF?handler=KickoutQuestionByID";
const GetMRFKickoutQuestionByIDURL = "/Manpower/MRF?handler=MRFKickoutQuestionByID";
const PostAddKickoutQuestionToMRFURL = "/Manpower/MRF?handler=AddKickoutQuestionToMRF";
const PostEditKickoutQuestionToMRFURL = "/Manpower/MRF?handler=EditKickoutQuestionToMRF";
const PostRemoveKickoutQuestionToMRFURL = "/Manpower/MRF?handler=RemoveKickoutQuestionToMRF";

const GetGetOrgGroupTypeURL = window.location.pathname + "/Add?handler=OrgGroupType";

const PostCloseInternalMRFURL = "/Manpower/MRF?handler=CloseInternalMRF";

var workflowStepDropdownOptions = [];
var workflowStepResultDropdownOptions = [];

$(document).ready(function () {
    objMRFListJS = {

        Initialize: function () {
            $("#divLogActivityAddPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divAccountabilityAddPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divKickoutQuestionBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divAssignedUserModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            $("#txtFilterDateCreatedFrom, #txtFilterDateCreatedTo, "
                + "#txtFilterDateApprovedFrom, #txtFilterDateApprovedTo, #txtFilterDateHiredFrom, #txtFilterDateHiredTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
                });

            //setTimeout(function () { $("#btnAdd").click(); }, 500);


        s.ElementBinding();
            var param = {
                ID: localStorage["ManpowerMRFListID"],
                MRFTransactionID: localStorage["ManpowerMRFListMRFTransactionID"],
                OrgGroupDelimited: localStorage["ManpowerMRFListOrgGroupDelimited"],
                ScopeOrgType: $("#hdnOrgListFilter").val(),
                ScopeOrgGroupDelimited: localStorage["ManpowerMRFListScopeOrgGroupDelimited"],
                PositionLevelDelimited: localStorage["ManpowerMRFListPositionLevelDelimited"],
                PositionDelimited: localStorage["ManpowerMRFListPositionDelimited"],
                NatureOfEmploymentDelimited: localStorage["ManpowerMRFListNatureOfEmploymentDelimited"],
                NoOfApplicantMin: localStorage["ManpowerMRFListNoOfApplicantMin"],
                NoOfApplicantMax: localStorage["ManpowerMRFListNoOfApplicantMax"],
                StatusDelimited: localStorage["DashboardFilterStatus"] || localStorage["ManpowerMRFListStatusDelimited"],
                DateCreatedFrom: localStorage["ManpowerMRFListDateCreatedFrom"],
                DateCreatedTo: localStorage["ManpowerMRFListDateCreatedTo"],
                DateApprovedFrom: localStorage["DashboardFilterDateApprovedFrom"] || localStorage["ManpowerMRFListDateApprovedFrom"],
                DateApprovedTo: localStorage["DashboardFilterDateApprovedTo"] || localStorage["ManpowerMRFListDateApprovedTo"],
                DateHiredFrom: localStorage["ManpowerMRFListDateHiredFrom"],
                DateHiredTo: localStorage["ManpowerMRFListDateHiredTo"],
                AgeMin: localStorage["DashboardFilterAgeMin"] || localStorage["ManpowerMRFListAgeMin"],
                AgeMax: localStorage["DashboardFilterAgeMax"] || localStorage["ManpowerMRFListAgeMax"],
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();


        },

        ElementBinding: function () {
            var s = this;

            $("#divMRFList #btnSearch").click(function () {
                var param = {
                    ID: $("#divMRFList #txtFilterMRFID").val(),
                    MRFTransactionID: $("#divMRFList #txtFilterMRFTransactionID").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedOrgGroup").value,
                    ScopeOrgType: $("#hdnOrgListFilter").val(),
                    ScopeOrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedScopeOrgGroup").value,
                    PositionLevelDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPositionLevel").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPosition").value,
                    NatureOfEmploymentDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedNatureOfEmployment").value,
                    NoOfApplicantMin: $("#divMRFList #txtFilterNoOfApplicantMin").val(),
                    NoOfApplicantMax: $("#divMRFList #txtFilterNoOfApplicantMax").val(),
                    StatusDelimited: objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedStatus").value,
                    DateCreatedFrom: $("#divMRFList #txtFilterDateCreatedFrom").val(),
                    DateCreatedTo: $("#divMRFList #txtFilterDateCreatedTo").val(),
                    DateApprovedFrom: $("#divMRFList #txtFilterDateApprovedFrom").val(),
                    DateApprovedTo: $("#divMRFList #txtFilterDateApprovedTo").val(),
                    DateHiredFrom: $("#divMRFList #txtFilterDateHiredFrom").val(),
                    DateHiredTo: $("#divMRFList #txtFilterDateHiredTo").val(),
                    AgeMin: $("#divMRFList #txtFilterAgeMin").val(),
                    AgeMax: $("#divMRFList #txtFilterAgeMax").val(),

                };
                s.SetLocalStorage();
                ResetJQGridState("#tblManpowerMRFList");
                s.LoadJQGrid(param);
            });

            $("#divMRFList #btnReset").click(function () {
                $("#divMRFList div.filterFields input[type='search']").val("");
                $("#divMRFList div.filterFields select").val("");
                $("#divMRFList div.filterFields input[type='checkbox']").prop("checked", true);
                $("#divMRFList #multiSelectedOrgGroup").html("");
                $("#divMRFList #multiSelectedScopeOrgGroup").html("");
                $("#divMRFList #multiSelectedPositionLevel").html("");
                $("#divMRFList #multiSelectedPosition").html("");
                $("#divMRFList #multiSelectedNatureOfEmployment").html("");
                $("#divMRFList #multiSelectedNatureOfEmploymentOption label, #divMRFList #multiSelectedNatureOfEmploymentOption input").prop("title", "add");
                $("#divMRFList #multiSelectedStatus").html("");
                $("#divMRFList #multiSelectedStatusOption label, #divMRFList #multiSelectedStatusOption input").prop("title", "add");
                $("#divMRFList #btnSearch").click();
            });

            $("#divMRFList #btnAdd").click(function () {
                LoadPartial(MRFAddURL, "divMRFBodyModal");
                $("#divMRFModal").modal("show");
            });

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFList #txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "divMRFList #multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFList #txtFilterScopeOrgGroup"
                , OrgGroupOrgTypeAutoCompleteURL + "&OrgType=" + $("#hdnOrgListFilter").val(), 20, "divMRFList #multiSelectedScopeOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFList #txtFilterPositionLevel"
                , PositionLevelAutoCompleteURL, 20, "divMRFList #multiSelectedPositionLevel");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("divMRFList #txtFilterPosition"
                , PositionAutoCompleteURL, 20, "divMRFList #multiSelectedPosition");

            objEMSCommonJS.BindFilterMultiSelectEnum("divMRFList #multiSelectedNatureOfEmployment", GetNatureOfEmploymentURL);

            objEMSCommonJS.BindFilterMultiSelectEnum("divMRFList #multiSelectedStatus", GetStatusURL);

            $("#divMRFList #btnExport").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objMRFListJS.ExportFunction()",
                    "function");
            });

        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblManpowerMRFList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblManpowerMRFList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#divMRFList #txtFilterMRFID").val()
                + "&MRFTransactionID=" + $("#divMRFList #txtFilterMRFTransactionID").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedOrgGroup").value
                + "&ScopeOrgType=" + $("#hdnOrgListFilter").val()
                + "&ScopeOrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedScopeOrgGroup").value
                + "&PositionLevelDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPositionLevel").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPosition").value
                + "&NatureOfEmploymentDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedNatureOfEmployment").value
                + "&NoOfApplicantMin=" + $("#divMRFList #txtFilterNoOfApplicantMin").val()
                + "&NoOfApplicantMax=" + $("#divMRFList #txtFilterNoOfApplicantMax").val()
                + "&StatusDelimited=" + objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedStatus").value
                + "&DateCreatedFrom=" + $("#divMRFList #txtFilterDateCreatedFrom").val()
                + "&DateCreatedTo=" + $("#divMRFList #txtFilterDateCreatedTo").val()
                + "&DateApprovedFrom=" + $("#divMRFList #txtFilterDateApprovedFrom").val()
                + "&DateApprovedTo=" + $("#divMRFList #txtFilterDateApprovedTo").val()
                + "&DateHiredFrom=" + $("#divMRFList #txtFilterDateHiredFrom").val()
                + "&DateHiredTo=" + $("#divMRFList #txtFilterDateHiredTo").val()
                + "&AgeMin=" + $("#divMRFList #txtFilterAgeMin").val()
                + "&AgeMax=" + $("#divMRFList #txtFilterAgeMax").val()
                + "&IsAdmin=" + (window.location.pathname.indexOf("admin") >= 0);

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadMRFExportListURL + parameters;
                    $("#divModal").modal("hide");

                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckMRFExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblManpowerMRFList") == "" ? "" : $.parseJSON(localStorage.getItem("tblManpowerMRFList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divMRFList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divMRFList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divMRFList #filterFieldsContainer");
                });

                $("#divMRFList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divMRFList div.filterFields").unbind("keyup");
                $("#divMRFList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divMRFList #btnSearch").click();
                    }
                });
            };
            moveFilterFields();
            $("#tblManpowerMRFList").jqGrid("GridUnload");
            $("#tblManpowerMRFList").jqGrid("GridDestroy");
            $("#tblManpowerMRFList").jqGrid({
                url: MRFListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Filter By Region", "MRF ID", "", "Organizational Group", "Position Level", "Position", "Nature of Employment", "Purpose", "No. of Applicant", "Status", "Date Created", "Date Approved", "Date Hired", "Age"],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true/*, formatter: objMRFListJS.AddLink*/ },
                    { hidden: true, name: "ScopeOrgGroup", index: "ScopeOrgGroup", editable: true, align: "left", sortable: false },
                    { name: "MRFID", index: "MRFID", align: "center", sortable: true, formatter: objMRFListJS.AddLink },
                    { name: "", index: "IsApproved", width: 40, align: "center", sortable: true, formatter: objMRFListJS.AddApplicantLink },
                    { name: "OrgGroupDescription", index: "OrgGroupDescription", editable: true, align: "left", sortable: true },
                    { name: "PositionLevelDescription", index: "PositionLevelDescription", editable: true, align: "left", sortable: true },
                    { name: "PositionDescription", index: "PositionDescription", editable: true, align: "left", sortable: true },
                    { name: "NatureOfEmployment", index: "NatureOfEmployment", editable: true, align: "left", sortable: true },
                    { name: "Purpose", index: "Purpose", editable: true, align: "left", sortable: true },
                    { name: "NoOfApplicant", index: "NoOfApplicant", editable: true, align: "center", sortable: true },
                    { name: "Status", index: "Status", editable: true, align: "left", sortable: true },
                    { name: "CreatedDate", index: "CreatedDate", editable: true, align: "left", sortable: true },
                    { name: "ApprovedDate", index: "ApprovedDate", editable: true, align: "left", sortable: true },
                    { name: "HiredDate", index: "HiredDate", editable: true, align: "left", sortable: true },
                    { name: "Age", index: "Age", editable: true, align: "center", sortable: true },
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
                    $("#tblManpowerMRFList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {
                    Loading(false);
                    //WIP
                    //setTimeout(function () { $("#tblManpowerMRFList .glyphicon-user:first").click(); }, 100);
                    //WIP

                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        // Show Filter By Region Column if user is non admin
                        if (window.location.pathname.indexOf("admin") >= 0) {
                            $("#tblManpowerMRFList").jqGrid('showCol', ["ScopeOrgGroup"]);
                        }
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {

                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblManpowerMRFList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divMRFList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divMRFList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#tblManpowerMRFList .jqgrid-id-link").click(function () {
                            $('#divMRFModal').modal('show');
                        });
                        $("#tblManpowerMRFList .jqgrid-id-link-applicant").click(function () {
                            $('#divMRFAddApplicantModal').modal('show');
                        });

                    }

                    if (localStorage["MRFListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["MRFListFilterOption"]));
                    }
                    objMRFListJS.ShowHideFilter();

                    $("#chkFilter").on('change', function () {
                        objMRFListJS.ShowHideFilter();
                        localStorage["MRFListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    if (localStorage["ApplicantSelectedMRFID"] != "") {
                        $("#jqgrid-applicant-" + localStorage["ApplicantSelectedMRFID"]).click();
                    }

                    localStorage["ApplicantSelectedMRFID"] = "";

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divMRFList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

                    ///* FOR DEVELOPMENT ONLY */
                    //$(".glyphicon-user:first").click();
                    ///* FOR DEVELOPMENT ONLY */

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
                    GetJQGridState("tblManpowerMRFList");
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
            localStorage["ManpowerMRFListID"] = $("#divMRFList #txtFilterMRFID").val();
            localStorage["ManpowerMRFListMRFTransactionID"] = $("#divMRFList #txtFilterMRFTransactionID").val();

            localStorage["ManpowerMRFListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedOrgGroup").value;
            localStorage["ManpowerMRFListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedOrgGroup").text;

            localStorage["ManpowerMRFListScopeOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedScopeOrgGroup").value;
            localStorage["ManpowerMRFListScopeOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedScopeOrgGroup").text;

            localStorage["ManpowerMRFListPositionLevelDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPositionLevel").value;
            localStorage["ManpowerMRFListPositionLevelDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPositionLevel").text;

            localStorage["ManpowerMRFListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPosition").value;
            localStorage["ManpowerMRFListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedPosition").text;

            localStorage["ManpowerMRFListNatureOfEmploymentDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedNatureOfEmployment").value;
            localStorage["ManpowerMRFListNatureOfEmploymentDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedNatureOfEmployment").text;

            localStorage["ManpowerMRFListNoOfApplicantMin"] = $("#divMRFList #txtFilterNoOfApplicantMin").val();
            localStorage["ManpowerMRFListNoOfApplicantMax"] = $("#divMRFList #txtFilterNoOfApplicantMax").val();

            localStorage["ManpowerMRFListStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedStatus").value;
            localStorage["ManpowerMRFListStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("divMRFList #multiSelectedStatus").text;

            localStorage["ManpowerMRFListDateCreatedFrom"] = $("#divMRFList #txtFilterDateCreatedFrom").val();
            localStorage["ManpowerMRFListDateCreatedTo"] = $("#divMRFList #txtFilterDateCreatedTo").val();
            localStorage["ManpowerMRFListDateApprovedFrom"] = $("#divMRFList #txtFilterDateApprovedFrom").val();
            localStorage["ManpowerMRFListDateApprovedTo"] = $("#divMRFList #txtFilterDateApprovedTo").val();
            localStorage["ManpowerMRFListDateHiredFrom"] = $("#divMRFList #txtFilterDateHiredFrom").val();
            localStorage["ManpowerMRFListDateHiredTo"] = $("#divMRFList #txtFilterDateHiredTo").val();
            localStorage["ManpowerMRFListAgeMin"] = $("#divMRFList #txtFilterAgeMin").val();
            localStorage["ManpowerMRFListAgeMax"] = $("#divMRFList #txtFilterAgeMax").val();
        },

        GetLocalStorage: function () {
            $("#divMRFList #txtFilterID").val(localStorage["ManpowerMRFListID"]);
            $("#divMRFList #txtFilterMRFTransactionID").val(localStorage["ManpowerMRFListMRFTransactionID"]);
            
            objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedOrgGroup"
                , "ManpowerMRFListOrgGroupDelimited"
                , "ManpowerMRFListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedScopeOrgGroup"
                , "ManpowerMRFListScopeOrgGroupDelimited"
                , "ManpowerMRFListScopeOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedPositionLevel"
                , "ManpowerMRFListPositionLevelDelimited"
                , "ManpowerMRFListPositionLevelDelimitedText");

            objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedPosition"
                , "ManpowerMRFListPositionDelimited"
                , "ManpowerMRFListPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedNatureOfEmployment"
                , "ManpowerMRFListNatureOfEmploymentDelimited"
                , "ManpowerMRFListNatureOfEmploymentDelimitedText");

            $("#divMRFList #txtFilterNoOfApplicantMin").val(localStorage["ManpowerMRFListNoOfApplicantMin"]);
            $("#divMRFList #txtFilterNoOfApplicantMax").val(localStorage["ManpowerMRFListNoOfApplicantMax"]);

            if (localStorage["DashboardFilterStatus"] != "") 
            {
                objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedStatus"
                  , "DashboardFilterStatus"
                  , "DashboardFilterStatusText");
            }
            else
            {
                objEMSCommonJS.SetMultiSelectList("divMRFList #multiSelectedStatus"
                  , "ManpowerMRFListStatusDelimited"
                  , "ManpowerMRFListStatusDelimitedText");
            }

            $("#divMRFList #txtFilterDateCreatedFrom").val(localStorage["ManpowerMRFListDateCreatedFrom"]);
            $("#divMRFList #txtFilterDateCreatedTo").val(localStorage["ManpowerMRFListDateCreatedTo"]);
            $("#divMRFList #txtFilterDateApprovedFrom").val(localStorage["DashboardFilterDateApprovedFrom"] || localStorage["ManpowerMRFListDateApprovedFrom"]);
            $("#divMRFList #txtFilterDateApprovedTo").val(localStorage["DashboardFilterDateApprovedTo"] || localStorage["ManpowerMRFListDateApprovedTo"]);
            $("#divMRFList #txtFilterDateHiredFrom").val(localStorage["ManpowerMRFListDateHiredFrom"]);
            $("#divMRFList #txtFilterDateHiredTo").val(localStorage["ManpowerMRFListDateHiredTo"]);
            $("#divMRFList #txtFilterAgeMin").val(localStorage["DashboardFilterAgeMin"] || localStorage["ManpowerMRFListAgeMin"]);
            $("#divMRFList #txtFilterAgeMax").val(localStorage["DashboardFilterAgeMax"] || localStorage["ManpowerMRFListAgeMax"]);

            localStorage["DashboardFilterDateApprovedFrom"] = "";
            localStorage["DashboardFilterDateApprovedTo"] = "";
            localStorage["DashboardFilterAgeMin"] = "";
            localStorage["DashboardFilterAgeMax"] = "";
            localStorage["DashboardFilterStatus"] = "";
            localStorage["DashboardFilterStatusText"] = "";
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + MRFViewURL + "?ID=" + rowObject.ID + "', 'divMRFBodyModal');\">" + rowObject.MRFID + "</a>";
        },

        AddApplicantLink: function (cellvalue, options, rowObject) {
            return rowObject.IsApproved ? "<a href=\"\" class='jqgrid-id-link-applicant' id='" + "jqgrid-applicant-" + rowObject.ID + "' onclick=\"return LoadPartial('" +
                MRFAddApplicantModalURL + "?ID=" + rowObject.ID + 
                "', 'divMRFAddApplicantBodyModal');\"><span class=\"btn-glyph-dynamic\"><span class=\"glyphicon glyphicon-user\"></span></span></a>" : "";
        /*<span class=\"btn-sub-glyph glyphicon glyphicon-plus\"></span>*/
        },

        LoadApprovalHistoryJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridUnload");
            $("#tblMRFFormApprovalHistoryList").jqGrid("GridDestroy");
            $("#tblMRFFormApprovalHistoryList").jqGrid({
                url: GetApprovalHistoryURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10,
                datatype: "json",
                mtype: "GET",
                colNames: ["Level", "Approver Name", "Status", "Remarks", "Timestamp", "", "", ""],
                colModel: [
                    { name: "HierarchyLevel", index: "HierarchyLevel", align: "center", sortable: false },
                    { name: "ApproverName", index: "ApproverName", align: "left", sortable: false },
                    { name: "ApprovalStatus", index: "ApprovalStatus", align: "left", sortable: false },
                    { name: "ApprovalRemarks", index: "ApprovalRemarks", align: "left", sortable: false },
                    { name: "ApprovedDate", index: "ApprovedDate", align: "left", sortable: false },
                    { hidden: true, name: "PositionID", index: "PositionID", align: "left", sortable: false },
                    { hidden: true, name: "OrgGroupID", index: "OrgGroupID", align: "left", sortable: false },
                    { hidden: true, name: "ApprovalStatusCode", index: "ApprovalStatusCode", align: "left", sortable: false },
                ],
                //toppager: $("#divMRFFormApprovalHistoryPager"),   
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
                                //if (data.rows[i].ApprovalStatusCode == "FOR_APPROVAL"
                                //) {
                                //    $("#hdnApproverPositionID").val(data.rows[i].PositionID);
                                //    $("#hdnApproverOrgGroupID").val(data.rows[i].OrgGroupID);
                                //}

                                //if (// Revised MRF
                                //    data.rows[i].ApprovalStatusCode == "REJECTED" &
                                //    $("#hdnStatusCode").val() == "REJECTED") {
                                //    $("#hdnApproverPositionID").val(data.rows[i].PositionID);
                                //    $("#hdnApproverOrgGroupID").val(data.rows[i].OrgGroupID);

                                //    return false;
                                //}
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblMRFFormApprovalHistoryList", data);

                        if ($("#hdnStatusCode").val() == "HR_CANCELLED") {
                            $("#divMRFButtons").hide();

                            $("tr.jqgrow td[aria-describedby='tblMRFFormApprovalHistoryList_ApprovalStatus']").css({ "text-decoration": "line-through" });

                            $("#tblMRFFormApprovalHistoryList").jqGrid("addRowData", 0, {
                                HierarchyLevel: "N/A",
                                ApproverName: "HR",
                                ApprovalStatus: $("#lblMRFStatus").text(),
                                ApprovalRemarks: $("#txtReasonForCancellation").val(),
                                ApprovedDate: $("#hdnDateModified").val()
                            });

                        }
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        GetComment: function () {
            var input = {
                MRFID: $("#hdnID").val()
            };

            $("#txtAreaComments").val("");

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    $("#divCommentsContainer").html("");
                    if (data.Result.length > 0) {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- Start --</span>");
                        $(data.Result).each(function (index, item) {
                            $("#divCommentsContainer").append("<span class='comment-details'>" + item.Timestamp
                                + " " + item.Sender + ": </span><span class='comment'> " + item.Comments + "</span><br>");
                            if (data.Result.length <= (index + 1)) {
                                setTimeout(function () { $('#divCommentsContainer').scrollTop($('#divCommentsContainer')[0].scrollHeight) }, 300);
                            }
                        });
                    }
                    else {
                        $("#divCommentsContainer").append("<span class='comment-header'>-- No comments found. --</span>");
                    }
                    $("#txtAreaComments").attr("readonly", false);
                    $("#txtAreaComments").prop("disabled", false);
                }
            };

            objEMSCommonJS.GetAjax(GetMRFCommentsURL, input, "", GetSuccessFunction);
        },

        GetCommentSectionFormData: function () {
            var formData = new FormData($('#frmMRF').get(0));
            formData.append("CommentsForm.MRFID", $("#hdnID").val());
            formData.append("CommentsForm.Comments", $("#txtAreaComments").val());
            return formData;
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                $(".jqgfirstrow .filterFields").hide();
            }
        },

        LoadKickoutQuestion: function (param) {
            var s = this;
            Loading(true);
            $("#tblKickoutQuestion").jqGrid("GridUnload");
            $("#tblKickoutQuestion").jqGrid("GridDestroy");
            $("#tblKickoutQuestion").jqGrid({
                url: GetMRFKickoutQuestionURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10000,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "Code", "Type", "Question", "Answer", "Order"],
                colModel: [
                    { key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objMRFListJS.ViewKickoutQuestion },
                    { name: "Code", index: "Code", align: "left", sortable: false },
                    { name: "QuestionType", index: "QuestionType", align: "left", sortable: false },
                    { name: "Question", index: "Question", align: "left", sortable: false },
                    { name: "Answer", index: "Answer", align: "left", sortable: false },
                    { name: "Order", index: "Order", align: "center", sortable: false },
                ],
                rowList: SetRowList(),
                loadonce: false,
                toppager: true,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: true,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: false,
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
                        }

                        AutoSizeColumnJQGrid("tblKickoutQuestion", data);
                        $("#tblKickoutQuestion_subgrid").width(20);
                    }

                    
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });
            $("#tblKickoutQuestion_toppager_center").hide();
            $("#tblKickoutQuestion_toppager_custom_block_right .ui-pg-selbox").hide();

            NumberOnly($("#txtKickoutQuestionOrder"));

            $("#btnAddQuestion").on("click", function () {
                $("#txtFunction").html("Add");
                $("#btnSaveKickoutQuestion").show();
                $("#btnEditKickoutQuestion").hide();
                $("#btnUpdateKickoutQuestion").hide();
                $("#btnBackKickoutQuestion").hide();

                $("#formKickoutQuestion input").val("");

                $("#hdnKickoutQuestionMRFID").val($("#hdnID").val());

                $("#formKickoutQuestion label[for='txtKickoutQuestionCode'] span,#formKickoutQuestion label[for='txtOrder'] span").addClass("reqField");
                $("#formKickoutQuestion label[for='txtKickoutQuestionCode'] span,#formKickoutQuestion label[for='txtOrder'] span").removeClass("unreqField");
                $("#txtKickoutQuestionCode,#txtKickoutQuestionOrder").addClass("required-field");

                $("#txtKickoutQuestionCode").prop("disabled", false);
                $("#txtKickoutQuestionOrder").prop("disabled", false);

                $("#divKickoutQuestionModal").modal("show");
            });

            $("#btnEditKickoutQuestion").on("click", function () {
                objMRFListJS.EditKickoutQuestionModal($("#hdnMRFKickoutQuestionID").val());
            });

            $("#btnBackKickoutQuestion").on("click", function () {
                objMRFListJS.ViewKickoutQuestionModal($("#hdnMRFKickoutQuestionID").val());
            });

            $("#btnSaveKickoutQuestion").on("click", function () {
                if (objEMSCommonJS.ValidateBlankFields("#formKickoutQuestion", "#divKickoutQuestionErrorMessage"))
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_SAVE,
                        "objEMSCommonJS.PostAjax(true \
                            , PostAddKickoutQuestionToMRFURL \
                            , new FormData($('#formKickoutQuestion').get(0)) \
                            , '#divKickoutQuestionErrorMessage' \
                            , '#btnSaveKickoutQuestion' \
                            , objMRFListJS.SuccessKickoutQuestion); ",
                        "function");
            });

            $("#btnUpdateKickoutQuestion").on("click", function () {
                if (objEMSCommonJS.ValidateBlankFields("#formKickoutQuestion", "#divKickoutQuestionErrorMessage"))
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_UPDATE,
                        "objEMSCommonJS.PostAjax(true \
                            , PostEditKickoutQuestionToMRFURL \
                            , new FormData($('#formKickoutQuestion').get(0)) \
                            , '#divKickoutQuestionErrorMessage' \
                            , '#btnUpdateKickoutQuestion' \
                            , objMRFListJS.SuccessKickoutQuestion); ",
                        "function");
            });

            $("#btnRemoveQuestion").on("click", function () {
                $('#divKickoutQuestionListErrorMessage').html('');
                var selRow = $("#tblKickoutQuestion").jqGrid("getGridParam", "selarrrow");
                if (selRow.length > 0) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_REMOVE,
                        "objEMSCommonJS.PostAjax(true \
                            , PostRemoveKickoutQuestionToMRFURL + '&ID="+ selRow.toString() +"' \
                            , new FormData($('#formKickoutQuestion').get(0)) \
                            , '#divKickoutQuestionErrorMessage' \
                            , '#btnRemoveQuestion' \
                            , objMRFListJS.SuccessKickoutQuestion); ",
                        "function");
                }
                else {
                    $("#divKickoutQuestionListErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Item</li></label><br />");
                }
            });

            var SuccessPopulate = function (ID) {
                var Success = function (data) {
                    $("#txtKickoutQuestionType").val(data.Result["QuestionType"]);
                    $("#txtKickoutQuestionQuestion").val(data.Result["Question"]);
                    $("#txtKickoutQuestionAnswer").val(data.Result["Answer"]);
                };
                objEMSCommonJS.GetAjax(GetKickoutQuestionByIDURL + "&ID=" + ID, "", "", Success)
            }
            objEMSCommonJS.BindAutoComplete("formKickoutQuestion #txtKickoutQuestionCode"
                , GetKickoutQuestionAutoCompleteURL, 20, "formKickoutQuestion #hdnKickoutQuestionID", "ID", "Description", SuccessPopulate);
        },

        ViewKickoutQuestion: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return objMRFListJS.ViewKickoutQuestionModal(" + rowObject.ID +");\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },
        ViewKickoutQuestionModal: function (ID) {
            $("#txtFunction").html("View");
            $("#btnSaveKickoutQuestion").hide();
            $("#btnEditKickoutQuestion").show();
            $("#btnUpdateKickoutQuestion").hide();
            $("#btnBackKickoutQuestion").hide();

            $("#txtKickoutQuestionCode").prop("disabled", true);
            $("#txtKickoutQuestionOrder").prop("disabled", true);

            objMRFListJS.PopulateKickoutQuestion(ID);
            $("#divKickoutQuestionModal").modal("show");
            return false;
        },
        EditKickoutQuestionModal: function(ID) {
            $("#txtFunction").html("Edit");
            $("#btnSaveKickoutQuestion").hide();
            $("#btnEditKickoutQuestion").hide();
            $("#btnUpdateKickoutQuestion").show();
            $("#btnBackKickoutQuestion").show();

            $("#txtKickoutQuestionCode").prop("disabled", false);
            $("#txtKickoutQuestionOrder").prop("disabled", false);

            objMRFListJS.PopulateKickoutQuestion(ID);
            $("#divKickoutQuestionModal").modal("show");
            return false;
        },
        PopulateKickoutQuestion: function (ID) {
            $("#hdnKickoutQuestionMRFID").val($("#hdnID").val());
            $("#hdnMRFKickoutQuestionID").val(ID);
            var Success = function (data) {
                $("#hdnKickoutQuestionID").val(data.Result["KickoutQuestionID"]);
                $("#txtKickoutQuestionCode").val(data.Result["Code"]);
                $("#txtKickoutQuestionType").val(data.Result["QuestionType"]);
                $("#txtKickoutQuestionQuestion").val(data.Result["Question"]);
                $("#txtKickoutQuestionAnswer").val(data.Result["Answer"]);
                $("#txtKickoutQuestionOrder").val(data.Result["Order"]);
            };
            objEMSCommonJS.GetAjax(GetMRFKickoutQuestionByIDURL + "&ID=" + ID, "", "", Success)
        },
        SuccessKickoutQuestion: function () {
            objMRFListJS.LoadKickoutQuestion({
                MRFID: $("#hdnID").val(),
            });
            $("#divKickoutQuestionModal").modal("hide");
        }
    };

    objMRFListJS.Initialize();

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