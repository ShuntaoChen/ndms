<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<% var TdID = "MesMSGManageWrite" + Guid.NewGuid().ToString();
                                   var attid = Model.AttachmentID!=null?Model.AttachmentID:""; %>
<script type="text/javascript">
      //发送
     function sendInstantMessaging() {
       var hiddens = $("#Receive :hidden", navTab.getCurrentPanel());
        if(hiddens.length < 1) {
            alertMsg.error("对不起，请在右边列表至少选择一个收件人");
            return false;
        }
        var $form = $("#form", navTab.getCurrentPanel());
        if(!$form.valid()){
            return false;
        }
        if ($(".jfile", navTab.getCurrentPanel()).children().size()==0) {
            $("#AttachmentID", navTab.getCurrentPanel()).val("");
        }
        if($("#MessageTitle",navTab.getCurrentPanel()).val()==""||$("#Content",navTab.getCurrentPanel()).html()==""){
                alertMsg.error("提交数据不完整，请改正后在提交！");
        }
        else{
                $("#form",navTab.getCurrentPanel()).attr("action", "<%= Url.Action("DoLook", "MSGDraft") %>"+"/1");
                $("#form",navTab.getCurrentPanel()).submit();
        }
    }
    //存草稿
    function savedraftInstantMessaging() {
        $("#Recives",navTab.getCurrentPanel()).attr("class","");
        if ($(".jfile", navTab.getCurrentPanel()).children().size()==0) {
            $("#AttachmentID", navTab.getCurrentPanel()).val("");
        }

        if($("#MessageTitle",navTab.getCurrentPanel()).val()==""||$("#Content",navTab.getCurrentPanel()).val()==""){
                alertMsg.error("提交数据不完整，请改正后在提交！");
        }else{
                $("#form",navTab.getCurrentPanel()).attr("action", "<%= Url.Action("DoLook", "MSGDraft") %>"+"/0");
                $("#form",navTab.getCurrentPanel()).submit();
            }
    }

 function Get(obj) {
        var val = $(obj).attr("tvalue");
        var name = $(obj).html();
        var hiddens = $("#Receive input:hidden", navTab.getCurrentPanel())
        // 该人员是否已选
        var flag = false;
        for (var i = 0; i < hiddens.length; i++) {
            if (hiddens[i].value == val) {
                flag = true; 
                break;
            }
        }
        
        if (!flag) {
            $("#Receive", navTab.getCurrentPanel()).append("<span class='ledlist' title='" + name + "'><a class='close' href='javascript:;' onclick='RemoveSpan(this)'></a>" + name + "<input type='hidden' name='ledids' value='" + val + "' /><input type='hidden' name='MessageReceive' value='" + name + "' /></span>");
        }

        IsTip();
    }
    function GetAll(id) {
        var ul = $("#" + id, navTab.getCurrentPanel());
        ul.find("a").each(function () {
            var val = $(this).attr("tvalue");
            var name = $(this).html();
            var hiddens = $("#Receive input:hidden", navTab.getCurrentPanel());
            var flag = false;
            for (var i = 0; i < hiddens.length; i++) {
                if (hiddens[i].value == val) {
                    flag = true;
                    break;
                }
            }
            if (!flag) {
                $("#Receive", navTab.getCurrentPanel()).append("<span class='ledlist' title='" + name + "'><a class='close' href='javascript:;' onclick='RemoveSpan(this)'></a>" + name + "<input type='hidden' name='ledids' value='" + val + "' /><input type='hidden' name='MessageReceive' value='" + name + "' /></span>");
            }
        });

        IsTip();
    }

    function RemoveSpan(obj) {
        $(obj).parents("span").remove();
        IsTip();
    }

    function IsTip() {
        if ($("#Receive input:hidden", navTab.getCurrentPanel()).length > 0) {
            $("#Receive #title", navTab.getCurrentPanel()).hide();
        }
        else {
            $("#Receive #title", navTab.getCurrentPanel()).show();
        }
    }
    //初始化
     $(function () {
        if ($("#Receive input:hidden", navTab.getCurrentPanel()).length > 0) {
            $("#Receive #title", navTab.getCurrentPanel()).hide();
        }
        else {
            $("#Receive #title", navTab.getCurrentPanel()).show();
        }
     
            JUploadGetAttas("<%=Model.AttachmentID!=null?Model.AttachmentID:"" %>","<%=TdID %>",2);
    });
    </script>
<div class="page">
    <div class="panelBar" style="border-top: 0px;">
        <ul class="toolBar">
            <li><a class="send" href="javascript:;" onclick="sendInstantMessaging();"><span>发送</span></a></li>
            <li><a class="save" href="javascript:;" onclick="savedraftInstantMessaging();"><span>
                存稿</span></a></li>
            <li><a class="cancel" href="javascript:;" onclick="navTab.closeCurrentTab()"><span>取消</span></a></li>
        </ul>
    </div>
    <div class="pageContent" style="padding: 5px">
        <div class="tabs">
            <div class="tabsHeader">
                <div class="tabsHeaderContent">
                    <ul>
                        <li><a href="javascript:;"><span>信息编辑</span></a></li>
                    </ul>
                </div>
            </div>
            <div class="tabsContent">
                <div>
                    <div layouth="80" style="margin-left: -1px; float: right; display: block; overflow: auto;
                        width: 240px; border: solid 1px #CCC; line-height: 21px; background: #fff">
                        <ul class="tree">
                            <% var depts = ViewData["Depts"] as List<JiYun.Modules.Core.Models.S_Dept>; %>
                            <% Html.RenderPartial("~/Views/Shared/DeptUserTree.ascx", depts.Where(t => t.Parent_ID == null).ToList()); %>
                        </ul>
                    </div>
                    <div id="messageWriteLeft" layouth="80" style="margin-right: 246px; border: solid 1px #CCC;
                        background: #fff; height: 100%">
                        <form method="post" id="form" action="<%= Url.Action("DoLook", "MSGDraft") %>"
                        class="pageForm required-validate" onsubmit="return validateCallback(this, navTabAjaxDone);">
                        <input type="hidden" name="MsgID" value="<%=ViewData["MsgID"]%>" />
                        <%= Html.Hidden("Guid") %>
                        <div class="pageFormContent">
                            <div class="unit">
                                <label style="width: 80px; display: inherit;">
                                    收件人：</label>
                                <div class="divtxt" id="Receive" style="margin-right: 10px; float: left; display: inherit;
                                    width: 80%;">
                                    <span class='ledlist'   id="title">请在右边列表选取收件人！</span>
                                     <%=MvcHtmlString.Create(ViewBag.UserShow)%>
                                </div>
                            </div>
                            <div class="divider">
                                divider</div>
                            <div class="unit">
                                <label style="width: 80px;">
                                    主 题：</label>
                                <input name="MessageTitle" id="MessageTitle" type="text" class="required" style="width: 80%" value="<%= Model.MessageTitle%>"/>
                            </div>
                            <div class="unit">
                                
                                <label style="width: 80px;">
                                    附 件：</label>
                                 <label id="<%=TdID %>" style=" width:70%;">
                                        <input type="hidden" name="AttachmentID" id="AttachmentID" value="<%:attid %>" />
                                         <a href="javascript:" onclick="JUpload('<%=attid %>','<%=TdID %>');"><img title="点击上传附件" style=" cursor:pointer;" src="/Content/Images/attachment_add.png" /></a>
                                   </label>
                            </div>
                            <div class="unit">
                                <label style="width: 80px;">
                                    发送方式：</label>
                                <label style="width: auto">
                                    <input type="checkbox" name="messageForm" value="1" checked />站内信</label>
                                <label style="width: auto">
                                    <input type="checkbox" name="mailForm" value="1" />邮件</label>
                            </div>
                            <div class="unit">
                                <label style="width: 80px;">
                                    内 容：</label>
                                <textarea name="Content" id="Content" tools="simple" class="editor" style="width: 80%"
                                    rows="15"><%= Model.MessageContent%></textarea>
                            </div>
                        </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="tabsFooter">
                <div class="tabsFooterContent">
                </div>
            </div>
        </div>
    </div>
</div>
