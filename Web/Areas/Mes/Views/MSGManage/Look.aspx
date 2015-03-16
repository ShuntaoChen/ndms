<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Web.Areas.Mes.Models.X_Messages>" %>
<style type="text/css">
    .mesView p {
    display: block;
    float: left;
    height: auto;
    line-height:21px;
    margin: 0;
    padding: 2px 0;
    width: 100%;
}
</style>

<script type="text/javascript">
    function deleteMSGManage(id) {
        alertMsg.confirm("确定将所选择消息放入垃圾箱吗？", {
            okCall: function () {
                $.post("<%= Url.Action("Delete", "MSGManage") %>", {id: id}, function (json) {
                    jsonMsgDone(json);
                    if (json.statusCode == 200) {
                        navTab.openTab("MesMSGManageMyMsg_Recive",  "<%= Url.Action("MyMsg_Recive", "MSGManage") %>", {title:"收件箱"});
                    }
                }, "json");
            }
        });
    }

    function SendMessage() {
        $.post("<%= Url.Action("FastSend", "MSGManage") %>", { id: "<%= ViewData["MsgID"]%>", CreaterUserID: "<%= Model.CreaterUserID %>", content: $("#content", navTab.getCurrentPanel()).val() }, function (json) {
            jsonMsgDone(json);
        }, "json");
    }

    $(function(){
        if("<%=ViewBag.fromIndex %>"=="Index")
        {
            ReloadIndexMessage();
        }
    });

    function alertErr(){
        alert("系统消息无法回复！");
    }
</script>
<% JiYun.Modules.Core.Services.AttachmentService attService = new JiYun.Modules.Core.Services.AttachmentService(); %>
<% var atts = attService.GetByAttachmentID(Model.AttachmentID); %>
<div class="page">
    <div class="panelBar" style="border-top:0px;">
		<ul class="toolBar">
			<%--<li><a class="look" onclick="lookMSGManage(<%= ViewData["Id"]%>)" href="javascript:;"><span>回复</span></a></li>--%>
            <li><a class="delete" onclick="deleteMSGManage(<%= ViewData["Id"]%>)" href="javascript:;"><span>删除</span></a></li>
            <li><a class="cancel" href="javascript:;" onclick="navTab.closeCurrentTab()"><span>取消</span></a></li>
		</ul>
	</div>
    <div class="pageContent">
        <div class="pageFormContent mesView" style="padding:0" layouth="150">
            <div style="border-bottom:1px solid #3A71A3;background:#F6F9FC">
                <div style="font-size:20px;" class="title"><%:Model.MessageTitle%></div>
                <div>
                    <span style="width:auto;">发件人：</span>
                    <span style="color:green;font-weight:700"><%:Model.MessageSend%></span>
                </div>

                <div>
                    <span style="width:auto;">收件人：</span>
                    <span><%: Model.MessageReceive%></span>
                </div>

                <div>
                    <span style="width:auto;">时　间：</span>
                    <span><%: Model.CreateDate.Value.ToString("yyyy年MM月dd日 HH:mm")%></span>
                </div>  
                         
                    <div>
                    <span style="width:auto;height:100%;vertical-align:top">附　件：</span>
                    <span style="display:inline-block">
                    <%  foreach (var item in atts)
                        {%>
                        <a style="margin-bottom:6px;display:block;color:Gray;" href="<%:Url.Action("DoBasewnload","UploadCommon",new{id=item.ID})%>" title="点击下载附件">
                            <img src="/Content/images/filetype/<%:item.Path.Substring(item.Path.LastIndexOf(".")+1)%>.gif" /><%:item.FileName%></a>   
                    <%  } %>
                    </span>
                </div>
                    
            </div>
            <div style="font-size:17px;" layouth="350">
                <%= Model.MessageContent%>
            </div>
        </div>
    </div>
    <div class="replyBox">
        <b class="title">快捷回复</b>
        <div class="box">
            <%
                string dis = "";
                string ale = "SendMessage();";
                if (Model.CreaterUserID == null || Model.CreaterUserID == 2)
                {
                    dis = "readonly='readonly'";
                    ale = "alertErr();";
                }
             %> 
             <a class="send" <%=dis %> href="javascript:;" onclick="<%=ale %>">发送</a>
            <div class="content"><textarea name="content" <%=dis %> id="content"></textarea></div>
        </div>
    </div>
     
</div>
