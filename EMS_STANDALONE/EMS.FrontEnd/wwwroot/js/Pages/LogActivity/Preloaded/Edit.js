var objPreloadedEditJS;
var ModuleDropDownOptions = [];
var TypeDropDownOptions = [];
var SubTypeDropDownOptions = [];

$(document).ready(function () {
    objPreloadedEditJS = {

        ID: $("#hdnID").val(),

        Initialize: function () {
            $("#divPreloadedBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divPreloadedModal #btnSave, #divPreloadedModal #btnBack").show();
            $("#divPreloadedModal .form-control").attr("readonly", false);
            $("#btnEdit").hide();
            //s.GetModuleDropDownOptions();
            s.GetTypeDropDownOptions();
            s.LoadPreloadedItems();
            //s.AddLogActivityItemDynamicFields();
        },

        DeleteSuccessFunction: function () {
          $("#btnSearch").click();
          $('#divPreloadedModal').modal('hide');
        },

        EditSuccessFunction: function () {
          $("#btnSearch").click();
          $("#btnBack").click();
        },

        ElementBinding: function () {
            var s = this;

            $("#btnAddLogActivity").click(function () {
                var fields = $("#divLogActivityItemDynamicFields .required-field");
                var addNewFields = true;

                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });

                if (addNewFields)
                    s.AddLogActivityItemDynamicFields();
            });

             $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , PreloadedDeleteURL + '?ID=' + objPreloadedEditJS.ID\
                    , {} \
                    , '#divPreloadedErrorMessage' \
                    , '#btnDelete' \
                    , objPreloadedEditJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#divPreloadedModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmPreloaded", "#divPreloadedErrorMessage")) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , PreloadedEditPostURL \
                        , objPreloadedEditJS.GetFormData() \
                        , '#divPreloadedErrorMessage' \
                        , '#divPreloadedModal #btnSave' \
                        , objPreloadedEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnBack").click(function () {
                LoadPartial(PreloadedViewURL + "?ID=" + objPreloadedEditJS.ID, "divPreloadedBodyModal");
            });
        },

        GetFormData: function () {

            var formData = new FormData($('#frmPreloaded').get(0));

            //$(".ModuleDynamicFields").each(function (index) {
            //    if ($(this).val() != "") {
            //        formData.append("LogActivityPreloaded.LogActivityList[" + index + "].Module", $(this).val());
            //    }
            //});

            $(".TypeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].Type", $(this).val());
                }
            });

            $(".SubTypeDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].SubType", $(this).val());
                }
            });

            $(".TitleDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].Title", $(this).val());
                }
            });

            $(".DescriptionDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].Description", $(this).val());
                }
            });

            //$(".IsPassFailDynamicFields").each(function (index) {
            //    var isChecked = $(this).is(':checked') ? true : false;
            //    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].IsPassFail", isChecked);
            //});

            //$(".IsAssignmentDynamicFields").each(function (index) {
            //    var isChecked = $(this).is(':checked') ? true : false;
            //    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].IsAssignment", isChecked);
            //});

            $(".IsVisibleDynamicFields").each(function (index) {
                var isChecked = $(this).is(':checked') ? true : false;
                formData.append("LogActivityPreloaded.LogActivityList[" + index + "].IsVisible", isChecked);
            });

            $(".AssignedUserDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("LogActivityPreloaded.LogActivityList[" + index + "].AssignedUserID", $(this).val());
                }
            });

            return formData;
        },

        //GetModuleDropDownOptions: function () {
        //    var s = this;
        //    var GetSuccessFunction = function (data) {
        //        $(data.Result).each(function (index, item) {
        //            ModuleDropDownOptions.push(
        //                {
        //                    Value: item.Value,
        //                    Text: item.Description
        //                });
        //        });
        //    };

        //    objEMSCommonJS.GetAjax(GetModuleDropDownURL, {}, "", GetSuccessFunction);
        //},

        GetTypeDropDownOptions: function () {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    TypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(GetTypeDropDownURL, {}, "", GetSuccessFunction);
        },

        GetSubTypeDropDownOptions: function (type) {
            var s = this;
            var GetSuccessFunction = function (data) {
                $(data.Result).each(function (index, item) {
                    SubTypeDropDownOptions.push(
                        {
                            Value: item.Value,
                            Text: item.Description
                        });
                });
            };

            objEMSCommonJS.GetAjax(GetSubTypeDropDownURL + "&Type=" + type, {}, "", GetSuccessFunction);
        },

        PopulateDropDown: function (id, collection) {
            $(id).append($('<option/>', {
                value: "",
                text: "- Select an Item -"
            }));

            $(collection).each(function (index, item) {
                $(id).append($('<option/>', {
                    value: item.Value,
                    text: item.Text
                }));
            });
        },

        AddLogActivityItemDynamicFields: function () {
            var s = this;
            var htmlId = $(".LogActivityItemDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("LogActivityItemDynamicFields_", "")) + 1;

            $("#divLogActivityItemDynamicFields").append(
                "<div class=\"form-group form-fields LogActivityItemDynamicFields\" id=\"LogActivityItemDynamicFields_" + ctr + "\">"
                //+ "    <div class=\"col-md-2 no-padding\">"
                //+ "        <select id=\"ddlModule_" + ctr + "\" title=\"Module\" class=\"form-control required-field ModuleDynamicFields\"></select>"
                //+ "    </div>"
                + "    <div class=\"col-md-0-5\">"
                + "        <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objPreloadedEditJS.RemoveDynamicFields('#LogActivityItemDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span>"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                + "        <select id=\"ddlType_" + ctr + "\" title=\"Type\" class=\"form-control required-field TypeDynamicFields\"></select>"
                + "    </div>"
                //+ "    <div class=\"col-md-1-5 no-padding\">"
                //+ "        <select id=\"ddlSubType_" + ctr + "\" title=\"Sub-Type\" class=\"form-control required-field SubTypeDynamicFields\"></select>"
                //+ "    </div>"
                + "    <div class=\"col-md-1-5 no-padding\">"
                + "        <input type=\"text\" id=\"txtTitle_" + ctr + "\" class=\"form-control required-field TitleDynamicFields\" title=\"Title\" maxlength=\"100\">"
                + "    </div>"
                + "    <div class=\"col-md-2 no-padding\">"
                //+ "        <input type=\"text\" id=\"txtDescription_" + ctr + "\" class=\"form-control DescriptionDynamicFields\" title=\"Description\" maxlength=\"255\">"
                + "        <textarea rows=\"3\" cols=\"10\" wrap=\"hard\" id=\"txtDescription_" + ctr + "\" class=\"form-control DescriptionDynamicFields\" maxlength=\"255\" title=\"Description\"></textarea>"
                + "    </div>"
                //+ "    <div class=\"col-md-1 text-align-center no-padding\">"
                //+ "        <input type=\"checkbox\" id=\"chkIsPassFail_" + ctr + "\" class=\"IsPassFailDynamicFields\" title=\"IsPassFail\">"
                //+ "    </div>"
                //+ "    <div class=\"col-md-1 text-align-center no-padding\">"
                //+ "        <input type=\"checkbox\" id=\"chkIsAssignment_" + ctr + "\" class=\"IsAssignmentDynamicFields\" title=\"IsAssignment\">"
                //+ "    </div>"
                + "    <div class=\"col-md-1-5 text-align-center no-padding\">"
                + "        <input type=\"checkbox\" id=\"chkIsVisible_" + ctr + "\" class=\"IsVisibleDynamicFields\" title=\"IsVisible\">"
                + "    </div>"
                + "    <div class=\"col-md-2-5 no-padding\">"
                + "        <input type=\"search\" placeholder=\"Search..\" id=\"txtAssignedUser_" + ctr + "\" class=\"form-control\" title=\"Assigned User\" maxlength=\"100\">"
                + "        <input type=\"hidden\" id=\"hdnAssignedUserID_" + ctr + "\" class=\"form-control AssignedUserDynamicFields\">"
                + "    </div>"
                + "</div>"
            );

            //s.PopulateDropDown("#ddlModule_" + ctr, ModuleDropDownOptions);
            s.PopulateDropDown("#ddlType_" + ctr, TypeDropDownOptions);
            //s.PopulateDropDown("#ddlSubType_" + ctr, SubTypeDropDownOptions);

            //$("#ddlType_" + ctr).change(function () {
            //    GenerateDropdownValues(GetSubTypeDropDownURL + "&Type=" + $("#ddlType_" + ctr).val(),
            //        "ddlSubType_" + ctr, "Value", "Description", "", "", false);
            //});

            objEMSCommonJS.BindAutoComplete("divPreloadedModal #txtAssignedUser_" + ctr
                , AssignedUserAutoComplete
                , 20, "divPreloadedModal #hdnAssignedUserID_" + ctr, "ID", "Description");
        },

        RemoveDynamicFields: function (id) {
            $(id).remove();
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;

            $(".TypeDynamicFields").each(function (n1, x1) {
                if (this.value != "") {
                    var currentVal = this.value;
                    var ctr = $(this).prop("id").replace("ddlType_", "");
                    //$("#LogActivityItemDynamicFields_" + ctr + " #ddlModule_" + ctr).removeClass("errMessage");
                    $("#LogActivityItemDynamicFields_" + ctr + " #ddlType_" + ctr).removeClass("errMessage");
                    //$("#LogActivityItemDynamicFields_" + ctr + " #ddlSubType_" + ctr).removeClass("errMessage");
                    $("#LogActivityItemDynamicFields_" + ctr + " #ddlTitle_" + ctr).removeClass("errMessage");

                    $(".TypeDynamicFields").not(this).filter(function () {
                        if (this.value == currentVal) {
                            //$("#LogActivityItemDynamicFields_" + ctr + " #ddlModule_" + ctr).addClass("errMessage");
                            $("#LogActivityItemDynamicFields_" + ctr + " #ddlType_" + ctr).addClass("errMessage");
                            //$("#LogActivityItemDynamicFields_" + ctr + " #ddlSubType_" + ctr).addClass("errMessage");
                            $("#LogActivityItemDynamicFields_" + ctr + " #ddlTitle_" + ctr).addClass("errMessage");
                            isDuplicate++;
                        }
                    });
                }
            });

            if (isDuplicate > 0) {
                $("#divPreloadedErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }

            return isValid;
        },

        LoadPreloadedItems: function () {
            var s = this;
            $("#divLogActivityItemDynamicFields").html("");

            var input = { ID: objPreloadedEditJS.ID };

            var GetSuccessFunction = function (data) {
                var populateFields = function (item, idCtr) {
                    //s.GetSubTypeDropDownOptions(item.Type);
                    s.AddLogActivityItemDynamicFields();
                    //$("#ddlModule_" + idCtr).val(item.Module);
                    $("#ddlType_" + idCtr).val(item.Type);
                    //$("#ddlSubType_" + idCtr).val(item.SubType);
                    $("#txtTitle_" + idCtr).val(item.Title);
                    $("#txtDescription_" + idCtr).val(item.Description);
                    //$("#chkIsPassFail_" + idCtr).val(item.IsPassFail);
                    //$("#chkIsPassFail_" + idCtr).attr('checked', item.IsPassFail);
                    //$("#chkIsAssignment_" + idCtr).val(item.IsAssignment);
                    //$("#chkIsAssignment_" + idCtr).attr('checked', item.IsAssignment);
                    $("#chkIsVisible_" + idCtr).val(item.IsVisible);
                    $("#chkIsVisible_" + idCtr).attr('checked', item.IsVisible);
                    $("#hdnAssignedUserID_" + idCtr).val(item.AssignedUserID);
                    $("#txtAssignedUser_" + idCtr).val(item.AssignedUserName);
                };
                var ctr = 1;
                $(data.Result).each(function (index, item) {
                    populateFields(item, ctr); ctr++;
                });
            };

            objEMSCommonJS.GetAjax(GetPreloadedItemsURL, input, "", GetSuccessFunction);
        },

    };

    objPreloadedEditJS.Initialize();
});