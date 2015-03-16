<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" ValidateRequest="false" %>
<%--现这个页面没有用到 统一用MSGManage中的Write         2012-2-1  龚荣  --%>
<script type="text/javascript" language="javascript">
    function check() {
        var users = "";
        var names = "";
        for (var i = 1; i <= document.getElementsByName("Users").length; i++) {
            if (document.getElementById("check" + i).checked) {
                if (users == "") {
                    users = document.getElementById("check" + i).value;
                }
                else {
                    users = users + "," + document.getElementById("check" + i).value;
                }
                if (names == "") {
                    names = document.getElementById("name" + i).innerHTML;
                }
                else {
                    names = names + "," + document.getElementById("name" + i).innerHTML;
                }
            }
        }
        document.getElementById("Recives").value = names;
        ResizeTextarea();
        document.getElementById("hid_Users").value = users;
    }

    function Clear(obj) {
        obj.value = "";
    }

    function SetValue(obj) {
        if (obj.value == "") {
            obj.value = "查找联系人...";
        }
    }

    function Search_Users() {
        var searchtext = document.getElementById("Search_User").value;
        if (searchtext == "" || searchtext == "查找联系人...") {
            document.getElementById("Search_User").focus();
        }
        else {
            alert('查找联系人！');
        }
    }

//    function uploadifyAllComplete(event, data) {
//        if (data.errors) {
//            var msg = "上传文件总数: " + data.filesUploaded + "\n"
//                  + "上传错误文件数: " + data.errors + "\n"
//                 + "上传文件总大小: " + data.allBytesLoaded + "\n"
//                  + "上传平均速度: " + data.speed;
//            alert("事件:" + event + "\n" + msg);
//        }
//    }

//    function uploadifyComplete(event, queueId, fileObj, response, data) {
//        DWZ.ajaxDone(DWZ.jsonEval(response));
//    }

//    function uploadifyError(event, queueId, fileObj, errorObj) {
//        alert("事件:" + event + "\n 自生成ID:" + queueId + "\n 文件名:"
//            + fileObj.name + "\n 错误类型:" + errorObj.type + "\n 错误信息:" + errorObj.info);
//    }

    function IsCheck() {
        var item = document.getElementsByName("Chk_User");
        var length = document.getElementsByName("Chk_User").length;
        var idlist = "";
        var namelist = "";
        for (var i = 0; i < length; i++) {
            if (item[i].checked) {
                if (idlist == "") {
                    idlist = item[i].value.split(',')[0];
                    namelist = item[i].value.split(',')[1];
                }
                else {
                var istrue =false;
                for (var j = 0; j < idlist.split(',').length; j++) {
                    var vtext = idlist.split(',')[j];
                    if (vtext == item[i].value.split(',')[0]) {
                        istrue = true;
                    }
                }
                if (!istrue) {
                    idlist = idlist + "," + item[i].value.split(',')[0];
                    namelist = namelist + "," + item[i].value.split(',')[1];
                }
                }
            }
        }
        document.getElementById("Recives").value = namelist;
        ResizeTextarea();
        document.getElementById("hid_Users").value = idlist;
    }

    function DoWriteMSGSend() {
        if ($.trim($('#MSGSendWriteQueue').html()) != "") {
            $("#form").attr("action", "<%= Url.Action("DoWrite", "MSGSend") %>");
            document.getElementById("guid").value=guid;
            $('#MSGSendWrite').uploadifySettings('scriptData',{'guid':guid,'pub':'IM'});
            $('#MSGSendWrite').uploadifyUpload();
        }
        else{
            $("#form").submit();
        }
    }

//    function send(event,data){
//        if(data.errors)
//        {
//            return false;
//        }
//        //alert('提交');
//        $("#form").submit();
//    }

        //自动提示
        function Search(obj){
        alert(obj.value);
        }
//   $("#Recives").style.height = $("#Recives").scrollHeight + 10 + "px";
</script>
<script type="text/javascript">
    // 最小高度
    var minRows = 1;
    // 最大高度，超过则出现滚动条
    var maxRows = 12;
    function ResizeTextarea() {
        var t = document.getElementById('Recives');
        if (t.scrollTop == 0) t.scrollTop = 1;
        while (t.scrollTop == 0) {
            if (t.rows > minRows)
                t.rows--;
            else
                break;
            t.scrollTop = 1;
            if (t.rows < maxRows)
                t.style.overflowY = "hidden";
            if (t.scrollTop > 0) {
                t.rows++;
                break;
            }
        }
        while (t.scrollTop > 0) {
            if (t.rows < maxRows) {
                t.rows++;
                if (t.scrollTop == 0) t.scrollTop = 1;
            }
            else {
                t.style.overflowY = "auto";
                break;
            }
        }
    }
    function AddRecivers() {
        $.pdialog.open("<%:Url.Action("UserSearch", "UploadCommon") %>/?ControlID="+"hid_Users"+"&ControlName="+"Recives","CommonUserSearch","添加收件人",{width: 800, height: 600, mask: true});
    }
</script>

<div class="page">
    <div class="pageHeader">
        <span class="info info_msg">发件箱 <b>></b> 写信息</span>
    </div>
    <div class="pageContent">
    <form method="post" id="form" action="<%= Url.Action("DoWrite", "MSGSend") %>"
class="pageForm required-validate" onsubmit="return validateCallback(this, navTabAjaxDone);">
        <input type="hidden" name="guid" value="" id="guid" />
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                
                <td style="width: 100%">
                    <div layouth="73">
                        <table class="formTable" style="width: 97%">
                            <tr>
                                <th>
                                    <label style="height:auto">
                                        <a href="javascript:void(0);" onclick="AddRecivers()" style="text-decoration:underline; color:Blue" >收件人</a>：
                                    </label>
                                </th>
                                <td>
                                    <%-- <input name="MessageReceive" id="Recives" class="required" style="width: 80%; vertical-align: middle;"
                                        type="text" readonly="readonly" />--%>
                                    <%--<textarea name="MessageReceive" id="Recives" class="required" rows="1" cols="80"style='overflow-y: hidden;'
                                         onpropertychange="ResizeTextarea()"
                                        oninput="ResizeTextarea()"
                                         ></textarea>--%>
                                    <input type="hidden" name="hid_Users" value="" id="hid_Users" />
                                    <textarea  name="MessageReceive" id="Recives" rows="1" style="width: 80%;overflow-y: hidden;background-color:#fff"  onkeyup="ResizeTextarea()"
                                         readonly="readonly" onclick="AddRecivers()"></textarea>
                                        
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        主&nbsp;&nbsp;&nbsp;&nbsp;题：</label>
                                </th>
                                <td>
                                    <input name="MessageTitle" class="required" id="MessageTitle" style="width:80%;
                                        vertical-align: middle;" type="text" size="135" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        内&nbsp;&nbsp;&nbsp;&nbsp;容：</label>
                                </th>
                                <td>
                                    <textarea name="Content" tools="simple" class="editor" style="width: 80%" rows="20"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        附&nbsp;&nbsp;&nbsp;&nbsp;件：</label>
                                </th>
                                <td>
                                    <% Html.RenderPartial("~/Views/Shared/Upload.ascx");%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <div class="formBar">
            <ul>
                <li>
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="button" onclick="SaveToDraft();">
                                存稿</button></div>
                    </div>
                </li>
                <li>
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="button" onclick="DoWriteMSGSend();">
                                发送</button></div>
                    </div>
                </li>
                <li>
                    <div class="button">
                        <div class="buttonContent">
                            <button type="button" class="close">
                                关闭</button></div>
                    </div>
                </li>
            </ul>
        </div>
        </form>
    </div>
</div>

