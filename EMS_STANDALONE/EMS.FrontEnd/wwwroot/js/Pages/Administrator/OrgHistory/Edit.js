var objOrgHistoryEditJS;

$(document).ready(function () {
    objOrgHistoryEditJS = {

        Initialize: function () {
            $("#divOrgGroupHistoryBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this
            s.ElementBinding();
            s.LoadJQGrid($("#hdnTDate").val(), $("#hdnIsLatest").val());

            $("#btnSave").prop("hidden", false);
            $("#btnBack").prop("hidden", false);
        },

        ElementBinding: function () {
            var s = this;

            $("#btnBack").click(function () {
                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_TO_CLOSE,
                    "objOrgHistoryEditJS.BackModal('true')",
                    "function");
            });

            $("#closeModal").click(function () {

                ModalConfirmation(MODAL_HEADER, MSG_CONFIRM_TO_CLOSE,
                    "objOrgHistoryEditJS.CloseModal('true')",
                    "function");

                //$("#divOrgGroupHistoryModal").modal("hide");
            });

            $("#btnSave").click(function () {
                if ($("#txtDateEffective").val() != "") {
                    //TO CLOSE EDITABLE
                    $("#tblOrgHistoryViewList").jqGrid('editCell', 0, 0, false);
                    //TO SAVE
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objOrgHistoryEditJS.SendAjax()",
                        "function");

                    /*ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , OrgHistoryAddURL \
                        , objOrgHistoryEditJS.GetFormData() \
                        , '#divEmployeeErrorMessage' \
                        , '#divEmployeeModal #btnSave' \
                        , '');",
                        "function");*/
                }
                else
                    $("#divOrgGroupHistoryErrorMessage").html("<label class=\"errMessage\"><li> Effective Date" + SUFF_REQUIRED + "</li></label><br />");
            });



        },

        CloseModal: function (Close) {
            if (Close == 'true') {
                $("#divModal").modal("hide");
                $("#divOrgGroupHistoryModal").modal("hide");
            }
        },

        BackModal: function (Close) {
            if (Close == 'true') {
                $("#divModal").modal("hide");
                //$("#divOrgGroupHistoryModal").modal("hide");
                LoadPartial(OrgHistoryViewURL + "?TDate=" + $("#hdnTDate").val() + "&IsLatest=" + $("#hdnIsLatest").val(), 'divOrgGroupHistoryBodyModal');
            }
        },

        LoadJQGrid: function (TDate,IsLatest) {
            var s = this;
            var param = {
                TDate: TDate,
                IsLatest: IsLatest
            };

            Loading(true);

            var tableInfo = localStorage.getItem("tblOrgHistoryViewList") == "" ? "" : $.parseJSON(localStorage.getItem("tblOrgHistoryViewList"));

            var moveFilterFields = function () {
                var intialHeight = $("#divOrgGroupTableViewList .jqgfirstrow").height();
                // Move filter fields from the first row of the JQGrid back to the filterContainer
                $("#divOrgGroupTableViewList .jqgfirstrow td .filterFields").each(function (n, element) {
                    $(this).appendTo("#filterFieldsContainer");
                });
                $("#tblOrgHistoryViewList .jqgfirstrow").css({ "height": intialHeight + "px" });

                $("div.filterFields").unbind("keyup");
                $("div.filterFields").keyup(function (e) {
                    if (e.keyCode == "13") {
                        $("#divOrgGroupTableViewList #btnSearch").click();
                    }
                });
            }
            moveFilterFields();

            $("#tblOrgHistoryViewList").jqGrid("GridUnload");
            $("#tblOrgHistoryViewList").jqGrid("GridDestroy");
            $("#tblOrgHistoryViewList").jqGrid({
                url: OrgHistoryEditListURL,
                postData: param,
                sortname: tableInfo == null ? "" : tableInfo.sortname,
                sortorder: tableInfo == null ? "" : tableInfo.sortorder,
                selrow: tableInfo == null ? "" : tableInfo.selrow,
                pageNumber: tableInfo == null ? 1 : tableInfo.page,
                rowNum: tableInfo == null ? 10 : tableInfo.rowNum,
                datatype: "json",
                mtype: "GET",
                colNames: ["ID", "", "", "", "", "Org Type", "Org Group", "Old Value", "New Value",""],
                colModel: [
                    { hidden: true, key: true, name: "ID", index: "ID", align: "center", sortable: true },
                    { hidden: true, name: "Code", index: "Code", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "Description", index: "Description", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "ParentOrgId", index: "ParentOrgId", editable: false, align: "left", sortable: true },
                    { hidden: true, name: "ParentDescription", index: "ParentDescription", editable: false, align: "left", sortable: true },
                    { name: "OrgType", index: "OrgType", editable: false, align: "center", sortable: true },
                    { name: "CodeDescription", index: "CodeDescription", editable: false, align: "left", sortable: true },
                    { name: "ParentDescriptionDisplay", index: "ParentDescriptionDisplay", editable: false, align: "left", sortable: true },
                    { formatter: objOrgHistoryEditJS.AddLink, name: "ParentCodeDescription", index: "ParentCodeDescription", align: "left", sortable: true },
                    { hidden: true, name: "ParentCodeDescriptionValue", index: "ParentCodeDescriptionValue", align: "left", sortable: true },
                ], 
                toppager: $("#divPager"),
                rowList: SetRowList(),
                loadonce: true,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false
                },
                emptyrecords: "No records to display",
                multiselect: false,
                rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                ignoreCase: true,
                cellEdit: true,
                cellsubmit: 'clientArray',
                /*ondblClickRow: function (rowId, iRow, iCol, e) {
                    //$("#tblOrgHistoryViewList tr:nth-child(" + (iRow + 1) + ") .jqgrid-id-link").click();

                    $("#tblOrgHistoryViewList").setColProp('ParentCodeDescription', { editable: true });
                    //objOrgHistoryEditJS.OrgGroupAutoComplete(iRow);
                    //$("#1_ParentCodeDescription").addClass("form-control").css("width", "100%");
                },
                afterEditCell: function (rowid, cellname, value, iRow, iCol) {
                    objOrgHistoryEditJS.BindAutoComplete(iRow + "_ParentCodeDescription"
                        , OrgHistoryAutoCompleteURL
                        , 20, "hdnParentOrgID", "ID", "Description");
                }, 
                afterSaveCell: function (rowid, cellname, value, iRow, iCol) {
                    $("#tblOrgHistoryViewList").jqGrid('setCell', rowid, 'ParentCodeDescriptionValue', $("#hdnParentOrgID").val());
                },
                beforeEditCell: function (rowid, cellname, value, iRow, iCol) {
                    $("#tblOrgHistoryViewList").setColProp('ParentCodeDescription', { editable: false });
                },
                beforeSaveCell: function (rowid, cellname, value, iRow, iCol) {
                    //alert("asd");
                },*/
                loadComplete: function (data) {
                    Loading(false);

                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    //$("#tblOrgHistoryViewList").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: "cn", ignoreCase: true });
                    //$("#gs_Code").addClass("form-control");

                    if (data.rows != null) {

                        $("#txtDateEffective").datetimepicker({
                            useCurrent: true,
                            format: 'DD-MMM-YYYY',
                            //minDate: data.tdate,
                            //maxDate: new Date(),
                            defaultDate: $("#hdnTDate").val()
                            //disabledDates: ["01-Nov-2022", "02-Nov-2022"]
                        });

                        // Set width of columns depending on content
                        //AutoSizeColumnJQGrid("tblOrgHistoryViewList", data);

                        // Move filter fields from filterContainer to the first row of the JQGrid
                        $("#filterFieldsContainer .filterFields").each(function (n, element) {
                            $(this).appendTo("#divOrgGroupTableList .jqgfirstrow td:nth-child(" + (n + 2) + ")");
                        });

                        var grid = $("#tblOrgHistoryViewList").jqGrid('getDataIDs');
                        $(grid).each(function (index, n) {
                            var rowData = $("#tblOrgHistoryViewList").jqGrid('getRowData', n);
                            if ($(rowData.ParentCodeDescription).val() != rowData.ParentDescriptionDisplay)
                                $("#tblOrgHistoryViewList").jqGrid('setRowData', rowData.ID, false, { background: '#ecd505' });
                        });

                        $(".EditParent").each(function (index, n) {
                            objOrgHistoryEditJS.BindAutoComplete(n.id
                                , OrgHistoryAutoCompleteURL
                                , 20, "hdnParentOrgID", "ID", "Description", n.value);
                        });
                    }

                    objOrgHistoryEditJS.ShowHideFilter($("#tblOrgHistoryViewList"));

                    $("#divOrgGroupHistoryModal #chkFilterOrg").on('change', function () {
                        objOrgHistoryEditJS.ShowHideFilter($("#tblOrgHistoryViewList"));
                        localStorage["OrgGroupListFilterOption"] = $("#chkFilterOrg").is(":checked");
                    });

                    //var localGridData = $("#tblOrgHistoryViewList").jqGrid('getGridParam', 'data');

                    
                    $("#gs_ID").css("display", "none");
                    $("#gs_OrgType").addClass("form-control").css("width", "100%");
                    $("#gs_CodeDescription").addClass("form-control").css("width", "100%");
                    $("#gs_ParentDescriptionDisplay").addClass("form-control").css("width", "100%");
                    $("#gs_ParentCodeDescription").addClass("form-control").css("width", "100%");
                    //$("td[aria-describedby='tblOrgHistoryViewList_ParentCodeDescription']").addClass("form-control").css("width", "100%");

                    // set minimum height to prevent multiselect tags from being hidden by the scroll
                    $(".ui-jqgrid-bdiv").css({ "min-height": "350px" });

                    $("#btnReset").click(function () {
                        $("#gs_CodeDescription").val("");
                        $("#gs_ParentCodeDescription").val("");
                        $("#tblOrgHistoryViewList").jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid");
                    });

                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
                beforeRequest: function () {
                    GetJQGridState("tblOrgHistoryViewList");
                    moveFilterFields();
                },
            }).navGrid("#divPager",
                { edit: false, add: false, del: false, search: false, refresh: false }
            ).jqGrid('filterToolbar', {
                stringResult: true,
                searchOnEnter: false,
                defaultSearch: "cn"
            });
            $("#divOrgGroupHistoryModal .ui-pg-selbox").closest("select").before("<label class=\"ui-row-label\">Show</label>");
            $("#divOrgGroupHistoryModal .ui-row-label").closest("label").before("<label class=\"ui-row-label\" id=\"lblFilter\">Show Filter</label>");
            $("#divOrgGroupHistoryModal #lblFilter").after("<input type=\"checkbox\" id=\"chkFilterOrg\" style=\"margin-right:15px;\"></div>");
        },

        OrgGroupAutoComplete: function (iRow) {
            objEMSCommonJS.BindAutoComplete(iRow + "_ParentCodeDescription"
                , OrgHistoryAutoCompleteURL
                , 20, "hdnParentOrgID", "ID", "Description");
        },

        ShowHideFilter: function (table) {
            if ($("#chkFilterOrg").is(":checked")) {
                $(".ui-search-toolbar").css("display", "");
            }
            else if ($("#chkFilterOrg").is(":not(:checked)")) {
                $(".ui-search-toolbar").css("display", "none");
            }
        },

        SendAjax: function () {
            var localGridData = $("#tblOrgHistoryViewList").jqGrid('getGridParam', 'data');

            //console.log(JSON.stringify(localGridData));return

            /*for (var data in localGridData) {
                if (localGridData[data]["ParentCodeDescriptionValue"] === null || localGridData[data]["ParentCodeDescriptionValue"] === undefined) {
                    delete localGridData[data];
                }
            }*/
            $("#btnSave").prop("disabled", true);
            $.ajax({
                type: "POST",
                dataType: 'json',
                async: true,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                url: OrgHistoryEditURL + "?TDate=" + $("#txtDateEffective").val() + "&IsLatest=" + ($("#hdnIsLatest").val() == "YES" ? true: false),
                data: JSON.stringify(localGridData),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    ModalAlert(MODAL_HEADER, data);
                    $("#divOrgGroupHistoryModal").modal("hide");
                    $("#btnSave").prop("disabled", false);
                    $("#btnSearch").click();
                }
            });
        },

        BindAutoComplete: function (id, url, noOfReturnedResults, hdnID, valueColumn, displayColumn, OldValue,isSuccessFunction) {
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
                change: function (event, ui) { // Event - triggers when the value of the textbox changed);
                    if (ui.item == null) {
                        $("#" + hdnID).val(0);
                        $(this).val("");

                        $("#" + id).val(OldValue);
                    } else {
                        $("#" + hdnID).val(ui.item.value);
                        $(this).val(ui.item.label);
                        var localGridId = id.replace("_ParentCodeDescription", "");
                        $("#tblOrgHistoryViewList").jqGrid('setCell', localGridId, 'ParentCodeDescription', ui.item.label);
                        $("#tblOrgHistoryViewList").jqGrid('setCell', localGridId, 'ParentCodeDescriptionValue', ui.item.value);
                        $("#tblOrgHistoryViewList").trigger('reloadGrid');
                        $("#tblOrgHistoryViewList").jqGrid('setRowData', localGridId, false, { background: '#ecd505' }); 
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

        AddLink: function (cellvalue, options, rowObject) {
            return "<input type=\"text\" class=\"form-control EditParent\" value=\"" + rowObject.ParentCodeDescription + "\" id=\"" + rowObject.ID + "_ParentCodeDescription\"/>";
            /*//TO ADD DYNAMIC EDITABLE
            if ($("#AllowEdit").val().split(",").includes(rowObject.OrgType))
                return "<input type=\"text\" class=\"form-control EditParent\" value=\"" + rowObject.ParentCodeDescription + "\" id=\"" + rowObject.ID + "_ParentCodeDescription\"/>";
            else
                return "<span>" + rowObject.ParentCodeDescription +"</span>";*/
        },

        /*GetFormData: function () {

            var formData = new FormData($('#frmOrgGroupHistory').get(0));
            $(".EditParent").each(function (index) {
                if ($(this).val() != "") {
                    formData.append("OrgGroup.[" + index + "].OrgGroupParentID", $(this).val());
                }
            });
            return formData;
        }*/
    };

    objOrgHistoryEditJS.Initialize();
});