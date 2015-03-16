<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<JiYun.Modules.Core.Models.S_Dept>>" %>
<% 
    foreach (var dept in Model.OrderBy(u=>u.Order))
    {
        var list = dept.S_DeptS_User; //用户
        var val = list.Count();
        var count = dept.Children;
        if (list.Any() == false && count.Any())
        {
            val = count.Count;
        }
%>
    <li><a class="11" href="javascript:;"><%= dept.Name%> (<b style="text-decoration:underline;color:Blue;font-weight:700" onclick="GetAll('userlist<%= dept.ID %>');" title="点击选择该组全部人员"><%= val%></b>)</a>
    <%  
        if (list.Any())
        { 
            %>
        <ul id="userlist<%= dept.ID %>">
        <%  foreach (var user in list)
            {
                if (user != null)
                {
                    
                %>
            <li><a class="User" href="javascript:;" style="color:#777" onclick="Get(this);" dvalue="<%=dept.ID %>" tvalue="<%= user.S_User_ID %>" title="点击选择人员"><%= user.S_User.Name%></a></li>
        <%      }
            } %>
        </ul>

    <%  } %>
        <% if (count.Any())
           { %>
            <ul class="expand"  id="userlist<%=dept.ID %>">
                <% Html.RenderPartial("~/Views/Shared/DeptUserTree.ascx", count.OrderBy(g => g.ID).ToList()); %>
            </ul>
        <% } %>      
    </li>
<%} %>