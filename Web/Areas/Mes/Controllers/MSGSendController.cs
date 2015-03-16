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
using JiYun.Modules.Core.Models;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 发件箱
    /// </summary>
    public class MSGSendController : MyBaseController
    {
        private MSGManagerService MsgService = new MSGManagerService();
        private UserService us = new UserService();
        private UserMessageService um = new UserMessageService();
        private MsgAttService mas = new MsgAttService();
        //private UserGroupService ugs = new UserGroupService();
        /// <summary>
        /// 消息中心(发件箱)
        /// </summary>
        /// <param name="form">页面参数</param>
        /// <returns></returns>
        public ActionResult MyMsg_Send(FormCollection form)
        {
            string userid = GetUserInfo().UserID.ToString();
            //Dictionary<string, string> formcollection = form.AllKeys.ToDictionary(k => k, v => form[v]);
            ViewData.Model = MsgService.SearchMyMsg(userid, "0", "0", "", "0", form);
            //ViewData["ToolBar"] = GetToolBar();
            ViewData["startDate"] = form["startDate"];
            ViewData["endDate"] = form["endDate"];
            ViewData["KeyWord"] = form["KeyWord"];
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
            ViewData["AttachType"] = "IM模块附件列表查看或详细页面";
            ViewData["AttachID"] = message.AttachmentID;

            return View();
        }

        /// <summary>
        /// 删除收件箱(修改信息阅读状态，)
        /// </summary>
        /// <param name="id">所选信息ID字符串(如1,2,3)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            bool result = MsgService.Update_IsDelete("1", id);
            if (result)
            {
                return SuccessMsg("MSGSendMyMsg_Send");
            }
            else
            {
                return SuccessMsg("MSGSendMyMsg_Send");
            }
        }

        /// <summary>
        /// 打开写信画面
        /// </summary>
        /// <returns></returns>
        public ActionResult Write()
        {
            //ViewData["UserGroups"] = ugs.FindUserGroupRoot();
            ViewData["UserID"] = GetUserInfo().UserID;
            ViewData["Action"] = "Write";
            ViewData["Controller"] = "MSGSend";
            ViewData["UserList"] = us.GetAllUsers();
            ViewData["UploadName"] = "MSGSendWrite";
            return View();
        }

        /// <summary>
        /// 提交写信内容
        /// </summary>
        /// <param name="from">页面参数</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult DoWrite(FormCollection form)
        {
            //当前用户
            int uid = GetUserInfo().UserID;
            List<S_Attachment> list = null;
            string guid = form["guid"];
            if (guid != "" && guid != null)
            {
                list = MsgService.SearchMsgAtt(guid);
            }
            //当前用户所有信息
            S_User u = us.GetById(uid);
            //收件人
            List<string> ids = form["hid_Users"].Split(';').ToList();
            ids.Remove(ids.Last());
            X_Messages msg = new X_Messages();
            msg.MessageTitle = form["MessageTitle"];
            msg.MessageContent = form["Content"];
            msg.MessageReceive = form["MessageReceive"];
            msg.MessageSend = u.Name;
            msg.AttachmentID = guid;
            msg.CreateDate = DateTime.Now;
            msg.CreaterUserID = u.ID;
            msg.CreateUserReal = u.Name;
            msg.AttachmentID = guid;
            MsgService.Create(msg);    //添加信息表
            //循环往X_UserMessages表中插入关联信息
            for (int i = 0; i < ids.Count(); i++)
            {
                int id = Convert.ToInt32(ids[i]);
                X_UserMessages ume = new X_UserMessages();
                ume.IsDelete = 0;
                ume.MessageStatus = 0;
                ume.X_Messages_ID = msg.ID;
                ume.MyMsg = 1;
                ume.ReadStatus = 0;
                ume.Bas_UserID = id;
                um.Create(ume);
            }
            X_UserMessages IUM = new X_UserMessages();
            IUM.IsDelete = 0;
            IUM.MessageStatus = 0;
            IUM.X_Messages_ID = msg.ID;
            IUM.MyMsg = 0;
            IUM.ReadStatus = 0;
            IUM.Bas_UserID = uid;
            um.Create(IUM);
            if (guid != "" && guid != null)
            {
                if (list.Count > 0)
                {
                    foreach (var model in list)
                    {
                        X_MsgAtts mt = new X_MsgAtts();
                        mt.AttID = model.ID;
                        mt.X_Messages_ID = msg.ID;
                        mas.Create(mt);
                    }
                }
            }
            return SuccessMsg("MSGManageMyMsg_Recive");
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
                        return ErrorMsg("操作失败！", "MSGSendMyMsg_Send","","");
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
                        return ErrorMsg("操作失败", "MSGSendMyMsg_Send","","");
                    }
                }
            }
            return SuccessMsg("MSGSendMyMsg_Send");
        }
    }
}
