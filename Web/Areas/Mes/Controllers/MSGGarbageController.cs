using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Areas.Mes.Services;
using Web.Controllers;
using JY.Module.Message.Services;
using JiYun.Modules.Core.Services;
using Web.Areas.Mes.Models;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 垃圾箱
    /// </summary>
    public class MSGGarbageController : MyBaseController
    {
        private MSGManagerService MsgService = new MSGManagerService();
        private UserService us = new UserService();
        private UserMessageService um = new UserMessageService();

        public ActionResult MyMsg_Garbage(FormCollection form)
        {
            string userid = GetUserInfo().UserID.ToString();
         
            ViewData.Model = MsgService.SearchMyMsg(userid, "", "1", "", "", form);
        
            ViewData["startDate"] = form["startDate"];
            ViewData["endDate"] = form["endDate"];
            ViewData["orderDir"] = (form["orderDir"] == "asc") ? "desc" : "asc";
            return View();
        }

        /// <summary>
        /// 查看信息
        /// </summary>
        /// <param name="id">所查看信息ID</param>
        /// <returns>查看信息页面</returns>
        public ActionResult Look(string id)
        {
            string[] ids = id.Split(',');
            int UM_id = int.Parse(ids[1]);
            MsgService.Update_ReadStatus(UM_id, "1", ids[1]);
            X_Messages message = MsgService.GetByID(Convert.ToInt32(ids[0]));

            ViewData.Model = message;

            ViewData["UploadName"] = "MSGGarbageLook";
            ViewData["AttachType"] = "IM模块附件列表查看或详细页面";
            ViewData["AttachID"] = message.AttachmentID;
            return View();
        }

        /// <summary>
        /// 删除(修改信息阅读状态，)
        /// </summary>
        /// <param name="id">所选信息ID字符串(如1,2,3)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            //MsgService.DeleteByID("IM_UserMessage", "ID", id);
            bool result = MsgService.Delete_Garbage(id);
            if (result)
            {
                return SuccessMsg("MSGGarbageMyMsg_Garbage");
            }
            else
            {
                return SuccessMsg("MSGGarbageMyMsg_Garbage");
            }
        }
        /// <summary>
        /// 转至
        /// </summary>
        /// <returns></returns>
        public ActionResult Turn(string id)
        {
            ViewData["list"] = id;
            return View();
        }

        /// <summary>
        /// 转至操作
        /// </summary>
        /// <returns></returns>
        public ActionResult DoTurn(FormCollection form)
        {
            string type = form["hid_type"];
            string[] list = form["list"].Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                if (type == "3")
                {
                    try
                    {
                        MsgService.Update_IsDelete("1", list[i]);
                    }
                    catch
                    {
                        return ErrorMsg("操作失败！", "MSGGarbageMyMsg_Garbage","","");
                    }
                }
                else
                {
                    try
                    {
                        int UM_id = int.Parse(list[i]);
                        MsgService.Update_MyMsg(UM_id, type, list[i]);
                    }
                    catch
                    {
                        return ErrorMsg("操作失败！", "MSGGarbageMyMsg_Garbage","","");
                    }
                }
            }
            return SuccessMsg("MSGGarbageMyMsg_Garbage");
        }
    }
}
