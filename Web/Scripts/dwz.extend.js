


function logout(url) {
    alertMsg.confirm("确定要退出系统吗？", {
        okCall: function () {
            window.location = url;
        }
    });
}

function dialogDone(json) {
    DWZ.ajaxDone(json);
    if (json.statusCode == DWZ.statusCode.ok) {
        navTabSearch($("#pagerForm", navTab.getCurrentPanel()));

        if ("closeCurrent" == json.callbackType) {
            $.pdialog.closeCurrent();
        }
    }
}

function check(obj, ids) {

    $(":checkbox[name='" + ids + "_all']", navTab.getCurrentPanel()).attr("checked", false);

    var $checkboxLi = $(":checkbox[name='" + ids + "']", navTab.getCurrentPanel());
    $checkboxLi.each(function () {
        $(this).attr("checked", false);
    });

    $(obj).find(":checkbox[name='" + ids + "']").attr("checked", true);
}


//判断游览器来阻止事件
function stopEvent(evt) {
    if (window.event) {
        event.cancelBubble = true;
    } else if (evt) {
        evt.stopPropagation();
    }
}   