var objChartJS;
var objOrgGroupListJS;

const GetOrgChart = window.location.pathname;
const OrgGroupListURL = window.location.pathname + "?handler=List";
const OrgTypeAutoCompleteURL = window.location.pathname + "?handler=OrgTypeAutoComplete";
const OrgGroupAddURL = window.location.pathname.replace("/chart", "") + "/Add";
const OrgGroupViewURL = window.location.pathname.replace("/chart", "") + "/View";
const GetOrgGroupChildrenURL = window.location.pathname.replace("/chart", "") + "/View?handler=ChildrenOrgDropDown";
const GetOrgGroupPositionURL = window.location.pathname.replace("/chart", "") + "/View?handler=OrgGroupPosition";
const GetOrgGroupNPRFURL = window.location.pathname.replace("/chart", "") + "/View?handler=OrgGroupNPRF";
const OrgGroupEditURL = window.location.pathname.replace("/chart", "") + "/Edit";
const OrgGroupDeleteURL = window.location.pathname.replace("/chart", "/list") + "/Delete";
const OrgGroupAddPostURL = window.location.pathname.replace("/chart", "") + "/Add";
const OrgGroupEditPostURL = window.location.pathname.replace("/chart", "") + "/Edit";
const PositionAddURL = "/Plantilla/Position/Add";
const PositionAddPostURL = "/Plantilla/Position/Add";
const ViewPositionByIDURL = window.location.pathname.replace("/chart", "") + "/View?handler=PositionDropDownByID";
const ViewOrgGroupDropDownURL = window.location.pathname.replace("/chart", "") + "/View?handler=OrgGroupDropDown";
const ViewPositionLevelDropDownURL = window.location.pathname.replace("/chart", "") + "/View?handler=PositionLevelDropDown";
const PositionDropDownURL = window.location.pathname.replace("/chart", "") + "/View?handler=PositionDropDown";
const EditPositionByIDURL = window.location.pathname.replace("/chart", "") + "/View?handler=PositionDropDownByID";
const GetOrgTypeURL = window.location.pathname + "?handler=ReferenceValue&RefCode=ORGGROUPTYPE";
const GetOrgGroupTagsByOrgGroupTypeURL = window.location.pathname.replace("/chart", "/list") + "?handler=OrgGroupTagsByOrgGroupType&RefCode=";
const DownloadOrgGroupFormURL = window.location.pathname.replace("/chart", "/list") + "?handler=DownloadTemplate&ID=1";
const UploadInsertOrgGroupURL = window.location.pathname.replace("/chart", "/list") + "?handler=UploadInsertOrgGroup";
const UploadEditOrgGroupURL = window.location.pathname.replace("/chart", "/list") + "?handler=UploadEditOrgGroup";
const GetCheckOrgGroupExportListURL = window.location.pathname.replace("/chart", "/list") + "?handler=CheckOrgGroupExportList";
const DownloadOrgGroupExportListURL = window.location.pathname.replace("/chart", "/list") + "?handler=DownloadOrgGroupExportList";
const OrgTypeHierarchyUpwardURL = window.location.pathname.replace("/chart", "/list") + "?handler=OrgGroupHierarchy";
const OrgTypeEmployeeListURL = window.location.pathname.replace("/chart", "") + "/View?handler=OrgGroupEmployeeList";
const PositionAutoCompleteURL = window.location.pathname.replace("/chart", "") + "/View?handler=PositionAutoComplete";
const OrgGroupNPRFListURL = window.location.pathname.replace("/chart", "") + "/View?handler=OrgGroupNPRFList";
const OrgGroupByOrgTypeDropDownURL = window.location.pathname + "?handler=OrgGroupByOrgType";
var PositionWithLevelAutoCompleteURL = window.location.pathname.replace("/chart", "/list") + "?handler=PositionWithLevelByAutoComplete";
var ParentOrgGroupAutoCompleteURL = window.location.pathname.replace("/chart", "/list") + "?handler=OrgGroupAutoComplete";

const UserOrgTypeHierarchyUpwardURL = window.location.pathname + "?handler=UserOrgGroup";


var HiddenOrgGroups = [];
var HiddenPositions = [];
var GlobalOrgChartData = [];
var GlobalPositionData = [];
var noChild = [];
var noChildWithLevel = [];
var ParentChildCount = [];
var _maxLevel = 1;
var _level = 1;

$(document).ready(function () {

    objOrgGroupListJS = {
        CalculateTotal: function (input, totalID) {
            var total = 0;
            var arr = $("." + input);

            for (var i = 0; i < arr.length; i++) {
                if (parseInt(arr[i].value))
                    total += parseInt(arr[i].value || 0);
            }

            $("#" + totalID).text(total);
        }
    };

    objChartJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            s.GetLocalStorage();
            $("#miniview").hide();
            $("#ddlChartOrgType").change();
            s.LoadUserOrgGroup();

        },

        ElementBinding: function () {
            var s = this;
            
            $("#ddlChartOrgType").change(function () {
                //s.SetLocalStorage();
                if ($(this).val() != "") {
                    
                    var GetSuccessFunction = function (data) {
                        var DropDownOptions = [];
                        $("#ddlChartOrgGroup").html("")
                        $(data.Result).each(function (index, item) {
                            DropDownOptions.push(
                                {
                                    Value: item.Value,
                                    Text: item.Text
                                });
                        });
                        objEMSCommonJS.PopulateDropDown("#ddlChartOrgGroup", DropDownOptions);
                        $("#ddlChartOrgGroup").val(localStorage["OrgChartOrgGroup"] || "");
                    };

                    objEMSCommonJS.GetAjax(OrgGroupByOrgTypeDropDownURL + "&OrgType=" + $("#ddlChartOrgType").val(), {}, "", GetSuccessFunction);
                }
                localStorage["OrgChartOrgType"] = $("#ddlChartOrgType").val() || "";
            });

            $("#miniview").on({
                mouseenter: function () {
                    $(this).css({
                        "position": "fixed",
                        "top": ($("#ChartContainer").position().top + 55) - $(window).scrollTop(),
                        "right": parseInt($(this).css("right").replace("px")) + 35,
                    });
                },
                mouseleave: function () {
                    $(this).css({
                        "position": "absolute",
                        "top": $(".org-chart-nav").height() + 15,
                        "right": 5
                    });
                }
            });

            $("#ddlChartOrgGroup").change(function () {
                localStorage["OrgChartOrgGroup"] = $("#ddlChartOrgGroup").val();
            });

            $("#btnGenerateChart, #cbShowClosedBranches").click(function () {
                $("#divOrgChartErrorMessage").html("");
                $("#ddlChartOrgGroup").removeClass("errMessage");
                $("#ddlChartOrgType").removeClass("errMessage");
                $("#ddlOrgGroupLevels").removeClass("errMessage");

                if (($("#ddlChartOrgGroup").val() || "") != "" && ($("#ddlOrgGroupLevels").val() || "") != "") {
                    s.LoadOrgChartData();
                }

                if (($("#ddlChartOrgGroup").val() || "") == "") {
                    $("#ddlChartOrgGroup").addClass("errMessage");
                }

                if (($("#ddlOrgGroupLevels").val() || "") == "") {
                    $("#ddlOrgGroupLevels").addClass("errMessage");
                }

                if (($("#ddlChartOrgType").val() || "") == "") {
                    $("#ddlChartOrgType").addClass("errMessage");
                }

                if ($("#ddlChartOrgGroup").hasClass("errMessage") || $("#ddlChartOrgType").hasClass("errMessage") || $("#ddlOrgGroupLevels").hasClass("errMessage"))
                    $("#divOrgChartErrorMessage").html("<label class=\"errMessage\"><li>" + REQ_HIGHLIGHTED_FIELDS + "</li></label><br />");

                s.SetLocalStorage();
            });

            $("#ChartContainer").scroll(function () {
                localStorage["OrgChartScrollLeft"] = $("#ChartContainer").scrollLeft();
                localStorage["OrgChartScrollTop"] = $("#ChartContainer").scrollTop();
            });

            $("#OrgChartReset").click(function () {
                $("#ChartDrawingDiv").css({ "zoom": 1 });
                s.SetLocalStorage();
            });

            $("#OrgChartZoomOut").click(function () {
                var current = parseFloat($("#ChartDrawingDiv").css("zoom"));
                current -= .1;
                if (current > 0) {
                    $("#ChartDrawingDiv").css({ "zoom": current });
                    s.SetLocalStorage();
                }
            });

            $("#OrgChartZoomIn").click(function () {
                var current = parseFloat($("#ChartDrawingDiv").css("zoom"));
                current += .1;
                $("#ChartDrawingDiv").css({ "zoom": current });
                s.SetLocalStorage();
            });

            $("#OrgChartFullScreen").click(function () {
                $("#ChartContainer").unbind("keyup");
                $("#ChartContainer").keyup(function (e) {
                    if (e.keyCode == 27 /*esc*/ || e.keyCode == 122 /*f11*/) {
                        $("#OrgChartSmallScreen").click();
                    }
                });
                $(".org-chart-nav").css({"position":"fixed"});
                $("#OrgChartSmallScreen").show();
                $("#OrgChartFullScreen").hide();
                var elem = $("#ChartContainer")[0];
                if (elem.requestFullscreen) {
                    elem.requestFullscreen();
                } else if (elem.mozRequestFullScreen) { /* Firefox */
                    elem.mozRequestFullScreen();
                } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
                    elem.webkitRequestFullscreen();
                } else if (elem.msRequestFullscreen) { /* IE/Edge */
                    elem.msRequestFullscreen();
                }
                $("#ChartContainer").css({
                    "height": "100%",
                    "width": "100%"
                });
            });

            $("#OrgChartSmallScreen").click(function () {
                $(".org-chart-nav").css({"position":"absolute"});
                $("#OrgChartSmallScreen").hide();
                $("#OrgChartFullScreen").show();
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                } else if (document.mozCancelFullScreen) { /* Firefox */
                    document.mozCancelFullScreen();
                } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
                    document.webkitExitFullscreen();
                } else if (document.msExitFullscreen) { /* IE/Edge */
                    document.msExitFullscreen();
                }
                $("#ChartContainer").css({
                    "height": "auto",
                    "width": "auto"
                });
            });

            $("#btnAddOrgGroup").click(function () {
                LoadPartial(OrgGroupAddURL, 'divOrgGroupBodyModal');
                $("#divOrgGroupModal").modal("show");
            });

            $("#btnAddPosition").click(function () {
                LoadPartial(PositionAddURL, "divPositionBodyModal");
                $("#divPositionModal").modal("show");
            });
        },

        LoadOrgChartData: function () {
            if (($("#ddlChartOrgGroup").val() || "") != "" && ($("#ddlOrgGroupLevels").val() || "") != "") {
                var s = this;
                noChild = [];
                ParentChildCount = [];
                $("#ChartDrawingDiv").html("");
                $("#miniview").html("");
                //$("body").plainOverlay("show");
                $("#ChartDrawingDiv").css({ "zoom": 1 });
                setTimeout(function () { s.LoadOrgChartPositionData(); }, 200);
            }
        },

        DrawOrgChart: function (data) {
            var s = this;

            $("#ChartDrawingDiv").html("");

            var OrgGroupRecursive = function (ID, data) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (item.ParentOrgID == ID) {

                        AppendOrgGroup(item, data);

                        $("#leaf_id_" + item.ID).css({ "left": ($("#leaf_id_" + item.ParentOrgID).position().left) });

                        AlignParents(item.ParentOrgID || 0, item.ID, data);
                        var currentRow = 1;
                        var currentElementRow = $("#leaf_id_" + item.ParentOrgID).parent("div").parent("div").prop("id");
                        if (currentElementRow != undefined) {
                            currentRow = parseInt(currentElementRow.replace("level_", ""));
                        }

                        var upperLevelMax = Math.max.apply(null, $("div#level_" + currentRow).find(".org-group-leaf").map(function () {
                            return $(this).height();
                        }).get());

                        var maxHeight = Math.max.apply(null, $("div#level_" + currentRow).find(".org-group-leaf").map(function () {
                            return $(this).height();
                        }).get());
                        
                        var topOffset = $("#leaf_id_" + item.ParentOrgID).position().top +
                            upperLevelMax + 
                             maxHeight;
                        var found = $.inArray(item.ID, noChild);
                        if (found >= 0) {
                            if ($("#leaf_id_" + item.ID).prev().prop("id") != undefined) {
                                topOffset = $("#leaf_id_" + item.ID).prev().position().top + $("#leaf_id_" + item.ID).prev().height() + 10;
                            }
                        }

                        $("#leaf_id_" + item.ID).css({ "top": topOffset });
                        var found = $.inArray((item.ID).toString(), HiddenOrgGroups);
                        if (found < 0) {
                            OrgGroupRecursive(item.ID, data);
                        }
                    }
                };
            };

            var AppendOrgGroup = function (item, data) {
                $(".org-group-clone").clone()
                    .prop("id", "leaf_id_" + item.ID)
                    .appendTo("#ChartDrawingDiv");

                //$("#ChartDrawingDiv .org-group-clone#" + "leaf_id_" + item.ID).addClass("children_of_" + (item.ParentOrgID || "0"));
                $("#ChartDrawingDiv .org-group-clone#" + "leaf_id_" + item.ID).addClass("org-group-leaf tooltips").attr("data-text", (item.Code + " - " + item.Description));;
                $("#ChartDrawingDiv .org-group-clone#" + "leaf_id_" + item.ID).removeClass("org-group-clone");

                var divSelector = "#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + item.ID;
                $(divSelector).css({ "display": "block" });

                //s.DrawOrgChartPosition(item.ID);


                if ($("#cbShowPosition").prop("checked")) {
                    $(divSelector).find("table").show();
                }
                else {
                    $(divSelector).find("table").show();

                    if ($(divSelector + " .leaf-position:visible").length > 0) {
                        var fixedWidth = $(divSelector).width();
                        $(divSelector).width(fixedWidth);
                    }
                    $(divSelector).find("table").hide();
                }

                if ($(".row-level").length == 0) {
                    $(divSelector).wrap(function () {
                        return "<div class='row-level' id='level_1'></div>";
                    });
                    $(divSelector).wrap(function () {
                        return "<div class='org-children-div children_of_0'></div>";
                    });
                }
                else {

                    var parentRowLevel = $("#leaf_id_" + item.ParentOrgID).parent("div").parent("div").prop("id");
                    if (parentRowLevel != undefined) {
                        var currentLevel = parseInt(parentRowLevel.replace("level_", "")) + 1;

                        if ($("#level_" + currentLevel).length > 0) {
                            $(divSelector).appendTo("#level_" + currentLevel);
                        }
                        else {
                            $(divSelector).wrap(function () {
                                return "<div class='row-level' id='level_" + currentLevel + "'></div>";
                            });
                        }
                    }

                    if ($(".children_of_" + item.ParentOrgID).length > 0) {
                        $(divSelector).appendTo(".children_of_" + item.ParentOrgID);
                    }
                    else {
                        $(divSelector).wrap(function () {
                            return "<div class='org-children-div children_of_" + item.ParentOrgID + "'></div>";
                        });
                    }

                }
                var ChildrenCount = 0;
                //$.grep(data.ParentOrgID, function (elem) {
                //    return elem == item.ID;
                //}).length;
                $(data).each(function (index, obj) {
                    if (obj.ParentOrgID == item.ID)
                        ChildrenCount++;
                });

                $(divSelector).find(".leaf-header").append(item.Code + "<br>" + item.Description.substring(0, 15) + "<br>(" + item.OrgType+")"); //CHANGE BOX LABEL
                if (ChildrenCount > 0)
                    $(divSelector).find(".leaf-header").after().append("<span style=\""
                    + "position: absolute;"
                    + "display: inline-table;"
                    + "color: black;"
                    + "top: -10px; "
                    + "right: -10px;"
                    + "\">" + ChildrenCount + "</span>");

                // #region Events
                //$(divSelector).find(".leaf-header-buttons .glyphicon-list").click(function () {
                //    var fixedWidth = $(divSelector).width();
                //    if ($(divSelector).find("table").is(":visible")) {
                //        $(divSelector).find("table").hide();
                //        $(divSelector).width(fixedWidth);
                //    }
                //    else {
                //        $(divSelector).find("table").show();
                //    }
                //    var id = $(this).parent("div").parent(".org-group-leaf").prop("id").replace("leaf_id_", "");

                //    var found = $.inArray((id).toString(), HiddenPositions);
                //    if (found < 0) {
                //        HiddenPositions.push(id);
                //    }
                //    else {
                //        HiddenPositions.splice(found, 1);
                //    }
                //    localStorage["OrgChartHiddenPositions"] = HiddenPositions;
                //    s.DrawLines(GlobalOrgChartData, id);
                //    //s.DrawOrgChart(data, $("#ddlChartOrgGroup").val());
                //});
                /*if ($("#hdnHasOrgGroupEditFeature").val() == "true") {

                    $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").click(function () {
                        var isSuccessFunction = function () {
                            $("#divOrgGroupBodyModal #btnDelete").remove();
                            $("#divOrgGroupBodyModal #btnBack").remove();
                            $("#divOrgGroupBodyModal #btnSave").click(function () {
                                objOrgGroupEditJS.EditSuccessFunction = function () {
                                    objChartJS.LoadOrgChartData();
                                };
                            });
                        };
                        LoadPartialSuccessFunction(OrgGroupEditURL + "?ID=" + item.ID, 'divOrgGroupBodyModal', isSuccessFunction);
                        $("#divOrgGroupModal").modal("show");
                    });
                }
                else {
                    $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").remove();
                }*/

                $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").click(function () {
                    var isSuccessFunction = function () {
                        $("#divOrgGroupBodyModal #btnDelete").remove();
                        $("#divOrgGroupBodyModal #btnBack").remove();
                        $("#divOrgGroupBodyModal #btnSave").click(function () {
                            objOrgGroupEditJS.EditSuccessFunction = function () {
                                objChartJS.LoadOrgChartData();
                            };
                        });
                    };
                    LoadPartialSuccessFunction(OrgGroupViewURL + "?ID=" + item.ID, 'divOrgGroupBodyModal', isSuccessFunction);
                    $("#divOrgGroupModal").modal("show");
                });
                $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").addClass("glyphicon-eye-open");
                $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").removeClass("glyphicon-pencil");

                //$(divSelector).find(".leaf-footer-buttons span").click(function () {
                //    var id = $(this).parent("div").parent(".org-group-leaf").prop("id").replace("leaf_id_", "");
                //    var found = $.inArray((id).toString(), HiddenOrgGroups);
                //    if (found < 0) {
                //        HiddenOrgGroups.push(id);
                //    }
                //    else {
                //        HiddenOrgGroups.splice(found, 1);
                //    }
                //    localStorage["OrgChartHiddenOrgGroups"] = HiddenOrgGroups;
                //    s.DrawOrgChart(data);
                //});

                $(divSelector).mouseover(function () {
                    $(divSelector).find(".leaf-header-buttons").show();
                }).mouseout(function () {
                    $(divSelector).find(".leaf-header-buttons").hide();
                });

                // #endregion

            };

            var AlignParents = function (ParentId, ChildId, data) {

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (ParentId == item.ID) {

                        var currentElementRow = $("#leaf_id_" + ChildId).parent("div").parent("div").prop("id");
                        var currentRow = 1;
                        if (currentElementRow != undefined) {
                            currentRow = parseInt(currentElementRow.replace("level_", ""));
                        }
                        var children = $(".children_of_" + ChildId).find("div.org-group-leaf").toArray().length;
                        var marginElement = $("#level_" + currentRow).find(".org-group-leaf").eq(-2).prop("id");
                        var childPosition = 0;

                        if (marginElement != undefined && children < 1) {
                            childPosition = ($("#level_" + currentRow).find(".org-group-leaf").eq(-2).position().left +
                                $("#level_" + currentRow).find(".org-group-leaf").eq(-2).width());

                            if (childPosition < $("#leaf_id_" + ChildId).position().left)
                                childPosition = $("#leaf_id_" + ChildId).position().left;

                            var maxHeight = Math.max.apply(null, $("div#level_" + currentRow).find(".org-group-leaf").map(function () {
                                return $(this).height();
                            }).get());

                            var leftOffset = (childPosition + 10);
                            var found = $.inArray(ChildId, noChild);
                            if (found >= 0) {
                                if ($("#leaf_id_" + ChildId).prev().prop("id") != undefined) {
                                    leftOffset = $("#leaf_id_" + ChildId).prev().position().left;
                                }
                            }

                            $("#leaf_id_" + ChildId).css({ "left": leftOffset/*, "top": maxHeight*/ });
                        }
                        var firstSibling = $("div.children_of_" + ParentId + " .org-group-leaf:first");
                        var lastSibling = $("div.children_of_" + ParentId + " .org-group-leaf:last");
                        var parentPosition = ((lastSibling.position().left - firstSibling.position().left) / 2) + firstSibling.position().left;

                        var leftOffset = (childPosition + 10);
                        var found = $.inArray(ChildId, noChild);
                        if (found >= 0) {
                            if ($("#leaf_id_" + ChildId).prev().prop("id") != undefined) {
                                parentPosition = Math.max.apply(null, $("div.children_of_" + ParentId).find(".org-group-leaf").map(function () {
                                    return $(this).width();
                                }).get());

                                parentPosition = (parentPosition / 2) + + firstSibling.position().left;
                            }
                        }


                        $("#leaf_id_" + ParentId).css({ "left": parentPosition });
                        AlignParents(item.ParentOrgID, ParentId, data);
                    }
                };
            }
            _maxLevel = 0;
            var CheckSingleFile = function (ID, data) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (item.ParentOrgID == ID) {
                        var childrenCount = 0;
                        $(data).each(function (index, obj) {
                            if (item.ID == obj.ParentOrgID) {
                                childrenCount++;
                            }
                        });
                        _level++;
                        _maxLevel = _level > _maxLevel ? _level : _maxLevel;
                        if (childrenCount == 0)
                        {
                            noChild.push(item.ID);
                            noChildWithLevel.push({ id: item.ID, level: _level });
                        }
                        else {
                            ParentChildCount.push({ id: item.ID, count: childrenCount });
                        }

                        CheckSingleFile(item.ID, data);
                    }
                };
                _level--;
            };

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                if (item.ID == $("#ddlChartOrgGroup").val()) {
                    _level = 1;
                    CheckSingleFile(item.ID, data);
                }
            };

            $(noChildWithLevel).each(function (index, obj) {
                var siblings = 0;
                var parentID = 0;

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (obj.id == item.ID) {
                        parentID = item.ParentOrgID;
                    }
                };
                
                $(noChild).each(function (index, obj1) {
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        if (parentID == item.ParentOrgID & obj1 == item.ID) {
                            siblings++;
                        }
                    };
                });
                var hasMatch = false;
                $(ParentChildCount).each(function (index, obj1) {
                    if (parentID == obj1.id) {
                        if (obj1.count != siblings) {
                            noChild = jQuery.grep(noChild, function (value) {
                                return value != obj.id;
                            });
                        }
                        hasMatch = true;
                    }
                });

                if (!hasMatch || obj.level < _maxLevel-1)
                    noChild = jQuery.grep(noChild, function (value) {
                        return value != obj.id;
                    });

            });


            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                if (item.ID == $("#ddlChartOrgGroup").val()) {

                    AppendOrgGroup(item, data);
                    $("#leaf_id_" + item.ID).css({ "left": 0, "top": 130 });
                    //var found = $.inArray((item.ID).toString(), HiddenOrgGroups);
                    //if (found < 0) {
                    OrgGroupRecursive(item.ID, data);
                    //}
                }
            };

            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var divSelector = "#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + item.ID;
                var found = $.inArray((item.ID).toString(), HiddenPositions);
                if (found >= 0) {
                    if ($("#cbShowPosition").prop("checked")) {
                        $(divSelector).find("table").show();

                        if ($(divSelector + " .leaf-position:visible").length > 0) {
                            var fixedWidth = $(divSelector).width();
                            $(divSelector).width(fixedWidth);
                        }
                        $(divSelector).find("table").hide();
                    }
                    else {
                        $(divSelector).find("table").show();
                    }
                }
            };


            //if ($("#cbShowExisting").prop("checked")) {
            //    $(".org-group-leaf .ex").show();
            //}
            //else {
            //    $(".org-group-leaf .ex").hide();
            //}

            //if ($("#cbShowActual").prop("checked")) {
            //    $(".org-group-leaf .ac").show();
            //}
            //else {
            //    $(".org-group-leaf .ac").hide();
            //}

            //Set Chart to middle
            var rightMost = 0;
            $('.org-group-leaf').each(function (i, obj) {
                rightMost = rightMost > $(this).position().left + $(this).width() ? rightMost : $(this).position().left + $(this).width();
            });
            if ($("#ChartDrawingDiv").width() >= rightMost) {
                $('.org-group-leaf').each(function (i, obj) {
                    $(this).css({ "left": $(this).position().left + (rightMost > 0 ? ($("#ChartDrawingDiv").width() - rightMost) / 2 : 0) });
                });
            }

            // Set position of single child leaf to the middle of its parent leaf
            //for (var i = 0; i < data.length; i++) {
            //    var item = data[i];
            //    var setToMiddle = 0;
            //    var siblings = $(".children_of_" + item.ParentOrgID || 0).find("div.org-group-leaf").toArray().length;
            //    if (siblings == 1 && (item.ParentOrgID || 0) != 0) {
            //        setToMiddle = ($("#leaf_id_" + item.ParentOrgID).width() - $("#leaf_id_" + item.ID).width()) / 2;
            //        $("#leaf_id_" + item.ID).css({ "left": $("#leaf_id_" + item.ParentOrgID).position().left + setToMiddle });
            //    }
            //};

            s.DrawLines(data);

            // Set Div Height
            $("#ChartDrawingDiv").height(
                ($(".row-level:first .org-group-leaf:first").length > 0 ? $(".row-level:first .org-group-leaf:first").position().top : 0) +
                Math.max.apply(null, $(".row-level:last .org-group-leaf").map(function () { return $(this).position().top; }).get()) + 100
            );

            $("#org-chart-nav-hidden span, #org-chart-nav-hidden #miniview").appendTo(".org-chart-nav");

            //$("body").plainOverlay("hide");

            setTimeout(function () {
                if ($("#ChartContainer").get(0).scrollWidth > $("#ChartContainer").innerWidth()) {
                    $("#miniview").show();
                }
                else {
                    $("#miniview").hide();
                }

                var nodes = $('#ChartContainer .org-group-leaf, #ChartContainer .OrgGroupLines');
                //nodes.each(function () {
                //    $(this).draggable({ containment: "#ChartContainer", scroll: false });
                //});
                $('#ChartContainer').miniview(nodes);
                $("#miniview").css({
                    "top": $(".org-chart-nav").height() + 15,
                    "left": "auto",//$(".org-chart-nav").position().left - $("#miniview").width(),
                    "right": 5//$(".org-chart-nav").width() + 60

                });

                $("#ChartDrawingDiv").css({ "zoom": localStorage["OrgChartZoom"] == undefined ? 1 : localStorage["OrgChartZoom"] });
                $("#ChartContainer").scrollLeft(localStorage["OrgChartScrollLeft"] || 0);
                $("#ChartContainer").scrollTop(localStorage["OrgChartScrollTop"] || 0);
            }, 200);

        },

        LoadOrgChartPositionData: function () {
            var s = this;

            var input = {
                handler: "Chart",
                OrgGroupID: $("#ddlChartOrgGroup").val(),
                Depth: $("#ddlOrgGroupLevels").val(),
                ShowClosedBranches: $("#cbShowClosedBranches").prop("checked")
            };
            var GetSuccessFunction = function (data) {
                GlobalOrgChartData = data;
                s.DrawOrgChart(data);
            };

            objEMSCommonJS.GetAjax(GetOrgChart, input, "", GetSuccessFunction, null, true);
        },

        SetLocalStorage: function () {
            localStorage["OrgChartOrgGroup"] = $("#ddlChartOrgGroup").val() || "";
            localStorage["OrgChartDepth"] = $("#ddlOrgGroupLevels").val() || "";
            localStorage["OrgChartZoom"] = $("#ChartDrawingDiv").css("zoom");
        },

        GetLocalStorage: function () {
            $("#ddlChartOrgType").val(localStorage["OrgChartOrgType"]);
            $("#ddlOrgGroupLevels").val(localStorage["OrgChartDepth"]);
        },

        DrawLines: function (data, ID) {
            //Draw lines
            
            if (ID == null)
                $(".OrgGroupLines").remove();
            else {
                $("#topBar_" + ID +
                    ", #bottomBar_" + ID +
                    ", #horizontalBar_" + ID).remove();
            }

            var leftMargin = $("#ChartDrawingDiv").scrollLeft();
            var singleID = [];
                singleID.push(ID);
            //$(data).each(function (i, item) {
            for (var i = 0; i < (ID != null ? singleID.length : data.length); i++)
            {
                var itemID = (ID != null ? singleID[i] : data[i].ID);
                var parentOrgs = [];
                for (var x = 0; x < data.length; x++) {
                    var pg = data[x];

                    var found = $.inArray((pg.ParentOrgID || 0).toString(), parentOrgs);
                    if (found < 0) {
                        parentOrgs.push((pg.ParentOrgID || 0).toString());
                    }
                };
                var id = $("#leaf_id_" + itemID);
                var lineWidth = 1;

                if (id.length > 0) {
                    //var currentElementRow = $("#leaf_id_" + itemID).parent("div").parent("div").prop("id");
                    //var currentRow = 1;
                    //if (currentElementRow != undefined) {
                    //    currentRow = parseInt(currentElementRow.replace("level_", ""));
                    //}

                    //var maxHeight = Math.max.apply(null, $("#level_" + currentRow).find(".org-group-leaf").map(function () { return $(this).height(); }).get());

                    //var element = $("#leaf_id_" + itemID + " .leaf-footer-buttons span");
                    //if (parentOrgs.indexOf((itemID).toString()) >= 0) {
                    //    var found = $.inArray((itemID).toString(), HiddenOrgGroups);
                    //    if (found < 0) {
                    //        $(element).addClass("glyphicon-chevron-up");
                    //        $(element).removeClass("glyphicon-chevron-down");
                    //    }
                    //    else {
                    //        $(element).addClass("glyphicon-chevron-down");
                    //        $(element).removeClass("glyphicon-chevron-up");
                    //    }
                    //}
                    //else {
                    //    $(element).remove();
                    //}
                    var found = $.inArray(itemID, noChild);
                    var isRightSide = false;
                    var isFirstChild = false;
                    if (found >= 0) {
                        if ($("#leaf_id_" + itemID).prev().prop("id") != undefined) {
                            isRightSide = true;
                        }
                        else {
                            if ($(".children_of_" + data[i].ParentOrgID).find(".org-group-leaf").length > 1) {
                                isRightSide = true;
                            }

                            isFirstChild = true;
                        }
                    }

                    if ((itemID || 0) != $("#ddlChartOrgGroup").val() && $("#leaf_id_" + itemID).is(":visible")) {
                        var topBar = $("<div class='OrgGroupLines' id='topBar_" + itemID +"'></div>").css({
                            "position": "absolute",
                            "background-color": "#999999",
                            "top": (isRightSide ? (!isFirstChild ? id.prev().position().top + (id.prev().height() / 2) : $("#leaf_id_" + data[i].ParentOrgID).position().top + $("#leaf_id_" + data[i].ParentOrgID).height() + 10): (id.position().top - 6)) + "px",
                            "height": (isRightSide ? (!isFirstChild ? (id.height() + id.prev().height()) / 2 + 10 : id.position().top - $("#leaf_id_" + data[i].ParentOrgID).position().top): 11 + parseInt(id.css("border-bottom-width").replace("px", ""))) + "px",
                            "width": lineWidth + "px",
                            "left": (isRightSide ? id.position().left + id.width() + 10 : (id.position().left + id.width() / 2)) + parseInt(id.css("margin").replace("px", "")) + leftMargin + "px"
                        });
                        $("#ChartDrawingDiv").append(topBar);
                    }

                    if ($("div.children_of_" + itemID + " > div:visible").toArray().length > 0) {
                        var bottomBarHeight = 0;
                        var oldHeight = $("#leaf_id_" + itemID).height();
                        var bHeight = 0;
                        var elem = $("div.children_of_" + (itemID || 0) + " > div:first");
                        bHeight = (elem.length > 0 ? elem.position().top : 0) - (id.position().top + oldHeight) - 50;

                        var bottomBar = $("<div class='OrgGroupLines bottomBar' id='bottomBar_"+ itemID +"'></div>").css({
                            "position": "absolute",
                            "background-color": "#999999",
                            "top": (id.position().top + id.height() + parseInt(id.css("margin").replace("px", "")) +
                                parseInt(id.css("border-bottom-width").replace("px", ""))) + "px",
                            "height": bottomBarHeight + ((itemID || 0) != $("#ddlChartOrgGroup").val() ? ((/*maxHeight - oldHeight*/ bHeight) + 41) : (oldHeight-10)) + "px",
                            "width": lineWidth + "px",
                            "left": (id.position().left + id.width() / 2) + parseInt(id.css("margin").replace("px", "")) + leftMargin + "px"
                        });

                        $("#ChartDrawingDiv").append(bottomBar);
                    }

                    if ($("div.children_of_" + itemID + " > div:visible").toArray().length > 1 | isRightSide) {
                        var firstSibling = $("div.children_of_" + itemID + " > div:first");
                        var lastSibling = $("div.children_of_" + itemID + " > div:last");
                        var horizontalBar = $("<div class='OrgGroupLines' id='horizontalBar_" + itemID +"'></div>").css({
                            "position": "absolute",
                            "background-color": "#999999",
                            "top": (isRightSide ? id.position().top + (id.height()/2) : (firstSibling.position().top - 6)) + "px",
                            "height": lineWidth + "px",
                            "width": (isRightSide ? 11 : (lastSibling.width() - firstSibling.width()) / 2 + (lastSibling.position().left - firstSibling.position().left)) + "px",
                            "left": (isRightSide ? id.position().left + id.width() + 5 : (firstSibling.position().left + (firstSibling.width() / 2)) + parseInt(id.css("margin").replace("px", ""))) + leftMargin + "px"
                        });
                        $("#ChartDrawingDiv").append(horizontalBar);
                    }

                }
            //});
            }

            for (var i = 0; i < (ID != null ? singleID.length : data.length); i++) {
                var itemID = (ID != null ? singleID[i] : data[i].ID);
                var found = $.inArray(itemID, noChild);
                var isRightSide = false;
                var isFirstChild = false;
                if (found >= 0) {
                    if ($("#leaf_id_" + itemID).prev().prop("id") == undefined) {
                        if ($(".children_of_" + data[i].ParentOrgID).find(".org-group-leaf").length > 1) {
                            if ($("#bottomBar_" + data[i].ParentOrgID).position() != undefined) {
                                $("#bottomBar_" + data[i].ParentOrgID).css({
                                    "left": $("#bottomBar_" + data[i].ParentOrgID).position().left + 10,
                                });
                            }
                        }
                    }
                }
            }
        },

        LoadUserOrgGroup: function () {
            var GetSuccessFunction = function (data) {
                var OrgGroupTitle;
                var OrgGroupId;
                var OrgGroupCode;
                var OrgGroupDescription

                $(data.Result).each(function (idx, item) {
                    OrgGroupTitle = item.OrgType;
                    OrgGroupId = item.ID;
                    OrgGroupCode = item.Code;
                    OrgGroupDescription = item.Description;
                });

                $("#ddlChartOrgType").val(OrgGroupTitle);
                $("#ddlChartOrgGroup").val(OrgGroupId);


                $("#ddlChartOrgGroup").html($('<option/>', {
                    value: OrgGroupId,
                    text: OrgGroupCode + " - " + OrgGroupDescription
                }));

                $("#ddlOrgGroupLevels").val(9);
                $("#cbShowClosedBranches").prop("checked", true);
                //$("#btnGenerateChart").click();
            };

            objEMSCommonJS.GetAjax(UserOrgTypeHierarchyUpwardURL
                + ""
                , {}
                , ""
                , GetSuccessFunction);
        },
    };

    objChartJS.Initialize();
});