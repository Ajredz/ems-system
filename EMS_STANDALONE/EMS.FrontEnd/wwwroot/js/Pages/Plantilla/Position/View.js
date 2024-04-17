var objPositionViewJS;

$(document).ready(function () {
    objPositionViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");
            $("#divPositionModal .form-control").prop("disabled", true);


            $('#SummerNoteJobDescription').summernote({
                toolbar: [
                ]
            });
            $('#SummerNoteJobQualification').summernote({
                toolbar: [
                ]
            });
            $('#SummerNoteJobDescription').summernote('disable');
            $('#SummerNoteJobQualification').summernote('disable');
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divPositionModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PositionDeleteURL + '?ID=' + objPositionViewJS.ID\
                    , {} \
                    , '#divPositionErrorMessage' \
                    , '#btnDelete' \
                    , objPositionViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(PositionEditURL + "?ID=" + objPositionViewJS.ID, "divPositionBodyModal");
            });

        },

    };

    objPositionViewJS.Initialize();
});