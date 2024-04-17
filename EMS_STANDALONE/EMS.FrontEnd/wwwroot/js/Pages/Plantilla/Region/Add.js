var objRegionAddJS;

$(document).ready(function () {
    objRegionAddJS = {

        Initialize: function () {
            $("#divRegionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divRegionModal #btnSave").show();
            $("#divRegionModal .form-control").attr("readonly", false);
            $("#divRegionModal #btnDelete, #divRegionModal #btnBack").remove();
        },

        AddSuccessFunction: function () {
            $("#divRegionFilter #btnSearch").click();
            $("#frmRegion").trigger("reset");
        },

        ElementBinding: function () {
            var s = this;
            Code($("#divRegionModal #txtCode"));
            PreventSpace($("#divRegionModal #txtCode"));
            $('#divRegionModal #txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#divRegionModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmRegion","#divRegionErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , RegionAddPostURL \
                        , new FormData($('#frmRegion').get(0)) \
                        , '#divRegionErrorMessage' \
                        , '#divRegionModal #btnSave' \
                        , objRegionAddJS.AddSuccessFunction); ",
                        "function");
                }
            });
        },

    };
    
     objRegionAddJS.Initialize();
});