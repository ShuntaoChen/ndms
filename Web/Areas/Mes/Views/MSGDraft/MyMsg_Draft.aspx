<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>草稿箱</title>
    <script language="javascript" type="text/javascript">
    function lookMSGDraft(){
        var id = $("#selectedSMSId",navTab.getCurrentPanel()).val();
        if (!id) {
            alertMsg.error("请先选择一条消息。");
            return false;
        }
        navTab.openTab("MesMSGDraftMyMsg_DraftLook",  "<%= Url.Action("Look", "MSGDraft") %>/"+id, {title:"查看消息"});
    }

    function writeMSGDraft(){
        navTab.openTab("MesMSGManageMyMsg_ReciveWrite",  "<%= Url.Action("Write", "MSGManage") %>", {title:"写信息"});
    }

    function deleteMSGDraft() {
        var deletelist="";
         $("input[name='dcheck_de']", navTab.getCurrentPanel()).each(function(){
            if($(this).attr("checked")=="checked"||$(this).attr("checked")==true)
            {
                if(deletelist=="")
                {
                    deletelist = $(this).val();
                }
                else
                {
                    deletelist= deletelist+","+$(this).val();
                }
            }
       });
        if (!deletelist) {
            alertMsg.error("请至少勾选一条消息。");
            return false;
        }
        alertMsg.confirm("确定将所选择消息放入垃圾箱吗？", {
            okCall: function () {
                $.post("<%= Url.Action("Delete", "MSGManage") %>", {id:deletelist}, function (json) {
                    jsonMsgDone(json);
                    if (json.statusCode == 200) {
                        navTab.openTab("MesMSGDraftMyMsg_Draft",  "<%= Url.Action("MyMsg_Draft", "MSGDraft") %>", {title:"草稿箱"});
                    }
                }, "json");
            }
        });
    }
    </script>
</head>
<body>
    <script type="text/javascript">
        window.onload = function () {
            var size = document.getElementById("size").value;
            var s1 = document.getElementById("sc_size");
            for (i = 0; i <= s1.length; i++) {
                if (s1.options[i].value == size) {
                    s1.options[i].selected = true;
                }
            }
        }
    </script>
   
    <div class="pageHeader">
        <form id="pagerForm" onsubmit="return navTabSearch(this);" action="<%= Url.Action("MyMsg_Draft", "MSGDraft") %>"
        method="post">
        <div class="searchBar">
            <input type="hidden" name="pageNum" value="<%= Model.PageNumber%>" />
	    <input type="hidden" name="numPerPage" value="<%= Model.PageSize%>" />
	    <input type="hidden" name="orderField" value="<%= ViewData["orderField"]%>" />
        <input type="hidden" name="orderDir" value="<%= ViewData["orderDir"]%>" />
            <table class="searchContent" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <th>时间：</th>
                    <td>
                        <input type="text" name="startDate" class="Wdate" size="13" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" readonly="readonly" value="<%:ViewData["startDate"] %>" />
                          <span class="limit">-</span>
                         <input type="text" name="endDate" class="Wdate"  size="13" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" readonly="readonly" value="<%:ViewData["endDate"] %>" />
                    </td>
                  
                </tr>
            </table>
            <div class="subBar">
                <span class="info_clear">符合条件的记录，共有 <strong><%= (ViewData.Model as PagedList.IPagedList<Web.Areas.Mes.Models.X_UserMessages>).Count %></strong> 条。</span>
                <ul>
                    <li>
                        <div class="buttonActive">
                            <div class="buttonContent">
                                <button type="submit">
                                    搜索</button></div>
                        </div>
                    </li>
                    <li>
                        <div class="button">
                            <div class="buttonContent">
                                <button type="reset">
                                    重置</button></div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        </form>
    </div>
    <div class="pageContent">
        <div class="panelBar">
            <ul class="toolBar">
                <li><a class="write" href="javascript:void(0)"  onclick="writeMSGDraft();"><span>写信</span></a></li>
                <li><a class="delete" href="javascript:void(0)"  onclick="deleteMSGDraft();"><span>删除</span></a></li>
            </ul>
        </div>
        <table class="table" width="100%" layouth="141">
            <thead>
                <tr>
                    <th width="30px">
                        <input type="checkbox" id="chk_all" group="dcheck_de"  class="checkboxCtrl" />
                    </th>
                    <th align="left">
                        主题
                    </th>
                    <th align="center" width="15%" orderfield="CreateDate" class="<%= ViewData["orderDir"] %>">
                        编写时间
                    </th>
                    <th align="center" width="35px">
                        附件
                    </th>
                </tr>
            </thead>
            <tbody>
                <%  int i = 0;
                    foreach (var m in ViewData.Model as PagedList.IPagedList<Web.Areas.Mes.Models.X_UserMessages>)
                    {
                        i++;%>
                <tr target="selectedSMSId" rel="<%= m.X_Messages_ID  %>,<%= m.ID %>" ondblclick="lookMSGDraft()">
                    <td style="text-align: center">
                        <input type="checkbox" id="chk<%= i %>" name="dcheck_de" value="<%: m.ID.ToString() %>" />
                    </td>
                    <td>
                        <% if (m.X_Messages.MessageTitle != "" && m.X_Messages.MessageTitle != null)
                           {
                               if (m.X_Messages.MessageTitle.Length > 60)
                               {%>
                        <%: m.X_Messages.MessageTitle.Substring(0, 60) + "..."%>
                        <%}
                            else
                            { %>
                        <%: m.X_Messages.MessageTitle%>
                        <%}
                        } %>
                    </td>
                    <td>
                        <% if (m.X_Messages.CreateDate.ToString() != "")
                           {%>
                        <%:Convert.ToDateTime(m.X_Messages.CreateDate.ToString()).ToString("yyyy年-MM月-dd日 hh:mm")%>
                        <%} %>
                    </td>
                    <td>
                        <%if (!string.IsNullOrEmpty(m.X_Messages.AttachmentID))
                          { %>
                        <a href="/Mes/UploadCommon/DownLoadList?attachment=<%:m.X_Messages.AttachmentID%>" rel="MsgDownLoadList"
                            target="dialog" mask="true" title="附件列表">
                            <img alt="" src="../../Content/images/attachment.png" alt="附件下载" /></a>
                        <%} %>
                    </td>
                </tr>
                <%  } %>
            </tbody>
        </table>
        <%:Html.Partial("~/Views/Shared/_Pager.cshtml")%>
    </div>
</body>
</html>
