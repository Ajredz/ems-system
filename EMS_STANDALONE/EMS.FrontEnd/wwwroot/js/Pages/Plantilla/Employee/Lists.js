var objEmployeeListJS;
const EmployeeListURL = "/Plantilla/Employee?handler=List";
const EmployeeAddURL = "/Plantilla/Employee/Add";
const EmployeeViewURL = "/Plantilla/Employee/View";
const EmployeeEditURL = "/Plantilla/Employee/Edit";
const EmployeeEditCompanyDetailsURL = "/Plantilla/Employee/EditCompanyDetails";
const EmployeeDeleteURL = "/Plantilla/Employee/Delete";
const EmployeeAddPostURL = "/Plantilla/Employee/Add";
const EmployeeEditPostURL = "/Plantilla/Employee/Edit";

const OrgGroupAutoCompleteURL = "/Plantilla/Employee?handler=OrgTypeAutoComplete";
const OrgGroupByOrgTypeAutoCompleteURL = "/Plantilla/Employee?handler=OrgGroupByOrgTypeAutoComplete";
const PositionAutoCompleteURL = "/Plantilla/Employee?handler=PositionAutoComplete";
const SystemUserAutoCompleteURL = "/Plantilla/Employee?handler=UserNameAutoComplete";
const OrgGroupDropDownURL = "/Plantilla/Employee?handler=OrgGroupDropDown";
const PositionDropDownURL = "/Plantilla/Employee?handler=PositionDropDown";
const GetEmployeeRovingURL = "/Plantilla/Employee/View?handler=EmployeeRoving";
const GetSystemUserNameURL = "/Plantilla/Employee/View?handler=SystemUserName";
const AddSystemUserURL = "/Plantilla/Employee/Add?handler=SystemUser";

const AttachmentTypeDropDown = "/Plantilla/Employee?handler=WorkflowReferenceValue&RefCode=ATTACHMENT_TYPE";
const ReferredByAutoComplete = "/Plantilla/Employee?handler=ReferredBy";
const GetProvinceDropDownByRegionURL = "/Plantilla/Employee?handler=ProvinceDropDownByRegion";
const GetCityMunicipalityDropDownByProvinceURL = "/Plantilla/Employee?handler=CityMunicipalityDropDownByProvince";
const GetBarangayDropDownByCityMunicipalityURL = "/Plantilla/Employee?handler=BarangayDropDownByCityMunicipality";
const RelationshipDropDownURL = "/Plantilla/Employee?handler=RelationshipDropDown";
const GetFamilyBackgroundURL = "/Plantilla/Employee?handler=FamilyByEmployeeID";
const GetWorkingHistoryURL = "/Plantilla/Employee?handler=WorkingHistoryByEmployeeID";
const GetEmploymentStatusURL = "/Plantilla/Employee?handler=EmploymentStatusByEmployeeID";

const EmploymentStatusDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMPLOYMENT_STATUS";
const GetCheckEmployeeExportListURL = "/Plantilla/Employee?handler=CheckEmployeeExportList";
const DownloadEmployeeExportListURL = "/Plantilla/Employee?handler=DownloadEmployeeExportList";

const GetCheckETFExportListURL = "/Plantilla/Employee?handler=CheckETFExportList";
const DownloadETFExportListURL = "/Plantilla/Employee?handler=DownloadETFExportList";


// Log Activity URLs
const EmployeeAddPreLoadedActivitiesURL = "/Plantilla/Employee?handler=AddPreloadedActivities";
const EmployeeAddLogActivityURL = "/Plantilla/Employee/AddLogActivity";
const EmployeeAddLogActivityPostURL = "/Plantilla/Employee/AddLogActivity";

const EmployeeUpdateLogActivityURL = "/Plantilla/Employee/UpdateLogActivity";
const EmployeeUpdateLogActivityPostURL = "/Plantilla/Employee/UpdateLogActivity";
const CheckFileIfExistsURL = "/Plantilla/Employee?handler=CheckFileIfExists";
const DownloadFileURL = "/Plantilla/Employee?handler=DownloadFile";

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
const EmployeeUpdateAssignedUserURL = "/Plantilla/Employee/Edit?handler=UpdateAssignedUser";

const UploadLogActivityURL = "/Plantilla/Employee?handler=UploadLogActivity&ID=";
const DownloadLogActivityFormURL = "/Plantilla/Employee?handler=DownloadLogActivityTemplate";
// END

// Accountability URLs
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

const UploadAccountabilityURL = "/Plantilla/Employee?handler=UploadAccountability&ID=";
const DownloadAccountabilityFormURL = "/Plantilla/Employee?handler=DownloadAccountabilityTemplate";
// END


// Onboarding Workflow URLs
const OnboardingWorkflowTransactionURL = "/Plantilla/Employee?handler=OnboardingWorkflowTransaction";
const SaveOnboardingWorkflowURL = "/Plantilla/Employee/Edit?handler=SaveOnboardingWorkflow";
const GetOnboardingWorkflowStepDropDownURL = "/Plantilla/Employee?handler=WorkflowStepDropdown";
const ReferenceValueURL = "/Plantilla/Employee/Edit?handler=ReferenceValue";
const OnboardingWorkflowStepURL = "/Plantilla/Employee/Edit?handler=WorkflowStep";
// END

// Reference URLs
const AddReferenceURL = "/Plantilla/Employee/AddReference";
const ReferencePostURL = "/Plantilla/Employee/AddReference?refcode=";
const ReferenceEditURL = "/Plantilla/Employee/EditReference";
const ReferenceEditPostURL = "/Plantilla/Employee/EditReference?refcode=";
const GetReferenceValueListURL = "/Plantilla/Employee/AddReference?handler=ReferenceValueList";
// END

const EmployeeCurrentStepAutoCompleteURL = "/Plantilla/Employee?handler=CurrentStepAutoComplete&WorkflowCode=ONBOARDING";
const UploadInsertURL = "/Plantilla/Employee?handler=UploadInsert";
const DownloadFormURL = "/Plantilla/Employee?handler=DownloadEmployeeTemplate";

//added
const tabEmployeeRovingURL = "/Plantilla/Employee/Roving";


// Employee Movement
const EmployeeMovementURL = "/Plantilla/Employee/Movement";

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

const EmployeeFieldListURL = "/Plantilla/Employee/MovementAdd?handler=EmployeeFieldList&type=";
const EmployeeFieldAddURL = "/Plantilla/Employee/AddEmployeeField";
const GetEmploymentMovementMappingURL = "/Plantilla/Employee/MovementAdd?handler=EmployeeMovementMapping";

// Log Activity List URLs
const EmployeeLogActivityURL = "/Plantilla/Employee/_LogActivity";
const EmployeeLogActivityGetCurrentStatusURL = "/Plantilla/Employee?handler=WorkflowReferenceValue&RefCode=ACTIVITY_STAT_FILTER";
const EmployeeLogActivityGetTypeURL = "/Plantilla/Employee?handler=WorkflowReferenceValue&RefCode=ACTIVITY_TYPE";
// END


// Education 
const SchoolLevelDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_SCHOOL_LEVEL";
const EducationalAttainmentDegreeDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_ED_ATT_DEG";
const EducationalAttainmentStatusDropDownURL = "/Plantilla/Employee?handler=ReferenceValue&RefCode=EMP_ED_ATT_STAT";
const GetEducationURL = "/Plantilla/Employee?handler=EducationByEmployeeID";
// END

const GetRegionByOrgGroupIDURL = "/Plantilla/Employee?handler=RegionByOrgGroupID";
const GetJobClassByPositionIDURL = "/Plantilla/Employee?handler=JobClassByPositionID";

// Main Attachment Tab
const EmployeeGetMainAttachmentURL = "/Plantilla/Employee?handler=MainAttachment";
const EmployeeSaveMainAttachmentURL = "/Plantilla/Employee?handler=SaveMainAttachment";
var EmployeeDeletedMainAttachments = [];
// END

const PositionOrgGroupDropDownURL = "/Plantilla/Employee?handler=OrgGroupPositionDropDown";
const CorporateEmailAutomateURL = "/Plantilla/Employee?handler=CorporateEmailAutomate";

const EmployeeAutoCompleteURL = "/Plantilla/Employee?handler=EmployeeAutoComplete";

//For movement Checker
const MovementBulkChangeStatus = "/Plantilla/Employee/Movement?handler=MovementChangeStatus";
const MovementChangeStatusPostURL = "/Plantilla/Employee/Movement?handler=ChangeStatus";


// Employee Training
const EmployeeTrainingURL = "/Plantilla/Employee/Training";

const TrainingListURL = "/Plantilla/Employee/Training?handler=List";
const TrainingViewURL = "/Plantilla/Employee/TrainingView";
const TrainingAddURL = "/Plantilla/Employee/TrainingAdd";
const TrainingEditURL = "/Plantilla/Employee/TrainingEdit";
const GetStatusURL = "/Plantilla/Employee/Training?handler=StatusFilter";
const GetTrainingTypeURL = "/Plantilla/Employee/Training?handler=ReferenceValue&RefCode=TRAINING_TYPE";
const GetTrainingTemplateDropdownURL = "/Plantilla/Employee/Training?handler=TrainingTemplateDropdown";
const TrainingTemplateByIDURL = "/Plantilla/Employee/Training?handler=TrainingTemplateByID";
const AddEmployeeTrainingTemplateURL = "/Plantilla/Employee/Training?handler=AddEmployeeTrainingTemplate";
const TrainingChangeStatus = "/Plantilla/Employee/Training?handler=ChangeStatus";
const GetClassroomAutoCompleteURL = "/Plantilla/Employee/Training?handler=ClassroomNameAutoComplete";
const GetTrainingScoreURL = "/Plantilla/Employee/Training?handler=TrainingScore";
const GetTrainingStatusHistoryURL = "/Plantilla/Employee/Training?handler=TrainingStatusHistory";

const EditDraftToProbationaryURL = "/Plantilla/Employee?handler=EditDraftToProbationary";
const GetCheckExportDraftURL = "/Plantilla/Employee?handler=CheckExportDraft";
const DownloadExportDraftURL = "/Plantilla/Employee?handler=DownloadExportDraft";
const UploadEditDraftURL = "/Plantilla/Employee?handler=UploadEditDraft";

// Tab First Load tags, set to True if tab is clicked/loaded
var personalInfoTabFirstLoad = false;
var educationTabFirstLoad = false;
var familyBackTabFirstLoad = false;
var WorkHisTabFirstLoad = false;
var SecDesigTabFirstLoad = false;
var logActivityTabFirstLoad = false;
var OnboardTabFirstLoad = false;
var AccountTabFirstLoad = false;
var MovementTabFirstLoad = false;
var skillsTabFirstLoad = false;


$(document).ready(function () {
    objEmployeeListJS = {
        Initialize: function () {
            $("#divLogActivityAddPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divAccountabilityAddPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            $("#divAssignedUserModal .modal-header").mousedown(handle_mousedown);
            $("#divUploadModal").css("width", "50%");
            $("#divUploadModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            localStorage["ShowActiveEmployee"] = true;
            s.ElementBinding();
            var param = {
                ID: localStorage["PlantillaEmployeeListID"],
                Code: localStorage["PlantillaEmployeeListCode"],
                Name: localStorage["PlantillaEmployeeListName"],
                OrgGroupDelimited: localStorage["PlantillaEmployeeListOrgGroupDelimited"],
                PositionDelimited: localStorage["PlantillaEmployeeListPositionDelimited"],
                EmploymentStatusDelimited: localStorage["PlantillaEmployeeListEmploymentStatusDelimited"],
                //CurrentStepDelimited: localStorage["PlantillaEmployeeListCurrentStepDelimited"],
                //DateScheduledFrom: localStorage["PlantillaEmployeeListDateScheduledFrom"],
                //DateScheduledTo: localStorage["PlantillaEmployeeListDateScheduledTo"],
                //DateCompletedFrom: localStorage["PlantillaEmployeeListDateCompletedFrom"],
                //DateCompletedTo: localStorage["PlantillaEmployeeListDateCompletedTo"],
                //Remarks: localStorage["PlantillaEmployeeListRemarks"],
                DateStatusFrom: localStorage["PlantillaEmployeeListDateStatusFrom"],
                DateStatusTo: localStorage["PlantillaEmployeeListDateStatusTo"],
                DateHiredFrom: localStorage["PlantillaEmployeeListDateHiredFrom"],
                DateHiredTo: localStorage["PlantillaEmployeeListDateHiredTo"],
                BirthDateFrom: localStorage["PlantillaEmployeeListBirthDateFrom"],
                BirthDateTo: localStorage["PlantillaEmployeeListBirthDateTo"],
                OldEmployeeID: localStorage["PlantillaEmployeeListOldEmployeeID"],
                ShowActiveEmployee: true
            };
            s.LoadJQGrid(param);
            s.GetLocalStorage();

            //WIP
            //setTimeout(function () { $("#btnUploadInsert").click(); },300);
            //setTimeout(function () { $("#btnDownloadForm").click(); },500);
            //WIP

        },

        ElementBinding: function () {
            var s = this;

            NumberOnly($("#txtFilterEmployeeID"));

            $("#txtFilterDateHiredFrom, #txtFilterDateHiredTo, #txtFilterStatusUpdateFrom, #txtFilterStatusUpdateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#txtFilterBirthDateFrom, #txtFilterBirthDateTo").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#divEmployeeList #btnSearch").click(function () {
                var param = {
                    ID: $("#txtFilterEmployeeID").val(),
                    Code: $("#txtFilterCode").val(),
                    Name: $("#txtFilterName").val(),
                    OrgGroupDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value,
                    PositionDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value,
                    EmploymentStatusDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value,
                    //CurrentStepDelimited: objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value,
                    //DateScheduledFrom: $("#txtFilterDateScheduledFrom").val(),
                    //DateScheduledTo: $("#txtFilterDateScheduledTo").val(),
                    //DateCompletedFrom: $("#txtFilterDateCompletedFrom").val(),
                    //DateCompletedTo: $("#txtFilterDateCompletedTo").val(),
                    //Remarks: $("#txtFilterRemarks").val(),
                    DateStatusFrom: $("#txtFilterStatusUpdateFrom").val(),
                    DateStatusTo: $("#txtFilterStatusUpdateTo").val(),
                    DateHiredFrom: $("#txtFilterDateHiredFrom").val(),
                    DateHiredTo: $("#txtFilterDateHiredTo").val(),
                    BirthDateFrom: $("#txtFilterBirthDateFrom").val(),
                    BirthDateTo: $("#txtFilterBirthDateTo").val(),
                    OldEmployeeID: $("#txtFilterOldEmployeeID").val(),
                    OrgGroupDelimitedClus: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupClus").value,
                    OrgGroupDelimitedArea: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupArea").value,
                    OrgGroupDelimitedReg: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupReg").value,
                    OrgGroupDelimitedZone: objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupZone").value,
                    ShowActiveEmployee: objEmployeeListJS.ShowActiveEmployee()
                };
                s.SetLocalStorage();
                ResetJQGridState("tblPlantillaEmployeeList");
                s.LoadJQGrid(param);
            });

            $("#divEmployeeList #btnReset").click(function () {
                $("div.filterFields input[type='search']").val("");
                $("div.filterFields select").val("");
                $("div.filterFields input[type='checkbox']").prop("checked", true);
                $("#multiSelectedOrgGroup").html("");
                $("#multiSelectedPosition").html("");
                //$("#multiSelectedCurrentStep").html("");
                $("#multiSelectedEmploymentStatus").html("");
                $("#multiSelectedEmploymentStatusOption label, #multiSelectedEmploymentStatusOption input").prop("title", "add");
                $("#multiSelectedOrgGroupClus").html("");
                $("#multiSelectedOrgGroupArea").html("");
                $("#multiSelectedOrgGroupReg").html("");
                $("#multiSelectedOrgGroupZone").html("");
                $("#divEmployeeList #btnSearch").click();
            });

            $("#btnAdd").click(function () {
                LoadPartial(EmployeeAddURL, "divEmployeeBodyModal");
                $("#divEmployeeModal").modal("show");
            });

            $("#btnExport").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeListJS.ExportFunction()",
                    "function");
            });

            objEMSCommonJS.BindFilterMultiSelectEnum("multiSelectedEmploymentStatus", EmploymentStatusDropDownURL);

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroup"
                , OrgGroupAutoCompleteURL, 20, "multiSelectedOrgGroup");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterPosition"
                , PositionAutoCompleteURL, 20, "multiSelectedPosition");

            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupClus"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=CLUS", 20, "multiSelectedOrgGroupClus");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupArea"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=AREA", 20, "multiSelectedOrgGroupArea");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupReg"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=REG", 20, "multiSelectedOrgGroupReg");
            objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterOrgGroupZone"
                , OrgGroupByOrgTypeAutoCompleteURL + "&OrgType=ZONE", 20, "multiSelectedOrgGroupZone");

            //objEMSCommonJS.BindFilterMultiSelectAutoComplete("txtFilterCurrentStep"
            //    , EmployeeCurrentStepAutoCompleteURL, 20, "multiSelectedCurrentStep");

            $("#btnUploadInsert").click(function () {
                objEMSCommonJS.UploadModal(UploadInsertURL, "Upload (Insert)", DownloadFormURL);
                $('#divModalErrorMessage').html('');
            });

            $("#btnExportETF").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeListJS.ExportETFFunction()",
                    "function");

            });

            $("#btnChangeToProby").on("click", function () {
                $('#divEmployeeErrorMessage').html('');
                var selRow = $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "selarrrow");
                var firstValue = "";
                var isValid = true;
                if (selRow.length > 0) {
                    $(selRow).each(function (index, item) {
                        if (firstValue == "")
                            firstValue = $("#tblPlantillaEmployeeList").getRowData(item).EmploymentStatus;
                        else if (firstValue != $("#tblPlantillaEmployeeList").getRowData(item).EmploymentStatus)
                            isValid = false;
                        if ($("#tblPlantillaEmployeeList").getRowData(item).Code == "")
                            isValid = false;
                    });
                    if (isValid) {
                        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                            "objEMSCommonJS.PostAjax(true \
                        , EditDraftToProbationaryURL + "+ ("'&ID=" + selRow + "'") + " \
                        , null \
                        , '#divEmployeeErrorMessage' \
                        , '#btnChangeToProby' \
                        , objEmployeeListJS.EditDraftToProbationary);",
                            "function");
                    }
                    else
                        $("#divEmployeeErrorMessage").html("<label class=\"errMessage\"><li>" + ERR_NO_CODE + " </li></label><br />");
                }
                else {
                    $("#divEmployeeErrorMessage").html("<label class=\"errMessage\"><li>" + PREF_SELECT_ONE + " Item</li></label><br />");
                }
            });


            $("#btnExportDraft").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM, "objEmployeeListJS.ExportDraftFunction()",
                    "function");
            });
            $("#btnUploadDraft").click(function () {
                objEMSCommonJS.UploadModal(UploadEditDraftURL, "Upload Draft (Edit)","");
                $('#divModalErrorMessage').html('');
            });
        },
        EditDraftToProbationary: function () {
            $("#btnSearch").click();
        },
        ExportETFFunction: function () {

            var parameters = "&sidx=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterEmployeeID").val()
                + "&Code=" + $("#txtFilterCode").val()
                + "&Name=" + $("#txtFilterName").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&OrgGroupDelimitedClus=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupClus").value
                + "&OrgGroupDelimitedArea=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupArea").value
                + "&OrgGroupDelimitedReg=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupReg").value
                + "&OrgGroupDelimitedZone=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupZone").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&EmploymentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value
                //+ "&CurrentStep=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value
                //+ "&DateScheduledFrom=" + $("#txtFilterDateScheduledFrom").val()
                //+ "&DateScheduledTo=" + $("#txtFilterDateScheduledTo").val()
                //+ "&DateCompletedFrom=" + $("#txtFilterDateCompletedFrom").val()
                //+ "&DateCompletedTo=" + $("#txtFilterDateCompletedTo").val()
                //+ "&Remarks=" + $("#txtFilterRemarks").val()
                + "&DateHiredFrom=" + $("#txtFilterDateHiredFrom").val()
                + "&DateHiredTo=" + $("#txtFilterDateHiredTo").val()
                + "&BirthDateFrom=" + $("#txtFilterBirthDateFrom").val()
                + "&BirthDateTo=" + $("#txtFilterBirthDateTo").val()
                + "&OldEmployeeID=" + $("#txtFilterOldEmployeeID").val();

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadETFExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckETFExportListURL + parameters, {}, "#btnExportETF", GetSuccessFunction, null, true);
        },

        ExportFunction: function () {

            var parameters = "&sidx=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortname")
                + "&sord=" + $("#tblPlantillaEmployeeList").jqGrid("getGridParam", "sortorder")
                + "&ID=" + $("#txtFilterEmployeeID").val()
                + "&Code=" + $("#txtFilterCode").val()
                + "&Name=" + $("#txtFilterName").val()
                + "&OrgGroupDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value
                + "&OrgGroupDelimitedClus=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupClus").value
                + "&OrgGroupDelimitedArea=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupArea").value
                + "&OrgGroupDelimitedReg=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupReg").value
                + "&OrgGroupDelimitedZone=" + objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroupZone").value
                + "&PositionDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value
                + "&EmploymentStatusDelimited=" + objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value
                //+ "&CurrentStep=" + objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value
                //+ "&DateScheduledFrom=" + $("#txtFilterDateScheduledFrom").val()
                //+ "&DateScheduledTo=" + $("#txtFilterDateScheduledTo").val()
                //+ "&DateCompletedFrom=" + $("#txtFilterDateCompletedFrom").val()
                //+ "&DateCompletedTo=" + $("#txtFilterDateCompletedTo").val()
                //+ "&Remarks=" + $("#txtFilterRemarks").val()
                + "&DateHiredFrom=" + $("#txtFilterDateHiredFrom").val()
                + "&DateHiredTo=" + $("#txtFilterDateHiredTo").val()
                + "&DateStatusFrom=" + $("#txtFilterDateStatusFrom").val()
                + "&DateStatusTo=" + $("#txtFilterDateStatusTo").val()
                + "&BirthDateFrom=" + $("#txtFilterBirthDateFrom").val()
                + "&BirthDateTo=" + $("#txtFilterBirthDateTo").val()
                + "&OldEmployeeID=" + $("#txtFilterOldEmployeeID").val();

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadEmployeeExportListURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckEmployeeExportListURL + parameters, {}, "#btnExport", GetSuccessFunction, null, true);
        },

        RemoveDynamicFields: function (id, deleteAttachmentFunction) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
            if (deleteAttachmentFunction != null)
                deleteAttachmentFunction();
        },

        LoadJQGrid: function (param) {
            var s = this;
            Loading(true);
            var tableInfo = localStorage.getItem("tblPlantillaEmployeeList") == "" ? "" : $.parseJSON(localStorage.getItem("tblPlantillaEmployeeList"));
            var moveFilterFields = function () {
                var intialHeight = $("#divEmployeeList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divEmployeeList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#divEmployeeList #filterFieldsContainer");
                });
                $("#divEmployeeList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("#divEmployeeList div.filterFields").unbind("keyup");
                $("#divEmployeeList div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divEmployeeList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();
            $("#tblPlantillaEmployeeList").jqGrid("GridUnload");
            $("#tblPlantillaEmployeeList").jqGrid("GridDestroy");
            $("#tblPlantillaEmployeeList").jqGrid({
                url: EmployeeListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "ID", "New Employee ID", "Name", "Org Group", "Position", "Employment Status", "Status Update Date", "Date Hired", "Clearance"// , "Birth Date"|
                    , "Old Employee ID","Cluster","Area","Region","Zone"], //"Onboarding Status", "Date Scheduled", "Date Completed", "Onboarding Remarks",
                colModel: [
                    { hidden: true },
                    { width: 15, key: true, name: "ID", index: "ID", align: "center", sortable: true, formatter: objEmployeeListJS.AddLink },
                    { name: "Code", index: "Code", editable: true, align: "left", sortable: true },
                    { name: "Name", index: "Name", editable: true, align: "left", sortable: true },
                    { name: "OrgGroup", index: "OrgGroup", editable: true, align: "left", sortable: true },
                    { name: "Position", index: "Position", editable: true, align: "left", sortable: true },
                    { name: "EmploymentStatus", index: "EmploymentStatus", editable: true, align: "left", sortable: true },
                    { name: "DateStatus", index: "DateStatus", editable: true, align: "left", sortable: true },
                    //{ name: "CurrentStep", index: "CurrentStep", editable: true, align: "left", sortable: true },
                    //{ name: "DateScheduled", index: "DateScheduled", editable: true, align: "left", sortable: true },
                    //{ name: "DateCompleted", index: "DateCompleted", editable: true, align: "left", sortable: true },
                    //{ name: "Remarks", index: "Remarks", editable: true, align: "left", sortable: true },
                    { name: "DateHired", index: "DateHired", editable: true, align: "left", sortable: true },
                    { hidden: true, name: "Percent", index: "Percent", editable: true, align: "left", sortable: true },
                    // { name: "BirthDate", index: "BirthDate", editable: true, align: "left", sortable: true },
                    { name: "OldEmployeeID", index: "OldEmployeeID", editable: true, align: "left", sortable: true },
                    { name: "Cluster", index: "Cluster", editable: true, align: "left", sortable: true },
                    { name: "Area", index: "Area", editable: true, align: "left", sortable: true },
                    { name: "Region", index: "Region", editable: true, align: "left", sortable: true },
                    { name: "Zone", index: "Zone", editable: true, align: "left", sortable: true },
                ],
                toppager: $("#divPager"),
                pager: $("#divPager"),
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
                multiselect: ($("#btnChangeToProby").is(":visible") ? true : false),
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ondblClickRow: function (rowId, iRow, iCol, e) {
                    $("#tblPlantillaEmployeeList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();
                },
                loadComplete: function (data) {

                    //WIP
                    //setTimeout(function () { $(".jqgrid-id-link:nth-child(1)")[1].click(); }, 100);
                    //WIP
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
                                if (data.rows[i].EmploymentStatus != "DRAFT") {
                                    $("#jqg_tblPlantillaEmployeeList_" + data.rows[i].ID).prop("disabled", true);
                                }
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblPlantillaEmployeeList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#divEmployeeList #filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divEmployeeList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        $("#divEmployeeList .jqgrid-id-link").click(function () {
                            $('#divEmployeeModal').modal('show');
                        });
                    }

                    if (localStorage["EmployeeListFilterOption"] != undefined) {
                        $("#chkFilter").prop('checked', JSON.parse(localStorage["EmployeeListFilterOption"]));
                    }

                    if (localStorage["ShowActiveEmployee"] != undefined) {
                        $("#chkActive").prop('checked', JSON.parse(localStorage["ShowActiveEmployee"]));
                    }

                    objEmployeeListJS.ShowHideFilter();
                    objEmployeeListJS.ShowActiveEmployee();

                    $("#chkFilter").on('change', function () {
                        objEmployeeListJS.ShowHideFilter();
                        localStorage["EmployeeListFilterOption"] = $("#chkFilter").is(":checked");
                    });

                    $("#chkActive").on('change', function () {
                        objEmployeeListJS.ShowActiveEmployee()
                        localStorage["ShowActiveEmployee"] = $("#chkActive").is(":checked");
                        $("#btnSearch").click();
                    });

                    // set minimum height to prevent datetimepicker from being hidden by the scroll
                    $("#divEmployeeList .ui-jqgrid-bdiv").css({ "min-height": "400px" });

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
                beforeSelectRow: function (id, e) {
                    var cbsdis = $("tr#" + id + ".jqgrow > td > input.cbox:disabled", $(this));

                    if (cbsdis.length == 0) {
                        return true;    // allow select the row
                    } else {
                        return false;   // not allow select the row
                    }
                },
                onSelectAll: function (aRowids, status) {
                    if (status) {
                        var grid = $("#tblPlantillaEmployeeList"), i;
                        // uncheck "protected" rows
                        var cbs = $("tr.jqgrow > td > input.cbox:disabled", grid[0]);
                        cbs.removeAttr("checked");

                        //modify the selarrrow parameter
                        grid[0].p.selarrrow = $(this).find("tr.jqgrow:has(td > input.cbox:checked)")
                            .map(function () { return this.id; }) // convert to set of ids
                            .get(); // convert to instance of Array

                        //deselect disabled rows
                        $(this).find("tr.jqgrow:has(td > input.cbox:disabled)")
                            .attr('aria-selected', 'false')
                            .removeClass('ui-state-highlight');
                    }
                },
                beforeRequest: function () {
                    GetJQGridState("tblPlantillaEmployeeList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            );
            $("#divPager").css("width", "100%");
            $("#divPager").css("height", "100%");

            $("#tblPlantillaEmployeeList_toppager_center").hide();
            $("#tblPlantillaEmployeeList_toppager_right").hide();
            $("#tblPlantillaEmployeeList_toppager_left").after("<label class=\"ui-row-label\" id=\"lblActive\" style=\"margin-left: 20%\">Show Active Employee</label>");
            jQuery("#lblActive").after("<input type=\"checkbox\" id=\"chkActive\" style=\"margin-right:15px; margin-top: 5px;\" checked=\"checked\"></div>");
            jQuery("#chkActive").after("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filters</label>");
            jQuery("#lblFilter").after("<input type=\"checkbox\" id=\"chkFilter\" style=\"margin-right:15px; margin-top: 5px;\"></div>");
            jQuery(".ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");

            $("#divPager_custom_block_right").appendTo("#divPager_left");
            $("#divPager_center .ui-pg-table").appendTo("#divPager_right");
        },

        SetLocalStorage: function () {
            localStorage["PlantillaEmployeeListID"] = $("#txtFilterEmployeeID").val();
            localStorage["PlantillaEmployeeListCode"] = $("#txtFilterCode").val();
            localStorage["PlantillaEmployeeListName"] = $("#txtFilterName").val();
            localStorage["PlantillaEmployeeListOrgGroupDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").value;
            localStorage["PlantillaEmployeeListOrgGroupDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedOrgGroup").text;
            localStorage["PlantillaEmployeeListPositionDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").value;
            localStorage["PlantillaEmployeeListPositionDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedPosition").text;
            localStorage["PlantillaEmployeeListEmploymentStatusDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").value;
            localStorage["PlantillaEmployeeListEmploymentStatusDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedEmploymentStatus").text;
            localStorage["PlantillaEmployeeListDateHiredFrom"] = $("#txtFilterDateHiredFrom").val();
            localStorage["PlantillaEmployeeListDateHiredTo"] = $("#txtFilterDateHiredTo").val();
            localStorage["PlantillaEmployeeListDateStatusFrom"] = $("#txtFilterDateStatusFrom").val();
            localStorage["PlantillaEmployeeListDateStatusTo"] = $("#txtFilterDateStatusTo").val();
            localStorage["PlantillaEmployeeListBirthDateFrom"] = $("#txtFilterBirthDateFrom").val();
            localStorage["PlantillaEmployeeListBirthDateTo"] = $("#txtFilterBirthDateTo").val();
            localStorage["PlantillaEmployeeListOldEmployeeID"] = $("#txtFilterOldEmployeeID").val();

            //localStorage["PlantillaEmployeeListCurrentStepDelimited"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").value;
            //localStorage["PlantillaEmployeeListCurrentStepDelimitedText"] = objEMSCommonJS.GetMultiSelectList("multiSelectedCurrentStep").text;
            //localStorage["PlantillaEmployeeListScheduledFrom"] = $("#txtFilterScheduledFrom").val();
            //localStorage["PlantillaEmployeeListScheduledTo"] = $("#txtFilterScheduledTo").val();
            //localStorage["PlantillaEmployeeListCompletedFrom"] = $("#txtFilterCompletedFrom").val();
            //localStorage["PlantillaEmployeeListCompletedTo"] = $("#txtFilterCompletedTo").val();
            //localStorage["PlantillaEmployeeListRemarks"] = $("#txtFilterRemarks").val();
        },

        GetLocalStorage: function () {
            $("#txtFilterEmployeeID").val(localStorage["PlantillaEmployeeListID"]);
            $("#txtFilterCode").val(localStorage["PlantillaEmployeeListCode"]);
            $("#txtFilterName").val(localStorage["PlantillaEmployeeListName"]);

            objEMSCommonJS.SetMultiSelectList("multiSelectedOrgGroup"
                , "PlantillaEmployeeListOrgGroupDelimited"
                , "PlantillaEmployeeListOrgGroupDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedPosition"
                , "PlantillaEmployeeListPositionDelimited"
                , "PlantillaEmployeeListPositionDelimitedText");

            objEMSCommonJS.SetMultiSelectList("multiSelectedEmploymentStatus"
                , "PlantillaEmployeeListEmploymentStatusDelimited"
                , "PlantillaEmployeeListEmploymentStatusDelimitedText");

            $("#txtFilterDateHiredFrom").val(localStorage["PlantillaEmployeeListDateHiredFrom"]);
            $("#txtFilterDateHiredTo").val(localStorage["PlantillaEmployeeListDateHiredTo"]);
            $("#txtFilterDateStatusFrom").val(localStorage["PlantillaEmployeeListDateStatusFrom"]);
            $("#txtFilterDateStatusTo").val(localStorage["PlantillaEmployeeListDateStatusTo"]);
            $("#txtFilterBirthDateFrom").val(localStorage["PlantillaEmployeeListBirthDateFrom"]);
            $("#txtFilterBirthDateTo").val(localStorage["PlantillaEmployeeListBirthDateTo"]);
            $("#txtFilterOldEmployeeID").val(localStorage["PlantillaEmployeeListOldEmployeeID"]);

            //objEMSCommonJS.SetMultiSelectList("multiSelectedCurrentStep"
            //    , "PlantillaEmployeeListCurrentStepDelimited"
            //    , "PlantillaEmployeeListCurrentStepDelimitedText");

            //$("#txtFilterScheduledFrom").val(localStorage["PlantillaEmployeeListScheduledFrom"]);
            //$("#txtFilterScheduledTo").val(localStorage["PlantillaEmployeeListScheduledTo"]);
            //$("#txtFilterCompletedFrom").val(localStorage["PlantillaEmployeeListCompletedFrom"]);
            //$("#txtFilterCompletedTo").val(localStorage["PlantillaEmployeeListCompletedTo"]);
            //$("#txtFilterRemarks").val(localStorage["PlantillaEmployeeListApproverRemarks"]);
        },

        AddLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + EmployeeViewURL + "?ID=" + rowObject.ID + "', 'divEmployeeBodyModal');\">" + objEMSCommonJS.JQGridIDFormat(rowObject.ID) + "</a>";
        },

        ShowHideFilter: function () {
            if ($("#chkFilter").is(":checked")) {
                 $(".jqgfirstrow .filterFields").show();
            }
            else if ($("#chkFilter").is(":not(:checked)")) {
                 $(".jqgfirstrow .filterFields").hide();
            }
        },
        ShowActiveEmployee: function () {
            if ($("#chkActive").is(":checked")) {
                return true;
            }
            else if ($("#chkActive").is(":not(:checked)")) {
                return false;
            }
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

        CorporateEmailAutomate: function (BranchID=1) {
            var Email;
            var GetSuccessFunctionGreen = function (data) {
                Email = data.Result['Email'];
            }
            objEMSCommonJS.GetAjaxNoLoading(CorporateEmailAutomateURL + "&BranchID=" + BranchID, "", "", GetSuccessFunctionGreen);
            return Email;
        },

        ExportDraftFunction: function () {
            var parameters = "&EmploymentStatusDelimited=560"; //554

            var GetSuccessFunction = function (data) {
                if (data.IsSuccess == true) {
                    window.location = DownloadExportDraftURL + parameters;
                    $("#divModal").modal("hide");
                }
                else {
                    ModalAlert(MODAL_HEADER, data.Result);
                }
            };

            objEMSCommonJS.GetAjax(GetCheckExportDraftURL + parameters, {}, "#btnExportDraft", GetSuccessFunction, null, true);
        },
    };

    objEmployeeListJS.Initialize();
});