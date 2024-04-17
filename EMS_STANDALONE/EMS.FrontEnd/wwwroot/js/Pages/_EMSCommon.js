var objEMSCommonJS;

$(document).ready(function () {
    objEMSCommonJS = {

        Initialize: function () {
            var s = this;
            s.ElementBinding();
        },

        ElementBinding: function () {
            var s = this;
            // Close modal when esc button is clicked
            $("body").keyup(function (e) {
                if (e.keyCode == 27 /*esc*/) {

                    if ($("#divModal").css("display") == "block") {
                        $("#divModal").modal("hide");
                    }

                    else {
                        // Close lastest popped modal
                        $(this).find(".modal.fade[style*='display: block'] .close:last").click();
                    }
                }
            });

        },

        ValidateBlankFields: function (formID, divErrID, isNoBlankFunction, isSingleBlankFunction) {
            var isValid = true;
            var fields = $(formID + " .required-field");
            var blankFields = 0;

            $(formID + " " + divErrID).html("");
            $(formID + " .errMessage").removeClass("errMessage");
            fields.each(function (n, element) {
                if (($(this).val() || "").trim() == "") {
                    $(this).addClass("errMessage");
                    blankFields++;
                }
            });
            if (blankFields == 1) {
                $(formID + " .required-field.errMessage:first").focus();
                $(formID + " " + divErrID).html("<label class=\"errMessage\"><li>" + $(".required-field.errMessage")[0].title + SUFF_REQUIRED + "</li></label><br />");
                if (isSingleBlankFunction != null)
                    isSingleBlankFunction();

                isValid = false;
            }
            else if (blankFields > 1) {
                $(formID + " .required-field.errMessage:first").focus();
                $(formID + " " + divErrID).html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");
                isValid = false;
            }
            else if (blankFields <= 0) {
                if (isNoBlankFunction != null)
                    isValid = isNoBlankFunction();
            }
            if (!isValid)
                $("html, body").animate({ scrollTop: 0 }, "slow");

            return isValid;
        },

        PostAjax: function (isForm, url, input, divErrID, btnID, isSuccessFunction, isFailedFunction, NoModalAlert, isAsync) {
            var contentType = isForm ? false : "application/json;";
            var processData = isForm ? false : true;
            var s = this;
            $(btnID).prop("disabled", true);
            Loading(true);
            $.ajax({
                url: url,
                type: "POST",
                data: input,
                async: isAsync == null | isAsync == undefined ? false : isAsync,
                dataType: "json",
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    if (data.IsSuccess) {
                        if (isSuccessFunction != null)
                            isSuccessFunction(data);

                        if (!NoModalAlert)
                            ModalAlert(MODAL_HEADER, data.Result);

                        $(divErrID).html("");
                    }
                    else {
                        var msg = "";
                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        ModalAlert(MODAL_HEADER, msg);
                        if (isFailedFunction != null)
                            isFailedFunction();
                    }
                    $(btnID).prop("disabled", false);
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (isFailedFunction != null)
                        isFailedFunction();
                    Loading(false);
                    $(btnID).prop("disabled", false);
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                },
            });
        },

        PostAjaxWithBeforeSend: function (isForm, url, input, divErrID, btnID, isSuccessFunction
            , isFailedFunction, isBeforeSendFunction, NoModalAlert, isAsync) {
            var contentType = isForm ? false : "application/json;";
            var processData = isForm ? false : true;

            var s = this;
            $(btnID).prop("disabled", true);
            Loading(true);
            $.ajax({
                url: url,
                type: "POST",
                data: input,
                async: isAsync == null | isAsync == undefined ? false : isAsync,
                dataType: "json",
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    if (data.IsSuccess) {
                        if (isSuccessFunction != null)
                            isSuccessFunction(data);

                        if (!NoModalAlert)
                            ModalAlert(MODAL_HEADER, data.Result);

                        $(divErrID).html("");
                    }
                    else {
                        var msg = "";
                        if (data.IsListResult == true) {
                            for (var i = 0; i < data.Result.length; i++) {
                                msg += data.Result[i] + "<br />";
                            }
                        } else {
                            msg += data.Result;
                        }
                        ModalAlert(MODAL_HEADER, msg);
                        if (isFailedFunction != null)
                            isFailedFunction();
                    }
                    $(btnID).prop("disabled", false);
                    Loading(false);
                },
                beforeSend: function () {
                    if (isBeforeSendFunction != null)
                        isBeforeSendFunction();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (isFailedFunction != null)
                        isFailedFunction();
                    Loading(false);
                    $(btnID).prop("disabled", false);
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                },
            });
        },

        GetAjax: function (url, input, btnID, isSuccessFunction, isFailedFunction, isAsync) {
            $(btnID).prop("disabled", true);
            Loading(true);
            $.ajax({
                url: url,
                type: "GET",
                data: input,
                async: isAsync == null | isAsync == undefined ? false : isAsync,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (isSuccessFunction != null)
                        isSuccessFunction(data);

                    $(btnID).prop("disabled", false);
                    Loading(false);

                },
                error: function (jqXHR, textStatus, errorThrown) {

                    $(btnID).prop("disabled", false);

                    if (isFailedFunction != null)
                        isFailedFunction();

                    ModalAlert(MODAL_HEADER, jqXHR.responseText);

                    Loading(false);
                },
            });
        },

        GetAjaxNoLoading: function (url, input, btnID, isSuccessFunction, isFailedFunction, isAsync) {
            $(btnID).prop("disabled", true);
            $.ajax({
                url: url,
                type: "GET",
                data: input,
                async: isAsync == null | isAsync == undefined ? false : isAsync,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (isSuccessFunction != null)
                        isSuccessFunction(data);

                    $(btnID).prop("disabled", false);

                },
                error: function (jqXHR, textStatus, errorThrown) {

                    $(btnID).prop("disabled", false);

                    if (isFailedFunction != null)
                        isFailedFunction();

                    ModalAlert(MODAL_HEADER, jqXHR.responseText);

                },
            });
        },


        RemoveFromMultiSelect: function (divId, id) {
            $("#" + divId + " .selected_" + id).remove();
            $("#" + divId + " .hdn_selected_" + id).remove();
        },

        GetMultiSelectList: function (divId) {
            var val = "";
            var text = "";

            $("#" + divId + " .selected-item").each(function (n, i) {
                val += i.value + ",";
            });

            $("#" + divId + " .multiselect-item").each(function (n, i) {
                // .replace(/,/, "{_}") - replace comma with {_} to prevent splitting
                text += $(i).text().replace(/,/, "{_}") + ",";
            });

            var result = {
                value: val.substring(0, val.length - 1),
                text: text.substring(0, text.length - 1)
            };
            return result;
        },

        SetMultiSelectList: function (divId, storageValue, storageText) {
            var valueList, textList = [];
            valueList = localStorage[storageValue] != undefined ? localStorage[storageValue].split(",") : [];
            textList = localStorage[storageText] != undefined ? localStorage[storageText].split(",") : [];
            $(valueList).each(function (index, item) {
                if (item != "" && item != undefined) {
                    // .replace(/{_}/, ",") - replace {_} with comma to display the correct description
                    var textVal = textList[index].replace(/{_}/, ",").trim();
                    $("#lbl_" + item).prop("title", "delete");
                    $("#" + divId).append('<label class="multiselect-item" id="' + item + '" title="delete">' + textVal +
                        ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelectEnum(&quot;' + divId + '&quot;,&quot;' + item + '&quot;);"></span></label>');
                    $("#" + divId).append('<input type="hidden" class="selected-item" id="hdnSelected_' + item + '" value="' + item + '" />');

                }
            });
        },

        BindFilterMultiSelectAutoComplete: function (id, url, noOfReturnedResults, selectedDivId) {
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
                            topResults: _noOfReturnedResults // No of returned results
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
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelect(&quot;' + selectedDivId + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + selectedDivId).append('<input type="hidden" class="selected-item hdn_selected_' + id + '" value="' + id + '" />');
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

        BindFilterMultiSelectEnum: function (multiSelectID, url) {

            var GetSuccessFunction = function (data) {

                $("#" + multiSelectID).html("");
                $("#" + multiSelectID + "Option").html("");

                $(data.Result).each(function (index, item) {
                    $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item.ID + "\" title=\"add\">" + item.Description + "</label>");
                    $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item.ID + "\" id=\"hdn_" + item.ID + "\">");
                });
                //$(data.Result).each(function (index, item) {
                //    $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item.Description.replace(/ /g, "-") + "\" title=\"add\">" + item.Description + "</label>");
                //    $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item.ID + "\" id=\"hdn_" + item.Description.replace(/ /g, "_") + "\">");
                //});
                $("#" + multiSelectID + "Option .dropdown-item").click(function () {
                    var text = $(this).text();
                    var id = $(this).prop("id").replace("lbl_", "");
                    //var id = $(this).text().replace(/ /g, "_");
                    var title = $(this).attr("title")
                    if (title == "add") {
                        $(this).prop("title", "delete");
                        $("#" + multiSelectID).append('<label class="multiselect-item" title="delete" id="' + id + '">' + text +
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelectEnum(&quot;' + multiSelectID + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + multiSelectID).append('<input type="hidden" id="hdnSelected_' + id + '" class="selected-item" value="' + $("#hdn_" + id).val() + '" />');
                    }
                    else {
                        $("#" + multiSelectID + " > #" + id).remove();
                        $("#" + multiSelectID + " > #hdnSelected_" + id).remove();
                        $(this).prop("title", "add");
                    }
                });

            };

            objEMSCommonJS.GetAjax(url, {}, "", GetSuccessFunction);
        },

        BindFilterMultiSelectEnumValueDisplay: function (multiSelectID, url, valueColumn, displayColumn) {

            var GetSuccessFunction = function (data) {

                $("#" + multiSelectID).html("");
                $("#" + multiSelectID + "Option").html("");

                $(data.Result).each(function (index, item) {
                    $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item[valueColumn] + "\" title=\"add\">" + item[displayColumn] + "</label>");
                    $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item[valueColumn] + "\" id=\"hdn_" + item[valueColumn] + "\">");
                });
                //$(data.Result).each(function (index, item) {
                //    $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item[displayColumn].replace(/ /g, "-") + "\" title=\"add\">" + item[displayColumn] + "</label>");
                //    $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item[valueColumn] + "\" id=\"hdn_" + item[displayColumn].replace(/ /g, "_") + "\">");
                //});
                $("#" + multiSelectID + "Option .dropdown-item").click(function () {
                    var text = $(this).text();
                    var id = $(this).prop("id").replace("lbl_", "");
                    //var id = $(this).text().replace(/ /g, "_");
                    var title = $(this).attr("title")
                    if (title == "add") {
                        $(this).prop("title", "delete");
                        $("#" + multiSelectID).append('<label class="multiselect-item" id="' + id + '" title="delete" >' + text +
                            ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelectEnum(&quot;' + multiSelectID + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                        $("#" + multiSelectID).append('<input type="hidden" id="hdnSelected_' + id + '" class="selected-item" value="' + $("#hdn_" + id).val() + '" />');
                    }
                    else {
                        $("#" + multiSelectID + " > #" + id).remove();
                        $("#" + multiSelectID + " > #hdnSelected_" + id).remove();
                        $(this).prop("title", "add");
                    }
                });

            };

            objEMSCommonJS.GetAjax(url, {}, "", GetSuccessFunction);
        },

        BindFilterMultiSelectEnumLocalData: function (multiSelectID, data) {
            $("#" + multiSelectID).html("");
            $("#" + multiSelectID + "Option").html("");

            $(data).each(function (index, item) {
                $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item.ID + "\" title=\"add\">" + item.Description + "</label>");
                $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item.ID + "\" id=\"hdn_" + item.ID + "\">");
            });
            //$(data.Result).each(function (index, item) {
            //    $("#" + multiSelectID + "Option").append("<label class=\"dropdown-item multiselect-item\" id=\"lbl_" + item.Description.replace(/ /g, "-") + "\" title=\"add\">" + item.Description + "</label>");
            //    $("#" + multiSelectID + "Option").append("<input type=\"hidden\" value=\"" + item.ID + "\" id=\"hdn_" + item.Description.replace(/ /g, "_") + "\">");
            //});
            $("#" + multiSelectID + "Option .dropdown-item").click(function () {
                var text = $(this).text();
                var id = $(this).prop("id").replace("lbl_", "");
                //var id = $(this).text().replace(/ /g, "_");
                var title = $(this).attr("title")
                if (title == "add") {
                    $(this).prop("title", "delete");
                    $("#" + multiSelectID).append('<label class="multiselect-item" title="delete" id="' + id + '">' + text +
                        ' <span class="glyphicon glyphicon-remove multiselect-glyph-remove" onclick="objEMSCommonJS.RemoveFromMultiSelectEnum(&quot;' + multiSelectID + '&quot;,&quot;' + id + '&quot;);"></span></label>');
                    $("#" + multiSelectID).append('<input type="hidden" id="hdnSelected_' + id + '" class="selected-item" value="' + $("#hdn_" + id).val() + '" />');
                }
                else {
                    $("#" + multiSelectID + " > #" + id).remove();
                    $("#" + multiSelectID + " > #hdnSelected_" + id).remove();
                    $(this).prop("title", "add");
                }
            });
        },

        RemoveFromMultiSelectEnum: function (divId, id) {
            $("#" + divId + " #" + id).remove();
            $("#" + divId + " #hdnSelected_" + id).remove();
            $("#lbl_" + id).prop("title", "add");
        },

        SetSelectedDropDownDetailedByID: function (ddlID, hdnInput, setSelectedFunction) {
            $(ddlID + " option").each(function (index) {
                if ($(this).val() != "") {
                    if (JSON.parse($(this).val()).ID == $(hdnInput).val()) {
                        $(ddlID + ' option:eq(' + index + ')').prop('selected', true);
                        if (setSelectedFunction != null)
                            setSelectedFunction();
                    }
                }
            });
        },

        ChangeTab: function (e, showTab, div) {
            $((div == undefined ? "" : div) + " .tablinks").removeClass("active");
            $(e).addClass("active");
            $((div == undefined ? "" : div) + " .tabcontent").css({ "display": "none" });
            $((div == undefined ? "" : div) + " #" + showTab).css({ "display": "block" });
            return false;
        },

        ChangeTabForm: function (e, showTab, div) {
            $((div == undefined ? "" : div) + " .tablinksform").removeClass("active");
            $(e).addClass("active");
            $((div == undefined ? "" : div) + " .tabcontentform").css({ "display": "none" });
            $((div == undefined ? "" : div) + " #" + showTab).css({ "display": "block" });
            return false;
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

        DownloadAttachment: function (URL, repoPath, serverFile, sourceFile) {
            $.ajax({
                url: URL,
                type: "GET",
                data: { repoPath: repoPath, serverFile: serverFile },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.IsSuccess) {
                        window.location = DownloadFileURL + "&repoPath=" + repoPath + "&serverFile=" + serverFile + "&sourceFile=" + sourceFile;
                    }
                    else {
                        //ModalAlert(MODAL_HEADER, data.Result);

                        //Check Job App Attachment Online
                        objEMSCommonJS.DownloadAttachmentOnline(URL, "RecruitmentService_Attachment_Path_Online", serverFile, sourceFile);
                    }
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                    Loading(false);
                },
            });
        },

        DownloadAttachmentOnline: function (URL, repoPath, serverFile, sourceFile) {
            $.ajax({
                url: URL,
                type: "GET",
                data: { repoPath: repoPath, serverFile: serverFile },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.IsSuccess) {
                        window.location = DownloadFileURL + "&repoPath=" + repoPath + "&serverFile=" + serverFile + "&sourceFile=" + sourceFile;
                    }
                    else {
                        //ModalAlert(MODAL_HEADER, data.Result);
                        window.open("https://forms.motortrade.com.ph/assets/attachments/applicants_resume/" + serverFile);
                    }
                    Loading(false);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    ModalAlert(MODAL_HEADER, jqXHR.responseText);
                    Loading(false);
                },
            });
        },

        UploadModal: function (url, header, sampleFormURL) {
            $("#divModalErrorMessage").html("");
            $('#fileUpload').removeClass("errMessage");

            var button = "<button type=\"button\" id=\"btnUploadFile\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objEMSCommonJS.UploadFile('" + url + "');\">Upload</button>";

            if (header != "")
                header += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>";
            $("#pheader").html(header);

            if (sampleFormURL != "")
                button += "&nbsp <button type=\"button\" id=\"btnDownloadForm\" class=\"btnBlue formbtn button-width-auto\" onclick=\"objEMSCommonJS.UploadModalDownloadTemplate('" + sampleFormURL + "');\">Download Template</button>";

            $("#fileUpload").val("");
            $("#divButtonUploadModal").html(button);
            $("#divUploadModal").modal("show");
        },

        UploadModalDownloadTemplate: function (url) {
            Loading(true);
            var newWin = window.open(url, "_blank");
            newWin.onunload = function () {
                Loading(false);
            };
            newWin.document.close(); // necessary for IE >= 10
            newWin.focus();
        },

        UploadSuccessFunction: function () {
            $("#btnSearch").click();

            // THIS FUNCTION IS TO RESET THE ACCOUNTABILITY LIST ON EMPLOYEE
            objAccountabilityJS.LoadAccountabilityJQGrid({
                EmployeeID: $("#hdnID").val()
            });
        },

        UploadFile: function (url) {
            var fileExtension = ['xls', 'xlsx'];
            var filename = $('#fileUpload').val();
            var filedata = new FormData();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;

            //--- Validation for excel file---  
            if (filename.length == 0) {
                $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select a file.</li></label><br />");
                return false;
            }
            else {
                var extension = filename.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtension) == -1) {
                    $("#divModalErrorMessage").html("<label class=\"errMessage\"><li>Please select only excel files.</li></label><br />");
                    return false;
                }
                else {
                    $("#divModalErrorMessage").html("");
                }
            }

            filedata.append(files[0].name, files[0]);

            objEMSCommonJS.PostAjax(true, url, filedata, "#divModalErrorMessage", "", objEMSCommonJS.UploadSuccessFunction, null, null, true);
            $("#divUploadModal").modal("hide");
        },

        BindAutoComplete: function (id, url, noOfReturnedResults, hdnID, valueColumn, displayColumn, isSuccessFunction) {
            var _noOfReturnedResults = noOfReturnedResults;
            var _incrementDisplayedBy = 20;

            //$("#" + id).focus(function () {
            //    _noOfReturnedResults = noOfReturnedResults;
            //    $("#" + id).autocomplete("search", $("#" + id).val() + " ");
            //});

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
                        $("#" + hdnID).val(0);
                        $(this).val("");
                    } else {
                        $("#" + hdnID).val(ui.item.value);
                        $(this).val(ui.item.label);
                        if (isSuccessFunction != null) {
                            isSuccessFunction(ui.item.value);
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

        JQGridIDFormat: function (str) {
            var s = this;
            var max = 7;
            str = str.toString();
            return str.length < max ? s.JQGridIDFormat("0" + str, max) : str;
        },

        ShowPrintModal: function (div) {
            $("#" + div).show();
            var divToPrint = document.getElementById(div);
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);

            $("#" + div).hide();

            // Disable Shortcut for DevTools
            newWin.document.addEventListener('contextmenu', event => event.preventDefault());
            newWin.document.onkeydown = function (e) {
                if (e.keyCode == 123) {
                    return false;
                }
                if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
                    return false;
                }
                if (e.ctrlKey && e.shiftKey && e.keyCode == 'C'.charCodeAt(0)) {
                    return false;
                }
                if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
                    return false;
                }
                if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
                    return false;
                }
            }

            newWin.document.close(); // necessary for IE >= 10
            newWin.focus(); // necessary for IE >= 10
            newWin.print();

        },

        JobPosting: function () {
            if ($("#cbIsAvailableOnline").prop("checked") == true) {
                $("#tabJobAppLink").show();
                $("#txtJobPosition").addClass("required-field");
                $("#txtJobLocation").addClass("required-field");
                $("#SummerNoteJobDescription").addClass("required-field");
                $("#SummerNoteJobQualification").addClass("required-field");
            }
            else {
                $("#tabJobAppLink").hide();
                objEMSCommonJS.ChangeTab($(".tablinks:nth-child(1)"), 'tabApprovalHistory');
                $("#txtJobPosition").removeClass("required-field");
                $("#txtJobLocation").removeClass("required-field");
                $("#SummerNoteJobDescription").removeClass("required-field");
                $("#SummerNoteJobQualification").removeClass("required-field");
                //$("#txtJobPosition").val("");
                //$("#txtJobLocation").val("");
                //$("#JobDescription").summernote("reset");
                //$("#JobQualification").summernote("reset");
            } 
        },

        removeItemAll: function (arr, value) {
            var i = 0;
            while (i < arr.length) {
                if (arr[i] === value) {
                    arr.splice(i, 1);
                } else {
                    ++i;
                }
            }
            return arr;
        },

        GetEmployeeDetails: function () {
            $.ajax({
                url:'/index?handler=EmployeeDetails',
                type: "GET",
                success: function (data) {
                    var x = JSON.stringify(data);
                    console.log(data.Result);
                },
                error: function (error) {
                    console.log(`Error ${error}`);
                }
            });
        },

    };

    objEMSCommonJS.Initialize();
});