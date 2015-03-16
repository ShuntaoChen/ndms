<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Web.Areas.Mes.Models.X_Schedules>" %>

<script language="javascript">
//    $(document).ready(function () {
//        iColorPicker(2);
//    });
    function EditCalendarDialogDone(json) {
        DWZ.ajaxDone(json);
        if (json.statusCode == DWZ.statusCode.ok) {
            $.pdialog.closeCurrent();
            if (typeof ShowCalendarInfo == "function") {
                ShowCalendarInfo();
            }
            navTab.reloadFlag(json.navTabId);
        }
    }

    function schedulerEditDone(json) {
        DWZ.ajaxDone(json);
        if (json.statusCode == DWZ.statusCode.ok) {
            $('#calendarr').fullCalendar('refetchEvents');
            $.pdialog.closeCurrent();
        }

    }
    function deleteScheduler(_id) {
        alertMsg.confirm("确定要删除这条记录吗？", {
            okCall: function () {
                $.post('<%= Url.Action("SchDelete", "Calendar") %>', { id: _id }, function (json) {
                    if (json.statusCode == 200) {
                        $('#calendarr').fullCalendar('refetchEvents');
                        $.pdialog.closeCurrent();
                    }
                }, "json");
            }
        });
    }
    function ClickCheck(obj) {
        $("#RemindAttribute").toggle();
        if ($(obj).attr("checked") == "checked" || $(obj).attr("checked") == true) {
            $("#RemindDays,#RemindRecyle").addClass("required");
        }
        else {
            $("#RemindDays,#RemindRecyle").removeClass("required");
        }
    }
</script>
<div class="page">
    <div class="pageContent">
        <form method="post" id="form" action="<%= Url.Action("DoEdit", "Calendar") %>" class="pageForm required-validate"
        onsubmit="return validateCallback(this, EditCalendarDialogDone);">
        <div class="pageFormContent" layouth="60">
            <table class="formTable">
                <tr>
                    <th>
                        <label class="right">
                            标题：</label>
                    </th>
                    <td colspan="3">
                        <input type="hidden" id="CalendarCreateTitle_COLOR" value="<%: Model.Color %>" name="Title_COLOR" />
                        <input type="text" size="70" id="CalendarCreateTitle" value="<%: Model.Title %>"
                            style="color: <%: Model.Color %>" name="Title" class="required iColorPicker" />
                        <input name="ID" value="<%: Model.ID %>" type="hidden" />
                        <input name="UserID" value="<%: Model.UserID %>" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            时间：</label>
                    </th>
                    <td colspan="3">
                        <input type="text" id="StartTime" name="StartTime" value="<%: Model.StartTime %>"
                            class="Wdate required" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',maxDate:'#F{$dp.$D(\'EndTime\');}'})"
                            value="<%=ViewData["startTime"] %>" /><label style="width: 10px">至：</label>
                        <input type="text" id="EndTime" name="EndTime" value="<%: Model.EndTime %>" class="Wdate required"
                            onkeyup="enddate();" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',minDate:'#F{$dp.$D(\'StartTime\');}'})" />
                    </td>
                </tr>
                <tr style=" display:none;">
                    <th>
                        <label class="right">
                            地点：</label>
                    </th>
                    <td colspan="3">
                        <input type="text" name="Address" size="60" value="<%: Model.Address %>" />
                    </td>
                </tr>
                <tr>  
                    <th>
                        <label class="right">
                            说明：</label>
                    </th>
                    <td colspan="3">
                        <textarea rows="12" style="width:97%" name="Contents" tools="simple" class="editor"><%: Model.Contents %></textarea><font style=" color:Red;">*</font>
                    </td>
                </tr>
                 <tr style=" display:none;">
                    <th>
                        <label class="right">
                            是否公开：
                        </label>
                    </th>
                    <td colspan="3">
                        <%if (Model.IsPublic == 1)
                          {
                        %>
                        <input type="radio" name="IsPublic" value="1" checked="checked" />公开
                        <input type="radio" name="IsPublic" value="0" />私人
                        <%
                            }
                          else
                          {
                        %>
                        <input type="radio" name="IsPublic" value="1" />公开
                        <input type="radio" name="IsPublic" value="0" checked="checked" />私人
                        <%
                            }      
                        %>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            系统提醒：
                        </label>
                    </th>
                    <td colspan="3">
                        <input type="checkbox" name="IsRemind" id="IsRemind" value="1" onclick="ClickCheck(this)" <%=Model.IsRemind==1?"checked":"" %> />
                    </td>
                </tr>
                <tr id="RemindAttribute" <%=Model.IsRemind==1?"":"style='display:none;'" %>>
                    <th>
                        <label class="right">
                            提醒方式：
                        </label>
                    </th>
                    <td colspan="3">
                        <table style="border: 0px;">
                            <tr>
                                <td style="border: 0px; display:none;">
                                    <% if (Model.RemindType.Contains("1"))
                                       { %>
                                       弹出窗口
                                    <%} if (Model.RemindType.Contains("2"))
                                       { %>
                                       手机短信
                                    <%} %>
                                </td>
                                <td style="border: 0px;">
                                    <input type="text" class="number <%=Model.IsRemind==1?"required":"" %>" name="RemindDays" id="RemindDays" value="<%=Model.RemindDays %>" size="5" />
                                </td>
                                <td style="border: 0px;">
                                <% string option1 = "", option2 = "", option3 = "", option4 = "";
                                   if (Model.RemindRecyle == 1)
                                       option1 = "selected";
                                   else if (Model.RemindRecyle == 2)
                                       option2 = "selected";
                                   else if (Model.RemindRecyle == 3)
                                       option3 = "selected";
                                   else if (Model.RemindRecyle == 4)
                                       option4 = "selected"; %>
                                    <select name="RemindRecyle" id="RemindRecyle" class="Combo <%=Model.IsRemind==1?"required":"" %>">
                                        <option value="">--请选择--</option>
                                        <option value="1" <%=option1 %>>分钟</option>
                                        <option value="2" <%=option2 %>>小时</option>
                                        <option value="3" <%=option3 %>>天</option>
                                       <%-- <option value="4" <%=option4 %>>周</option>--%>
                                    </select>
                                </td>
                                 <td style="border: 0px;">
                                    <% string radio1 = "", radio2 = "";
                                       if (Model.RemainBl == 1) { radio1 = "checked"; } else { radio2 = "checked"; } %>
                                    <input type="radio" name="RemainBl" value="1" title="根据设定的频率重复提醒" <%=radio1 %>  />重复提醒
                                    <input type="radio" name="RemainBl" value="2" title="在结束时间到期之前提醒一次" <%=radio2 %> />一次提醒
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="formBar">
            <ul>
                <li>
                    <div class="button">
                        <div class="buttonContent">
                            <button type="button" onclick="deleteScheduler(<%: Model.ID %>)">
                                删除</button></div>
                    </div>
                </li>
                <% if ((int)ViewBag.IsEdit == 1)
                   { %>
                <li>
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="submit">
                                确定</button></div>
                    </div>
                </li>
                <%} %>
                <li>
                    <div class="button">
                        <div class="buttonContent">
                            <button class="close">
                                取消</button></div>
                    </div>
                </li>
            </ul>
        </div>
        <input type="hidden" name="StartTime" value="<%:Model.StartTime %>" />
        <input type="hidden" name="EndTime" value="<%:Model.EndTime %>" />
        <input type="hidden" name="IsRemind" value="<%:Model.IsRemind %>" />
        <input type="hidden" name="RemindType" value="<%:Model.RemindType %>" />
        <input type="hidden" name="ScType" value="<%:Model.ScType %>" />
        <input type="hidden" name="CreateDate" value="<%:Model.CreateDate %>" />
        <input type="hidden" name="RemaindCode" value="<%:Model.RemaindCode %>" />
        <input type="hidden" name="UserID" value="<%:Model.UserID %>" />
        <input type="hidden" name="LastRemaindTime" value="<%:Model.LastRemaindTime %>" />
        </form>
    </div>
</div>
