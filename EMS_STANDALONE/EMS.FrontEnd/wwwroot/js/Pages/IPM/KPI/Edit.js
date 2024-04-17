var objKPIEditJS;

$(document).ready(function () {
    objKPIEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divKPIBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $(".reqField").addClass(".unreqField");
            $(".reqField").removeClass(".reqField");
            $("#btnEdit").hide();
            $("#btnSave, #btnBack").show();
            $("#divKPIModal .form-control").attr("readonly", false);
            //$("#txtCode").attr("readonly", true);
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIModal').modal('hide');
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

            $("#btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKPI", "#divKPIErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KPIEditPostURL \
                        , new FormData($('#frmKPI').get(0)) \
                        , '#divKPIErrorMessage' \
                        , '#btnSave' \
                        , objKPIEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , KPIDeleteURL + '?ID=' + objKPIViewJS.ID\
                    , {} \
                    , '#divKPIErrorMessage' \
                    , '#btnDelete' \
                    , objKPIEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnBack").click(function () {
                LoadPartial(KPIViewURL + "?ID=" + objKPIEditJS.ID, "divKPIBodyModal");
            });

        },

    };

    objKPIEditJS.Initialize();
});