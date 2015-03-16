//--------------------------------------------JUpload控件相关--------------------------------------------
//调用上传页面
function JUpload(guid, navTabId, controlid) {
    $.pdialog.open("/Plugs/Upload/JUpload?guid=" + guid + "&navTabID=" + navTabId + "&controlid=" + controlid, "UploadUpload", "上传", { width: 713, height: 500, mask: true, maxable: false, resizable: false });
}
//修改画面获得附件列表
function JUploadGetAttas(attId, id, sign) {
    $.post("/Plugs/Upload/GetAttas", { guid: attId, sign: sign }, function (json) {
        $("#" + id).find(".jfile").remove().end().append(json).find("div").css("marginTop", "3px");
    });
}
//删除上传附件
function deleteFile(arg, id, controlid) {
    $.get("/Plugs/Upload/Delete", { id: id }, function () {
        
        if($(arg).parent().parent().find("div").size() == 1){
           $("#" + controlid).val("");
       }
       $(arg).parent().remove();
    });
}
//--------------------------------------------JUpload控件相关--------------------------------------------