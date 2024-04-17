var objKPIPositionEditJS;
var KPIDropDownOptions = [];

$(document).ready(function () {
    objKPIPositionEditJS = {

        ID: $("#hdnID").val(),
        EffectiveDate: $("#txtEffectiveDate").val(),

        Initialize: function () {
            $("#divKPIPositionBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            $("#divKPIPositionModal .form-control").not("#txtPosition, #txtEffectiveDate").attr("readonly", false);
            $("#divKPIPositionModal #btnEdit").hide();
            $("#divKPIPositionModal #btnSave, #divKPIPositionModal #btnBack").show();
            s.LoadKPIPosition();
        },

        EditSuccessFunction: function () {
            $("#btnSearch").click();
            $("#btnBack").click();
        },

        DeleteSuccessFunction: function () {
            $("#btnSearch").click();
            $('#divKPIPositionModal').modal('hide');
        },

        ElementBinding: function () {
            var s = this;

            $("#txtEffectiveDate").datetimepicker({
                useCurrent: false,
                format: 'MM/DD/YYYY'
            });

            $("#btnAddChildrenSetupFields").click(function () {
                $(".KPIDynamicFields").addClass("required-field");
                $(".WeightDynamicFields").addClass("required-field");
                var fields = $("#DivChildrenSetupDynamicFields .required-field");
                var addNewFields = true;
                fields.each(function (n, element) {
                    if ($(this).val() == "") {
                        $(this).focus();
                        addNewFields = false;
                        return false;
                    }
                });
                if (addNewFields)
                    s.AddChildrenSetupDynamicFields();
            });

            $("#divKPIPositionModal #btnSave").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmKPIPosition", "#divKPIPositionErrorMessage", objKPIPositionEditJS.ValidateDuplicateFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , KPIPositionEditPostURL \
                        , objKPIPositionEditJS.GetFormData() \
                        , '#divKPIPositionErrorMessage' \
                        , '#divKPIPositionModal #btnSave' \
                        , objKPIPositionEditJS.EditSuccessFunction);",
                        "function");
                }
            });

            $("#btnDelete").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE,
                    "objEMSCommonJS.PostAjax(false \
                    , KPIPositionDeleteURL + '?ID=' + objKPIPositionViewJS.ID\ + '&EffectiveDate=' + objKPIPositionViewJS.EffectiveDate\
                    , {} \
                    , '#divKPIPositionErrorMessage' \
                    , '#btnDelete' \
                    , objKPIPositionViewJS.DeleteSuccessFunction);",
                    "function");
            });

            $("#btnBack").click(function () {
                LoadPartial(KPIPositionViewURL + "?ID=" + objKPIPositionEditJS.ID + "&EffectiveDate=" + objKPIPositionEditJS.EffectiveDate, "divKPIPositionBodyModal");
            });

            objEMSCommonJS.BindAutoComplete("txtPosition",
                GetPositionAutoCompleteURL, 20, "hdnPositionID", "ID", "Description");
        },

        GetFormData: function () {

            var formData = new FormData($('#frmKPIPosition').get(0));

            $(".IDDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIPosition.KPIPositionList[" + index + "].ID", $(this).val());
                }
            });

            $(".KPIIDDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIPosition.KPIPositionList[" + index + "].KPIID", $(this).val());
                }
            });

            $(".KPIDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIPosition.KPIPositionList[" + index + "].KPI", $(this).val());
                }
            });

            $(".WeightDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIPosition.KPIPositionList[" + index + "].Weight", $(this).val());
                }
            });
            $(".WeightNoServiceBayDynamicFields").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("KPIPosition.KPIPositionList[" + index + "].WeightNoServiceBay", $(this).val());
                }
            });

            return formData;
        },

        AddChildrenSetupDynamicFields: function () {
            var s = this;
            var htmlId = $(".ChildrenSetupDynamicFields:last").prop("id");
            var ctr = parseInt(htmlId == undefined ? 0 : htmlId.replace("ChildrenSetupDynamicFields_", "")) + 1;

            $("#DivChildrenSetupDynamicFields").append(
                "<tbody class=\"ChildrenSetupDynamicFields\" id=\"ChildrenSetupDynamicFields_" + ctr + "\"> \
                   <input type=\"hidden\" id=\"txtID_" + ctr + "\" readonly> \
                   <tr> \
                       <td class=\"col-md-0-5 text-align-center no-padding\"> \
                           <span class=\"btn-glyph-dynamic glyphicon glyphicon-trash\" \
                            onclick=\"ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_DELETE_SINGLE, &quot;objKPIPositionEditJS.RemoveDynamicFields('#ChildrenSetupDynamicFields_" + ctr + "')&quot;, &quot;function&quot;);\"></span> \
                       </td> \
                       <td class=\"text-align-center\"> \
                           <input type=\"text\" id=\"txtKRAGroup_" + ctr + "\" class=\"form-control KRAGroupDynamicFields\" title=\"KRA Group\" readonly> \
                       </td> \
                       <td class=\"text-align-center\"> \
                           <input type=\"text\" id=\"txtKRASubGroup_" + ctr + "\" class=\"form-control KRASubGroupDynamicFields\" title=\"KRA Sub Group\" readonly> \
                       </td> \
                       <td class=\"text-align-center\"> \
                           <input type=\"text\" id=\"txtKPI_" + ctr + "\" class=\"form-control required-field KPIDynamicFields\" title=\"KPI\"> \
                           <input type=\"hidden\" id=\"hdnKPIID_" + ctr + "\" class=\"KPIIDDynamicFields\" readonly> \
                       </td> \
                       <td class=\"text-align-center\"> \
                           <textarea id=\"txtDescription_" + ctr + "\" style=\"resize: none;\" class=\"form-control DescriptionDynamicFields\" title=\"Description\" readonly></textarea> \
                       </td> \
                       <td class=\"col-md-1 text-align-center no-padding\"> \
                           <input type=\"number\" id=\"txtWeight_" + ctr + "\" class=\"form-control text-amount required-field WeightDynamicFields\" title=\"Weight w/ Service Bay\"> \
                       </td> \
                       <td class=\"col-md-1 text-align-center no-padding\"> \
                           <input type=\"number\" id=\"txtWeightNoServiceBay_" + ctr + "\" class=\"form-control text-amount required-field WeightNoServiceBayDynamicFields\" title=\"Weight w/o Service Bay\"> \
                       </td> \
                   </tr> \
               </tbody>"
            );

            DecimalsFormat($(".WeightDynamicFields"));
            DecimalsFormat($(".WeightNoServiceBayDynamicFields"));

            objKPIPositionEditJS.BindAutoComplete("txtKPI_" + ctr,
                GetKPIAutoCompleteURL, 20, "hdnKPIID_" + ctr, "ID", "Description", ctr);

            $("#txtWeight_" + ctr).keyup(function () {
                if ($("#txtWeight_" + ctr).val() > 100)
                    $("#txtWeight_" + ctr).val(0);

                s.CalculateTotal("WeightDynamicFields", "txtTotal");
            });
            $("#txtWeightNoServiceBay_" + ctr).keyup(function () {
                if ($("#txtWeightNoServiceBay_" + ctr).val() > 100)
                    $("#txtWeightNoServiceBay_" + ctr).val(0);

                s.CalculateTotal("WeightNoServiceBayDynamicFields", "txtTotalNoServiceBay");
            });
        },

        RemoveDynamicFields: function (id) {
            var s = this;

            $(id).remove();
            s.CalculateTotal("WeightDynamicFields", "txtTotal");
            s.CalculateTotal("WeightNoServiceBayDynamicFields", "txtTotalNoServiceBay");
            ModalAlert(MODAL_HEADER, MSG_SUCCESS_DELETE_REC);
        },

        ValidateDuplicateFields: function () {
            var isValid = true;
            var isDuplicate = 0;

            if ($("#txtTotal").text() != "100%") {
                $("#divKPIPositionErrorMessage").html("<label class=\"errMessage\"><li>" + "Total Weight w/ Service Bay must be 100%." + "</li></label><br />");
                isValid = false;
            }
            if ($("#txtTotalNoServiceBay").text() != "100%") {
                $("#divKPIPositionErrorMessage").html("<label class=\"errMessage\"><li>" + "Total Weight w/o Service Bay must be 100%." + "</li></label><br />");
                isValid = false;
            }

            if (isValid) {
                $(".KPIDynamicFields").each(function (n1, x1) {
                    if (this.value != "") {
                        var currentVal = this.value;
                        var ctr = $(this).prop("id").replace("txtKPI_", "");
                        $("#ChildrenSetupDynamicFields_" + ctr + " #txtKPI_" + ctr).removeClass("errMessage");
                        $("#ChildrenSetupDynamicFields_" + ctr + " #txtWeight_" + ctr).removeClass("errMessage");
                        $("#ChildrenSetupDynamicFields_" + ctr + " #txtWeightNoServiceBay_" + ctr).removeClass("errMessage");

                        $(".KPIDynamicFields").not(this).filter(function () {
                            if (this.value == currentVal) {
                                $("#ChildrenSetupDynamicFields_" + ctr + " #txtKPI_" + ctr).addClass("errMessage");
                                $("#ChildrenSetupDynamicFields_" + ctr + " #txtWeight_" + ctr).addClass("errMessage");
                                $("#ChildrenSetupDynamicFields_" + ctr + " #txtWeightNoServiceBay_" + ctr).addClass("errMessage");
                                isDuplicate++;
                            }
                        });
                    }
                });

                if (isDuplicate > 0) {
                    $("#divKPIPositionErrorMessage").html("<label class=\"errMessage\"><li>" + DUPLICATE_HIGHLIGHTED_FIELDS + "</li></label><br />");
                    isValid = false;
                }
            }

            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        LoadKPIPosition: function () {
            var s = this;
            $("#DivChildrenSetupDynamicFields").html("");
            
            $(KPIPositionList).each(function (index, item) {
                s.AddChildrenSetupDynamicFields();

                $("#txtID_" + (index + 1)).val(item.ID);
                $("#txtKRAGroup_" + (index + 1)).val(item.KRAGroup);
                $("#txtKRASubGroup_" + (index + 1)).val(item.KRASubGroup);
                $("#txtKPI_" + (index + 1)).val(item.KPI);
                $("#hdnKPIID_" + (index + 1)).val(item.KPIID);
                $("#txtDescription_" + (index + 1)).val(item.KPIDescription);
                $("#txtWeight_" + (index + 1)).val(item.Weight);
                $("#txtWeightNoServiceBay_" + (index + 1)).val(item.WeightNoServiceBay);

                s.CalculateTotal("WeightDynamicFields", "txtTotal");
                s.CalculateTotal("WeightNoServiceBayDynamicFields", "txtTotalNoServiceBay");
            });
        },

        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);

            for (var i = 0; i < arr.length; i++) {
                if (parseFloat(arr[i].value))
                    total += parseFloat(arr[i].value || 0);
            }

            $("#" + totalID).text((Math.round(total * 100) / 100) + "%");
        },

        // Custom for KPI Position
        BindAutoComplete: function (id, url, noOfReturnedResults, hdnID, valueColumn, displayColumn, increment) {
            var _noOfReturnedResults = noOfReturnedResults;
            var _incrementDisplayedBy = 20;

            $("#" + id).focus(function () {
                _noOfReturnedResults = noOfReturnedResults;
                $("#" + id).autocomplete("search", $("#" + id).val() + " ");
            });

            $("#" + id).keyup(function () {
                _noOfReturnedResults = noOfReturnedResults;
            });

            $("#" + id).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: url,
                        type: "GET",
                        dataType: "json",
                        data: {
                            term: request.term,
                            topResults: _noOfReturnedResults // No of returned results
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item[displayColumn || "Description"],
                                        value: item[valueColumn || "Value"]
                                    };
                                }))
                            }
                            else {
                                ModalAlert(MODAL_HEADER, data.Result);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            ModalAlert(MODAL_HEADER, jqXHR.responseText);
                        }
                    });
                },
                select: function (event, ui) { // Event - triggers after selection on list
                    if (ui.item.label != null) {
                        $(this).val(ui.item.label);
                    }
                    return false;
                },
                change: function (event, ui) { // Event - triggers when the value of the textbox changed
                    if (ui.item == null) {
                        $("#" + hdnID).val(0)
                        $(this).val("");

                        $("#txtKRAGroup_" + increment).val("");
                        $("#txtKRASubGroup_" + increment).val("");
                        $("#txtDescription_" + increment).val("");
                        $("#txtWeight_" + increment).val("");
                        $("#txtWeightNoServiceBay_" + increment).val("");

                    } else {
                        $("#" + hdnID).val(ui.item.value)
                        $(this).val(ui.item.label);

                        var kpiVal = $.grep(AllKPIList, function (e, i) {
                            return e.ID == ui.item.value;
                        });

                        if (kpiVal.length > 0) {
                            $("#txtKRAGroup_" + increment).val(kpiVal[0].KRAGroup);
                            $("#txtKRASubGroup_" + increment).val(kpiVal[0].KRASubGroup);
                            $("#txtDescription_" +  increment).val(kpiVal[0].KPIDescription);
                        }

                    }
                },
                focus: function (event, ui) {
                    //$(this).val(ui.item.label);
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            $("#" + id).autocomplete("widget").scroll(function () {
                if (($(this).scrollTop() + $(this).innerHeight()) >= $(this)[0].scrollHeight) {
                    _noOfReturnedResults = _noOfReturnedResults + _incrementDisplayedBy;
                    $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                }
            });

            $("#" + id).autocomplete("widget").attr('style', 'max-height: 400px; overflow-y: auto; overflow-x: hidden;');
        },
    };

    objKPIPositionEditJS.Initialize();
});