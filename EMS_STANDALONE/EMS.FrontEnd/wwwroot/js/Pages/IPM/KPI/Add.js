var objKPIAddJS;

$(document).ready(function () {
    objKPIAddJS = {

        Initialize: function () {
            $("#divKPIBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divKPIModal #btnSave").show();
            $("#divKPIModal .form-control").attr("readonly", false);
            $("#divKPIModal #btnDelete, #divKPIModal #btnBack").remove();

        },

        AddSuccessFunction: function () {
            $("#btnSearch").click();
            $("#frmKPI").trigger("reset");
        },

        ElementBinding: function () {
            var s = this;
            Code($("#txtCode"));
            PreventSpace($("#txtCode"));
            $('#txtCode').bind('copy paste cut', function (e) {
                e.preventDefault();
            });

            $("#ddlKRAGroup").change(function () {
                if ($("#ddlKRAGroup").val() != "") {
                    GenerateDropdownValues(GetKRASubGroupDropdownURL + "&KRAGroup=" + $("#ddlKRAGroup").val(),
                        "ddlKRASubGroup", "Value", "Text", "", "", false);
                }
            });

            $("#btnAddKRAGroup").click(function () {
                LoadPartial(KRAGroupAddURL, "divKRAGroupBodyModal");
                $("#divKRAGroupModal").modal("show");
            });

            $("#btnAddKRASubGroup").click(function () {
                LoadPartial(KRASubGroupAddURL, "divKRASubGroupBodyModal");
                $("#divKRASubGroupModal").modal("show");
            });

            $("#divKPIModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKPI", "#divKPIErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KPIAddPostURL \
                        , new FormData($('#frmKPI').get(0)) \
                        , '#divKPIErrorMessage' \
                        , '#divKPIModal #btnSave' \
                        , objKPIAddJS.AddSuccessFunction);",
                        "function");
                }
            });
        },

    };

    objKPIAddJS.Initialize();
});