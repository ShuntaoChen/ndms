//百家姓功能函数（点击后调用）
function letterCallback(hidnm, letter, callback) {
   
    var key = $(":hidden[name='" + hidnm + "']").val(letter).val();
    $(".cls_letter").css("color", "black");
    $(".cls_letter:contains('" + key + "')").css("color", "blue");

    setTimeout(callback+"()",500);
}


//用户选择使用函数
//重置下标
function UserSelectReSetBoxOrder(ControlID, Hid_Name) {

    var i = 0;
    $("#" + ControlID).find("input[name^='" + Hid_Name + "']").each(function () {
        var name = $(this).attr("name");
        var newName = name.substring(0, name.indexOf("[") + 1) + i + name.substring(name.indexOf("]"));
        $(this).attr("name", newName);
        i++;
    });
}
//移除单个选中
function UserSelectRemoveSpan(obj) {
    //如果对应的行在左边table中为选中状态则更新为未选状态
    $("#CommonUserSearchLeftTbody").find("tr[rel='" + $(obj).parent().attr("id") + "']").find("td:last").find("a").hide();
    $(obj).parent().remove();
    CheckCoreCommonUserSearchSelectUserCount();
}


//单个下载
function DownSignlFile(id) {
    $.ajax({
        type: 'POST', url: "/Upload/Download", cache: false,
        data: { "id": id }, global: false,
        success: function (response) {
            if (response == "0") {
                alert("文件不存在或已删除！");
            } else {
                location.href = "/Upload/Download2?id=" + id;
            }

        }
    });
}