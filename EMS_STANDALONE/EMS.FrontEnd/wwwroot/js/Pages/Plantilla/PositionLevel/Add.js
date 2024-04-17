var objPositionLevelAddJS;

$(document).ready(function () {
    objPositionLevelAddJS = {

        Initialize: function () {
            $("#divPositionLevelBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#btnSave").show();
            $("#divPositionLevelModal .form-control").attr("readonly", false);
            $("#btnDelete, #btnBack").remove();
            
        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmPositionLevel").trigger("reset");
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPositionLevel", "#divPositionLevelErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , PositionLevelAddPostURL \
                        , new FormData($('#frmPositionLevel').get(0)) \
                        ,'#divPositionLevelErrorMessage' \
                        , '#btnSave' \
                        , objPositionLevelAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

    };
    
     objPositionLevelAddJS.Initialize();
});