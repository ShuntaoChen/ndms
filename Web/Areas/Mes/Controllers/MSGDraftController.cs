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
using JiYun.Common.Utils;
using JiYun.Modules.Core.Models;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 草稿箱
    /// </summary>
    public class MSGDraftController : MyBaseController
    {
        private MSGManagerService MsgService = new MSGManagerService();
        private UserService us = new UserService();
        private UserMessageService um = new UserMessageService();
        private MsgAttService mas = new MsgAttService();
        private DeptService deptService = new DeptService();
        //附件
        AttachmentService _AttachmentService = new AttachmentService();
        public ActionResult MyMsg_Draft(FormCollection form)
        {
            //Dictionary<string, string> formcollection = form.AllKeys.ToDictionary(k => k, v => form[v]);
            ViewData.Model = MsgService.SearchMyMsg(GetUserInfo().UserID.ToString(), "0", "0", "", "2", form);
            //ViewData["ToolBar"] = GetToolBar();
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
            string UserShow = "";
            string[] ids = id.Split(',');
            int UM_id = int.Parse(ids[1]);
            MsgService.Update_ReadStatus(UM_id, "1", ids[1]);
            X_Messages message = MsgService.GetByID(Convert.ToInt32(ids[0]));
            ViewData.Model = message;
            ViewData["UserID"] = GetUserInfo().UserID;
            ViewData["UploadName"] = "MSGDraftLook";
            ViewData["navTab"] = "navTab";
            ViewData["AttachType"] = "IM模块附件列表查看或详细页面";
            ViewData["AttachID"] = message.AttachmentID;
            ViewData["MsgID"] = message.ID;
            //绑定人员
            var list = MsgService.SearchUser(message.ID);
            if (list.Count() > 0)
            {
                foreach (var m in list)
                {
                    int uid = Utils.StrToInt(m.Bas_UserID, 0);
                    var user = us.GetById(uid);
                    UserShow = UserShow + "<span class=\"ledlist\" title=\"" + user.Name + "\"><a class=\"close\" href=\"javascript:;\" onclick=\"RemoveSpan(this);\"></a>" + user.Name + "<input type=\"hidden\" name=\"ledids\" value=\"" + user.ID + "\" /></span>";
                }
                ViewBag.MsgHidden = "1";
            }
            ViewBag.UserShow = UserShow;
            //上传附件新加代码 开始
            if (!string.IsNullOrEmpty(message.AttachmentID))
            {
                string str = _AttachmentService.GetFileList(message.AttachmentID);
                ViewData["ATTACHMENTID"] = str;
            }
            else
            {
                ViewData["ATTACHMENTID"] = "";
            }
            // 联系人列表
            ViewData["Depts"] = deptService.GetAllDepts().OrderBy(p => p.Parent_ID).ToList();
            ViewData["Users"] = us.GetAllUsers().ToList();
            return View();
        }

        /// <summary>
        /// 删除(修改信息阅读状态)
        /// </summary>
        /// <param name="id">所选信息ID字符串(如1,2,3)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            bool result = MsgService.Update_IsDelete("1", id);
            if (result)
            {
                return SuccessMsg("MSGDraftMyMsg_Draft");
            }
            else
            {
                return ErrorMsg("操作失败！", "MSGDraftMyMsg_Draft","","");
            }
        }

        /// <summary>
        /// 保存修改的消息
        /// </summary>
        /// <param name="form">页面参数</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoLook(FormCollection form, string id, string[] ledids, string AttachmentID)
        {
            int uid = GetUserInfo().UserID;//(Session["JY_SESSION_ID"] as UserSession).UserID;
            S_User u = us.GetById(uid);//当前用户所有信息
            X_Messages msg = new X_Messages(); //消息对象
            msg.MessageTitle = form["MessageTitle"];

            msg.MessageContent = form["Content"];
            msg.MessageReceive = form["MessageReceive"];
            int MsgID = Utils.StrToInt(form["MsgID"], 0);
            if (id == "1")
            {
                //发送新消息
                MsgService.SendMessage(u, ledids, AttachmentID, msg);
                //删除原有信息
                MsgService.Delete(MsgID);
                return SuccessMsg("MesMSGSendMyMsg_Send");
            }
            else
            {
                msg.MessageSend = u.Name;
                msg.AttachmentID = AttachmentID;
                msg.CreateDate = DateTime.Now;
                msg.CreaterUserID = u.ID;
                msg.CreateUserReal = u.Name;
                msg.ID = MsgID;
                MsgService.UpdateMsgToDraft(msg, GetUserInfo().UserID.ToString(), GetUserInfo().Name);
                return SuccessMsg("MesMSGDraftMyMsg_Draft");
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
            ViewData["UploadName"] = "MSGDraftWrite";
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

            string guid = form["guid"];     //附件guid
            List<S_Attachment> list = null;
            if (guid != "" && guid != null)
            {
                list = MsgService.SearchMsgAtt(guid);    //获得附件列表
            }
            int uid = GetUserInfo().UserID;
            S_User u = us.GetById(uid);
            X_Messages msg = new X_Messages();
            msg.MessageTitle = form["MessageTitle"];
            msg.MessageContent = form["Content"];
            msg.Remark = form["Remark"];
            msg.MessageSend = u.Name;
            msg.AttachmentID = guid;
            msg.CreateDate = DateTime.Now;
            msg.CreaterUserID = u.ID;
            msg.CreateUserReal = u.Name;
            MsgService.Create(msg);
            X_UserMessages ume = new X_UserMessages();
            ume.IsDelete = 0;
            ume.MessageStatus = 0;
            ume.X_Messages_ID = msg.ID;
            ume.MyMsg = 2;
            ume.ReadStatus = 1;
            ume.Bas_UserID = uid;
            um.Create(ume);
            //添加附件关联
            if (guid != "" && guid != null)
            {
                if (list.Count > 0)
                {
                    foreach (var model in list)
                    {
                        X_MsgAtts mt = new X_MsgAtts();
                        mt.X_Messages_ID = msg.ID;
                        mt.AttID = model.ID;
                        mas.Create(mt);
                    }
                }
            }
            return SuccessMsg("MSGDraftMyMsg_Draft");
        }
    }
}
