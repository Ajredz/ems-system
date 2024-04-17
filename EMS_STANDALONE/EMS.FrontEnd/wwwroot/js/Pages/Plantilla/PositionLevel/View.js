var objPositionLevelViewJS;

$(document).ready(function () {
    objPositionLevelViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPositionLevelBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");

        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divPositionLevelModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PositionLevelDeleteURL + '?ID=' + objPositionLevelViewJS.ID \
                    , {} \
                    , '#divPositionLevelErrorMessage' \
                    , '#btnDelete' \
                    , objPositionLevelViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(PositionLevelEditURL + "?ID=" + objPositionLevelViewJS.ID, "divPositionLevelBodyModal");
            });

        },

    };
    
     objPositionLevelViewJS.Initialize();
});