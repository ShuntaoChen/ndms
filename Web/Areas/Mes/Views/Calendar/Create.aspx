<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<script language="javascript">

//    $(document).ready(function () {
//        iColorPicker(2);
//    });
    function CreateCalendarDialogDone(json) {
        DWZ.ajaxDone(json);
        if (json.statusCode == DWZ.statusCode.ok) {
            $.pdialog.closeCurrent();
            if (typeof ShowCalendarInfo == "function") {
                ShowCalendarInfo();
            }
            navTab.reloadFlag(json.navTabId);
        }
    }

    function changeCol(color) {
        document.getElementById("col").style.backgroundColor = color;
        document.getElementById("tmpValue").value = color;
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
        <form method="post" id="form" action="<%= Url.Action("DoCreate", "Calendar") %>"
        class="pageForm required-validate" onsubmit="return validateCallback(this, CreateCalendarDialogDone);">
        <div class="pageFormContent" layouth="60">
            <table class="formTable">
                <tr style=" display:none;">
                    <th>
                        <label class="right">
                            类型：</label>
                    </th>
                    <td colspan="3">
                        <input type="radio" name="ScType" value="1" checked="checked" />日程
                        <input type="radio" name="ScType" value="2" />任务
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            标题：</label>
                    </th>
                    <td colspan="3">
                        <input type="hidden" id="CalendarCreateTitle_COLOR" value="#3366CC" name="Title_COLOR" />
                        <input type="text" size="70" id="CalendarCreateTitle" name="Title" class="required iColorPicker" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            时间：</label>
                    </th>
                    <td colspan="3">
                        <input type="text" id="StartTime" name="StartTime" class="Wdate required" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',maxDate:'#F{$dp.$D(\'EndTime\');}'})"
                            value="<%=ViewData["startTime"] %>" /><label style="width: 10px">至：</label>
                        <input type="text" id="EndTime" name="EndTime" class="Wdate required" value="<%=ViewData["endtime"] %>"
                            onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',minDate:'#F{$dp.$D(\'StartTime\');}'})" />
                    </td>
                </tr>
                <tr style=" display:none;">
                    <th>
                        <label class="right">
                            地点：</label>
                    </th>
                    <td colspan="3">
                        <input type="text" name="Address" size="60" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            说明：</label>
                    </th>
                    <td colspan="3">
                        <textarea rows="12" style="width:97%" name="Contents" tools="simple" class="editor"></textarea><font style=" color:Red;">*</font>
                    </td>
                </tr>
                <tr style="display: none;">
                    <th>
                        <label class="right">
                            是否公开：
                        </label>
                    </th>
                    <td colspan="3">
                        <input type="radio" name="IsPublic" value="1" />公开
                        <input type="radio" name="IsPublic" value="0" checked="checked" />私人
                    </td>
                </tr>
                <tr>
                    <th>
                        <label class="right">
                            系统提醒：
                        </label>
                    </th>
                    <td colspan="3">
                        <input type="checkbox" name="IsRemind" id="IsRemind" value="1" onclick="ClickCheck(this)" checked="checked" />
                    </td>
                </tr>
                <tr id="RemindAttribute">
                    <th>
                        <label class="right">
                            提醒方式：
                        </label>
                    </th>
                    <td colspan="3">
                        <table style="border: 0px;">
                            <tr>
                                <td style="border: 0px; display:none;">
                                    <input type="checkbox" name="RemindType" value="1" checked="checked" />弹出式窗口
                                    <%--  <input type="checkbox" name="RemindType" value="1" />电子邮件--%>
                                   <%-- <input type="checkbox" name="RemindType" value="2" />手机短信--%>
                                </td>
                                <td style="border: 0px;">
                                    <input type="text" class="number required" name="RemindDays" id="RemindDays" size="5" />
                                </td>
                                <td style="border: 0px;">
                                    <select name="RemindRecyle" id="RemindRecyle" class="Combo required">
                                        <option value="">--请选择--</option>
                                        <option value="1" selected="selected">分钟</option>
                                        <option value="2">小时</option>
                                        <option value="3">天</option>
                                      <%--  <option value="4">周</option>--%>
                                    </select>
                                </td>
                                <td style="border: 0px;">
                                    <input type="radio" name="RemainBl" value="1" title="根据设定的频率重复提醒" checked />重复提醒
                                    <input type="radio" name="RemainBl" value="2" title="在结束时间到期之前提醒一次" />一次提醒
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
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="submit">
                                确定</button></div>
                    </div>
                </li>
                <li>
                    <div class="button">
                        <div class="buttonContent">
                            <button class="close">
                                取消</button></div>
                    </div>
                </li>
            </ul>
        </div>
        </form>
    </div>
</div>
