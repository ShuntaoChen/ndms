using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using JiYun.Web.Services;
using Web.Areas.Mes.Data;
using Web.Areas.Mes.Models;
using PagedList;
using JiYun.Modules.Core.Services;
using JiYun.Modules.Core.Models;
using System.Linq.Expressions;
using System.Collections.Specialized;

namespace Web.Areas.Mes.Services
{
    /// <summary>
    /// 系统提醒
    /// </summary>
    public interface IScheduleService : IDependency
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="forms"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        IPagedList<X_ScheduleDetails> SearchReminds(NameValueCollection forms, int uid);

        /// <summary>
        /// 添加提醒
        /// </summary>
        /// <param name="obj"></param>
        void Create(X_Schedules obj);

        /// <summary>
        /// 更新提醒
        /// </summary>
        /// <param name="obj"></param>
        void Update(X_Schedules obj);

        /// <summary>
        /// 根据编号获取提醒
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        X_Schedules GetByID(int id);

        /// <summary>
        /// 删除提醒
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 获取所有提醒
        /// </summary>
        /// <returns></returns>
        List<X_Schedules> GetList();

        /// <summary>
        /// 根据编号获取提醒明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        X_ScheduleDetails getDetailByID(int id);

        /// <summary>
        /// 更新任务的有效状态
        /// </summary>
        /// <param name="TaskID"></param>
        void UpdateDetailTask(int TaskID);

        /// <summary>
        /// 根据用户编号获取用户的提醒事项
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        List<X_Schedules> SearchSchedulers(int uid);

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="id"></param>
        void DeleteDetail(int id);

        /// <summary>
        /// 根据编号删除提醒
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sid"></param>
        void DeleteScheduler(int uid, int sid);

        /// <summary>
        /// 添加提醒
        /// </summary>
        /// <param name="sch">提醒模型</param>
        /// <param name="RemindRecyle">提醒周期</param>
        /// <param name="RemindDays">提醒间隔</param>
        void CreateScheduler(X_Schedules sch, int RemindRecyle, int RemindDays);

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        string SubString(string stringToSub, int length);

        /// <summary>
        ///  添加系统提醒
        /// </summary>
        /// <param name="Title">提醒标题</param>
        /// <param name="Content">提醒内容</param>
        /// <param name="UserID">发送人编号</param>
        /// <param name="StartTime">提醒开始时间</param>
        /// <param name="EndTime">提醒结束时间</param>
        /// <param name="Receiver">接收人集合</param>
        /// <param name="RemindDays">提醒间隔时间 默认间隔 1天</param>
        /// <param name="RemindType">提醒方式 1 系统提醒 2 短信提醒 默认系统提醒</param>
        /// <param name="RemindRecyle">提醒周期  1 天  2 小时  3分钟 默认提醒周期为1天</param>
        /// <param name="RemainBl">1 重复提醒   2 一次提醒 默认为一次提醒</param>
        void InsertRemaind(string Title, string Content, int UserID, DateTime StartTime, DateTime EndTime, List<X_ScheduleDetails> Receiver, int RemindDays = 1, string RemindType = "1", int RemindRecyle = 1, int RemainBl = 2);
    }
    public class ScheduleService : IScheduleService
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        public IPagedList<X_ScheduleDetails> SearchReminds(NameValueCollection forms, int uid)
        {
            Expression<Func<X_ScheduleDetails, bool>> filter = t => true;
            string orderField = string.Empty;
            string orderDir = string.Empty;
            int pageSize = 0;
            int page = 1;

            filter = filter.And(q => q.X_Schedules.IsRemind == 1 && q.UserID == uid && q.X_Schedules.X_ScheduleDetails.Count() == 1);

            foreach (string key in forms)
            {
                string condition = forms[key];
                if (string.IsNullOrEmpty(condition))
                {
                    continue;
                }
                switch (key)
                {
                    case "Name":
                        filter = filter.And(u => u.DisplayName.Contains(condition));
                        break;
                    case "startDate":
                        DateTime start = DateTime.Parse(condition);
                        filter = filter.And(q => q.X_Schedules.StartTime <= start);
                        break;
                    case "endDate":
                        DateTime end = DateTime.Parse(condition);
                        filter = filter.And(q => q.X_Schedules.EndTime >= end);
                        break;
                    case "numPerPage":
                        int.TryParse(condition, out pageSize);
                        break;
                    case "pageNum":
                        int.TryParse(condition, out page);
                        break;

                    case "orderField":
                        orderField = condition;
                        break;
                    case "orderDir":
                        orderDir = condition;
                        break;
                }
            }
            //query = from q in query orderby q.UserID, q.X_Schedules.StartTime descending select q;

            Func<IQueryable<X_ScheduleDetails>, IOrderedQueryable<X_ScheduleDetails>> order;
            switch (orderField)
            {
                default:
                    order = t => t.OrderBy(q => q.UserID).OrderByDescending(s => s.X_Schedules.StartTime);
                    break;
            }
            return unitOfWork.X_ScheduleDetailsRepository().Get(page, filter, order, pageSize);
        }


        public void Create(X_Schedules obj)
        {
            unitOfWork.X_SchedulesRepository().Insert(obj);
            unitOfWork.Save();
        }
        public void Update(X_Schedules obj)
        {
            unitOfWork.X_SchedulesRepository().Update(obj);
            unitOfWork.Save();
        }
        public X_Schedules GetByID(int id)
        {
            return unitOfWork.X_SchedulesRepository().GetById(id);
        }
        public void Delete(int id)
        {
            unitOfWork.X_SchedulesRepository().Delete(id);
            unitOfWork.Save();
        }
        public List<X_Schedules> GetList()
        {
            return unitOfWork.X_SchedulesRepository().GetAll().ToList();
        }
        public X_ScheduleDetails getDetailByID(int id)
        {
            return unitOfWork.X_ScheduleDetailsRepository().GetById(id);
        }

        public List<X_Schedules> SearchSchedulers(int uid)
        {
            var query = unitOfWork.X_SchedulesRepository().GetAll().Where(s => s.X_ScheduleDetails.Where(u => u.UserID == uid).Count() > 0);
            return query.ToList<X_Schedules>();
        }

        public void DeleteDetail(int id)
        {
            unitOfWork.X_ScheduleDetailsRepository().Delete(id);
            unitOfWork.Save();
        }
        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="sid"></param>
        public void DeleteScheduler(int uid, int sid)
        {
            var scheduler = GetByID(sid);
            if (scheduler.UserID != uid)
            {
                throw new Exception("对不起，只能删除用户自己创建的记录！");
            }
            Delete(sid);
        }

        /// <summary>
        /// 添加提醒
        /// </summary>
        /// <param name="sch">提醒模型</param>
        /// <param name="RemindRecyle">提醒周期</param>
        /// <param name="RemindDays">提醒间隔</param>
        public void CreateScheduler(X_Schedules sch, int RemindRecyle, int RemindDays)
        {

            Create(sch);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sch"></param>
        public void UpdateScheduler(X_Schedules sch)
        {
            Update(sch);
        }


        public DateTime TimestampToDateTime(string timestamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        public string SubString(string stringToSub, int length)
        {
            if (stringToSub == null) return "";
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                    break;
            }

            return sb.ToString();
        }
      
        /// <summary>
        /// 以分钟为间隔的提醒信息转换
        /// </summary>
        /// <returns></returns>
        public string ScheduleToMessageMinute()
        {
            string result = "";
            try
            {
                DateTime now = DateTime.Now;

                #region 以分钟为间隔的重复提醒
                List<X_Messages> messagelist = unitOfWork.X_MessagesRepository().GetAll().Where(u => u.CreateDate.Value.Year == now.Year && u.CreateDate.Value.Month == now.Month && u.CreateDate.Value.Day == now.Day).ToList();
                List<X_Schedules> ScheduleList = unitOfWork.X_SchedulesRepository().GetAll().Where(u => u.EndTime >= now && u.RemindRecyle == 1 && u.RemainBl == 1 && u.IsRemind == 1).ToList();
                foreach (X_Schedules Schedule in ScheduleList)
                {
                    if (Schedule.RemindType.Contains("1"))
                    {
                        if (Schedule.RemindRecyle == 1 && (((DateTime)Schedule.LastRemaindTime).AddMinutes((int)Schedule.RemindDays).ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm") || Schedule.LastRemaindTime == null))
                        {
                            if (((DateTime)Schedule.LastRemaindTime).AddMinutes((int)Schedule.RemindDays) < DateTime.Now)
                            {
                                SendRemaindMessage(Schedule, messagelist);
                                continue;
                            }
                            else
                                continue;
                        }
                        SendRemaindMessage(Schedule, messagelist);
                    }
                }
                #endregion

                #region 一次提醒
                List<X_Schedules> ScheduleOnelist = unitOfWork.X_SchedulesRepository().GetAll().Where(u => u.EndTime >= now && u.RemainBl == 2 && u.IsRemind == 1).ToList();
                foreach (X_Schedules Schedule in ScheduleOnelist)
                {
                    if (Schedule.RemindRecyle == 1 && ((DateTime)Schedule.EndTime).AddMinutes(-(int)Schedule.RemindDays).ToString("yyyy-MM-dd HH:mm") == now.ToString("yyyy-MM-dd HH:mm"))
                    {
                        SendRemaindMessage(Schedule, messagelist);
                    }
                    else if (Schedule.RemindRecyle == 2 && ((DateTime)Schedule.EndTime).AddHours(-(int)Schedule.RemindDays).ToString("yyyy-MM-dd HH") == now.ToString("yyyy-MM-dd HH"))
                    {
                        SendRemaindMessage(Schedule, messagelist);
                    }
                    else if (Schedule.RemindRecyle == 3 && ((DateTime)Schedule.EndTime).AddDays(-(int)Schedule.RemindDays).ToString("yyyy-MM-dd") == now.ToString("yyyy-MM-dd"))
                    {
                        SendRemaindMessage(Schedule, messagelist);
                    }
                    else if (Schedule.RemindRecyle == 4 && ((DateTime)Schedule.EndTime).AddDays(-(int)Schedule.RemindDays * 7).ToString("yyyy-MM-dd") == now.ToString("yyyy-MM-dd"))
                    {
                        SendRemaindMessage(Schedule, messagelist);
                    }
                }
                #endregion
                unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 将提醒转化为提醒信息
        /// </summary>
        /// <returns></returns>
        public string ScheduleToMessage()
        {

            try
            {
                DateTime now = DateTime.Now;

                #region  以小时、天、周为间隔的重复提醒
                List<X_Messages> messagelist = unitOfWork.X_MessagesRepository().GetAll().Where(u => u.CreateDate.Value.Year == now.Year && u.CreateDate.Value.Month == now.Month - 1).ToList();

                List<X_Schedules> ScheduleList = unitOfWork.X_SchedulesRepository().GetAll().Where(u => u.EndTime >= now && u.RemindRecyle != 1 && u.RemainBl == 1 && u.IsRemind == 1).ToList();
                foreach (X_Schedules Schedule in ScheduleList)
                {
                    if (Schedule.RemindType.Contains("1"))
                    {
                        //提醒周期为小时
                        if (Schedule.RemindRecyle == 2 && (((DateTime)Schedule.LastRemaindTime).AddHours((int)Schedule.RemindDays).ToString("yyyy-MM-dd HH") != DateTime.Now.ToString("yyyy-MM-dd HH") || Schedule.LastRemaindTime == null))
                        {
                            if (((DateTime)Schedule.LastRemaindTime).AddHours((int)Schedule.RemindDays) < DateTime.Now)
                            {
                                SendRemaindMessage(Schedule, messagelist);
                                continue;
                            }
                            else
                                continue;
                        }
                        else if (Schedule.RemindRecyle == 3 && (((DateTime)Schedule.LastRemaindTime).AddDays((int)Schedule.RemindDays).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd") || Schedule.LastRemaindTime == null))
                        {  //提醒周期为天
                            if (((DateTime)Schedule.LastRemaindTime).AddDays((int)Schedule.RemindDays) < DateTime.Now)
                            {
                                SendRemaindMessage(Schedule, messagelist);
                                continue;
                            }
                            else
                                continue;
                        }
                        else if (Schedule.RemindRecyle == 4 && (((DateTime)Schedule.LastRemaindTime).AddDays((int)Schedule.RemindDays * 7).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd") || Schedule.LastRemaindTime == null))
                        {   //提醒周期为周
                            if (((DateTime)Schedule.LastRemaindTime).AddDays((int)Schedule.RemindDays * 7) < DateTime.Now)
                            {
                                SendRemaindMessage(Schedule, messagelist);
                                continue;
                            }
                            else
                                continue;
                        }
                        SendRemaindMessage(Schedule, messagelist);
                    }
                }
                unitOfWork.Save();
                #endregion
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// 发送提醒信息
        /// </summary>
        /// <param name="remaind"></param>
        public void SendRemaindMessage(X_Schedules Schedule, List<X_Messages> messagelist)
        {
            X_Messages message = new X_Messages();
            message.X_Schedules_ID = Schedule.ID;
            message.CreateDate = DateTime.Now;
            Schedule.LastRemaindTime = DateTime.Now;
            if (Schedule.RemainBl == 1)
            {
                message.MessageTitle = "系统提醒：" + Schedule.Title;
            }
            else
            {
                string remindRecyle = "";
                if (Schedule.RemindRecyle == 1)
                    remindRecyle = "分钟";
                else if (Schedule.RemindRecyle == 2)
                    remindRecyle = "小时";
                else if (Schedule.RemindRecyle == 3 || Schedule.RemindRecyle == 4)
                    remindRecyle = "天";
                message.MessageTitle = "系统日程提醒：离【" + Schedule.Title + "】到期事件只有" + (Schedule.RemindRecyle == 4 ? Schedule.RemindDays * 7 : Schedule.RemindDays) + remindRecyle;
            }
            message.MessageContent += "提醒主题：" + Schedule.Title + "<br />";
            message.MessageContent += "到期时间：" + Schedule.EndTime + "<br />";
            message.MessageContent += "提醒内容：" + Schedule.Contents;

            message.MessageSend = "系统管理员"; //系统管理员发送

            bool bo = true; //判断当前发送时间数据库中是否已经有该记录
            if (Schedule.RemindRecyle == 1) //分钟
            {
                if (messagelist.Where(u => u.CreateDate.Value.Year == message.CreateDate.Value.Year && u.CreateDate.Value.Month == message.CreateDate.Value.Month && u.CreateDate.Value.Day == message.CreateDate.Value.Day && u.CreateDate.Value.Hour == message.CreateDate.Value.Hour && u.CreateDate.Value.Minute == message.CreateDate.Value.Minute).Count() > 0)
                    bo = false;
            }
            else if (Schedule.RemindRecyle == 2) //小时
            {
                if (messagelist.Where(u => u.CreateDate.Value.Year == message.CreateDate.Value.Year && u.CreateDate.Value.Month == message.CreateDate.Value.Month && u.CreateDate.Value.Day == message.CreateDate.Value.Day && u.CreateDate.Value.Hour == message.CreateDate.Value.Hour).Count() > 0)
                    bo = false;
            }
            else if (Schedule.RemindRecyle == 3 || Schedule.RemindRecyle == 4) //天 、周
            {
                if (messagelist.Where(u => u.CreateDate.Value.Year == message.CreateDate.Value.Year && u.CreateDate.Value.Month == message.CreateDate.Value.Month && u.CreateDate.Value.Day == message.CreateDate.Value.Day).Count() > 0)
                    bo = false;
            }

            foreach (X_ScheduleDetails ScheduleDetail in Schedule.X_ScheduleDetails)
            {
                message.MessageReceive += ScheduleDetail.DisplayName + ";";
            }
            if (bo == true)
            {

                foreach (X_ScheduleDetails ScheduleDetail in Schedule.X_ScheduleDetails)
                {
                    X_UserMessages mesdetail = new X_UserMessages();
                    mesdetail.ReadStatus = 0;
                    mesdetail.MyMsg = 1;
                    mesdetail.MessageStatus = 0;
                    mesdetail.IsDelete = 0;
                    mesdetail.Bas_UserID = ScheduleDetail.UserID;
                    message.X_UserMessages.Add(mesdetail); //添加收件箱信息明细
                }
                X_UserMessages Indetail = new X_UserMessages();
                Indetail.ReadStatus = 1;
                Indetail.MyMsg = 0;
                Indetail.IsDelete = 0;
                Indetail.Bas_UserID = 1;//root发送
                message.X_UserMessages.Add(Indetail); //添加发件箱信息明细

                unitOfWork.X_MessagesRepository().Insert(message);  //添加信息

                unitOfWork.X_SchedulesRepository().Update(Schedule);

            }
        }

        /// <summary>
        ///  添加系统提醒
        /// </summary>
        /// <param name="Title">提醒标题</param>
        /// <param name="Content">提醒内容</param>
        /// <param name="UserID">发送人编号</param>
        /// <param name="StartTime">提醒开始时间</param>
        /// <param name="EndTime">提醒结束时间</param>
        /// <param name="Receiver">接收人集合</param>
        /// <param name="RemindDays">提醒间隔时间 默认间隔 1天</param>
        /// <param name="RemindType">提醒方式 1 系统提醒 2 短信提醒 默认系统提醒</param>
        /// <param name="RemindRecyle">提醒周期   1 分钟   2 小时  3天 默认提醒周期为3天</param>
        /// <param name="RemainBl">1 重复提醒   2 一次提醒 默认为一次提醒</param>
        public void InsertRemaind(string Title, string Content, int UserID, DateTime StartTime, DateTime EndTime,List<X_ScheduleDetails> Receiver, int RemindDays=1, string RemindType="1", int RemindRecyle=3, int RemainBl=2)
        {
            //提醒主题
            X_Schedules Schedules = new X_Schedules();
            Schedules.Contents = Content;
            Schedules.EndTime = EndTime;
            Schedules.IsRemind = 1;
            Schedules.RemainBl = RemainBl;
            Schedules.RemaindCode = Guid.NewGuid().ToString();
            Schedules.RemindDays = RemindDays;
            Schedules.RemindRecyle = RemindRecyle;
            Schedules.RemindType = RemindType;
            Schedules.ScType = 1;
            Schedules.StartTime = StartTime;
            Schedules.Title = Title;
            Schedules.UserID = UserID;
            //提醒对象
            foreach (var ScheduleDetails in Receiver)
            {
                ScheduleDetails.CreateOn = DateTime.Now;
                ScheduleDetails.CreatorId = UserID;
                Schedules.X_ScheduleDetails.Add(ScheduleDetails);
            }
            unitOfWork.X_SchedulesRepository().Insert(Schedules);
            unitOfWork.Save();
        }

        /// <summary>
        /// 更新任务的有效状态
        /// </summary>
        /// <param name="TaskID"></param>
        public void UpdateDetailTask(int TaskID)
        {
            Expression<Func<X_ScheduleDetails, bool>> filter = t => t.TaskID==TaskID;
            var list = unitOfWork.X_ScheduleDetailsRepository().GetAll(filter);
            if (list.Any())
            {
                var model = list.First();
                model.IsExpired = true;
                unitOfWork.X_ScheduleDetailsRepository().Update(model);
                unitOfWork.Save();
            }
        }
    }
}