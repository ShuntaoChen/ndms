<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发件箱</title>
    <script language="javascript" type="text/javascript">
    function lookMSGSend(){
        var id = $("#selectedId",navTab.getCurrentPanel()).val();
        if (!id) {
            alertMsg.error("请先选择一条消息。");
            return false;
        }
        $("tr.selected").find("img.readStatus").attr("src","../../Content/images/Read.png");
        navTab.openTab("MesMSGManageMyMsg_ReciveLook",  "<%= Url.Action("Look", "MSGManage") %>/"+id, {title:"查看消息"});//统一用MSGManage中的Look页面
    }

     function replyMSGManage(){
        var id = $("#selectedId",navTab.getCurrentPanel()).val();
        if (!id) {
            alertMsg.error("请先选择一条消息。");
            return false;
        }
         navTab.openTab("MesMSGSendMyMsg_SendReply",  "<%= Url.Action("Reply", "MSGSend") %>/"+id.split(',')[0], {title:"回复消息"});
    }

    function writeMSGSend(){
        navTab.openTab("MesMSGManageMyMsg_ReciveWrite",  "<%= Url.Action("Write", "MSGManage") %>", {title:"写信息"});
    }
     //获取勾选中的记录
    function GetMSGSendCheckMes()
    {
       var list="";
       $("input[name='scheck_de']", navTab.getCurrentPanel()).each(function(){
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
    //转至
    function turnMSGSend()
    {
        var list=GetMSGSendCheckMes();
       
        if (!list) {
            alertMsg.error("请至少勾选一条消息。");
            return false;
        }
        $.pdialog.open("<%= Url.Action("Turn", "MSGSend") %>/"+list, "TurnTo", "转至", {width: 380, height: 150, mask: true});
    }
    function deleteMSGSend() {
        var deletelist=GetMSGSendCheckMes();;
        if (!deletelist) {
            alertMsg.error("请至少勾选一条消息。");
            return false;
        }
        alertMsg.confirm("确定将所选择消息放入垃圾箱吗？", {
            okCall: function () {
                $.post("<%= Url.Action("Delete", "MSGSend") %>", {id:deletelist}, function (json) {
                    jsonMsgDone(json);
                    if (json.statusCode == 200) {
                        navTab.openTab("MesMSGSendMyMsg_Send",  "<%= Url.Action("MyMsg_Send", "MSGSend") %>", {title:"发件箱"});
                    }
                }, "json");
            }
        });
    }

    function reloadMSGSend(){
        navTab.openTab("MesMSGSendMyMsg_Send",  "<%= Url.Action("MyMsg_Send", "MSGSend") %>", {title:"发件箱"});
    }
    </script>
</head>
<body>
    <script>
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
        <form id="pagerForm" onsubmit="return navTabSearch(this);" action="<%= Url.Action("MyMsg_Send", "MSGSend") %>"
        method="post">
        <div class="searchBar">
            <input type="hidden" name="Dir" value="<%= Html.Encode(ViewData["Dir"]) %>" />
            <input type="hidden" name="pageNum" value="<%= Model.PageNumber%>" />
	        <input type="hidden" name="numPerPage" value="<%= Model.PageSize%>" />
	        <input type="hidden" name="orderField" value="<%= ViewData["orderField"]%>" />
            <input type="hidden" name="orderDir" value="<%= ViewData["orderDir"]%>" />
            <table class="searchContent" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <th>发送时间：</th>
                    <td>
                        <input type="text" name="startDate" class="Wdate" size="13" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" readonly="readonly" value="<%:ViewData["startDate"] %>"/>
                         <span class="limit">-</span>
                        <input type="text" name="endDate" class="Wdate"  size="13" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" readonly="readonly" value="<%:ViewData["endDate"] %>"/>
                    </td>
                    <th>
                        关键字：
                    </th>
                    <td>
                        <input type="text" name="KeyWord" value="<%=ViewData["KeyWord"] %>" size="30" placeholder="信息标题/接收人/信息内容" />
                    </td>
                </tr>
            </table>
            <div class="subBar">
                <span class="info_clear">符合条件的记录，共有 <strong><%=(ViewData.Model as PagedList.IPagedList<Web.Areas.Mes.Models.X_UserMessages>).Count %></strong> 条。</span>
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
                <li><a class="write" href="javascript:void(0)"  onclick="writeMSGSend();"><span>写信</span></a></li>
                <li><a class="turn" href="javascript:void(0)"  onclick="turnMSGSend();"><span>转至</span></a></li>
                <li><a class="reload" href="javascript:void(0)"  onclick="reloadMSGSend();"><span>刷新</span></a></li>
                <li><a class="delete" href="javascript:void(0)"  onclick="deleteMSGSend();"><span>删除</span></a></li>
            </ul>
        </div>
        <table class="table" width="100%" layouth="141">
            <thead>
                <tr>
                    <th width="30px">
                        <input type="checkbox" id="schk_all" group="scheck_de" class="checkboxCtrl" />
                    </th>
                    <th align="left" width="15%">
                        发件人
                    </th>
                    <th align="left">
                        主题
                    </th>
                    <th align="center" width="15%" orderfield="CreateDate" class="<%= ViewData["orderDir"] %>">
                        发送时间
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
                <tr target="selectedId" rel="<%= m.X_Messages_ID  %>,<%= m.ID %>" ondblclick="lookMSGSend()">
                   <td style="text-align:center">
                        <input type="checkbox" id="schk<%= i %>" name="scheck_de" value="<%: m.ID.ToString() %>" />
                    </td>
                    <td>
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
                        %>
                    </td>
                    <td>
                        <span style="display:inline-block;width:84%">
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
                        </span>
                        <span style="color:blue;display:inline-block;text-align:right;width:15%" title="【已读/全部】">【<%:m.X_Messages.X_UserMessages.Where(w => w.X_Messages_ID == m.X_Messages_ID && w.MyMsg == 1 && w.ReadStatus == 1).Count() + "/" + m.X_Messages.X_UserMessages.Where(w => w.X_Messages_ID == m.X_Messages_ID && w.MyMsg == 1).Count()%>】</span>
                    </td>
                    <td>
                        <% if (m.X_Messages.CreateDate.ToString() != "")
                           {%>
                        <%:Convert.ToDateTime(m.X_Messages.CreateDate.ToString()).ToString("yyyy-MM-dd hh:mm")%>
                        <%} %>
                    </td>
                    <td>
                        <%if (!string.IsNullOrEmpty(m.X_Messages.AttachmentID))
                          { %>
                        <a href="/Mes/UploadCommon/DownLoadList?attachment=<%:m.X_Messages.AttachmentID%>"
                            rel="UploadCommonDownLoadList" target="dialog" mask="true" title="附件列表">
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
