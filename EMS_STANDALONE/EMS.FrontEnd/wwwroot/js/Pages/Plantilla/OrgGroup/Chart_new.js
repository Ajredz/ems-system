var objChartJS;

const GetOrgChart = "/Plantilla/OrgGroup/Chart";
const OrgGroupAddURL = "/Plantilla/OrgGroup/Add";
const OrgGroupViewURL = "/Plantilla/OrgGroup/View";
const OrgGroupEditURL = "/Plantilla/OrgGroup/Edit";
const OrgGroupDeleteURL = "/Plantilla/OrgGroup/Delete";
const OrgGroupAddPostURL = "/Plantilla/OrgGroup/Add";
const OrgGroupEditPostURL = "/Plantilla/OrgGroup/Edit";
const RegionAddURL = "/Plantilla/Region/Add";
const RegionAddPostURL = "/Plantilla/Region/Add";
const PositionAddURL = "/Plantilla/Position/Add";
const PositionAddPostURL = "/Plantilla/Position/Add";
const ViewPositionByIDURL = "/Plantilla/OrgGroup/View?handler=PositionDropDownByID";
const ViewOrgGroupDropDownURL = "/Plantilla/OrgGroup/View?handler=OrgGroupDropDown";
const ViewPositionLevelDropDownURL = "/Plantilla/OrgGroup/View?handler=PositionLevelDropDown";
const AddRegionDropDownURL = "/Plantilla/OrgGroup/Add?handler=RegionDropDown";
const AddOrgGroupDropDownURL = "/Plantilla/OrgGroup/Add?handler=OrgGroupDropDown";
const AddPositionLevelDropDownURL = "/Plantilla/OrgGroup/Add?handler=PositionLevelDropDown";
const AddPositionDropDownURL = "/Plantilla/OrgGroup/Add?handler=PositionDropDown";
const EditRegionDropDownURL = "/Plantilla/OrgGroup/Edit?handler=RegionDropDown";
const EditOrgGroupDropDownURL = "/Plantilla/OrgGroup/Edit?handler=OrgGroupDropDown";
const EditPositionLevelDropDownURL = "/Plantilla/OrgGroup/Edit?handler=PositionLevelDropDown";
const EditPositionDropDownURL = "/Plantilla/OrgGroup/Edit?handler=PositionDropDown";
const EditPositionByIDURL = "/Plantilla/OrgGroup/Edit?handler=PositionDropDownByID";

var HiddenOrgGroups = [];
var HiddenPositions = [];
var GlobalOrgChartData = [];
var GlobalPositionData = [];

$(document).ready(function () {
    objChartJS = {

        Initialize: function () {
            var s = this;

            s.ElementBinding();
            s.GetLocalStorage();
            if (!$("#cbShowPosition").prop("checked")) {
                $(".org-group-clone table").hide();
            }
            if (!$("#cbShowExisting").prop("checked")) {
                $(".org-group-clone .ex").hide();
            }
            if (!$("#cbShowActual").prop("checked")) {
                $(".org-group-clone .ac").hide();
            }
            HiddenOrgGroups = localStorage["OrgChartHiddenOrgGroups"] != undefined ? localStorage["OrgChartHiddenOrgGroups"].split(",") : [];
            HiddenPositions = localStorage["OrgChartHiddenPositions"] != undefined ? localStorage["OrgChartHiddenPositions"].split(",") : [];
            s.LoadOrgChartData();
        },

        ElementBinding: function () {
            var s = this;

            $("#ddlOrgGroup, #cbShowPosition, #cbShowExisting, #cbShowActual").change(function () {

                if ($(this).prop("id") == "ddlOrgGroup")
                    HiddenOrgGroups = [];
                if ($(this).prop("id") == "cbShowPosition")
                    HiddenPositions = [];
                if ($("#cbShowPosition").prop("checked")) {
                    localStorage["OrgChartHiddenPositions"] = [];
                    localStorage["OrgChartShowPositions"] = [];
                    //$(".org-group-clone table").show();
                    $("#cbShowExisting").prop("disabled", false);
                    $("#cbShowActual").prop("disabled", false);

                }
                else {
                    //$(".org-group-clone table").hide();
                    $("#cbShowExisting").prop("disabled", true);
                    $("#cbShowActual").prop("disabled", true);
                    $("#cbShowExisting").prop("checked", false);
                    $("#cbShowActual").prop("checked", false);
                }
                //s.LoadOrgChartData();
                //s.DrawOrgChart(GlobalOrgChartData);
                if ($(this).prop("id") != "cbShowExisting" & $(this).prop("id") != "cbShowActual")
                    s.LoadOrgChartData();
                else {
                    if ($("#cbShowExisting").prop("checked")) {
                        $(".org-group-leaf .ex").show();
                    }
                    else {
                        $(".org-group-leaf .ex").hide();
                    }

                    if ($("#cbShowActual").prop("checked")) {
                        $(".org-group-leaf .ac").show();
                    }
                    else {
                        $(".org-group-leaf .ac").hide();
                    }
                    s.DrawLines(GlobalOrgChartData);

                }
                s.SetLocalStorage();
            });

            $("#ChartContainer").scroll(function () {
                localStorage["OrgChartScrollLeft"] = $("#ChartContainer").scrollLeft();
                localStorage["OrgChartScrollTop"] = $("#ChartContainer").scrollTop();
            });

            $("#OrgChartReset").mouseup(function () {
                $("#ChartDrawingDiv").css({ "zoom": 1 });
                s.SetLocalStorage();
            });

            $("#OrgChartZoomOut").mouseup(function () {
                var current = parseFloat($("#ChartDrawingDiv").css("zoom"));
                current -= .1;
                if (current > 0) {
                    $("#ChartDrawingDiv").css({ "zoom": current });
                    s.SetLocalStorage();
                }
            });

            $("#OrgChartZoomIn").mouseup(function () {
                var current = parseFloat($("#ChartDrawingDiv").css("zoom"));
                current += .1;
                $("#ChartDrawingDiv").css({ "zoom": current });
                s.SetLocalStorage();
            });

            $("#OrgChartFullScreen").mouseup(function () {
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

            $("#OrgChartSmallScreen").mouseup(function () {
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

            $("#btnAddOrgGroup").mouseup(function () {
                LoadPartial(OrgGroupAddURL, 'divOrgGroupBodyModal');
                $("#divOrgGroupModal").modal("show");
            });

            $("#btnAddPosition").mouseup(function () {
                LoadPartial(PositionAddURL, "divPositionBodyModal");
                $("#divPositionModal").modal("show");
            });
        },

        LoadOrgChartData: function () {
            var s = this;

            s.LoadOrgChartPositionData();
        },

        AlignParents: function (data) {
            var s = this;
            $(".right_padding, .left_padding").remove();
            var recursiveFunction = function (ParentId, ChildId, data) {
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

                        var parentPadding = (($("#leaf_id_" + ChildId).width() / 2))  ;
                        if ($(".left_padding_" + ParentId).length == 0)
                            $("#leaf_id_" + ParentId).before("<div class=\"left_padding left_padding_" + ParentId + "\" style=\"width:"
                                + (parentPadding - ($("#leaf_id_" + ParentId).width() / 2) + 1) + "px\"></div>");
                        else {
                            $(".left_padding_" + ParentId).width($(".left_padding_" + ParentId).width() + parentPadding + 5);
                        }

                        if ($(".right_padding_" + ParentId).length == 0)
                            $("#leaf_id_" + ParentId).after("<div class=\"right_padding right_padding_" + ParentId + "\" style=\"width:"
                                + (50 + parentPadding - ($("#leaf_id_" + ParentId).width() / 2) + 1) + "px\"></div>");
                        else {
                            $(".right_padding_" + ParentId).width($(".right_padding_" + ParentId).width() + parentPadding + 5);
                        }

                        //var padding = $(".org-children-div.children_of_" + ParentId).width() / 2;

                        //if ($(".left_padding_" + ParentId).length == 0)
                        //    $("#leaf_id_" + ParentId).before("<div class=\"left_padding_" + ParentId + "\" style=\"width:"
                        //        + (padding - ($("#leaf_id_" + ParentId).width() / 2)) + "px\"></div>");

                        //if ($(".right_padding_" + ParentId).length == 0)
                        //    $("#leaf_id_" + ParentId).after("<div class=\"right_padding_" + ParentId + "\" style=\"width:"
                        //        + (50 + padding - ($("#leaf_id_" + ParentId).width() / 2)) + "px\"></div>");

                        recursiveFunction(item.ParentOrgID, ParentId, data);
                    }
                };
            };

            $(data).each(function (index, obj) {
                    recursiveFunction(obj.ParentOrgID, obj.ID, data);
            });

            $(".row-level").each(function (index, obj) {
                if ((index + 1) < $(".row-level").length & (index + 1) > 1)
                    $("#" + ($(obj).prop("id")) + " .org-children-div").each(function (index1, obj1) {
                        if (index1 > 0)
                            $(obj1).css({ "margin-left": 5 });
                    });
            }); 

            s.DrawLines(data);


            //$(".org-children-div").each(function (index, obj) {
            //    var id = $(this).prop("class").replace("org-children-div children_of_", "");
            //    var padding = ($(this).width() / 2);

            //        $("#leaf_id_" + id).before("<div class=\"left_padding_" + id + "\" style=\"width:"
            //        + (padding - ($("#leaf_id_" + id).width() / 2) + 5) + "px\"></div>");

            //    $("#leaf_id_" + id).after("<div class=\"right_padding_" + id + "\" style=\"width:"
            //        + (padding - ($("#leaf_id_" + id).width() / 2) + 40 ) + "px\"></div>");
            //});

        },

        DrawOrgChart: function (data) {
            var s = this;
            $("#ChartDrawingDiv").html("");

            var OrgGroupRecursive = function (ID, data) {
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    if (item.ParentOrgID == ID) {
                        AppendOrgGroup(item, data);
                        //$("#leaf_id_" + item.ID).css({ "left": ($("#leaf_id_" + item.ParentOrgID).position().left) });
                        //AlignParents(item.ParentOrgID || 0, item.ID, data);
                        var currentRow = 1;
                        var currentElementRow = $("#leaf_id_" + item.ParentOrgID).parent("div").parent("div").prop("id");
                        if (currentElementRow != undefined) {
                            currentRow = parseInt(currentElementRow.replace("level_", ""));
                        }
                        var maxHeight = Math.max.apply(null, $("div#level_" + currentRow).find(".org-group-leaf").map(function () {
                            return $(this).height();
                        }).get());
                        $("#level_" + currentRow).css({ "height": 30 + maxHeight });
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
                $("#ChartDrawingDiv .org-group-clone#" + "leaf_id_" + item.ID).addClass("org-group-leaf");
                $("#ChartDrawingDiv .org-group-clone#" + "leaf_id_" + item.ID).removeClass("org-group-clone");

                var divSelector = "#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + item.ID;
                $(divSelector).css({ "display": "block" });

                s.DrawOrgChartPosition(item.ID);


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

                $(divSelector).find(".leaf-header").append(item.Code + "<br>" + item.Description);

                // #region Events
                $(divSelector).find(".leaf-header-buttons .glyphicon-list").mouseup(function () {
                    var fixedWidth = $(divSelector).width();
                    if ($(divSelector).find("table").is(":visible")) {
                        $(divSelector).find("table").hide();
                        $(divSelector).width(fixedWidth);
                    }
                    else {
                        $(divSelector).find("table").show();
                    }
                    var id = $(this).parent("div").parent(".org-group-leaf").prop("id").replace("leaf_id_", "");

                    var found = $.inArray((id).toString(), HiddenPositions);
                    if (found < 0) {
                        HiddenPositions.push(id);
                    }
                    else {
                        HiddenPositions.splice(found, 1);
                    }
                    localStorage["OrgChartHiddenPositions"] = HiddenPositions;
                    s.DrawLines(GlobalOrgChartData, id);
                    //s.DrawOrgChart(data, $("#ddlOrgGroup").val());
                });

                $(divSelector).find(".leaf-header-buttons .glyphicon-pencil").mouseup(function () {
                    var isSuccessFunction = function () {
                        $("#divOrgGroupBodyModal #btnDelete").remove();
                        $("#divOrgGroupBodyModal #btnBack").remove();
                        $("#divOrgGroupBodyModal #btnSave").mouseup(function () {
                            objOrgGroupEditJS.EditSuccessFunction = function () {
                                objChartJS.LoadOrgChartData();
                            };
                        });
                    };
                    LoadPartialSuccessFunction(OrgGroupEditURL + "?ID=" + item.ID, 'divOrgGroupBodyModal', isSuccessFunction);
                    $("#divOrgGroupModal").modal("show");
                });

                $(divSelector).find(".leaf-footer-buttons span").mouseup(function () {
                    var id = $(this).parent("div").parent(".org-group-leaf").prop("id").replace("leaf_id_", "");
                    var found = $.inArray((id).toString(), HiddenOrgGroups);
                    if (found < 0) {
                        HiddenOrgGroups.push(id);
                    }
                    else {
                        HiddenOrgGroups.splice(found, 1);
                    }
                    localStorage["OrgChartHiddenOrgGroups"] = HiddenOrgGroups;
                    s.DrawOrgChart(data);
                });

                $(divSelector).mouseover(function () {
                    $(divSelector).find(".leaf-header-buttons").show();
                }).mouseout(function () {
                    $(divSelector).find(".leaf-header-buttons").hide();
                });

                // #endregion

            };

           
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                if (item.ID == $("#ddlOrgGroup").val()) {
                    AppendOrgGroup(item, data);
                    //$("#leaf_id_" + item.ID).css({ "left": 0 });
                    var found = $.inArray((item.ID).toString(), HiddenOrgGroups);
                    if (found < 0) {
                        OrgGroupRecursive(item.ID, data);
                    }
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


            if ($("#cbShowExisting").prop("checked")) {
                $(".org-group-leaf .ex").show();
            }
            else {
                $(".org-group-leaf .ex").hide();
            }

            if ($("#cbShowActual").prop("checked")) {
                $(".org-group-leaf .ac").show();
            }
            else {
                $(".org-group-leaf .ac").hide();
            }

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
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var setToMiddle = 0;
                var siblings = $(".children_of_" + item.ParentOrgID || 0).find("div.org-group-leaf").toArray().length;
                if (siblings == 1 && (item.ParentOrgID || 0) != 0) {
                    setToMiddle = ($("#leaf_id_" + item.ParentOrgID).width() - $("#leaf_id_" + item.ID).width()) / 2;
                    $("#leaf_id_" + item.ID).css({ "left": $("#leaf_id_" + item.ParentOrgID).position().left + setToMiddle });
                }
            };

            s.AlignParents(data);
            
            $("#ChartDrawingDiv").css({ "zoom": localStorage["OrgChartZoom"] == undefined ? 1 : localStorage["OrgChartZoom"] });
            $("#ChartContainer").scrollLeft(localStorage["OrgChartScrollLeft"] || 0);
            $("#ChartContainer").scrollTop(localStorage["OrgChartScrollTop"] || 0);

            // Set Div Height
            $("#ChartDrawingDiv").height(
                ($(".row-level:first .org-group-leaf:first").length > 0 ? $(".row-level:first .org-group-leaf:first").position().top : 0) +
                Math.max.apply(null, $(".row-level:last .org-group-leaf").map(function () { return $(this).position().top; }).get()) + 100
            );

            $("#org-chart-nav-hidden span").appendTo(".org-chart-nav");
        },

        LoadOrgChartPositionData: function () {
            var s = this;

            var input = { handler: "ChartPosition", OrgGroupID: $("#ddlOrgGroup").val() };
            var GetSuccessFunction = function (data) {
                GlobalPositionData = data;

                var input = { handler: "Chart", OrgGroupID: $("#ddlOrgGroup").val() };
                var GetSuccessFunction = function (data) {
                    GlobalOrgChartData = data;
                    s.DrawOrgChart(data);
                };

                objEMSCommonJS.GetAjax(GetOrgChart, input, "", GetSuccessFunction);
            };

            objEMSCommonJS.GetAjax(GetOrgChart, input, "", GetSuccessFunction);
        },

        DrawOrgChartPosition: function (orgGroupID) {
            var hasPosition = 0;
            $(GlobalPositionData).each(function (index, item) {

                if (orgGroupID == item.ID) {
                    hasPosition++;
                    var divSelector = "#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + item.ID;
                    //$(divSelector).find("table").css({"display":"table-row"});
                    $(divSelector).find("table > tbody").append(
                        "<tr class=\"leaf-position" + (item.IsHead == true ? "-head" : "") + "\">"
                        + "<td>" + item.PositionCode + ' - ' + item.PositionTitle + "</td>"
                        + "<td class=\"ex\">" + item.ExistingManPower + "</td>"
                        + "<td class=\"ac\">" + item.ActualManPower + "</td>"
                        + "</tr>"
                    );


                }
            });

            if (hasPosition == 0) {
                $("#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + orgGroupID)
                    .find("table > tbody > tr > th").remove();
                $("#ChartDrawingDiv .org-group-leaf#" + "leaf_id_" + orgGroupID + " .glyphicon-list").remove();
            }
            else {

            }
        },

        SetLocalStorage: function () {
            localStorage["OrgChartOrgGroup"] = $("#ddlOrgGroup").val();
            localStorage["OrgChartShowPosition"] = $("#cbShowPosition").prop("checked");
            localStorage["OrgChartShowExisting"] = $("#cbShowExisting").prop("checked");
            localStorage["OrgChartShowActual"] = $("#cbShowActual").prop("checked");
            localStorage["OrgChartZoom"] = $("#ChartDrawingDiv").css("zoom");
            //localStorage["OrgChartScrollLeft"] = $("#ChartContainer").scrollLeft();
            //localStorage["OrgChartScrollTop"] = $("#ChartContainer").scrollTop();
        },

        GetLocalStorage: function () {
            $("#ddlOrgGroup").val(localStorage["OrgChartOrgGroup"]);
            $("#txtFilterDescription").val(localStorage["PlantillaPositionLevelListDescription"]);
            $("#cbShowPosition").prop("checked",
                JSON.parse(localStorage["OrgChartShowPosition"] == undefined ? true : localStorage["OrgChartShowPosition"] || true)
            );
            $("#cbShowExisting").prop("checked",
                JSON.parse(localStorage["OrgChartShowExisting"] == undefined ? true : localStorage["OrgChartShowExisting"] || true)
            );
            $("#cbShowActual").prop("checked",
                JSON.parse(localStorage["OrgChartShowActual"] == undefined ? true : localStorage["OrgChartShowActual"] || true)
            );

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
            for (var i = 0; i < (ID != null ? singleID.length : data.length); i++) {
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
                var lineWidth = 3;

                if (id.length > 0) {
                    //var currentElementRow = $("#leaf_id_" + itemID).parent("div").parent("div").prop("id");
                    //var currentRow = 1;
                    //if (currentElementRow != undefined) {
                    //    currentRow = parseInt(currentElementRow.replace("level_", ""));
                    //}

                    //var maxHeight = Math.max.apply(null, $("#level_" + currentRow).find(".org-group-leaf").map(function () { return $(this).height(); }).get());

                    var element = $("#leaf_id_" + itemID + " .leaf-footer-buttons span");
                    if (parentOrgs.indexOf((itemID).toString()) >= 0) {
                        var found = $.inArray((itemID).toString(), HiddenOrgGroups);
                        if (found < 0) {
                            $(element).addClass("glyphicon-chevron-up");
                            $(element).removeClass("glyphicon-chevron-down");
                        }
                        else {
                            $(element).addClass("glyphicon-chevron-down");
                            $(element).removeClass("glyphicon-chevron-up");
                        }
                    }
                    else {
                        $(element).remove();
                    }

                    if ((itemID || 0) != $("#ddlOrgGroup").val() && $("#leaf_id_" + itemID).is(":visible")) {
                        var topBar = $("<div class='OrgGroupLines' id='topBar_" + itemID + "'></div>").css({
                            "position": "absolute",
                            "background-color": "#000",
                            "top": (id.position().top - 8) + "px",
                            //"height": 25 + parseInt(id.css("border-bottom-width").replace("px", "")) + "px",
                            "height": 10 + parseInt(id.css("border-bottom-width").replace("px", "")) + "px",
                            "width": lineWidth + "px",
                            "left": (id.position().left + id.width() / 2) + parseInt(id.css("margin").replace("px", "")) + leftMargin + "px"
                        });
                        $("#ChartDrawingDiv").append(topBar);
                    }

                    if ($("div.children_of_" + itemID + " > div:visible").toArray().length > 0) {
                        var bottomBarHeight = 0;
                        var oldHeight = $("#leaf_id_" + itemID).height();
                        var bHeight = 0;
                        var elem = $("div.children_of_" + (itemID || 0) + " > div:first");
                        bHeight = (elem.length > 0 ? elem.position().top : 0) - (id.position().top + oldHeight) - 50;

                        var bottomBar = $("<div class='OrgGroupLines bottomBar' id='bottomBar_" + itemID + "'></div>").css({
                            "position": "absolute",
                            "background-color": "#000",
                            "top": (id.position().top + id.height() + parseInt(id.css("margin").replace("px", "")) +
                                parseInt(id.css("border-bottom-width").replace("px", ""))) + "px",
                            "height": bottomBarHeight + ((itemID || 0) != $("#ddlOrgGroup").val() ? ((/*maxHeight - oldHeight*/ bHeight) + 41) : 44) + "px",
                            "width": lineWidth + "px",
                            "left": (id.position().left + id.width() / 2) + parseInt(id.css("margin").replace("px", "")) + leftMargin + "px"
                        });

                        $("#ChartDrawingDiv").append(bottomBar);
                    }

                    if ($("div.children_of_" + itemID + " > div:visible").toArray().length > 1) {
                        var firstSibling = $("div.children_of_" + itemID + " > div.org-group-leaf:first");
                        var lastSibling = $("div.children_of_" + itemID + " > div.org-group-leaf:last");
                        var horizontalBar = $("<div class='OrgGroupLines' id='horizontalBar_" + itemID + "'></div>").css({
                            "position": "absolute",
                            "background-color": "#000",
                            "top": (firstSibling.position().top - 8) + "px",
                            "height": lineWidth + "px",
                            "width": (lastSibling.width() - firstSibling.width()) / 2 +
                                (lastSibling.position().left - firstSibling.position().left) + "px",
                            "left": (firstSibling.position().left + (firstSibling.width() / 2)) + parseInt(id.css("margin").replace("px", "")) + leftMargin + "px"
                        });
                        $("#ChartDrawingDiv").append(horizontalBar);
                    }

                }
                //});
            }

        },

    };

    objChartJS.Initialize();
});