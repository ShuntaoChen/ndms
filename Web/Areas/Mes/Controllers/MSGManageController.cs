using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.EnterpriseServices;
using System.Data;
using System.ComponentModel;
using System.Xml;
using System.Text;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Areas.Mes.Services;
using Web.Controllers;
using JiYun.Modules.Core.Models;
using Web.Areas.Mes.Models;
using JiYun.Common.Utils;
using JY.Module.Message.Services;
using JiYun.Modules.Core.Services;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 收件箱
    /// </summary>
    public class MSGManageController : MyBaseController
    {
        #region 引用类
        private MSGManagerService MsgService = new MSGManagerService();    //收件箱
        private UserService us = new UserService();                        //用户
        private UserMessageService um = new UserMessageService();
        private DeptService deptService = new DeptService();
        #endregion

        private readonly IUserMesCountService _iUserMesCountService;
        public MSGManageController(IUserMesCountService userMesCountService)
        {
            _iUserMesCountService = userMesCountService;
        }

        /// <summary>
        /// 用户消息中心(收件箱)
        /// </summary>
        public ActionResult MyMsg_Recive(FormCollection form)
        {
            //Dictionary<string, string> formcollection = form.AllKeys.ToDictionary(k => k, v => form[v]);
            int userid = GetUserInfo().UserID;
            ViewData.Model = MsgService.SearchMyMsg(userid.ToString(), "0", "0", "", "1", form);
            //ViewData["ToolBar"] = GetToolBar();
            ViewData["startDate"] = form["startDate"];
            ViewData["endDate"] = form["endDate"];
            ViewData["KeyWord"] = form["KeyWord"];
            ViewData["ReadStatus"] = form["ReadStatus"];
            ViewData["orderDir"] = (form["orderDir"] == "asc") ? "desc" : "asc";
            MsgService.UpdateUserMesCount(userid);

            return View();
        }
        /// <summary>
        /// 查看信息
        /// </summary>
        /// <param name="id">所查看信息ID</param>
        public ActionResult Look(string id, string fromIndex)
        {
            ViewData["MsgID"] = id.Split(',')[0];
            ViewData["ID"] = id.Split(',')[1];
            ViewData["UploadName"] = "MSGManageLook";
            ViewBag.fromIndex = fromIndex == null ? "" : fromIndex;
            string[] ids = id.Split(',');
            if (id.Contains(","))
            {
                int UM_id = int.Parse(ids[1]);
                MsgService.Update_ReadStatus(UM_id, "1", ids[1]);
            }
            else
            {
                var mes = um.GetByID(int.Parse(id));
                mes.ReadStatus = 1;
                um.Update(mes);
                ids[0] = mes.X_Messages_ID.ToString();
            }

            X_Messages message = MsgService.GetByID(Convert.ToInt32(ids[0]));
            ViewData["AttachType"] = "IM模块附件列表查看或详细页面";
            ViewData["AttachID"] = message.AttachmentID;
            ViewData.Model = message;

            return View();
        }
        /// <summary>
        /// 检查是不是系统消息
        /// </summary>
        public JsonResult checkReply(string id)
        {
            X_Messages m = MsgService.GetByID(int.Parse(id));
            S_User user = us.GetAllUsers().Where(p => p.ID == m.CreaterUserID).FirstOrDefault();
            if (user == null || user.Name == null)
            {
                return ErrorMsg("系统消息无法回复！", "MSGManageMyMsg_Recive","","");
            }
            else
            {
                return SuccessMsg("MSGManageMyMsg_Recive");
            }
        }
        /// <summary>
        /// 回复来信
        /// </summary>
        /// <param name="id">所选信息ID</param>
        public ActionResult Reply(string id)
        {
            X_Messages m = MsgService.GetByID(int.Parse(id));
            TempData["Reciver"] = m.MessageSend;
            S_User user = us.GetAllUsers().Where(p => p.ID == m.CreaterUserID).FirstOrDefault();//Account
            TempData["sendid"] = user.ID;
            ViewData["UserID"] = GetUserInfo().UserID;
            ViewData["UploadName"] = "MSGManageReply";
            return View();
        }

        /// <summary>
        /// 删除收件箱(修改信息阅读状态，)
        /// </summary>
        /// <param name="id">所选信息ID字符串(如1,2,3)</param>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            //更改信息状态并返回结果（true成功   false失败）
            bool result = MsgService.Update_IsDelete("1", id);
            if (result)
            {
                return SuccessMsg("MSGManageMyMsg_Recive");
            }
            else
            {
                return ErrorMsg("操作失败！", "MSGManageMyMsg_Recive","","");
            }
        }

        [ValidateInput(false)]
        public ActionResult DoReply(FormCollection form)
        {
            MsgAttService mas = new MsgAttService();
            int uid = GetUserInfo().UserID;
            string guid = form["guid"];
            List<S_Attachment> list = null;
            if (guid != "" && guid != null)
            {
                list = MsgService.SearchMsgAtt(guid);
            }
            S_User u = us.GetById(uid);
            X_Messages MsgModel = new X_Messages();
            X_UserMessages UModel = new X_UserMessages();
            MsgModel.MessageTitle = form["MessageTitle"];
            MsgModel.MessageContent = form["Content"].ToString();
            MsgModel.MessageReceive = form["MessageReceive"];
            MsgModel.MessageSend = u.Name;
            MsgModel.AttachmentID = guid;
            MsgModel.Remark = form["Remark"];
            MsgModel.CreateDate = DateTime.Now;
            MsgModel.CreaterUserID = uid;
            MsgModel.CreateUserReal = u.Name;
            MsgService.Create(MsgModel);
            UModel.IsDelete = 0;
            UModel.MessageStatus = 0;
            UModel.X_Messages_ID = MsgModel.ID;
            UModel.MyMsg = 1;
            UModel.Bas_UserID = Convert.ToInt32(form["userid"]);
            UModel.ReadStatus = 0;

            um.Create(UModel);
            if (guid != "" && guid != null)
            {
                if (list.Count > 0)
                {
                    foreach (var model in list)
                    {
                        X_MsgAtts mt = new X_MsgAtts();
                        mt.AttID = model.ID;
                        mt.X_Messages_ID = MsgModel.ID;
                        mas.Create(mt);
                    }
                }
            }
            return SuccessMsg("MSGManageMyMsg_Recive");
        }

        public ActionResult Turn(string id)
        {
            ViewData["list"] = id;
            return View();
        }

        public ActionResult DoTurn(FormCollection form)
        {
            //转往哪里
            string type = form["hid_type"];
            //需要转得信息列表
            string[] list = form["list"].Split(',');
            for (int i = 0; i < list.Length; i++)
            {
                //转往垃圾箱
                if (type == "3")
                {
                    try
                    {
                        bool bo= MsgService.Update_IsDelete("1", list[i]);
                        if (bo == false)
                        {
                            return ErrorMsg("操作失败！", "MSGManageMyMsg_Recive", "", "");
                        }
                    }
                    catch
                    {
                        return ErrorMsg("操作失败！", "MSGManageMyMsg_Recive","","");
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
                        return ErrorMsg("操作失败！", "MSGManageMyMsg_Recive", "", "");
                    }
                }
            }
            return SuccessMsg("MSGManageMyMsg_Recive");
        }

        public ActionResult Write(string User, string Title)
        {
            // 联系人列表
            ViewData["Depts"] = deptService.GetAllDepts().OrderBy(p => p.Parent_ID).ToList();
            ViewData["Users"] = us.GetAllUsers().ToList();
            ViewData["UserID"] = GetUserInfo().UserID;
            ViewBag.User = User;
            ViewData["Title"] = Title;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id">1.发送 0.保存</param>
        /// <param name="ledids"></param>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoWrite(X_Messages message, string type, string[] ledids, FormCollection form)
        {
            message.MessageReceive = form["MessageReceive"];
            int uid = GetUserInfo().UserID;
            S_User u = us.GetById(uid);//当前用户所有信息
            if (type == "send")
            {
                //站内信
                if (form["messageForm"] != null)
                {
                    MsgService.SendMessage(u, ledids, message.AttachmentID, message);
                }
                //邮件
                if (form["mailForm"] != null)
                {
                    string attaNames = "";
                    if (!string.IsNullOrEmpty(message.AttachmentID))
                    {
                        var attas = MsgService.GetAttaPathByGUID(message.AttachmentID);
                        foreach (var item in attas)
                        {
                            attaNames += Server.MapPath(item) + ";";
                        }
                    }
                    string receives = "";
                    foreach (var item in ledids)
                    {
                        receives += item + ";";
                    }
                    string receiveEmails = MsgService.getEmailsByUIDS(receives);
                    MsgService.SendEmail(receiveEmails, message.MessageTitle, message.MessageContent, attaNames);
                }
                ////手机短信
                //if (form["mobileForm"] != null)
                //{
                //    var serviceCode = System.Configuration.ConfigurationManager.AppSettings["SmsServiceCode"];
                //    var mobiles = APIResponse<List<string>>("/API/GetMobiles?userIds=" + form["ledids"]);

                //    SmsService smsService = new SmsService();

                //    Sms model = new Sms();
                //    model.Content = form["MessageContent"].RemoveHTML();
                //    model.ReceivingNum = mobiles;
                //    model.ServiceCode = serviceCode;

                //    smsService.Send(model);

                //}
                return SuccessMsg("MesMSGSendMyMsg_Send");
            }
            else if (type == "save")
            {
                message.MessageSend = u.Name;
                message.CreateDate = DateTime.Now;
                message.CreaterUserID = u.ID;
                message.CreateUserReal = u.Name;
                MsgService.SaveMsgToDraft(message,ledids);

                return SuccessMsg("MesMSGDraftMyMsg_Draft");
            }
            else
            {
                return ErrorMsg("对不起，操作出现异常！", "MesMSGDraftMyMsg_Draft", "", "");
            }
        }

        /// <summary>
        /// 快捷回复
        /// </summary>
        /// <param name="messageSend">接收人</param>
        /// <param name="content">内容</param>
        [HttpPost]
        public JsonResult FastSend(int id, string CreaterUserID, string content)
        {
            var user = us.GetAllUsers().Where(p => p.ID == TypeParse.StrToInt(CreaterUserID,0)).FirstOrDefault();
            if (user == null) return ShowMessage("对不起，发件人不存在");

            X_Messages oldmessage = MsgService.GetByID(id) as X_Messages;

            int uid = GetUserInfo().UserID;
            S_User u = us.GetById(uid);

            X_Messages message = new X_Messages();
            message.MessageTitle = "回复：" + oldmessage.MessageTitle;
            message.MessageContent = content;


            MsgService.SendMessage(u, (new List<string>() { CreaterUserID }).ToArray(), "", message);

            return SuccessMsg("回复成功！", "", "", "");
        }


        /// <summary>
        /// 定时检索最新信息、未读消息
        /// </summary>
        /// <returns></returns>
        public JsonResult checkNewMessage()
        {
            //1.提醒信息
            int? count = 0; //最新信息条数
            int? total = 0;   //未读信息条数
            int IsReflesh = 0;
            Web.Areas.Mes.Models.X_UserMesCounts UserMesCount = new Areas.Mes.Models.X_UserMesCounts();

            UserMesCount.NewCount = 0;

            var userinfo = GetUserInfo();
            if (userinfo != null)
            {
                UserMesCount = _iUserMesCountService.CheckNewMessageCount(userinfo.UserID);
                if (UserMesCount != null)
                {
                    count = UserMesCount.NewCount - UserMesCount.OldCount;
                    total = UserMesCount.NoReadCount;
                    if (count > 0 && UserMesCount.IsMessage == 1)
                    {
                        IsReflesh = 1;
                        try
                        {
                            //信息提示音
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                            player.SoundLocation = Server.MapPath("~") + "Content\\images\\msg.wav";
                            player.LoadAsync();
                            player.PlaySync();
                        }
                        catch { }
                        //改变信息提示音判断条件
                        UserMesCount.IsMessage = 0;
                        _iUserMesCountService.Update(UserMesCount);
                    }
                }
            }
            var json = new
            {
                count = count,
                Total = total,
                IsReflesh = IsReflesh
            };
            return Json(json);
        }
    }
}