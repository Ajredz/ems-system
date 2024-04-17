var objBranchInfoEditJS;

$(document).ready(function () {
    objBranchInfoEditJS = {

        ID: $("#divBranchInfoModal #hdnID").val(),

        Initialize: function () {
            $("#divBranchInfoBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.LoadHierarchyUpward();
            $("#divBranchInfoModal .form-control").attr("readonly", false);
            $("#divBranchInfoModal #btnEdit").hide();
            $("#divBranchInfoModal #btnSave, #divBranchInfoModal #btnBack").show();

            $("#cbIsBranchActive").prop("checked", ($("#hdncbBranchActive").val() == "True" ? true : false));
            $("#cbIsBranchActive").prop("disabled", ($("#hdnHasEditBranchActive").val() == "true" ? false : true));

        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            AmountOnly($("#divBranchInfoModal #txtBranchSize"));
            AmountOnly($("#divBranchInfoModal #txtParkingSize"));
            NumberOnly($("#divBranchInfoModal #txtServiceBay"));

            $("#cbIsBranchActive").click(function () {
                if ($("#cbIsBranchActive").prop("checked"))
                    $("#hdncbBranchActive").val(true);
                else
                    $("#hdncbBranchActive").val(false);
            });

            $("#btnSave").click(function () {
                if ($("#divBranchInfoModal #txtBranchEmail").val() != "" && !ValidEmail($("#divBranchInfoModal #txtBranchEmail"))) {
                    $("#divBranchInfoErrorMessage").html("");
                    $("#divBranchInfoModal #txtBranchEmail").addClass("errMessage");
                    $("#divBranchInfoModal #txtBranchEmail").focus();
                    $("#divBranchInfoErrorMessage").append("<label class=\"errMessage\"><li>Branch Email is invalid</li></label><br />");
                }
                else {
                    var isNoBlankFunction = function () {
                        return true;
                    };

                    if (objEMSCommonJS.ValidateBlankFields("#frmBranchInfo", "#divBranchInfoErrorMessage", isNoBlankFunction)) {
                        ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                            "objEMSCommonJS.PostAjax(true \
                        , BranchInfoEditURL \
                        , objBranchInfoEditJS.GetFormData() \
                        ,'#divBranchInfoErrorMessage' \
                        , '#btnSave' \
                        , objBranchInfoEditJS.EditSuccessFunction);",
                            "function");
                    }
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(BranchInfoViewURL + "?ID=" + objBranchInfoEditJS.ID, "divBranchInfoBodyModal");
            });

            $("#divBranchInfoModal #txtBranchEmail").on("keyup", function () {
                $("#divBranchInfoErrorMessage").html("");

                if (!ValidEmail($("#divBranchInfoModal #txtBranchEmail"))) {
                    $("#divBranchInfoModal #txtBranchEmail").addClass("errMessage");
                    $("#divBranchInfoModal #txtBranchEmail").focus();
                    $("#divBranchInfoErrorMessage").append("<label class=\"errMessage\"><li>Branch Email is invalid</li></label><br />");
                }
                return false;
            });

            objEMSCommonJS.BindAutoComplete("txtPSGC"
                , PSGCAutoCompleteURL
                , 20, "hdnPSGC", "Code", "Description");

            objEMSCommonJS.BindAutoComplete("txtCsodAm"
                , OrgGroupAutoCompleteURL
                , 20, "hdnCsodAm", "ID");

            objEMSCommonJS.BindAutoComplete("txtHRBP"
                , OrgGroupAutoCompleteURL
                , 20, "hdnHRBP", "ID");

            objEMSCommonJS.BindAutoComplete("txtRRT"
                , OrgGroupAutoCompleteURL
                , 20, "hdnRRT", "ID");

        },

        GetFormData: function () {
            var formData = new FormData($('#frmBranchInfo').get(0));
            return formData;
        },

        LoadHierarchyUpward: function () {

            var GetSuccessFunction = function (data) {

                var length = data.Result.length;
                var codeValue = $("#hdnOrgCode").val();
                var descValue = $("#hdnOrgDescription").val();
                var parentValue = "";

                $("#divHierarchyUpward").html("");

                $(data.Result).each(function (idx, item) {
                    $("#divHierarchyUpward").append(item.Code + " - " + item.Description + "<span style='color:#395B77;font-size:14px;font-weight: bold;'> > </span>");
                    parentValue = item.Code + " - " + item.Description;
                });

                if (codeValue != "")
                    $("#divHierarchyUpward").append("<span style='color:#395B77;font-weight: bold;font-size:12px;'>" + codeValue + " - " + descValue + "</span>");
            };

            objEMSCommonJS.GetAjax(OrgTypeHierarchyUpwardURL
                + "&ID=" + $("#hdnParentOrgID").val()
                , {}
                , ""
                , GetSuccessFunction);
        },
    };

    objBranchInfoEditJS.Initialize();
});