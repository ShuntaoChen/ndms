<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" validateRequest="false"  %>
<%--现这个页面没有用到 统一用MSGManage中的Write         2012-2-1  龚荣  --%>
<script language="javascript" type="text/javascript">
function DoWriteMSGDraft() {
        if ($.trim($('#MSGDraftWriteQueue').html()) != "") {
            $("#form").attr("action", "<%= Url.Action("DoWrite", "MSGDraft") %>");
            document.getElementById("guid").value=guid;
            $('#MSGDraftWrite').uploadifySettings('scriptData',{'guid':guid,'pub':'IM'});
            $('#MSGDraftWrite').uploadifyUpload();
        }
        else{
            $("#form").submit();
        }
    }
</script>
<div class="page">
<div class="pageHeader">
        <span class="info info_msg">草稿箱 <b>></b> 写草稿</span>
    </div>
    <div class="pageContent">
        <form method="post" id="form" action="<%= Url.Action("DoWrite", "MSGDraft") %>" class="pageForm required-validate"
        onsubmit="return validateCallback(this, navTabAjaxDone);">
        <input type="hidden" name="hid_Users" value="" id="hid_Users" />
        <input type="hidden" name="guid" id="guid" value="" />
        <div class="pageFormContent" layouth="90">
            <table class="formTable">
                <tr>
                    <th>
                        <label>
                            主题：</label>
                    </th>
                    <td style="width: 100%; height: 5%; vertical-align: middle;">
                        <input name="MessageTitle" id="MessageTitle" style="width: 97%; vertical-align: middle;"
                            type="text" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            内容：</label>
                    </th>
                    <td style="width: 98%; height:auto; vertical-align: top;">
                        <textarea name="Content" class="editor" style="width: 98%;" rows="22"></textarea>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            附件：</label>
                    </th>
                    <td style="width: 100%; height: auto; vertical-align: top;">
                        <% Html.RenderPartial("~/Views/Shared/Upload.ascx");%>
                    </td>
                </tr>
            </table>
        </div>
           <div class="formBar">
            <ul>
                <li>
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="button" onclick="DoWriteMSGDraft();">
                                提交</button></div>
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
