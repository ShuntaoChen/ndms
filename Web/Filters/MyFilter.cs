/*********************************************************************
 * 作者     : ChenXD
 * 创建日期 : 2012-12-11
 * 功能描述 : 判断用户是否已经登录，如果没有登录或者登录已经过期就跳转 
 *            到登录页面重新登录。           
 * 
 * 修改历史
 * --------------------------------------------------------------------
 * 日期       | 作者            | 描述
 * --------------------------------------------------------------------
 *            |                 | 
 *            |                 | 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using JiYun.Common.Sessions;
using JiYun.Web.Models;

namespace Web.Filters
{
    public class MyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            //ActionDescriptor action = ctx.ActionDescriptor;
            //ControllerDescriptor controller = action.ControllerDescriptor;

            //var session = ctx.HttpContext.Session[SessionUtil.LoginUserExpandKey] as UserExpand;

            ////检查界别是否切换
            //if (session != null)
            //{
            //    if (session.CurrentNPCID != "")
            //    {
            //        string loginUrl = new UrlHelper(ctx.RequestContext).Action("Login", "Home", new { area = "" });

            //        ctx.Result = new ContentResult
            //        {
            //            Content = "<script language='javascript'>"
            //                          + "alertMsg.error('系统设定已更改！请重新登录！', {okCall:function(){"
            //                          + "window.location.href = '" + loginUrl + "';}});</script>"
            //        };
            //        ctx.HttpContext.Response.Clear();
            //        return;
            //    }
              
            //}

            //检测是否有权限执行当前controller当前action
            //if (session.UserType == "super") //超级用户不需要判断
            //{
            //    return;
            //}

            ////如果是以"_"开头的Action，就跳过验证。
            //if (action.ActionName.StartsWith("_"))
            //{

            //}

        }
    }
}