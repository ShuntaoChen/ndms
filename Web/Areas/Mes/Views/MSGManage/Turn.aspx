<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<script type="text/javascript" language="javascript">
    function check(type) {
        //alert(type.value);
        document.getElementById("hid_type").value = type.value;
    }
</script>
<div class="page">
    <div class="pageContent">
        <form method="post" action="<%= Url.Action("DoTurn", "MSGManage") %>" class="pageForm required-validate"
        onsubmit="return validateCallback(this, dialogAjaxDone);">
        <input name="hid_type"  id="hid_type" type="hidden" value="" />
        <input name="list" id="list" type="hidden" value="<%=ViewData["list"] %>" />
        <div style="margin: 10px; overflow: auto; line-height: 21px; background: #FFF;" layouth="93">
            <label>
                <input type="radio" name="MSGType" value="1" onclick="check(this)" />
                收件箱</label>
            <label>
                <input type="radio" name="MSGType" value="0" onclick="check(this)" />
                发件箱</label>
            <label>
                <input type="radio" name="MSGType" value="2" onclick="check(this)" />
                草稿</label>
            <label>
                <input type="radio" name="MSGType" value="3" onclick="check(this)" />
                垃圾箱</label>
        </div>
        <div class="formBar">
            <ul>
                <li>
                    <div class="buttonActive">
                        <div class="buttonContent">
                            <button type="submit">
                                提交</button></div>
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
