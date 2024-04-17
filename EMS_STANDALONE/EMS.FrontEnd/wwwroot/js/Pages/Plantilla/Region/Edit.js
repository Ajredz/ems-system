var objRegionEditJS;

$(document).ready(function () {
    objRegionEditJS = {
        
        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divRegionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            $("#divRegionModal .form-control").attr("readonly", false);
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divRegionModal').modal('hide');
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , RegionDeleteURL + '?ID=' + objRegionEditJS.ID \
                    , {} \
                    , '#divRegionErrorMessage' \
                    , '#btnDelete' \
                    , objRegionEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmRegion", "#divRegionErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , RegionEditPostURL \
                        , new FormData($('#frmRegion').get(0)) \
                        , '#divRegionErrorMessage' \
                        , '#btnSave' \
                        , objRegionEditJS.EditSuccessFunction);", "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(RegionViewURL + "?ID=" + objRegionEditJS.ID, "divRegionBodyModal");
            });

        },

    };
    
     objRegionEditJS.Initialize();
});