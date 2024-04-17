var objAddReferenceJS;

$(document).ready(function () {
    objAddReferenceJS = {

        ReferenceCode: $("#divReferenceModal #hdnReferenceCode").val(),

        Initialize: function () {
            $("#divReferenceBodyModal .modal-header").mousedown(handle_mousedown);
            var s = this;
            s.ElementBinding();
            s.LoadReferenceListJQGrid({
                RefCode: s.ReferenceCode
            });
        },

        ElementBinding: function () {
            var s = this;

            $("#btnSaveReference").click(function () {
                if (objEMSCommonJS.ValidateBlankFields("#frmAddReference", "#divReferenceErrorMessage", objAddReferenceJS.ValidateDuplicateFields)) {
                    ModalConfirmation(MODAL_HEADER, MSG_CONFIRM,
                        "objEMSCommonJS.PostAjax(true \
                        , ReferencePostURL + objAddReferenceJS.ReferenceCode \
                        , new FormData($('#frmAddReference').get(0)) \
                        , '#divReferenceErrorMessage' \
                        , '#btnSaveReference' \
                        , objAddReferenceJS.SaveSuccessFunction); ", "function");
                }
            });

        },

        SaveSuccessFunction: function () {
            $("#frmAddReference").trigger("reset");
            objAddReferenceJS.LoadReferenceListJQGrid({
                RefCode: objAddReferenceJS.ReferenceCode
            });
        },

        LoadReferenceListJQGrid: function (param) {
            var s = this;
            Loading(true);
            $("#tblReferenceList").jqGrid("GridUnload");
            $("#tblReferenceList").jqGrid("GridDestroy");
            $("#tblReferenceList").jqGrid({
                url: GetReferenceValueListURL,
                postData: param,
                sortname: "",
                sortorder: "",
                selrow: "",
                pageNumber: 1,
                rowNum: 10,
                datatype: "json",
                mtype: "GET",
                colNames: ["", "", "Code", "Description"],
                colModel: [
                    { name: "", hidden: true },
                    { key: true, name: "ID", index: "ID", align: "center", sortable: false, formatter: objAddReferenceJS.EditLink },
                    { name: "Value", index: "Value", align: "left", sortable: false },
                    { name: "Description", index: "Description", align: "left", sortable: false },
                ],
                toppager: $("#divPager"),
                rowList: [],
                loadonce: true,
                viewrecords: true,
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
                emptyrecords: "No records to display",
                multiselect: false,
                //rowNumbers: true,
                width: "100%",
                height: "100%",
                sortable: true,
                loadComplete: function (data) {
                    Loading(false);
                    if (data.IsSuccess != undefined) {
                        if (!data.IsSuccess) {
                            $("#divErrorMessage").html("");
                            ModalAlert(MODAL_HEADER, data.Result);
                        }
                    }

                    if (data.rows != null) {
                        if (data.rows.length > 0) {
                            for (var i = 0; i < data.rows.length; i++) {
                            }
                        }
                        // Set width of columns depending on content
                        AutoSizeColumnJQGrid("tblReferenceList", data);
                        $("#tblReferenceList_subgrid").width(20);
                        $(".jqgfirstrow td:first").width(20);
                    }
                },
                loadError: function (xhr, status, error) {
                    ModalAlert(MODAL_HEADER, xhr.responseText);
                    Loading(false);
                },
            });

        },

        EditLink: function (cellvalue, options, rowObject) {
            return "<a href=\"\" class='jqgrid-id-link' onclick=\"return LoadPartial('" + ReferenceEditURL + "?ID=" + rowObject.ID + "', 'divReferenceBodyModal');\">Edit</a>";
        },
    };

    objAddReferenceJS.Initialize();
});