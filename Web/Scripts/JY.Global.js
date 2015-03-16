

var Option = {
    //清理多个SELECT OPTION，以“，”隔开
    ClearOption: function (ids) {
        if (ids.length > 0) {
            var id = ids.split(",");
            for (var i = 0; i < id.length; i++) {
                if (id[i] != "") {
                    $("#" + id[i] + " option[value!='']").remove();
                }
            }
        }
    },
    //str=>json格式 id=>SELECT的ID
    BindOption: function (str, id) {
        if (str.length > 0) {
            var ops = str.split(",");
            var html = "";
            for (var i = 0; i < ops.length; i++) {
                if (ops[i].length > 0) {
                    html += "<option value='" + ops[i].split(":")[1] + "'>" + ops[i].split(":")[0] + "</option>";
                }
            }
            this.ClearOption(id);
            $("#" + id).append(html);
        }
    }
};

//简单的自动提示**************************************************************************************************************************************
function ownSugInit() {
    //绑定事件
    $(".sug").live("keyup", function (event) {
        if (event.which != 38 && event.which != 40 && event.which != 13) {  //排除向上，向下，回车
            suggestSuccess($(this));
        }
    });
    //选中其他空间时移除候选词列表并检查
    $("*").click(function (e) {
        if ($(".sug").size() > 0 && $(".sug").next(".sugdemo").size() > 0) {
            var $sug = $(".sugdemo").prev(".sug");
            if ((e.pageX < $sug.offset().left || e.pageX > $sug.offset().left + $sug.width() + 16 || e.pageY < $sug.offset().top || e.pageY > $sug.offset().top + $sug.height() + 16)) {
                $(".sugdemo").remove();
            }
        }
    });
    //窗口大小改变时，动态更新位置
    $(window).resize(function () {
        setPosition();
    });
    //键盘操作，上一条，下一条，回车选择
    var lindex = 0;
    $(".sug").live("keydown", function (event) {
        var $sugdemo = $(this).next(".sugdemo");
        if (event.which == 38) {//向上
            if ($sugdemo.find("li.selected").size() == 0) { //未选中任何一条
                $sugdemo.find("li:last").addClass("selected"); //最后一条选中
                $sugdemo.scrollTop($sugdemo.find("li:first").outerHeight(true) * $sugdemo.find("li").size() - 1); //滚动条到底
            }
            else {
                lindex = $sugdemo.find("li.selected").index(); //当前选中的索引
                if (lindex == 0) { //已选中第一条
                    $sugdemo.find("li:last").addClass("selected").siblings().removeClass("selected"); //最后一条选中
                    $sugdemo.scrollTop($sugdemo.find("li:first").outerHeight(true) * $sugdemo.find("li").size() - 1); //滚动条到底
                }
                else {
                    lindex--; //索引减1
                    $sugdemo.find("li:eq(" + lindex + ")").addClass("selected").siblings().removeClass("selected"); //选中当前的上一条
                    $sugdemo.scrollTop($sugdemo.scrollTop() - $sugdemo.find("li:first").outerHeight(true) + 1); //滚动条减去一条的宽度
                }
            }
        } else if (event.which == 40) {//向下
            if ($sugdemo.find("li.selected").size() == 0) {
                $sugdemo.find("li:first").addClass("selected");
                $sugdemo.scrollTop(0);
            }
            else {
                lindex = $sugdemo.find("li.selected").index();
                if (lindex == $sugdemo.find("li").size() - 1) {
                    $sugdemo.find("li:first").addClass("selected").siblings().removeClass("selected");
                    $sugdemo.scrollTop(0);
                }
                else {
                    lindex++;
                    $sugdemo.find("li:eq(" + lindex + ")").addClass("selected").siblings().removeClass("selected");
                    $sugdemo.scrollTop($sugdemo.scrollTop() + $sugdemo.find("li:first").outerHeight(true) - 1);
                }
            }
        } else if (event.which == 13) { //回车
            $sugdemo.find("li.selected").click();
            return false;

        }
        return false;
    });
}
//设置候选框的位置
function setPosition() {
    if ($(".sugdemo").size() > 0) {
        var $gg = $(".sugdemo").prev(".sug");
        var sposition = $gg.position();
        //设置候选词列表位置
        var top;
        var left;
        if ($gg.parents("div.pageFormContent").size() > 0) {
            top = $gg.parents("div.pageFormContent:eq(0)").scrollTop();
            left = $gg.parents("div.pageFormContent:eq(0)").scrollLeft();
        }
        else {
            top = $gg.parents("div.pageContent:eq(0)").scrollTop();
            left = $gg.parents("div.pageContent:eq(0)").scrollLeft();
        }
        $(".sugdemo").css({ "left": sposition.left + left, "top": sposition.top + $gg.height() + top, "width": $gg.width() + 4 });
        $(".sugdemo").slideDown("fast");
    }
}
var ajax;
function suggestSuccess(arg) {
    //后台地址
    var url = $(arg).attr("sugUrl");
    if ($.trim($(arg).val())) {
        if (ajax) { //立即结束上一次ajax
            ajax.abort();
        }
        ajax = $.ajax({
            type: 'POST', dataType: "json", url: url, cache: false,
            data: { "name": $(arg).val() }, global: false,
            success: function (response) {
                //为空则清空值
                if (response == "" || response == null) {
                    if ($(arg).next(".sugdemo").size() > 0) {
                        $(arg).next(".sugdemo").remove();
                    }
                    $(arg).parent().find(".sugID").val("");
                    doCallBack('');
                    return;
                }
                if ($(arg).next(".sugdemo").size() == 0) {
                    $(arg).after("<div class='sugdemo'></div>");
                }
                //                var html = '';
                //                //后台返回的json
                //                var array = response.split(";");
                //                var viewName = "";
                //                $.each(array, function (j) {
                //                    var liLabel = '';
                //                    //将json转为可用的javascript对象
                //                    var json = eval('(' + this + ')');
                //                    liLabel = json["Name"];
                //                    //填充候选词列表
                //                    html += '<li class="sugtext" lookupId="' + json["id"] + '" ><a href="#">' + liLabel + '</a></li>';
                //                });
                var $sugdemo = $(arg).next(".sugdemo");
                //设置候选词列表位置
                setPosition();
                $sugdemo.html('<ul>' + response + '</ul>').slideDown("fast");
                //默认选中第一条并添加候选词选中时的样式和点击事件
                $sugdemo.find("li:first").addClass("selected").end().find("li").mouseover(function () { $(this).addClass("selected").siblings("li").removeClass("selected"); }).click(function () {
                    $(arg).val($(this).find("a").html());
                    $(arg).nextAll(".sugID:eq(0)").val($(this).attr("lookupId"));
                    $sugdemo.remove();
                    doCallBack($(this).attr("otherInfo"));
                });
            }
        });
    }
    else {
        $(arg).parent().find(".sugID").val("");
        doCallBack('');
    }
}
//根据业务，在页面上复写
function doCallBack(otherInfo) { }
//****************************************************************************************************************************************************

//执行重置密码****************************************************************************************************************************************
function doResetPwd(resetUid) {
    window.art.dialog({
        fixed: true,
        content: "　新密码：<input type='password' id='newPwd' style='margin:10px 2px;'/><br/>确认密码：<input type='password' id='confirmPwd' />",
        padding: 0,
        id: 'resetPwdDialog',
        lock: true,
        title: '重置密码',
        width: 300,
        height: 150,
        ok: function () {
            var message = "success";
            if (!$("#newPwd").val() || !$("#confirmPwd").val() || $("#newPwd").val() != $("#confirmPwd").val() || $("#newPwd").val().length > 20) {
                message = "请正确填写！";
                $("#resetError").remove();
                $("#newPwd").parent().prepend("<div id='resetError' style='width:100%;text-align:center;color:red'>" + message + "</div>");
            }
            else {
                $.ajax({
                    type: 'POST', dataType: "json", url: "/Home/DoResetPwd", cache: false,
                    data: { "uid": resetUid, "pwd": $("#newPwd").val() },
                    global: false,
                    async: false,
                    success: function (response) {
                        if (response.Mes == "success") {
                            window.art.dialog({
                                content: '密码重置成功！',
                                lock: true,
                                time: 3000
                            });
                            $(".d-content").css({ width: "100%", textAlign: "center", padding: "20px 3px" });
                        }
                        else {
                            message = response.Mes;
                            $("#resetError").remove();
                            $("#newPwd").parent().prepend("<div id='resetError' style='width:100%;text-align:center;color:red'>" + message + "</div>");
                        }
                    }
                });
            }
            if (message != "success") {
                this.shake && this.shake(); // 调用抖动接口
                return false;
            }
            return false;
        },
        cancel: true
    });
}

//弹出层，密码找回************************************************************************************************************************************
function showPwdDialog() {
    $("#PB").html("");
    $("#pwdBack").find(":input").each(function () {
        $(this).val("");
    });
    window.art.dialog({
        fixed: true,
        content: document.getElementById("pwdBack"),
        padding: 0,
        id: 'pwdBackDialog',
        lock: true,
        title: '密码找回',
        width: 300,
        height: 150,
        ok: function () {
            var message = "success";
            $.ajax({
                type: 'POST', dataType: "json", url: "/Home/PasswordBack", cache: false,
                data: { "pbUname": $("#pbUname").val() },
                global: false,
                async: false,
                success: function (response) {
                    if (response.Mes == "success") {
                        window.art.dialog({
                            content: '密码重置链接已发送至您的邮箱，请在3天内重置，过期无效！',
                            lock: true,
                            time: 3000
                        });
                        $(".d-content").css({ width: "100%", textAlign: "center", padding: "20px 3px" });
                    }
                    else {
                        message = response.Mes;
                        $("#PB").html(message);
                    }
                }
            });
            if (message != "success") {
                this.shake && this.shake(); // 调用抖动接口
                return false;
            }
            return false;
        },
        cancel: true
    });
}
//****************************************************************************************************************************************************

//设为首页
function shouye(obj) {
    var vrl = window.location.href;
    try {
        obj.style.behavior = 'url(#default#homepage)'; obj.setHomePage(vrl);
    }
    catch (e) {
        if (window.netscape) {
            try {
                window.netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            }
            catch (e) {
                alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
            }
            var prefs = window.Components.classes['@mozilla.org/preferences-service;1'].getService(window.Components.interfaces.nsIPrefBranch);
            prefs.setCharPref('browser.startup.homepage', vrl);
        }
    }
}
//添加到收藏夹
function shoucang() {
    var url = window.location.href;
    var title = document.title;
    try {
        window.external.addfavorite(url, title);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(title, url, "");
        }
        catch (e) {
            alert("加入收藏失败，请使用ctrl+d进行添加");
        }
    }
}
//定时检索是否有新消息********************************************************************************************************************************
var interval;
function checkNewMessage(title, url) {
    window.getOnlineUsers();
    $.ajax({
        type: "POST",
        url: url,
        global: false, // 禁用全局Ajax事件.
        success: function (json) {
            if (json.Total != "0") {
                clearInterval(interval);
                interval = setInterval("changeTitle('" + title + "','【　　　　　　　　】" + title + "','【您有 " + json.Total + " 条新消息】" + title + "')", 495);

            }
            else {
                clearInterval(interval);
                document.title = title;
            }
        }
    });
}
//改变标题
function changeTitle(title, oldTitle, newTitle) {
    if (document.title == title || document.title == oldTitle) {
        document.title = newTitle;
    }
    else if (document.title == newTitle) {
        document.title = oldTitle;
    }
}

//多选下拉框******************************************************************************************************************************************
function JSelect(arg) {
    var $jselect = $(arg).nextAll(".JSelect:first");
    //绑定点击事件，点击外部时隐藏下拉框
    $("*").live("click", function (e) {
        if ($(arg).nextAll(".JSelect:hidden").size() == 0) {
            if (e.pageX < $jselect.offset().left || e.pageX > $jselect.offset().left + $jselect.width() || e.pageY < $(arg).offset().top || e.pageY > ($(arg).offset().top + $(arg).outerHeight() + $jselect.outerHeight())) {
                $jselect.stop(true, true).slideUp();
                if ($(arg).hasClass("required")) {
                    $(arg).css("backgroundImage", "url(/Content/images/rdown.gif)");
                }
                else {
                    $(arg).css("backgroundImage", "url(/Content/images/down.gif)");
                }
            }
        }
    });
    $(window).resize(function () {
        setMuiSelectPosition($jselect);
    });
    //设置位置
    setMuiSelectPosition($jselect);
    if ($(arg).nextAll(".JSelect:hidden").size() > 0) {
        $jselect.stop(true, true).slideDown(200);
        if ($(arg).hasClass("required")) {
            $(arg).css("backgroundImage", "url(/Content/images/rup.gif)");
        }
        else {
            $(arg).css("backgroundImage", "url(/Content/images/up.gif)");
        }
    }
    else {
        $jselect.stop(true, true).slideUp(200);
        if ($(arg).hasClass("required")) {
            $(arg).css("backgroundImage", "url(/Content/images/rdown.gif)");
        }
        else {
            $(arg).css("backgroundImage", "url(/Content/images/down.gif)");
        }
    }
}
//设置值
function JResult(arg) {
    //设置传入的默认值（修改画面）
    if (!$(arg).attr("checked")) {
        $(arg).attr("checked", "checked");
    }
    else {
        $(arg).removeAttr("checked");
    }
    var result = "";
    var hidResult = "";
    $(arg).parents(".JSelect:first").find(":checkbox:checked").each(function () {
        result += $(this).siblings("span").html() + ",";
        hidResult += $(this).val() + ",";
    });
    $(arg).parents(".JSelect:first").prevAll(".JResult:first").val(result);
    $(arg).parents(".JSelect:first").prevAll(".hidJResult:first").val(hidResult);
}
//设置多选下拉框位置
function setMuiSelectPosition(arg) {
    if ($(arg).size() > 0) {
        var $gg = $(arg).prevAll(".JResult:first");
        var sposition = $gg.position();
        //设置候选词列表位置
        var top;
        var left;
        if ($gg.parents("div.pageFormContent").size() > 0) {
            top = $gg.parents("div.pageFormContent:eq(0)").scrollTop();
            left = $gg.parents("div.pageFormContent:eq(0)").scrollLeft();
        }
        else {
            top = $gg.parents("div.pageContent:eq(0)").scrollTop();
            left = $gg.parents("div.pageContent:eq(0)").scrollLeft();
        }
        $(arg).css({ "left": sposition.left + left, "top": sposition.top + $gg.height() + 5 + top, "width": $gg.width() + 13 });
    }
}
//****************************************************************************************************************************************************
//右键菜单
function InitRightMenu(arg) {
    RightMenu(arg);
}
function RightMenu(arg) {
    $(arg).live("contextmenu", function (e) {
        $("#clsno").val($(e.target).attr('id'));
        if ($(this).attr("rel")) {
            $(this).addClass("selected").siblings("tr").removeClass("selected");
        }
        var $contextMenu = $(this).parents(".page").find(".JMenu");
        $contextMenu.show();
        if ($contextMenu.height() > 250)
            $contextMenu.height(250);
        var top;
        var left;
        if ($(arg).parents("div.pageFormContent").size() > 0) {
            top = $(arg).parents("div.pageFormContent:eq(0)").scrollTop();
            left = $(arg).parents("div.pageFormContent:eq(0)").scrollLeft();
        }
        else {
            top = $(arg).parents("div.pageContent:eq(0)").scrollTop();
            left = $(arg).parents("div.pageContent:eq(0)").scrollLeft();
        }
        $contextMenu.css({ top: e.clientY - 95 + top, left: e.pageX - 210 + left });
        if ($contextMenu.offset().top + $contextMenu.height() > document.body.offsetHeight + $("#container").height()) {
            $contextMenu.css({ top: e.pageY - 95 - $contextMenu.height() + top });
        }
        if ($contextMenu.offset().left + $contextMenu.width() + 10 > document.body.offsetWidth) {
            $contextMenu.css({ left: e.pageX - 210 - $contextMenu.width() });
        }
        return false;
    });
    $("*").click(function (e) {
        //        $(".JMenu").hide();
        if ($(arg).parents(".page").find(".JMenu:hidden").size() == 0) {
            var $menu = $(arg).parents(".page").find(".JMenu");
            if ((e.pageX < $menu.offset().left || e.pageX > $menu.offset().left + $menu.width() || e.pageY < $menu.offset().top || e.pageY > $menu.offset().top + $menu.height())) {
                $menu.hide();
            }
        }
    });
}
//导出excel******************************************************************************************************************************************
var ExcelInv;
function ExportExcel(url) {
    $("#background,#progressBar").show();
    
    window.location.href = url;
    ExcelInv = setInterval(checkExcel, 500);
}
function checkExcel() {
    if ($.cookie('ExcelType') == "complete") {
        $("#background,#progressBar").hide();
        $.cookie('ExcelType', '');
        clearInterval(ExcelInv);
    }
}
//高亮显示title******************************************************************************************************************************************
function showTitle(color) {
    var sTitle = "";
    $(".showTitle").live("mouseover", function (e) {
        sTitle = $(this).attr("title");
        $(this).attr("title", "");
        var label = "<div class='tempTitle jTitle" + color + "'>" + sTitle + "</div>";
        $(this).parents("body:first").prepend(label);

        if ($(".tempTitle").width() > 250) {
            //$(".tempTitle").width("250px"); //2013-06-04 cp 注释掉该句
        }
        var tp = -10;
        if (navigator.userAgent.indexOf("MSIE") > 0) {
            tp = -13;
        }
        $(".tempTitle").prepend("<div style='position:absolute;top:" + tp + "px;left:" + $(".tempTitle").width() / 5 + "px'><img src='/Content/Images/tempTitleUp" + color + ".gif'/></div>");
        var top = e.pageY + 30;
        var left = e.pageX - $(".tempTitle").width() / 5 - 10;
        $(".tempTitle").css({ top: top, left: left });
    }).live("mousemove", function (e) {
        var top = e.pageY + 30;
        var left = e.pageX - $(".tempTitle").width() / 5 - 10;
        $(".tempTitle").css({ top: top, left: left });
    }).live("mouseout", function () {
        $(this).attr("title", sTitle);
        $(".tempTitle").remove();
    });
}
//设置文本框默认值******************************************************************************************************************************************
function setTitle($p) {
    var $title = $(".setTitle", $p);
    $title.after("<span class='st' style='display:none;'>" + $title.attr("title") + "</span>");
    var $st = $title.next(".st");
    var width = $title.outerWidth() - 2;
    $st.css({ cursor: "text", color: "gray", position: "absolute", display: "inline-block", Top: "15px", marginLeft: "-" + width + "px", lineHeight: "normal", zIndex: "1" });

    if (!$.trim($title.val())) {
        $st.show();
    }
    $st.click(function () {
        $st.css("zIndex", "-1");
        $title.focus();
    });
    $title.live("focus", function () {
        $st.css("zIndex", "-1");
    }).live("blur", function () {
        if (!$.trim($title.val())) {
            $st.css("zIndex", "1");
        }
    });
}
//打开帮助页面*********************************************************************************************************************************
function openJHelp(id) {
    $.pdialog.open("/Permission/ViewHelps/" + id, "PermissionViewHelps", "帮助", { width: 600, height: 400, mask: true });
}


$.fn.navMenuInit = function () {
    this.each(function () {
        var $box = $(this);
        $box.find("li>a").click(function () {
            var $a = $(this);
            $.post($a.attr("menu"), {}, function (html) {
                if ($a.attr("menu").indexOf("Sidebar") > 0) {
                    $("#sidebar .toggleCollapse h2").html($a.html());
                }
                else {
                    $("#sidebar .toggleCollapse h2").html($a.attr("site"));
                }

                $("#sidebar").find(".accordion").remove().end().append(html).initUI();
                $box.find("li").removeClass("cn");
                $a.parent().addClass("cn");
            });

            if ($a.attr("href")) {
                navTab.openTab($a.attr("rel"), $a.attr("href"), { title: $a.html(), fresh: true });
            }

            return false;
        });
    });

    var li = this.find("li").get(0);
    $.post($(li).find("a").attr("menu"), {}, function (html) {
        $("#sidebar .toggleCollapse h2").html($(li).find("a").html());
        $("#sidebar").find(".accordion").remove().end().append(html).initUI();
        $(li).addClass("cn");
    });

    return false;
};
$.fn.navSiteInit = function () {
    this.each(function () {
        var $box = $(this);
        $box.find("a").click(function () {
            var $a = $(this);
            $.post($a.attr("menu"), {}, function (html) {
                if ($a.attr("menu").indexOf("Sidebar") > 0) {
                    $("#sidebar .toggleCollapse h2").html($a.html());
                }
                else {
                    $("#sidebar .toggleCollapse h2").html($a.attr("site"));
                }
                navTab.closeAllTab();
                $("#sidebar").find(".accordion").remove().end().append(html).initUI();
            });

            if ($a.attr("href")) {
                navTab.openTab($a.attr("rel"), $a.attr("href"), { title: $a.html(), fresh: true });
            }

            return false;
        });
    });

    return false;
};
//高级检索*********************************************************************************************************************************
var _NavTabMasterInitSearchHeight = 0;
function _NavTabMasterInit() {
    if ($(".toolBar", navTab.getCurrentPanel()).size() > 0 && $(".pageHeader", navTab.getCurrentPanel()).size() == 0) {
        $(".toolBar", navTab.getCurrentPanel()).parent(".panelBar").css("marginTop", "-1px");
    }
    _NavTabMasterInitSearchHeight = $('.searchBar', navTab.getCurrentPanel()).height();
    $('.searchBar', navTab.getCurrentPanel()).hide();
    //动态设置layoutH
    var headerHt = 1;
    if ($(".pageHeader", navTab.getCurrentPanel()).size() > 0) {
        headerHt = 13;
    }
    var panelBarHeight1 = 0;
    if ($(".toolBar", navTab.getCurrentPanel()).size() > 0) {
        panelBarHeight1 = 28;
    }
    var tabHeight = 0;
    if ($(".tabsHeader", navTab.getCurrentPanel()).size() > 0) {
        tabHeight = 28;
    }
    var gridHead = 0;
    if ($(".table", navTab.getCurrentPanel()).size() > 0) {
        gridHead = 22;
    }
    $("[layoutH]", navTab.getCurrentPanel()).attr("layoutH", $("div.pagination", navTab.getCurrentPanel()).parent(".panelBar").height() + headerHt + panelBarHeight1 + tabHeight + gridHead);
    //固定分页条
    $("div.pagination", navTab.getCurrentPanel()).parent(".panelBar").css({ position: "fixed", width: $(window).width() - $("#sidebar").width() - 17, bottom: "33px", right: "6px" });
    //窗体改变大小时重新计算分页条位置
    $(window).resize(function () {
        if ($("div.pagination", navTab.getCurrentPanel()).size() > 0) {
            $("div.pagination", navTab.getCurrentPanel()).parent(".panelBar").css({ width: $(window).width() - $("#sidebar").width() - 17 });
        }
    });
    //判断是否默认显示搜索栏
    if ($(".superImgST", navTab.getCurrentPanel()).size() > 0 && $(".superImgST", navTab.getCurrentPanel()).val() == "1") {
        superSearch();
    }
    superSearch(true);
    $(".superIMG", navTab.getCurrentPanel()).hover(function () {
        if ($(this).attr("src") == "../../Content/Images/superSearch.png") {
            $(this).attr("src", "../../Content/Images/superSearch11.png");
        }
        else if ($(this).attr("src") == "../../Content/Images/superSearch1.png") {
            $(this).attr("src", "../../Content/Images/superSearch22.png");
        }
    }, function () {
        if ($(this).attr("src") == "../../Content/Images/superSearch11.png") {
            $(this).attr("src", "../../Content/Images/superSearch.png");
        }
        else if ($(this).attr("src") == "../../Content/Images/superSearch22.png") {
            $(this).attr("src", "../../Content/Images/superSearch1.png");
        }
    });
}
function superSearch(arg) {
    var searchBarHet = _NavTabMasterInitSearchHeight;
    //显示搜索栏
    if ($('.searchBar', navTab.getCurrentPanel()).css("display") == "none") {
        $('.searchBar', navTab.getCurrentPanel()).stop(true, false).slideDown(50);
        $(".superIMG", navTab.getCurrentPanel()).attr("src", "../../Content/Images/superSearch1.png");
        $(".superIMG", navTab.getCurrentPanel()).attr("title", "点击隐藏搜索");
        $(".superImgST", navTab.getCurrentPanel()).val("1");
        //计算列表大小
        if ($(".ContentList", navTab.getCurrentPanel()).size() > 0) {
            $(".ContentList [layoutH]", navTab.getCurrentPanel()).attr("layoutH", $("[layoutH]", navTab.getCurrentPanel()).attr("layoutH") * 1 + searchBarHet);
            if (!arg) {
                $(".ContentList [layoutH]", navTab.getCurrentPanel()).layoutH();
            }
        }
    }
    //隐藏搜索栏
    else {
        $('.searchBar', navTab.getCurrentPanel()).stop(true, false).slideUp(50);
        $('.superSearchContent', navTab.getCurrentPanel()).find("input,select").val("");
        $(".superIMG", navTab.getCurrentPanel()).attr("src", "../../Content/Images/superSearch.png");
        $(".superIMG", navTab.getCurrentPanel()).attr("title", "点击展开搜索");
        $(".superImgST", navTab.getCurrentPanel()).val("0");
        //计算列表大小
        if ($(".ContentList", navTab.getCurrentPanel()).size() > 0) {
            $(".ContentList [layoutH]", navTab.getCurrentPanel()).attr("layoutH", $("[layoutH]", navTab.getCurrentPanel()).attr("layoutH") * 1 - searchBarHet);
            $(".ContentList [layoutH]", navTab.getCurrentPanel()).layoutH();
        }
    }
}
//表单格式化成json
$.fn.serializeObject = function () {
    var obj = {};
    var count = 0;
    $.each(this.serializeArray(), function (i, o) {
        var n = o.name, v = o.value;
        count++;
        obj[n] = obj[n] === undefined ? v
        : $.isArray(obj[n]) ? obj[n].concat(v)
        : [obj[n], v];
    });
    obj.nameCounts = count + ""; //表单name个数
    return JSON.stringify(obj);
};

//调用上传页面
function JUpload(guid, navTabId) {
    $.pdialog.open("/Upload/JUpload?guid=" + guid + "&navTabID=" + navTabId, "UploadUpload", "上传", { width: 713, height: 500, mask: true, maxable: false, resizable: false });
}
//修改画面获得附件列表
function JUploadGetAttas(attId, id, sign) {
    $.post("/Upload/GetAttas", { guid: attId, sign: sign }, function (json) {
        $("#" + id).find(".jfile").remove().end().append(json).find("div").css("marginTop", "3px");
    });
}
//伪树形表格*********************************************************************************************************************************
function toggleDept(arg) {
    var id = $(arg).parents("tr:first").attr("rel");
    if ($(arg).find("img.treeImg").attr("src") == "/Content/images/nPlus.gif") {
        $(arg).find("img.treeImg").attr("src", "/Content/images/nMinus.gif");
        $(arg).parents("tr:first").parent().find("tr[prel=" + id + "]").stop(true, true).fadeIn(200);
        $(arg).parents("tr:first").parent().find("tr[prel=" + id + "]").each(function () {
            $(this).find("img.treeImg").attr("src", "/Content/images/nPlus.gif");
        });
    }
    else {
        $(arg).find("img.treeImg").attr("src", "/Content/images/nPlus.gif");
        var tleft = $(arg).parent().css("paddingLeft").replace("px", "") * 1;
        $(arg).parents("tr:first").nextAll("tr").each(function () {
            var tt = $(this).find("td.cc").css("paddingLeft").replace("px", "") * 1;
            if ($(this).find("td.cc").find(".treeImg").size() == 0) {
                tt = tt - $(".treeImg:eq(0)").outerWidth() - 1;
            }
            if (tt > tleft) {
                $(this).hide();
            }
        });
    }
}
//设置级联tr的padding-left，体现层次
function initTreeTable(arg) {
    $("img.treeImg[src$='nPlus.gif']", $(arg)).attr("src", "/Content/images/nMinus.gif");
    $("tbody.Jtt", $(arg)).each(function () {
        var tb = $(this);
        $(this).find("tr").each(function () {
            var rel = $(this).attr("prel");
            if (tb.find("tr[rel=" + rel + "]").size() > 0) {
                $(this).find("td.cc").css("paddingLeft", tb.find("tr[rel=" + rel + "]").find("td.cc").css("paddingLeft").replace("px", "") * 1 + 20 + "px");
                if ($(this).find("td.cc").find("img.treeImg").size() == 0) {
                    var width = 29;
                    if ($(this).find("td.cc").find(":checkbox,:radio").size() > 0) {
                        width = 44;
                    }
                    $(this).find("td.cc").css("paddingLeft", tb.find("tr[rel=" + rel + "]").find("td.cc").css("paddingLeft").replace("px", "") * 1 + width + "px");

                }
            }
        });
    });
}
//自定义打开navTab方法
function customNavTab(tabid, url, title) {
    navTab.openTab(tabid, url, { title: title });
}
//自定义打开Dialog方法
function customDialog(dlgId, url, title, options) {
    $.pdialog.open(url, dlgId, title, options);
}


//身份证验证（15和18位）(可以获得籍贯(#Area)、出生年月(#Birthday)、性别(#Sex【select】或name=Sex【radio】且0为女1为男))
//url为验证身份证是否存在的处理方法路径
function ValidateIDCard(obj) {
    var sId = obj;
    var iSum = 0;
    if (sId != "" && !/^(\d{15}|\d{17}[\dXx])$/.test(sId)) {
        if (sId.length == 15 || sId.length == 18) {
            alertMsg.warn("身份证号格式不正确");
        }
        else {
            alertMsg.warn("您输入的身份证号为（" + sId.length == 15 + "）位数字，应为15或者18为数字");
        }

        return false;
    }

    var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "內蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };
    var sBirthday;
    var d;
    if (sId.length == 15) {//15位身份證驗證
        sBirthday = "19" + sId.substr(6, 2) + "-" + Number(sId.substr(8, 2)) + "-" + Number(sId.substr(10, 2));
        d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) {
            sBirthday = "";
        }
        else {
            sBirthday = "19" + sId.substr(6, 2) + "-" + (Number(sId.substr(8, 2)) >= 10 ? Number(sId.substr(8, 2)) : "0" + Number(sId.substr(8, 2))) + "-" + (Number(sId.substr(10, 2)) >= 10 ? Number(sId.substr(10, 2)) : "0" + Number(sId.substr(10, 2)));
        }
    }
    else {//18位身份證驗證
        sId = sId.replace(/x$/i, "a");
        sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
        d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) {
            sBirthday = "";
        }
        else {
            sBirthday = sId.substr(6, 4) + "-" + (Number(sId.substr(10, 2)) >= 10 ? Number(sId.substr(10, 2)) : "0" + Number(sId.substr(10, 2))) + "-" + (Number(sId.substr(12, 2)) >= 10 ? Number(sId.substr(12, 2)) : "0" + Number(sId.substr(12, 2)));
        }
        for (var i = 17; i >= 0; i--) {
            iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
        }
        if (iSum % 11 != 1) {
            alertMsg.warn("身份证信息为重要信息，请仔细核对");
        }
    }
    var sexFlag = (sId.length == 15) ? sId.substr(14, 1) : sId.substr(16, 1); //男性為奇數，女性為偶數

    //radio
    if (sexFlag % 2 == 0) {
        $("#Sex", navTab.getCurrentPanel()).val("女");
        $("#SexDiv", navTab.getCurrentPanel()).html("女");
    } else {
        $("#Sex", navTab.getCurrentPanel()).val("男");
        $("#SexDiv", navTab.getCurrentPanel()).html("男");
    }
    $("#Birthday", navTab.getCurrentPanel()).val(sBirthday);
    $("#Area", navTab.getCurrentPanel()).val(aCity[parseInt(sId.substr(0, 2))]);
    return false;
}

//身份证验证（15和18位）(可以获得籍贯(#Area)、出生年月(#Birthday)、性别(#Sex【select】或name=Sex【radio】且0为女1为男))
//url为验证身份证是否存在的处理方法路径
function ValidateIDCardValue(obj) {
    var sId = obj;
    var iSum = 0;
    if (sId != "" && !/^(\d{15}|\d{17}[\dXx])$/.test(sId)) {
        return;
    }

    var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "內蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };
    var sBirthday;
    var d;
    if (sId.length == 15) {//15位身份證驗證
        sBirthday = "19" + sId.substr(6, 2) + "-" + Number(sId.substr(8, 2)) + "-" + Number(sId.substr(10, 2));
        d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) {
            sBirthday = "";
        }
        else {
            sBirthday = "19" + sId.substr(6, 2) + "-" + (Number(sId.substr(8, 2)) >= 10 ? Number(sId.substr(8, 2)) : "0" + Number(sId.substr(8, 2))) + "-" + (Number(sId.substr(10, 2)) >= 10 ? Number(sId.substr(10, 2)) : "0" + Number(sId.substr(10, 2)));
        }
    }
    else {//18位身份證驗證
        sId = sId.replace(/x$/i, "a");
        sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
        d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) {
            sBirthday = "";
        }
        else {
            sBirthday = sId.substr(6, 4) + "-" + (Number(sId.substr(10, 2)) >= 10 ? Number(sId.substr(10, 2)) : "0" + Number(sId.substr(10, 2))) + "-" + (Number(sId.substr(12, 2)) >= 10 ? Number(sId.substr(12, 2)) : "0" + Number(sId.substr(12, 2)));
        }
        for (var i = 17; i >= 0; i--) {
            iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
        }
        if (iSum % 11 != 1) {
            alert("身份证信息为重要信息，请仔细核对");
        }
    }
    var sexFlag = (sId.length == 15) ? sId.substr(14, 1) : sId.substr(16, 1); //男性為奇數，女性為偶數
    var Arr = new Array();
    var Sex = "";
    if (sexFlag % 2 == 0) {
        Sex = "女";
    } else {
        Sex = "男";
    }
    Arr.push(Sex);
    Arr.push(sBirthday);
    Arr.push(aCity[parseInt(sId.substr(0, 2))]);
    return Arr;
}

function UploadImgAjaxDone(json, url) {
    $("#picshow", navTab.getCurrentPanel()).attr("src", json.message);
    $("#avatar", navTab.getCurrentPanel()).val(json.message);

    var $obj = $("#pageForm", navTab.getCurrentPanel());
    $obj.attr("action", url);
    $obj.attr("enctype", "");
    $obj.attr("onsubmit", "return validateCallback(this, navTabAjaxDone);");
}

//居民信息数据添加或修改数据验证
function ValidationData() {
    var spe = $(":hidden[name='exte.IsSpecialCare']", navTab.getCurrentPanel()).val();
    if (spe == "true") {
        var lishu = $("#Martyr_lieshu", navTab.getCurrentPanel()).attr("checked");
        var scjr = $("#Martyr_scjr", navTab.getCurrentPanel()).attr("checked");
        var disnoy = $("#dis_CertificateNo", navTab.getCurrentPanel()).val();
        if (lishu != "checked" && scjr != "checked" && (typeof (disnoy) != "undefined" && (disnoy == null || disnoy == ""))) {
            alertMsg.warn("请确认优抚对象，或者取消（人员属性：优抚人口）勾选！");
            return false;
        }
    }
    var soc = $(":hidden[name='exte.IsMinimalNeeds']", navTab.getCurrentPanel()).val();
    if (soc == "true") {
        var dibao = $("#DifficultyType_dibao", navTab.getCurrentPanel()).attr("checked");
        var socName = $("#soc_UnitName", navTab.getCurrentPanel()).val();
        if (typeof (dibao) != "undefined" && (dibao != "checked") || typeof (socName) != "undefined" && (socName == null || socName == "")) {
            alertMsg.warn("请填写完整劳动保障信息，或者取消（人员属性：低保人口）勾选！");
            return false;
        }
    }
    var dis = $(":hidden[name='exte.IsDisability']", navTab.getCurrentPanel()).val();
    if (dis == "true") {
        var disno = $("#dis_CertificateNo", navTab.getCurrentPanel()).val();
        if (typeof (disno) != "undefined" && (disno == null || disno == "")) {
            alertMsg.warn("请填写残疾信息，或者取消（人员属性：残疾人）勾选！");
            return false;
        }
    }
    var ret = $(":hidden[name='exte.IsVeteranCadres']", navTab.getCurrentPanel()).val();
    if (ret == "true") {
        var retdw = $("#ret_OriginalUnit", navTab.getCurrentPanel()).val();
        if (typeof (retdw) != "undefined" && (retdw == null || retdw == "")) {
            alertMsg.warn("请填写退休信息，或者取消（人员属性：离休老干部）勾选！");
            return false;
        }
    }
    var com = $(":hidden[name='exte.IsComprehensive']", navTab.getCurrentPanel()).val();
    if (com == "true") {
        var comTypes = $(":checkbox[name='comTypes'][checked='checked']", navTab.getCurrentPanel()).val();
        if (typeof (comTypes) != "undefined" && (comTypes == null || comTypes == "")) {
            alertMsg.warn("请填写综治信息，或者取消（人员属性：综治人员）勾选！");
            return false;
        }
    }
    return true;
}
//解析xml字符串
loadXML = function (xmlString) {
    var xmlDoc = null;
    //判断浏览器的类型
    //支持IE浏览器 
    if (!window.DOMParser && window.ActiveXObject) {   //window.DOMParser 判断是否是非ie浏览器
        var xmlDomVersions = ['MSXML.2.DOMDocument.6.0', 'MSXML.2.DOMDocument.3.0', 'Microsoft.XMLDOM'];
        for (var i = 0; i < xmlDomVersions.length; i++) {
            try {
                xmlDoc = new ActiveXObject(xmlDomVersions[i]);
                xmlDoc.async = false;
                xmlDoc.loadXML(xmlString); //loadXML方法载入xml字符串
                break;
            } catch (e) {
            }
        }
    }
    //支持Mozilla浏览器
    else if (window.DOMParser && document.implementation && document.implementation.createDocument) {
        try {
            /* DOMParser 对象解析 XML 文本并返回一个 XML Document 对象。
            * 要使用 DOMParser，使用不带参数的构造函数来实例化它，然后调用其 parseFromString() 方法
            * parseFromString(text, contentType) 参数text:要解析的 XML 标记 参数contentType文本的内容类型
            * 可能是 "text/xml" 、"application/xml" 或 "application/xhtml+xml" 中的一个。注意，不支持 "text/html"。
            */
            domParser = new DOMParser();
            xmlDoc = domParser.parseFromString(xmlString, 'text/xml');
        } catch (e) {
        }
    }
    else {
        return null;
    }

    return xmlDoc;
}
//时间函数，在日期上加上指定的部门  interval参数：
//y	年
//q	季度
//m	月
//d	日
//w	周
//h	小时
//n	分钟
//s	秒
//ms 毫秒
Date.prototype.dateAdd = function (interval, number) {
    var d = this;
    var k = { 'y': 'FullYear', 'q': 'Month', 'm': 'Month', 'w': 'Date', 'd': 'Date', 'h': 'Hours', 'n': 'Minutes', 's': 'Seconds', 'ms': 'MilliSeconds' };
    var n = { 'q': 3, 'w': 7 };
    eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
    return d;
}
//日期格式化
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
        (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}