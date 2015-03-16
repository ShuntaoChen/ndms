<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>"  validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>回复信息</title>
    <script language="javascript" type="text/javascript" >
    function DoReplyMSGManage() {
        if ($.trim($('#MSGManageReplyQueue').html()) != "") {
            $("#form").attr("action", "<%= Url.Action("DoReply", "MSGManage") %>");
            document.getElementById("guid").value=guid;
            $('#MSGManageReply',navTab.getCurrentPanel()).uploadifySettings('scriptData',{'guid':guid,'pub':'IM'});
            $('#MSGManageReply').uploadifyUpload();
        }
        else{
            $("#form").submit();
        }
    }
    </script>
</head>
<body>
    <div class="page">
    <div class="pageHeader">
        <span class="info info_msg">回复消息</span>
    </div>
        <div class="pageContent">
            <form method="post" id="form" action="<%= Url.Action("DoReply", "MSGManage") %>" class="pageForm required-validate"
            onsubmit="return validateCallback(this, navTabAjaxDone);">
            <input type="hidden" id="guid" name="guid" value="" />
            <div class="pageFormContent" layouth="93">
                <table class="formTable" style="width: 98%; height: 95%;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <th>
                                    <label>
                                        收件人：</label>
                                        </th>
                                <td style="width: 100%; height: 5%; vertical-align: middle;">
                                    <input name="MessageReceive" id="Recives" style="width: 98%; vertical-align: middle;"
                                        type="text" readonly="readonly" value="<%= TempData["Reciver"] %>" />
                                </td>
                            </tr>
                            <tr>
                               <th>
                                    <label>
                                        主题：</label>
                                        </th>
                                <td style="width: 100%; height: 5%; vertical-align: middle;">
                                    <input name="MessageTitle" id="MessageTitle" style="width: 98%; vertical-align: middle;"
                                        type="text" />
                                </td>
                            </tr>
                             <tr>
                               <th>
                                    <label>
                                        内容：</label>
                                        </th>
                                <td style="width: 99%; height: 80%; vertical-align: top;">
                                    <textarea name="Content" class="editor" style="width: 100%; height: 380%;"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        附件：</label>
                                        </th>
                                <td style="width: 100%; height: auto; vertical-align: middle;">
                                    <% Html.RenderPartial("~/Views/Shared/Upload.ascx");%>
                                </td>
                            </tr>
                           
                        </table>
                        
                <input type="hidden" name="userid" value="<%= TempData["sendid"] %>" />
            </div>
            <div class="formBar">
                <ul>
                    <li>
                        <div class="buttonActive">
                            <div class="buttonContent">
                                <button type="button" onclick="DoReplyMSGManage();">
                                    回复</button></div>
                        </div>
                    </li>
                    <li>
                        <div class="button">
                            <div class="buttonContent">
                                <button type="button" class="close">
                                    取消</button></div>
                        </div>
                    </li>
                </ul>
            </div>
            </form>

        </div>
    </div>
</body>
</html>
