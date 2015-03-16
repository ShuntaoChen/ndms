using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiYun.Web.Services;
using Web.Areas.Mes.Models;
using Web.Areas.Mes.Data;

namespace JY.Module.Message.Services
{
    public class UserMessageService : BaseService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public void Create(X_UserMessages message)
        {
            unitOfWork.X_UserMessagesRepository().Insert(message);
            unitOfWork.Save();
        }
        public void Update(X_UserMessages message)
        {
            unitOfWork.X_UserMessagesRepository().Update(message);
            unitOfWork.Save();
        }
        public void Delete(int id)
        {
            unitOfWork.X_UserMessagesRepository().Delete(id);
            unitOfWork.Save();
        }
        public X_UserMessages GetByID(int id)
        {
            return unitOfWork.X_UserMessagesRepository().GetById(id);
        }
        /// <summary>
        /// 获得未读消息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<List<string>> getUnReadMessage(int uid)
        {
            var message = new List<List<string>>();
            var mes = unitOfWork.X_UserMessagesRepository().GetAll().Where(m => m.ReadStatus == 0 && m.Bas_UserID == uid && m.MyMsg == 1).Take(5).ToList();
            foreach (var item in mes)
            {
                var list = new List<string>();
                list.Add(item.X_Messages.MessageTitle);
                list.Add(item.X_Messages.MessageSend);
                list.Add(item.X_Messages.CreateDate.ToString());
                list.Add(item.ID.ToString());
                message.Add(list);
            }
            return message;
        }

        public List<List<string>> getUnReadMessageByType(int uid, int type)
        {
            var message = new List<List<string>>();
            var mes = unitOfWork.X_UserMessagesRepository().GetAll().Where(m => m.ReadStatus == 0 && m.Bas_UserID == uid && m.MyMsg == 1 && m.MessageStatus == type).Take(5).ToList();
            foreach (var item in mes)
            {
                var list = new List<string>();
                list.Add(item.X_Messages.MessageTitle);
                list.Add(item.X_Messages.MessageSend);
                list.Add(item.X_Messages.CreateDate.ToString());
                list.Add(item.ID.ToString());
                message.Add(list);
            }
            return message;
        }
        /// <summary>
        /// 获得未读消息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int getUnReadMessageCount(int uid)
        {
            return unitOfWork.X_UserMessagesRepository().GetAll().Where(m => m.ReadStatus == 0 && m.Bas_UserID == uid && m.MyMsg == 1).Count();
        }
        /// <summary>
        /// 获得当前用户指定时间的提醒
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="currentDay"></param>
        /// <returns></returns>
        public List<List<string>> getSchedules(int uid, DateTime currentDay)
        {
            var schedule = new List<List<string>>();
            var sch = unitOfWork.X_ScheduleDetailsRepository().GetAll().Where(s => s.UserID == uid && s.X_Schedules.StartTime <= currentDay && s.X_Schedules.EndTime >= currentDay).ToList();
            foreach (var item in sch)
            {
                schedule.Add(new List<string> { item.ID.ToString(), item.X_Schedules.ID.ToString(), item.X_Schedules.Title, item.X_Schedules.Color, item.X_Schedules.StartTime.ToString(), item.X_Schedules.EndTime.ToString(), item.X_Schedules.Contents });
            }
            return schedule;
        }

        public List<X_Messages> getList()
        {
            return unitOfWork.X_MessagesRepository().GetAll().ToList();
        }


        /// <summary>
        /// 获得未读消息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<List<string>> getUnReadMessage(int uid, int msgtype)
        {
            var message = new List<List<string>>();
            var mes = unitOfWork.X_UserMessagesRepository().GetAll().Where(m => m.ReadStatus == 0 && m.Bas_UserID == uid && m.MyMsg == 1 && m.MessageStatus == msgtype).OrderByDescending(m => m.X_Messages.CreateDate).ToList();
            foreach (var item in mes)
            {
                var list = new List<string>();
                list.Add(item.X_Messages.MessageTitle);
                list.Add(item.X_Messages.MessageSend);
                list.Add(item.X_Messages.CreateDate.ToString());
                list.Add(item.X_Messages.CreateDate.Value.ToString("yyyy-MM-dd"));
                list.Add(item.X_Messages.MessageContent.ToString());
                list.Add(item.X_Messages_ID.ToString() + "," + item.ID.ToString());
                message.Add(list);
            }
            return message;
        }
    }
}