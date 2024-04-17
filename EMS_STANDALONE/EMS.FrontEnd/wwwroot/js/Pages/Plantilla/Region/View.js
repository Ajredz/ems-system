var objRegionViewJS;

$(document).ready(function () {
    objRegionViewJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divRegionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass("unreqField");
            $(".reqField").removeClass("reqField");
            $(".required-field").removeClass("required-field");

        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divRegionModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , RegionDeleteURL + '?ID=' + objRegionViewJS.ID\
                    , {} \
                    , '#divRegionErrorMessage' \
                    , '#btnDelete' \
                    , objRegionViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnEdit").click(function () {
                LoadPartial(RegionEditURL + "?ID=" + objRegionViewJS.ID, "divRegionBodyModal");
            });

        },

    };
    
     objRegionViewJS.Initialize();
});