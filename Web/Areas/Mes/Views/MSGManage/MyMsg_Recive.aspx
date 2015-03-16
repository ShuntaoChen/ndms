<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<script language="javascript" type="text/javascript">
    function lookMSGManage(){
        var id = $("#selectedSMSId",navTab.getCurrentPanel()).val();
        if (!id) {
            alertMsg.error("请先选择一条消息。");
            return false;
        }
        $("tr.selected").find("img.readStatus").attr("src","../../Content/images/email_open.png");
        navTab.openTab("MesMSGManageMyMsg_ReciveLook",  "<%= Url.Action("Look", "MSGManage") %>/"+id, {title:"查看消息"});
    }

     function replyMSGManage(){
        var id = $("#selectedSMSId",navTab.getCurrentPanel()).val();
        if (!id) {
            alertMsg.error("请先选择一条消息。");
            return false;
        };
        var tmpid=id.split(',')[0];
        $.post("<%= Url.Action("checkReply", "MSGManage") %>", {id:tmpid}, function (json) {
                    if (json.statusCode == 200) {
                         navTab.openTab("MesMSGManageMyMsg_ReciveReply","<%= Url.Action("Reply", "MSGManage") %>/"+tmpid,{title:"回复消息"});
                    }
                    else{
                        jsonMsgDone(json);
                    }
                }, "json");
    }

    function writeMSGManage(){
        navTab.openTab("MesMSGManageMyMsg_ReciveWrite",  "<%= Url.Action("Write", "MSGManage") %>", {title:"写信息"});
    }
    function turnMSGManage()
    {
        var lenght = document.getElementsByName("rcheck_de").length;
        var list=GetCheckMes();
        document.getElementById("hid_list").value=list;
        if (!list) {
            alertMsg.error("请至少勾选一条消息。");
            return false;
        }
        $.pdialog.open("<%= Url.Action("Turn", "MSGManage") %>/"+list, "TurnTo", "转至", {width: 380, height: 150, mask: true});
    }
    //获取勾选中的记录
    function GetCheckMes()
    {
       var list="";
       $("input[name='rcheck_de']", navTab.getCurrentPanel()).each(function(){
            if($(this).attr("checked")=="checked"||$(this).attr("checked")==true)
            {
                if(list=="")
                {
                    list = $(this).val();
                }
                else
                {
                    list= list+","+$(this).val();
                }
            }
       });
       return list;
    }
    function deleteMSGManage() {
        var lenght = document.getElementsByName("rcheck_de").length;
        var deletelist=GetCheckMes();
       
        if (!deletelist) {
            alertMsg.error("请至少勾选一条消息。");
            return false;
        }
        alertMsg.confirm("确定将所选择消息放入垃圾箱吗？", {
            okCall: function () {
                $.post("<%= Url.Action("Delete", "MSGManage") %>", {id:deletelist}, function (json) {
                    jsonMsgDone(json);
                    if (json.statusCode == 200) {
                        navTab.openTab("MesMSGManageMyMsg_Recive",  "<%= Url.Action("MyMsg_Recive", "MSGManage") %>", {title:"收件箱"});
                    }
                }, "json");
            }
        });
    }
    function reloadMSGManage(){
        navTab.openTab("MesMSGManageMyMsg_Recive",  "<%= Url.Action("MyMsg_Recive", "MSGManage") %>", {title:"收件箱"});
    }

</script>
<div class="pageHeader">
    <input type="hidden" name="hid_list" id="hid_list" value="" />
    <form id="pagerForm" onsubmit="return navTabSearch(this);" action="<%= Url.Action("MyMsg_Recive", "MSGManage") %>"
    method="post">
    <input type="hidden" name="pageNum" value="<%= Model.PageNumber%>" />
	<input type="hidden" name="numPerPage" value="<%= Model.PageSize%>" />
	<input type="hidden" name="orderField" value="<%= ViewData["orderField"]%>" />
    <input type="hidden" name="orderDir" value="<%= ViewData["orderDir"]%>" />
    <div class="searchBar">
        <table class="searchContent" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <th> 发送时间：</th>
                <td>
                    <%= Html.TextBox("startDate", null, new { @readonly = "readonly",@onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})", @class = "Wdate", size = "13" })%>
                    <span class="limit">-</span>
                    <%= Html.TextBox("endDate", null, new { @readonly = "readonly", @onfocus = "WdatePicker({dateFmt:'yyyy-MM-dd'})", @class = "Wdate", size = "13" })%>
                </td>
                <th> 读取状态：</th>
                <td>
                    <select name="ReadStatus" >
                        <option value="" <%=(ViewData["ReadStatus"] == null || ViewData["ReadStatus"].ToString() == "") ? "selected" : ""%>>所有</option>
                        <option value="1" <%=(ViewData["ReadStatus"] != null && ViewData["ReadStatus"].ToString() == "1") ? "selected" : ""%>>已读</option>
                        <option value="0" <%=(ViewData["ReadStatus"] != null && ViewData["ReadStatus"].ToString() == "0") ? "selected" : ""%>>未读</option>
                    </select>
                </td>
                <th> 关键字：</th>
                <td>
                    <input type="text" name="KeyWord" value="<%=ViewData["KeyWord"] %>" size="30" placeholder="信息标题/接收人/信息内容" />
                </td>
            </tr>
        </table>
        <div class="subBar">
            <span class="info_clear">符合条件的记录，共有 <strong>
                <%=(ViewData.Model as PagedList.IPagedList<Web.Areas.Mes.Models.X_UserMessages>).Count %></strong>
                条。</span>
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
            <li><a class="write" href="javascript:void(0)"  onclick="writeMSGManage();"><span>写信</span></a></li>
            <li><a class="turn" href="javascript:void(0)"  onclick="turnMSGManage();"><span>转至</span></a></li>
            <li><a class="reply" href="javascript:void(0)"  onclick="replyMSGManage();"><span>回复</span></a></li>
            <li><a class="reload" href="javascript:void(0)"  onclick="reloadMSGManage();"><span>刷新</span></a></li>
            <li><a class="delete" href="javascript:void(0)"  onclick="deleteMSGManage();"><span>删除</span></a></li>
        </ul>
    </div>
    <table class="table" width="100%" layouth="137">
        <thead>
            <tr>
                <th width="30px">
                    <input type="checkbox" id="rchk_all" group="rcheck_de"  class="checkboxCtrl" />
                </th>
                <th align="center" width="35px">
                    状态
                </th>
                <th width="15%">
                    发件人
                </th>
                <th>
                    主题
                </th>
                <th width="15%" orderfield="CreateDate" class="<%= ViewData["orderDir"] %>">
                    发送时间
                </th>
            </tr>
        </thead>
        <tbody>
            <%  int i = 0;
                foreach (var m in ViewData.Model as PagedList.IPagedList<Web.Areas.Mes.Models.X_UserMessages>)
                {
                    i++;%>
            <tr target="selectedSMSId" rel="<%= m.X_Messages_ID  %>,<%= m.ID %>" ondblclick="lookMSGManage()">
                <td style="text-align: center">
                    <input type="checkbox" id="rchk<%= i %>" name="rcheck_de" value="<%: m.ID.ToString() %>" />
                </td>
                <td>
                    <%
                        if (m.ReadStatus == 0)
                        {
                    %>
                    <img src="../../Content/images/mailUnread.gif" class="readStatus" alt="未读" 
                        />
                    <% }else{%>
                  <img src="../../Content/images/email_open.png" class="readStatus" alt="已读" 
                         />
                    <%} %>
                </td>
                <td <%= m.ReadStatus == 0 ? "style='font-weight:700'" : "" %>>
                    <% if (m.X_Messages.MessageSend != "" && m.X_Messages.MessageSend != null)
                       {
                           if (m.X_Messages.MessageSend.Length > 12)
                           {%>
                    <%: m.X_Messages.MessageSend.Substring(0, 12) + "..."%>
                    <%}
                           else
                           { %>
                    <%: m.X_Messages.MessageSend%>
                    <%}
                       }
                       else
                       {%>
                        <%:"系统消息"%>   
                      <% }
                    %>
                </td>
                <td <%= m.ReadStatus == 0 ? "style='font-weight:700'" : "" %>>
                    <%if (!string.IsNullOrEmpty(m.X_Messages.AttachmentID))
                      { %>
                    <a href="/Mes/UploadCommon/DownLoadList?attachment=<%:m.X_Messages.AttachmentID%>" rel="MsgDownLoadList"
                        target="dialog" mask="true" title="附件列表" style="float:right;">
                        <img alt="" src="../../Content/images/attachment.png" alt="附件下载" /></a>
                    <%} %>
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
                <td <%= m.ReadStatus == 0 ? "style='font-weight:700'" : "" %>>
                    <% if (m.X_Messages.CreateDate.ToString() != "")
                       {%>
                    <%:Convert.ToDateTime(m.X_Messages.CreateDate.ToString()).ToString("yyyy-MM-dd HH:mm")%>
                    <%} %>
                </td>
            </tr>
            <%  } %>
        </tbody>
    </table>
</div>
<%:Html.Partial("~/Views/Shared/_Pager.cshtml")%>