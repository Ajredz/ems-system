var objEmployeeScoreAddJS;

//var scores = [];
//var employees = [];

//var ctr = 0;
//var totalProgress = 0;



$(document).ready(function () {
    objEmployeeScoreAddJS = {

        Initialize: function () {
            $("#divRunEmployeeScoreBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.CheckOnGoingTransaction();
            //$("#txtDateTo").val("06/01/2020");
        },

        AddSuccessFunction: function (data) {

            isRunOnGoing = false;
            if (data.Result.TransSummaryID != 0) {
                $("#btnSearch").click();
                $("#divRunEmployeeScoreModal").modal("hide");
                objEmployeeScoreListJS.ViewSummary(data.Result.TransSummaryID, false);
            }
            else {
                ModalAlert(MODAL_HEADER, data.Result.Message);
            }
        },

        CheckOnGoingTransaction: function () {
            isRunOnGoing = true;
            isCheckOnLoad = true;
            clearTimeout(timer);
            objEmployeeScoreListJS.DisplayProgressBar("#divRunEmployeeScoreModal", "#frmRunEmployeeScore");

        },

        ElementBinding: function () {
            var s = this;

            $("#txtEmployee").attr("readonly", true);
            $("#multiSelectedEmployee").attr("readonly", true);
            $("#multiSelectedEmployee").html("");
            $("#hdnEmployeeID").val("All");

            $("#txtRunOrgGroup").attr("readonly", true);
            $("#multiSelectedRunOrgGroup").attr("readonly", true);
            $("#multiSelectedRunOrgGroup").html("");
            $("#hdnFilterID").val("All");

            $("#cbIncludeAllLevelsBelow").prop("checked", true);
            $("#cbUseCurrent").prop("checked", true);
            $("#cbUseCurrent").change(function () {

                if ($("#cbUseCurrent").is(':checked')) {
                    $("#divUseCurrentWarning").hide();
                }
                else {
                    $("#divUseCurrentWarning").show();
                }

            });

            $("#cbIncludeSecDesig").change(function () {
                if ($("#cbIncludeSecDesig").is(':checked')) {
                    $("#divIncludeSecDesigWarning").show();
                }
                else {
                    $("#divIncludeSecDesigWarning").hide();
                }

            });

            var year = new Date().getFullYear() - 1;
            var startOfYear = new Date("01/01/" + year);
            var endOfYear = new Date("12/31/" + year);

            $("#txtDateFrom").datetimepicker({
                date: startOfYear,
                format: 'MM/DD/YYYY'
            });

            $("#txtDateTo").datetimepicker({
                date: endOfYear,
                format: 'MM/DD/YYYY'
            });

            $('#txtDateFrom, #txtDateTo').datetimepicker().on('dp.show', function () {
                $('#divRunEmployeeScoreModal .modal-body').css({ 'overflow': 'visible' });
                $('#divRunEmployeeScoreModal.modal').css({ 'overflow': 'visible' });
            }).on('dp.hide', function () {
                $('#divRunEmployeeScoreModal .modal-body').css({ 'overflow': 'auto' });
                $('#divRunEmployeeScoreModal.modal').css({ 'overflow': 'auto' });
            });

            $("#ddlFilter").change(function () {

                var filter = $("#ddlFilter").val();
                $("#hdnFilterID").val("");

                if (filter == "OPS" ||
                    filter == "REG" ||
                    filter == "AREA" ||
                    filter == "CLUS" ||
                    filter == "BRN" ||
                    filter == "DEPT")
                {
                    $(".divOrgGroup").show();
                    $("#divPosition").hide();
                    $(".divEmployee").show();
                }
                else if (filter == "POS") {
                    $("#divPosition").show();
                    $(".divOrgGroup").hide();
                    $(".divEmployee").show();
                }
                else {
                    $(".divOrgGroup").hide();
                    $("#divPosition").hide();
                    $(".divEmployee").hide();
                    $("#hdnFilterID").val("0");
                }

                $("#multiSelectedRunOrgGroup").html("");

            });

            $("#ddlPosition").change(function () {
                $("#hdnFilterID").val($("#ddlPosition").val());
            });

            $('input[type=radio][name=cbEmployee]').change(function () {
                if (this.value == 'All') {
                    $("#txtEmployee").attr("readonly", true);
                    $("#multiSelectedEmployee").attr("readonly", true);
                    $("#multiSelectedEmployee").html("");
                    $("#hdnEmployeeID").val("All");
                }
                else if (this.value == 'Specific') {
                    $("#txtEmployee").attr("readonly", false);
                    $("#multiSelectedEmployee").attr("readonly", false);
                    $("#multiSelectedEmployee").html("");
                    $("#hdnEmployeeID").val("");
                }
            });

            $('input[type=radio][name=cbOrgGroup]').change(function () {
                if (this.value == 'All') {
                    $("#txtRunOrgGroup").attr("readonly", true);
                    $("#multiSelectedRunOrgGroup").attr("readonly", true);
                    $("#multiSelectedRunOrgGroup").html("");
                    $("#hdnFilterID").val("All");
                }
                else if (this.value == 'Specific') {
                    $("#txtRunOrgGroup").attr("readonly", false);
                    $("#multiSelectedRunOrgGroup").attr("readonly", false);
                    $("#multiSelectedRunOrgGroup").html("");
                    $("#hdnFilterID").val("");
                }
            });

            $("#divRunEmployeeScoreModal #btnSave").click(function () {

                $("#divEmployeeScoreErrorMessage").html("");
                $("#txtNewDescription").removeClass("errMessage");
                if ($("#txtNewDescription").val() != "") {

                    ModalConfirmation(MODAL_HEADER,
                        MSG_CONFIRM,
                        "objEMSCommonJS.PostAjaxWithBeforeSend(true \
                        , EmployeeScoreAddPostURL \
                        , objEmployeeScoreAddJS.GetFormData() \
                        , '#divEmployeeScoreErrorMessage' \
                        , '#divRunEmployeeScoreModal #btnSave' \
                        , objEmployeeScoreAddJS.AddSuccessFunction \
                        , null \
                        , objEmployeeScoreAddJS.BeforeSendFunction \
                        , true \
                        , true);",
                        "function");
                }
                else {
                    $("#divEmployeeScoreErrorMessage").html("<label class=\"errMessage\"><li>Report Name" + SUFF_REQUIRED + "</li></label><br />");
                    $("#txtNewDescription").addClass("errMessage");
                }
            });

            objEmployeeScoreAddJS.BindOrgGroupMultiSelectAutoComplete("txtRunOrgGroup"
                , GetOrgGroupFilteredAutoCompleteURL, 20, "multiSelectedRunOrgGroup");

            objEmployeeScoreAddJS.BindEmployeeFilterMultiSelectAutoComplete("txtEmployee"
                , GetEmployeeFilteredAutoCompleteURL, 20, "multiSelectedEmployee");
        },

        BeforeSendFunction: function () {
            $("#divModal").modal("hide");
            isRunOnGoing = true;
            isCheckOnLoad = false;
            objEmployeeScoreListJS.DisplayProgressBar('#divRunEmployeeScoreModal', '#frmRunEmployeeScore');
        },

        GetFormData: function () {
            Loading(true);
            var formData = new FormData($('#frmRunEmployeeScore').get(0));

            formData.append("RunScoreForm.Filter", $("#ddlFilter").val());
            formData.append("RunScoreForm.IDs", $("#hdnFilterID").val());
            formData.append("RunScoreForm.Employees", $("#hdnEmployeeID").val());
            formData.append("RunScoreForm.DateFrom", $("#txtDateFrom").val());
            formData.append("RunScoreForm.DateTo", $("#txtDateTo").val());
            formData.append("RunScoreForm.Description", $("#txtNewDescription").val());
            formData.append("RunScoreForm.UseCurrent", $("#cbUseCurrent").is(':checked'));
            formData.append("RunScoreForm.RegularOnly", $("#cbRegularOnly").is(':checked'));
            formData.append("RunScoreForm.IncludeAllLevelsBelow", $("#cbIncludeAllLevelsBelow").is(':checked'));
            formData.append("RunScoreForm.Override", $("#cbOverride").is(':checked'));

            return formData;

        },

        RemoveFromMultiSelect: function (divId, id) {
            $("#" + divId + " .selected_" + id).remove();
            $("#" + divId + " .hdn_selected_" + id).remove();
            $("#hdnFilterID").val(objEMSCommonJS.GetMultiSelectList("multiSelectedRunOrgGroup").value);
        },


        BindOrgGroupMultiSelectAutoComplete: function (id, url, noOfReturnedResults, selectedDivId) {
            var _noOfReturnedResults = noOfReturnedResults;
            var _incrementDisplayedBy = 20;

            // isFocus - to prevent overlapping of focus and click events
            var isFocus = false;
            $("#" + id).focus(function () {
                _noOfReturnedResults = noOfReturnedResults;
                $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                isFocus = true;
                $("#" + id).unbind("click");
                $("#" + id).click(function () {
                    if (!isFocus) {
                        _noOfReturnedResults = noOfReturnedResults;
                        $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                    }
                    isFocus = false;
                });
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
                            topResults: _noOfReturnedResults, // No of returned results
                            filter: $("#ddlFilter").val()
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
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
                        $(this).val("");
                        var id = ui.item.value;
                        $("#" + selectedDivId).focus();
                        $("#" + selectedDivId).append('<label class="multiselect-item selected_' + id + '" title="delete">' + ui.item.label +
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEmployeeScoreAddJS.RemoveFromMultiSelect(&quot;' + selectedDivId + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + selectedDivId).append('<input type="hidden" class="selected-item hdn_selected_' + id + '" value="' + id + '" />');

                        $("#hdnFilterID").val(objEMSCommonJS.GetMultiSelectList("multiSelectedRunOrgGroup").value);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            // reload additional results if lowest scroll is detected 
            $("#" + id).autocomplete("widget").scroll(function () {
                //    $(this).scrollTop() + (window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                //) + " | innerHeight = " + $(this).innerHeight() + " | scrollHeight = " + $(this)[0].scrollHeight);
                if (
                    (
                        (
                            $(this).scrollTop() + Math.abs(window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                        ) + $(this).innerHeight()
                    ) >= $(this)[0].scrollHeight) {
                    _noOfReturnedResults = _noOfReturnedResults + _incrementDisplayedBy;
                    $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                }
            });

            $("#" + id).autocomplete("widget").attr('style', 'max-height: 400px; overflow-y: auto; overflow-x: hidden;');
        },

        BindEmployeeFilterMultiSelectAutoComplete: function (id, url, noOfReturnedResults, selectedDivId) {
            var _noOfReturnedResults = noOfReturnedResults;
            var _incrementDisplayedBy = 20;

            // isFocus - to prevent overlapping of focus and click events
            var isFocus = false;
            $("#" + id).focus(function () {
                _noOfReturnedResults = noOfReturnedResults;
                $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                isFocus = true;
                $("#" + id).unbind("click");
                $("#" + id).click(function () {
                    if (!isFocus) {
                        _noOfReturnedResults = noOfReturnedResults;
                        $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                    }
                    isFocus = false;
                });
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
                            topResults: _noOfReturnedResults, // No of returned results
                            filter: $("#ddlFilter").val(),
                            filterID: $("#hdnFilterID").val()
                            //filterID: objEMSCommonJS.GetMultiSelectList("multiSelectedRunOrgGroup").value
                        },
                        success: function (data) {
                            if (data.IsSuccess) {
                                response($.map(data.Result, function (item) {
                                    return {
                                        label: item.Description,
                                        value: item.ID
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
                        $(this).val("");
                        var id = ui.item.value;
                        $("#" + selectedDivId).focus();
                        $("#" + selectedDivId).append('<label class="multiselect-item selected_' + id + '" title="delete">' + ui.item.label +
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEmployeeScoreAddJS.RemoveFromMultiSelect(&quot;' + selectedDivId + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + selectedDivId).append('<input type="hidden" class="selected-item hdn_selected_' + id + '" value="' + id + '" />');

                        $("#hdnEmployeeID").val(objEMSCommonJS.GetMultiSelectList("multiSelectedEmployee").value);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    event.preventDefault(); // Prevent the default focus behavior.
                }
            });

            // reload additional results if lowest scroll is detected 
            $("#" + id).autocomplete("widget").scroll(function () {
                //    $(this).scrollTop() + (window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                //) + " | innerHeight = " + $(this).innerHeight() + " | scrollHeight = " + $(this)[0].scrollHeight);
                if (
                    (
                        (
                            $(this).scrollTop() + Math.abs(window.devicePixelRatio * 100 - 100 /*Subtract pixel ration from Zooming the page*/)
                        ) + $(this).innerHeight()
                    ) >= $(this)[0].scrollHeight) {
                    _noOfReturnedResults = _noOfReturnedResults + _incrementDisplayedBy;
                    $("#" + id).autocomplete("search", $("#" + id).val() + " ");
                }
            });

            $("#" + id).autocomplete("widget").attr('style', 'max-height: 400px; overflow-y: auto; overflow-x: hidden;');
        },
    };

    objEmployeeScoreAddJS.Initialize();
});