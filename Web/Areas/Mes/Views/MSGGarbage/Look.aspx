<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" validateRequest="false" %>

<style type="text/css">
    .pageFormContent p {
    display: block;
    float: left;
    height: auto;
    line-height:25px;
    margin: 0;
    padding: 5px 0;
    position: relative;
    width: 100%;
}
</style>
<%JiYun.Modules.Core.Services.AttachmentService atts = new JiYun.Modules.Core.Services.AttachmentService(); %>
    <div class="page">
        <div class="pageContent">
            <div class="pageFormContent mesView" layouth="56" style="padding:10px;">
                <div style="border-bottom:2px solid #56B1DD;background:#e4f1fb">
                    <div style="font-size:20px;"><%:Model.MessageTitle%></div>
                    <div>
                        <span style="width:auto;">发件人：</span>
                        <span style="color:green"><%:Model.MessageSend%></span>
                    </div>

                    <div>
                        <span style="width:auto;">收件人：</span>
                        <span><%: Model.MessageReceive%></span>
                    </div>

                    <div>
                        <span style="width:auto;">发送时间：</span>
                        <span><%: Model.CreateDate.ToString("yyyy年MM月dd日 HH:mm")%></span>
                    </div>  
                         
                        <div>
                        <span style="width:auto;height:100%;vertical-align:top">附件：</span>
                        <span style="display:inline-block">
                        <%foreach (var item in atts.GetAllAttachment().Where(a=>a.AttachmentID==Model.AttachmentID))
                          {%>
                            <a style="margin-bottom:6px;display:block;color:Gray;" href="<%:Url.Action("DoBasewnload","UploadCommon",new{id=item.ID})%>" title="点击下载附件">
                                <img src="/Content/images/filetype/<%:item.Path.Substring(item.Path.LastIndexOf(".")+1)%>.gif" /><%:item.FileName %></a>   
                         <% } %>
                        </span>
                    </div>
                    
                </div>
                    <div style="font-size:17px;">
                        <%= Model.MessageContent%>
                    </div>
            </div>
            <div class="formBar">
                <ul>
                    <li>
                        <div class="button">
                            <div class="buttonContent">
                                <button type="button" class="close">
                                    关闭</button></div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>

