using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JiYun.Web.Controllers;
using Web.Controllers;
using JY.Module.Message.Services;
using JiYun.Modules.Core.Services;
using Web.Areas.Mes.Models;
using Web.Areas.Mes.Services;

namespace Web.Areas.Mes.Controllers
{
    /// <summary>
    /// 提醒管理
    /// </summary>
    public class RemindController : MyBaseController
    {
        private readonly IScheduleService sch;
        public RemindController(IScheduleService IScheduleService)
        {
            sch = IScheduleService;
        }
        public ActionResult Index(FormCollection formCollection)
        {
            int uid = GetUserInfo().UserID;
            ViewData.Model = sch.SearchReminds(formCollection, uid);
            ViewBag.Name = formCollection["Name"];
            ViewBag.startDate = formCollection["startDate"];
            ViewBag.endDate = formCollection["endDate"];
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public JsonResult DoCreate(X_Schedules remind)
        {
            int uid = GetUserInfo().UserID;
            remind.UserID = uid;
            remind.IsRemind = 1;
            remind.IsPublic = 0;
            remind.RemindType = "1";
            remind.ScType = 1;
            remind.CreateDate = DateTime.Now;
            var detail = new X_ScheduleDetails();
            detail.UserID = uid;
            detail.DisplayName = GetUserInfo().Name;
            remind.X_ScheduleDetails.Add(detail);
            sch.Create(remind);
            return SuccessMsg("MesRemindIndex");
        }

        public ActionResult Edit(int id)
        {
            ViewData.Model = sch.GetByID(id);
            return View();
        }
        public JsonResult DoEdit(X_Schedules remind)
        {
            remind.ModifyDate = DateTime.Now;
            sch.Update(remind);
            return SuccessMsg("MesRemindIndex");
        }

        public ActionResult Detail(int id)
        {
            ViewData.Model = sch.GetByID(id);
            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            var detail = sch.GetByID(id);
            if (detail != null)
            {
                int sid = id;
                if (detail.X_ScheduleDetails.Count() > 1)
                {
                    sch.DeleteDetail(id);
                }
                else if (detail.X_ScheduleDetails.Count() == 1)
                {
                    sch.DeleteDetail(id);
                    sch.Delete(sid);
                }
            }
            return SuccessMsg("MesRemindIndex");
        }

    }
}