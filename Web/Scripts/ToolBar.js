function OpenDialog(title, url, rel, width, height) {
    $.pdialog.open(url, rel, title, { width: width, height: height, mask: true });
    return false;
}

//确认并做下载文件
function ConfirmAndDownload(url, sid) {
    var uid = $("#" + sid, navTab.getCurrentPanel()).val();
    if (!uid) {
        alertMsg.warn("请先选择一个文件！");
        return false;
    }
    var char = url.indexOf("?") > 0 ? "&" : "?";
    alertMsg.confirm("确定要下载这个文件吗？", {
        okCall: function () {
            window.location.href = url + char + "id=" + uid;
        }
    });
    return false;
}

//生成pdf
function GenPdf(title, url, sid) {
    var uid = $("#" + sid, navTab.getCurrentPanel()).val();
    if (!uid) {
        return false;
    }

    alertMsg.confirm(title, {
        okCall: function () {
            var char = url.indexOf("?") > 0 ? "&" : "?";
            $(window.parent.document).find("#gen_pdf").attr("src", url + char + "id=" + uid);
            setTimeout(reload, 1000);
        }
    });
    return false;
}

function GenPdfMulti(title, url, ids) {
    var selected = [];

    var $checkboxLi = $(":checkbox[name='" + ids + "']", navTab.getCurrentPanel());
    $checkboxLi.each(function () {
        if ($(this).attr("checked") == "checked") {
            selected.push($(this).attr("value"));
        }
    });

    if (selected.length == 0) {
        alertMsg.warn("至少选择一条数据！");
        return false;
    }

    alertMsg.confirm(title, {
        okCall: function () {
            var char = url.indexOf("?") > 0 ? "&" : "?";
            $(window.parent.document).find("#gen_pdf").attr("src", url + char + "ids=" + selected.join(","));
            //setTimeout(reload, 5000);
        }
    });
    return false;
}

function reload() {
    navTabSearch($("#pagerForm", navTab.getCurrentPanel()));
}

//确认并做操作,传一个id参数。
function ConfirmAndAjaxTodo(title, url, sid) {
    var uid = $("#" + sid, navTab.getCurrentPanel()).val();
    if (!uid) {
        alertMsg.warn("请先选择一条数据！");
        return false;
    }

    alertMsg.confirm(title, {
        okCall: function () {

            $.ajax({
                type: 'POST',
                url: url,
                data: { id: uid, title: title },
                dataType: "json",
                cache: false,
                success: function (json) {
                    DWZ.ajaxDone(json);
                    navTabSearch($("#pagerForm", navTab.getCurrentPanel()));
                },
                error: DWZ.ajaxError
            });
        }
    });
    return false;
}

//确认并做操作,传多个id参数。
function ConfirmAndAjaxTodoWithMultiIds(title, ids, url) {
    var selected = [];

    var $checkboxLi = $(":checkbox[name='" + ids + "']", navTab.getCurrentPanel());
    $checkboxLi.each(function () {
        if ($(this).attr("checked") == "checked") {
            selected.push($(this).attr("value"));
        }
    });


    if (selected.length == 0) {
        alertMsg.warn("至少选择一条数据！");
        return false;
    }

    alertMsg.confirm(title, {
        okCall: function () {

            $.ajax({
                type: 'POST',
                url: url,
                data: { ids: selected.join(",") },
                dataType: "json",
                cache: false,
                success: function (json) {
                    DWZ.ajaxDone(json);
                    navTabSearch($("#pagerForm", navTab.getCurrentPanel()));
                },
                error: DWZ.ajaxError
            });

            
        }
    });
    return false;
}

//确认并做导出数据。
function ConfirmAndExport(url) {
    alertMsg.confirm("确定要导出当前数据吗？", {
        okCall: function () {
            var data = $("#pagerForm", navTab.getCurrentPanel()).serialize();
            var inputs = '';
            jQuery.each(data.split('&'), function () {
                var pair = this.split('=');
                inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
            });
            jQuery('<form action="' + url + '" method="post">' + inputs + '</form>').appendTo('body').submit().remove();
        }
    });
    return false;
}

//传一个id参数,打开一个窗口。
function OpenDialogWithID(title, url, rel, width, height, sid) {
    var uid = $("#" + sid, navTab.getCurrentPanel()).val();
    if (!uid) {
        alertMsg.error("请先选择一条数据！");
        return false;
    }
    if (uid.indexOf("_") >= 0) //uid当中包括id和标题信息
    {
        var arr = uid.split("_");
        uid = arr[0];
        title = arr[1];
        if (arr[2]) {
            rel = arr[2];
        }
    }
    var char = url.indexOf("?") > 0 ? "&" : "?";
    $.pdialog.open(url + char + "id=" + uid, rel, title, { width: width, height: height, mask: true });
    return false;
}

//传多个id参数,打开一个窗口。
function OpenDialogWithMulti(title, url, rel, width, height, ids) {

    var selected = [];

    var $checkboxLi = $(":checkbox[name='" + ids + "']", navTab.getCurrentPanel());
    $checkboxLi.each(function () {
        if ($(this).attr("checked") == "checked") {
            selected.push($(this).attr("value"));
        }
    });

    if (selected.length == 0) {
        alertMsg.warn("至少勾选一条数据！");
        return false;
    }

    var char = url.indexOf("?") > 0 ? "&" : "?";
    $.pdialog.open(url + char + "ids=" + selected.join(","), rel, title, { width: width, height: height, mask: true });
    return false;
}

function OpenNavTabWithMulti(title, url, rel, ids) {
    var selected = [];

    var $checkboxLi = $(":checkbox[name='" + ids + "']", navTab.getCurrentPanel());
    $checkboxLi.each(function () {
        if ($(this).attr("checked") == "checked") {
            selected.push($(this).attr("value"));
        }
    });

    if (selected.length == 0) {
        alertMsg.warn("至少选择一条数据！");
        return false;
    }

    var char = url.indexOf("?") > 0 ? "&" : "?";
    navTab.openTab(rel, url + char + "ids=" + selected.join(","), { title: title });
    return false;
}

//传零个或者一个个id参数,打开一个窗口
function OpenDialogOrWithID(title, url, rel, width, height, sid) {
    var uid = $("#" + sid, navTab.getCurrentPanel()).val();

    if (uid.indexOf("_") >= 0) //uid当中包括id和标题信息
    {
        var arr = uid.split("_");
        uid = arr[0];
        title = arr[1];
        if (arr[2]) {
            rel = arr[2];
        }
    }

    var char = url.indexOf("?") > 0 ? "&" : "?";
    $.pdialog.open(url + char + "id=" + uid, rel, title, { width: width, height: height, mask: true });
    return false;
}

//传一个id参数,打开一个NavTab。
function OpenNavTabWithID(title, url, rel) {
    var uid = $("#selectedId", navTab.getCurrentPanel()).val();
    if (!uid) {
        alertMsg.error("请先选择一条数据！");
        return false;
    }

    if (uid.indexOf("_") >= 0) //uid当中包括id和标题信息
    {
        var arr = uid.split("_");
        uid = arr[0];
        title = arr[1];
        if (arr[2]) {
            rel = arr[2];
        }
    }

    var char = url.indexOf("?") > 0 ? "&" : "?";
    navTab.openTab(rel, url + char + "id=" + uid, { title: title, fresh: false });
    return false;
}

function OpenNavTab(title, url, rel) {
    navTab.openTab(rel, url, { title: title, fresh: false });
    return false;
}