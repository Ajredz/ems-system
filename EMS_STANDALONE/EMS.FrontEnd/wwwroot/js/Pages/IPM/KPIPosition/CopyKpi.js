var objKPIPositionCopyKpiJS;

$(document).ready(function () {
    objKPIPositionCopyKpiJS = {

        Initialize: function () {
            $("#divCopyKPIPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();

            /*$("#txtCopyPosition").val($("#txtPosition").val());
            $("#hdnCopyPositionID").val($("#hdnPositionID").val());
            $("#txtCopyEffectiveDate").val($("#txtEffectiveDate").val());*/
        },

        ElementBinding: function () {
            var s = this;

            /*$("#txtNewEffectiveDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });*/

            /*$('#txtNewEffectiveDate').datetimepicker().on('dp.show', function () {
                $('#divCopyKPIPositionModal .modal-body').css({ 'overflow': 'visible' });
                $('#divCopyKPIPositionModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divCopyKPIPositionModal .modal-body').css({ 'overflow': 'auto' });
                $('#divCopyKPIPositionModal.modal').css({ 'overflow': 'auto' });
            });*/

            $("#divCopyKPIPositionModal #btnSave").click(function () {
                $("#divCopyKPIPositionModal").modal("hide");
                objKPIPositionAddJS.LoadKPIPosition($("#hdnCopyPositionID").val());
                /*if (objEMSCommonJS.ValidateBlankFields("#frmCopyKPIPosition", "#divCopyKPIPositionErrorMessage", objKPIPositionCopyKpiJS.ValidateDuplicateFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , CopyKPIPositionAddPostURL \
                        , objKPIPositionCopyKpiJS.GetFormData() \
                        , '#divCopyKPIPositionErrorMessage' \
                        , '#divCopyKPIPositionModal #btnSave' \
                        , objKPIPositionCopyKpiJS.AddSuccessFunction);",
                        "function");
                }*/
            });

            objEMSCommonJS.BindAutoComplete("txtCopyPosition"
                , GetCopyPositionAutoCompleteURL, 20, "hdnCopyPositionID","ID");

        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;

            if ($("#txtNewPosition").val() == "") {
                $("#divCopyKPIPositionErrorMessage").html("<label class=\"errMessage\"><li>" + "Position is Required" + "</li></label><br />");
                isValid = false;
            }
            if ($("#txtNewEffectiveDate").val() == "") {
                $("#divCopyKPIPositionErrorMessage").html("<label class=\"errMessage\"><li>" + "Effective Date is Required" + "</li></label><br />");
                isValid = false;
            }

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },
        GetFormData: function () {
            var formData = new FormData($('#frmCopyKPIPosition').get(0));

            return formData;

        },
        AddSuccessFunction: function () {
            $("#btnReset").click();
            $("#divKPIPositionModal").modal("hide");
            $("#divCopyKPIPositionModal").modal("hide");
        },
    };

    objKPIPositionCopyKpiJS.Initialize();
});