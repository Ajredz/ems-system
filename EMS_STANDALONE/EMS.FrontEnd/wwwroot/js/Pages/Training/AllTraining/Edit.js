var objEmployeeTrainingEditJS;

$(document).ready(function () {
    objEmployeeTrainingEditJS = {

        ID: $("#divTrainingModal #hdnID").val(),
        EmployeeID: $("#hdnEmployeeID").val(),

        Initialize: function () {
            $("#divTrainingBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;

            $("#divTrainingModal #btnEdit").prop("hidden", true);
            $("#divTrainingModal #btnUpdate,#divTrainingModal #btnBack").prop("hidden", false);
            $("#divTrainingModal #ddlType, \
               #divTrainingModal #txtTitle, \
               #divTrainingModal #txtDescription, \
               #divTrainingModal #txtDateSchedule, \
               #divTrainingModal #txtClassroomName").attr("disabled", false);


            s.ElementBinding();

            objEmployeeTrainingListJS.LoadStatusHistory({
                ID: objEmployeeTrainingEditJS.ID,
            });
            objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabHistory', '#divTrainingModal');
            HistoryLoadOnce = true;
            ScoreLoadOnce = false;
        },

        ElementBinding: function () {
            var s = this;

            $("#txtDateSchedule").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $(".tablinks").find("span:contains('Score')").parent("button").click(function () {
                objEMSCommonJS.ChangeTab($(".tablinks:nth-child(2)"), 'tabScore', '#divTrainingModal');
                if (!ScoreLoadOnce) {
                    objEmployeeTrainingListJS.LoadScore({
                        ID: objEmployeeTrainingEditJS.ID,
                    });
                    ScoreLoadOnce = true;
                }
            });

            $("#divTrainingModal #btnBack").click(function () {
                LoadPartial(TrainingViewURL + "?ID=" + objEmployeeTrainingEditJS.ID, "divTrainingBodyModal");
            });

            $("#divTrainingModal #btnUpdate").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#formEmployeeTraining", "#divTrainingErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , "+ "'" + TrainingEditURL + "'" + "\
                        , objEmployeeTrainingEditJS.GetFormData() \
                        , '#divTrainingErrorMessage' \
                        , '#btnUpdate' \
                        , objEmployeeTrainingEditJS.SuccessUpdateFunction);",
                        "function");
                }
            });

            objEMSCommonJS.BindAutoComplete("divTrainingModal #txtClassroomName"
                , GetClassroomAutoCompleteURL
                , 20, "divTrainingModal #txtClassroomID", "Value", "Text");
        },
        GetFormData: function () {
            var formData = new FormData($('#formEmployeeTraining').get(0));
            return formData;
        },
        SuccessUpdateFunction: function () {
            $('#divTrainingModal').modal('hide');
            $("#btnSearch").click();
        },
    }

    objEmployeeTrainingEditJS.Initialize();
});