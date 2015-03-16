using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiYun.Web.Services;
using JiYun.Modules.Core.Services;
using PagedList;
using JiYun.Modules.Core.Models;
using System.Linq.Expressions;
using System.Collections.Specialized;
using JiYun.Common.Utils;
using System.Net.Mail;
using System.IO;
using Web.Areas.Mes.Data;
using Web.Areas.Mes.Models;

namespace JY.Module.Message.Services
{
    public class MSGManagerService : BaseService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private JiYun.Modules.Core.Services.IAttachmentService unit = new JiYun.Modules.Core.Services.AttachmentService();
        private UserMessageService um = new UserMessageService();
        private UserService us = new UserService();
        private BaseService bs = new BaseService();                        //基本
        /// <summary>
        /// 消息中心，获得各种消息
        /// </summary>
        /// <param name="Userid">用户编号</param>
        /// <param name="readStatus">消息阅读状态 1,已读 0,未读</param>
        /// <param name="IsDelete">是否已删除 1,已删除 0,未删除</param>
        /// <param name="currentPage">第几页</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="MessageStatus">消息状态 0,系统消息 1,即时消息</param>
        /// <param name="MyMsgs">所属性质 0,发送1,接收2,草稿3,垃圾，4，所有</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderdir">排序规则(esc ， desc)</param>
        /// <param name="startdate">搜索所用的时间范围(开始时间)</param>
        /// <param name="enddate">搜索所用的时间范围(结束时间)</param>
        /// <returns>消息列表集合</returns>
        public IPagedList<X_UserMessages> SearchMyMsg(string Userid, string readStatus, string IsDelete, string MessageStatus, string MyMsgs, NameValueCollection forms)
        {
            Expression<Func<X_UserMessages, bool>> filter = t => true;
            string orderField = string.Empty;
            string orderDir = string.Empty;
            int pageSize = 0;
            int page = 1;

            int U_id = 0;
            if (Userid != "" && Userid != null)
            {
                U_id = int.Parse(Userid);
            }
            if (MyMsgs != "" && MyMsgs != null)
            {
                int my = int.Parse(MyMsgs);
                if (my == 4)
                {
                    filter = filter.And(t => t.Bas_UserID == U_id);
                }
                else
                {
                    filter = filter.And(m => m.Bas_UserID == U_id && m.MyMsg == my);
                }
            }
            if (!string.IsNullOrEmpty(IsDelete))
            {
                int isdele = int.Parse(IsDelete);
                filter = filter.And(m => m.IsDelete == isdele);
            }


            foreach (string key in forms)
            {
                string condition = forms[key];
                if (string.IsNullOrEmpty(condition))
                {
                    continue;
                }
                switch (key)
                {
                    case "startDate":
                        DateTime start = DateTime.Parse(condition);
                        filter = filter.And(t => t.X_Messages.CreateDate >= start);
                        break;
                    case "endDate":
                        DateTime end = DateTime.Parse(condition);
                        filter = filter.And(t => t.X_Messages.CreateDate <= end);
                        break;
                    case "ReadStatus":
                        int _ReadStatus = int.Parse(condition);
                        filter = filter.And(t => t.ReadStatus == _ReadStatus);
                        break;
                    case "KeyWord":
                        filter = filter.And(t => t.X_Messages.MessageTitle.Contains(condition) || t.X_Messages.MessageContent.Contains(condition) || (t.X_Messages.MessageReceive != null && t.X_Messages.MessageReceive.Contains(condition)));
                        break;
                    case "numPerPage":
                        int.TryParse(condition, out pageSize);
                        break;
                    case "pageNum":
                        int.TryParse(condition, out page);
                        break;

                    case "orderField":
                        //switch (condition)
                        //{
                        //    case "CreateDate":
                        //        if (orderdir == "desc")
                        //            Msg = Msg.OrderByDescending(u => u.X_Messages.CreateDate);
                        //        else
                        //            Msg = Msg.OrderBy(u => u.X_Messages.CreateDate);
                        //        break;
                        //    default:
                        //        Msg = Msg.OrderByDescending(u => u.X_Messages.ID);
                        //        break;
                        //}
                        orderField = condition;
                        break;
                    case "orderDir":
                        orderDir = condition;
                        break;
                }
            }
            //if (currentPage == "" || currentPage == null)
            //{
            //    currentPage = "1";
            //}
            //if (numPerPage == "" || numPerPage == null)
            //{
            //    numPerPage = "10";
            //}
            Func<IQueryable<X_UserMessages>, IOrderedQueryable<X_UserMessages>> order;
            switch (orderField)
            {
                default:
                    order = t => t.OrderByDescending(s => s.X_Messages.CreateDate);
                    break;
            }
            return unitOfWork.X_UserMessagesRepository().Get(page, filter, order, pageSize);
        }

        /// <summary>
        /// 获取未读邮件数量
        /// </summary>
        /// <returns></returns>
        public int GetUnReadCount(int userID)
        {
            return unitOfWork.X_UserMessagesRepository().GetAll().Where(m => m.Bas_UserID == userID && m.MyMsg == 1 && m.IsDelete == 0 && m.ReadStatus == 0).Count();
        }

        /// <summary>
        /// 获得用户当前最新的消息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns>消息数量</returns>
        public int GetNewMsg(string UserID)
        {
            if (UserID != "" && UserID != null)
            {
                int userid = int.Parse(UserID);
                //获取当前用户未读，未删的消息数量
                List<X_UserMessages> list = unitOfWork.X_UserMessagesRepository().GetAll().Where(t => t.Bas_UserID == userid && t.ReadStatus == 0 && t.IsDelete == 0).ToList();
                return list.Count;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获得用户当前最新的消息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns>消息数量</returns>
        public List<X_UserMessages> GetUserMsg(string UserID)
        {
            if (UserID != "" && UserID != null)
            {
                int userid = int.Parse(UserID);
                //获取当前用户未读，未删的消息数量
                List<X_UserMessages> list = unitOfWork.X_UserMessagesRepository().GetAll().Where(t => t.Bas_UserID == userid).ToList();
                return list;
            }
            else
            {
                return new List<X_UserMessages>();
            }
        }
        /// <summary>
        /// 更新消息的阅读状态
        /// </summary>
        /// <param name="model">单个实体对象</param>
        /// <param name="readstatus">消息状态</param>
        /// <param name="id">消息ID</param>
        public void Update_ReadStatus(int uid, string readstatus, string id)
        {
            var model = unitOfWork.X_UserMessagesRepository().GetById(uid);
            if (!string.IsNullOrEmpty(id))
            {
                model.ReadStatus = Convert.ToInt32(readstatus);
            }
            else
            {
                model.ReadStatus = 0;
            }
            unitOfWork.X_UserMessagesRepository().Update(model);
            unitOfWork.Save();
        }

        public List<S_Attachment> SearchMsgAtt(string guid)
        {
            return unit.GetAllAttachment().Where(t => t.AttachmentID == guid).ToList();
        }
        /// <summary>
        /// 更新消息所属状态
        /// </summary>
        /// <param name="model">单个实体对象</param>
        /// <param name="MyMsg">所属性质 0.发送 1.接收 2.草稿 3.垃圾 4.所有</param>
        /// <param name="id">消息ID</param>
        public void Update_MyMsg(int mid, string MyMsg, string id)
        {
            X_UserMessages model = unitOfWork.X_UserMessagesRepository().GetById(mid);
            if (!string.IsNullOrEmpty(id))
            {
                model.MyMsg = Convert.ToInt32(MyMsg);
                model.IsDelete = 0;
            }
            else
            {
                model.MyMsg = 1;
                model.IsDelete = model.IsDelete;
            }
            unitOfWork.X_UserMessagesRepository().Update(model);
            unitOfWork.Save();
        }

        /// <summary>
        /// 更新消息的删除状态
        /// </summary>
        /// <param name="IsDelete">更改状态</param>
        /// <param name="id">消息ID</param>
        /// <returns>是否成功</returns>
        public bool Update_IsDelete(string IsDelete, string id)
        {
            try
            {
                string[] idlist = id.Split(',');
                for (int i = 0; i < idlist.Length; i++)
                {
                    string detailid = idlist[i];
                    int mid = int.Parse(detailid);
                    X_UserMessages model = unitOfWork.X_UserMessagesRepository().GetById(mid);
                    if (!string.IsNullOrEmpty(detailid))
                    {
                        model.IsDelete = Convert.ToInt32(IsDelete);
                    }
                    else
                    {
                        model.IsDelete = 0;
                    }
                    unitOfWork.X_UserMessagesRepository().Update(model);
                    unitOfWork.Save();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 垃圾箱删除
        /// </summary>
        /// <param name="id">删除ID</param>
        /// <returns>是否成功</returns>
        public bool Delete_Garbage(string id)
        {
            try
            {
                string[] idlist = id.Split(',');
                for (int i = 0; i < idlist.Length; i++)
                {
                    string detailid = idlist[i];
                    um.Delete(int.Parse(detailid));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新消息内容
        /// </summary>
        /// <param name="message">消息实体对象</param>
        /// <param name="userid">用户ID</param>
        /// <param name="userreal">用户名</param>
        /// <returns>是否成功</returns>
        public bool Update_Message(X_Messages message, string userid, string userreal)
        {
            int user = int.Parse(userid);
            try
            {
                if (!string.IsNullOrEmpty(message.ID.ToString()))
                {
                    X_Messages model = GetByID(message.ID);
                    model.MessageContent = message.MessageContent;
                    model.MessageTitle = message.MessageTitle;
                    model.Remark = message.Remark;
                    model.AttachmentID = message.AttachmentID;
                    model.ModifyDate = DateTime.Now;
                    model.ModifyUserID = user;
                    model.ModifyUserReal = userreal;
                }
                else
                {
                }
                unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="userIds">接受人ID</param>
        /// <param name="guid">附件Guid</param>
        /// <param name="msg">消息对象</param>
        public void SendMessage(S_User user, string[] userIds, string guid, X_Messages msg)
        {
            //当前用户
            int uid = user.ID;
            List<S_Attachment> list = null;
            if (guid != "" && guid != null)
            {
                list = SearchMsgAtt(guid);
            }
            //收件人
            msg.MessageSend = user.Name;
            msg.CreateDate = DateTime.Now;
            msg.CreaterUserID = user.ID;
            msg.CreateUserReal = user.Name;
            msg.AttachmentID = guid;
            Create(msg);    //添加信息表
            //循环往X_UserMessage表中插入关联信息
            for (int i = 0; i < userIds.Count(); i++)
            {
                int id = Convert.ToInt32(userIds[i]);
                X_UserMessages ume = new X_UserMessages();
                ume.IsDelete = 0;
                ume.MessageStatus = 1;
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
                        unitOfWork.X_MsgAttsRepository().Insert(mt);
                        unitOfWork.Save();
                    }
                }
            }
        }
        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="sys">建议值为（"系统消息"）</param>
        /// <param name="reciver">接受人ID列表</param>
        /// <param name="msg">消息对象</param>
        public void SendMessage(string sys, string reciver, X_Messages msg)
        {
            //收件人
            string[] ids = reciver.Split(',');
            msg.MessageSend = sys;
            msg.CreateDate = DateTime.Now;
            msg.CreaterUserID = null;
            msg.CreateUserReal = sys;
            Create(msg);    //添加信息表
            //循环往X_UserMessage表中插入关联信息
            for (int i = 0; i < ids.Length; i++)
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
        }

        public void SaveMsgToDraft(X_Messages msg, string[] userIds)
        {
            List<S_Attachment> list = null;
            string guid = msg.AttachmentID;
            if (guid != "" && guid != null)
            {
                list = SearchMsgAtt(guid);    //获得附件列表
            }
            //循环往X_UserMessage表中插入接收人记录
            for (int i = 0; i < userIds.Count(); i++)
            {
                int id = Convert.ToInt32(userIds[i]);
                X_UserMessages ume = new X_UserMessages();
                ume.IsDelete = 0;
                ume.MessageStatus = 1;
                ume.X_Messages_ID = msg.ID;
                ume.MyMsg = 1;
                ume.ReadStatus = 0;
                ume.Bas_UserID = id;
                msg.X_UserMessages.Add(ume);
            }
            //插入发送人记录
            X_UserMessages send = new X_UserMessages();
            send.IsDelete = 0;
            send.MessageStatus = 0;
            send.X_Messages_ID = msg.ID;
            send.MyMsg = 2;
            send.ReadStatus = 1;
            send.Bas_UserID = msg.CreaterUserID;
            msg.X_UserMessages.Add(send);
            Create(msg);
           
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
                        unitOfWork.X_MsgAttsRepository().Insert(mt);
                        unitOfWork.Save();
                    }
                }
            }
        }
        public void UpdateMsgToDraft(X_Messages msg, string userid, string username)
        {
            Update_Message(msg, userid, username);
        }

        public void Create(X_Messages message)
        {
            unitOfWork.X_MessagesRepository().Insert(message);
            unitOfWork.Save();
        }
        public void Update(X_Messages message)
        {
            unitOfWork.X_MessagesRepository().Update(message);
            unitOfWork.Save();
        }
        public void Delete(int id)
        {
            unitOfWork.X_MessagesRepository().Delete(id);
            unitOfWork.Save();
        }
        public X_Messages GetByID(int id)
        {
            return unitOfWork.X_MessagesRepository().GetById(id);
        }

        /// <summary>
        /// 更新用户信息条数
        /// </summary>
        /// <param name="UserID"></param>
        public void UpdateUserMesCount(int UserID)
        {
            X_UserMesCounts Mescount = new X_UserMesCounts();
            Mescount = unitOfWork.X_UserMesCountsRepository().GetAll().Where(u => u.UserID == UserID).FirstOrDefault();
            if (Mescount != null)
            {
                Mescount.OldCount = Mescount.NewCount;
                Mescount.IsMessage = 0;
                unitOfWork.X_UserMesCountsRepository().Update(Mescount);
                unitOfWork.Save();
            }
        }
        /// <summary>
        /// 根据信息编号获取收件人记录
        /// </summary>
        /// <param name="msgID"></param>
        /// <returns></returns>
        public List<X_UserMessages> SearchUser(int msgID)
        {
            return unitOfWork.X_UserMessagesRepository().GetAll().Where(t => t.X_Messages_ID == msgID && t.MyMsg == 1).ToList();
        }

        #region 原API方法
        public List<string> GetAttaPathByGUID(string id)
        {
            AttachmentService attas = new AttachmentService();
            var list = attas.GetAllAttachment().Where(a => a.AttachmentID == id).Select(a => a.Path).ToList();
            return list;
        }

        public string getEmailsByUIDS(string id)
        {
            UserService us = new UserService();
           // UserInfoService userInfo = new UserInfoService();
            List<string> ids = id.Split(';').ToList();
            string emails = "";
            foreach (var item in ids)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    //var user = userInfo.GetUserInfoByUserID(TypeParse.StrToInt(item, 0));
                    //if (user != null && !string.IsNullOrEmpty(user.Mail))
                    //{
                    //    emails += user.Mail + ";";
                    //}
                }
            }
            return emails;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receivePer">收件人(多个收件人以";"隔开)</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="mailBody">邮件内容</param>
        /// <param name="attachment">附件（取绝对路径，多个附件以";"隔开）</param>
        public void SendEmail(string receivePer, string subject, string mailBody, string attachment)
        {
            string sendPerson = System.Configuration.ConfigurationManager.AppSettings["SendEmail"];
            string sendPersonPWD = System.Configuration.ConfigurationManager.AppSettings["EmailPassword"];
            string mtp = System.Configuration.ConfigurationManager.AppSettings["SMTP"];
            MailMessage mail = new MailMessage();
            if (receivePer.Length == 0)
            {
                throw new Exception("请检查收件人是否设置了Email！");
            }
            mail.From = new MailAddress(sendPerson); //设置发信人地址
            List<string> sendList = new List<string>();
            if (receivePer.IndexOf(";") > 0)
            {
                sendList = receivePer.Split(';').ToList();
            }
            else
            {
                sendList.Add(receivePer);
            }
            foreach (var item in sendList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    mail.To.Add(item); //添加收件人
                }
            }
            mail.IsBodyHtml = true;  //正文以html格式显示
            mail.Subject = subject;
            mail.Body = mailBody;
            if (!string.IsNullOrEmpty(attachment))
            {
                List<string> attas = new List<string>();
                if (attachment.IndexOf(";") > 0)
                {
                    attas = attachment.Split(';').ToList();
                }
                else
                {
                    attas.Add(attachment);
                }
                foreach (var item in attas)
                {
                    if (!string.IsNullOrEmpty(item) && File.Exists(item))
                    {
                        Attachment att = new Attachment(item);
                        mail.Attachments.Add(att);
                    }
                }
            }
            mail.SubjectEncoding = Encoding.GetEncoding("gb2312");
            mail.BodyEncoding = Encoding.GetEncoding("gb2312");
            SmtpClient smtp = new SmtpClient(mtp);
            smtp.Credentials = new System.Net.NetworkCredential(sendPerson, sendPersonPWD);
            smtp.Send(mail);
            mail.Dispose();
        }
        #endregion
    }
}